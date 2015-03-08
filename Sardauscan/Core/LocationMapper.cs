#region COPYRIGHT
/****************************************************************************
 *  Copyright (c) 2015 Fabio Ferretti <https://plus.google.com/+FabioFerretti3D>                 *
 *  This file is part of Sardauscan.                                        *
 *                                                                          *
 *  Sardauscan is free software: you can redistribute it and/or modify      *
 *  it under the terms of the GNU General Public License as published by    *
 *  the Free Software Foundation, either version 3 of the License, or       *
 *  (at your option) any later version.                                     *
 *                                                                          *
 *  Sardauscan is distributed in the hope that it will be useful,           *
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of          *
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the           *
 *  GNU General Public License for more details.                            *
 *                                                                          *
 *  You are not allowed to Sell in any form this code                       * 
 *  or any compiled version. This code is free and for free purpose only    *
 *                                                                          *
 *  You should have received a copy of the GNU General Public License       *
 *  along with Sardaukar.  If not, see <http://www.gnu.org/licenses/>       *
 ****************************************************************************
*/
#region Based on Frelss Copyright Uriah Liggett <freelss@murobo.com >
/*
 ****************************************************************************
 *  Copyright (c) 2014 Uriah Liggett <freelss@murobo.com >                   *
 *	This file is part of FreeLSS.                                           *
 *                                                                          *
 *  FreeLSS is free software: you can redistribute it and/or modify         *
 *  it under the terms of the GNU General Public License as published by    *
 *  the Free Software Foundation, either version 3 of the License, or       *
 *  (at your option) any later version.                                     *
 *                                                                          *
 *  FreeLSS is distributed in the hope that it will be useful,              *
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of          *
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the           *
 *  GNU General Public License for more details.                            *
 *                                                                          *
 *   You should have received a copy of the GNU General Public License      *
 *   along with FreeLSS.  If not, see <http://www.gnu.org/licenses/>.       *
 ****************************************************************************
*/
#endregion
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using Sardauscan.Core.Interface;
using OpenTK;
using Sardauscan.Core.Geometry;

namespace Sardauscan.Core
{
	public class LocationMapper
	{
		public LocationMapper(Vector3 laserLocation, Vector3 cameraLocation, SizeF tableSize)
		{
			m_LaserPos = laserLocation;
			m_CameraPos = cameraLocation;
			TableSize = tableSize;

			ICameraProxy camera = Settings.Get<ICameraProxy>();
			m_Image = new Size(camera.ImageWidth, camera.ImageHeight);

			m_focalLength = camera.FocalLength;
			m_Sensor = new SizeF(camera.SensorWidth, camera.SensorHeight);

			m_LaserPlane = new Plane(new Vector3(), new Vector3());
			CalculateLaserPlane();

		}

		public SizeF TableSize { get; private set; }
		/** Lookup the 3D points for each pixel location */
		public Point3DList MapPoints(List<PixelLocation> laserLocations, Bitmap image, Color defColor)
		{
			float MAX_DIST_Y = TableSize.Height * 2;
			float MAX_DIST_XZ_SQ = (TableSize.Width / 2) * (TableSize.Width / 2);

			Point3DList points = new Point3DList(laserLocations.Count);

			int numIntersectionFails = 0;
			int numDistanceFails = 0;

			Ray ray;
			bool haveImage = image != null;

			// Initialize our output variable
			for (int iLoc = 0; iLoc < laserLocations.Count; iLoc++)
			{
				// Compute the back projection ray
				ray = CalculateCameraRay(laserLocations[iLoc]);

				// Intersect the laser plane and populate the XYZ
				Point3D point = new Point3D();
				if (IntersectLaserPlane(ray, ref point, laserLocations[iLoc]))
				{
					// The point must be above the turn table and less than the max distance from the center of the turn table
					float distXZSq = point.Position.X * point.Position.X + point.Position.Z * point.Position.Z;

					if (point.Position.Y >= 0.0 && distXZSq < MAX_DIST_XZ_SQ && point.Position.Y < MAX_DIST_Y)
					{
						// Set the color
						if (haveImage)
						{
							point.Color = image.GetPixel(Utils.ROUND(laserLocations[iLoc].X), Utils.ROUND(laserLocations[iLoc].Y));
						}
						else
							point.Color = defColor;

						// Make sure we have the correct laser location
						laserLocations[points.Count] = laserLocations[iLoc];
						points.Add(point);
					}
					else
					{
						numDistanceFails++;
					}
				}
				else
				{
					numIntersectionFails++;
				}
			}

			if (numIntersectionFails > 0)
			{
				Debug.WriteLine("!! " + numIntersectionFails + " laser plane intersection failures.");
			}

			if (numDistanceFails > 0)
			{
				Debug.WriteLine("!! " + numDistanceFails + " object bounds failures. ");
			}
			return points;
		}

		/** 
		 * Calculates a camera back projection Ray for the given image point.  The ray
		 * will begin at globalPt and extend out into the scene with a vector that would also
		 * take it through the camera focal point.
		 * @param globalPt - Point on the camera sensor in global coordinates
		 * @param ray - The output ray.
		 */
		private Ray CalculateCameraRay(PixelLocation imagePixel)
		{

			// Performance Note: Most of this could be pre-computed
			// and all the division could be removed.

			// and distance to camera

			// We subtract by one because the image is 0 indexed
			float x = imagePixel.X / (float)(m_Image.Width - 1);

			// Subtract the height so it goes from bottom to top
			float y = (m_Image.Height - imagePixel.Y) / (float)(m_Image.Height - 1);

			// The center of the sensor is at 0 in the X dimension
			x = (x * m_Sensor.Width) - (m_Sensor.Width * 0.5f) + m_CameraPos.X;
			y = (y * m_Sensor.Height) - (m_Sensor.Height * 0.5f) + m_CameraPos.Y;
			float z = m_CameraPos.Z - m_focalLength;

			Ray ray = new Ray(new Vector3(x, y, z), new Vector3(x - m_CameraPos.X, y - m_CameraPos.Y, z - m_CameraPos.Z));
			ray.Direction.Normalize();

			return ray;
		}

		/** Calculate where the ray will hit the laser plane and write it to @p point */
		private bool IntersectLaserPlane(Ray ray, ref Point3D point, PixelLocation pixel)
		{
			// Reference: http://www.scratchapixel.com/lessons/3d-basic-lessons/lesson-7-intersecting-simple-shapes/ray-plane-and-ray-disk-intersection/
			// d = ((p0 - l0) * n) / (l * n)

			// If dn is close to 0 then they don't intersect.  This should never happen
			float denominator = ray.Direction.Dot(m_LaserPlane.Normal);
			if (Math.Abs(denominator) < 0.000001)
			{
				Debug.WriteLine("!!! Ray never hits laser plane, pixel=" + pixel.X + ", " + pixel.Y + ", laserX=" + m_LaserPos.X + ", denom=" + denominator);
				return false;
			}

			Vector3 v;
			v.X = m_LaserPlane.Point.X - ray.Origin.X;
			v.Y = m_LaserPlane.Point.Y - ray.Origin.Y;
			v.Z = m_LaserPlane.Point.Z - ray.Origin.Z;

			float numerator = v.Dot(m_LaserPlane.Normal);

			// Compute the distance along the ray to the plane
			float d = numerator / denominator;
			if (d < 0)
			{
				// The ray is going away from the plane.  This should never happen.
				Debug.WriteLine("!!! Back projection ray is going the wrong direction!  Ray Origin = (" +
											 ray.Origin.X + "," + ray.Origin.Y + "," + ray.Origin.Z + ") Direction = (" +
											 ray.Direction.X + "," + ray.Direction.Y + "," + ray.Direction.Z + ")");
				return false;
			}

			// Extend the ray out this distance
			point.Position.X = ray.Origin.X + (ray.Direction.X * d);
			point.Position.Y = ray.Origin.Y + (ray.Direction.Y * d);
			point.Position.Z = ray.Origin.Z + (ray.Direction.Z * d);
			point.Normal.X = m_LaserPos.X - point.Position.X;
			point.Normal.Y = m_LaserPos.Y - point.Position.Y;
			point.Normal.Z = m_LaserPos.Z - point.Position.Z;

			point.Normal.Normalize();

			return true;
		}


		/** Calculate the plane equation for the plane that the laser is in */
		private void CalculateLaserPlane()
		{
			// The origin is a point in the plane
			m_LaserPlane.Point.X = 0;
			m_LaserPlane.Point.Y = 0;
			m_LaserPlane.Point.Z = 0;

			// (0, 0) to
			// Generate the plane normal
			// Reference: http://stackoverflow.com/questions/1243614/how-do-i-calculate-the-normal-vector-of-a-line-segment
			m_LaserPlane.Normal.X = m_LaserPos.Z;
			m_LaserPlane.Normal.Y = 0;
			m_LaserPlane.Normal.Z = -1 * m_LaserPos.X;
			m_LaserPlane.Normal.Normalize();
		}


		private Plane m_LaserPlane;
		private Vector3 m_LaserPos;

		private Vector3 m_CameraPos;

		private Size m_Image;
		private float m_focalLength;
		SizeF m_Sensor;

	}
}
