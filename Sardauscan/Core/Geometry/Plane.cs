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
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using System.Diagnostics;

namespace Sardauscan.Core.Geometry
{
	/// <summary>
	/// Class describing a Plane
	/// </summary>
	public class Plane
	{
		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="point"></param>
		/// <param name="normal"></param>
		public Plane(Vector3 point, Vector3 normal)
		{
			Point = point;
			Normal = normal;
			Normal.Normalize();
		}

		/** The plane normal */
		public Vector3 Normal;

		/** A point in the plane */
		public Vector3 Point;

		/// <summary>
		/// Is the a Ray intersecting this plane
		/// </summary>
		/// <param name="inRay"></param>
		/// <param name="intersection"></param>
		/// <returns></returns>
		public bool Intersect(Ray inRay, out Vector3 intersection)
		{
			intersection = new Vector3();
			// Reference: http://www.scratchapixel.com/lessons/3d-basic-lessons/lesson-7-intersecting-simple-shapes/ray-plane-and-ray-disk-intersection/
			// d = ((p0 - l0) * n) / (l * n)

			Ray ray = inRay;

			// Negate Z since this is a RH coordinate system
			ray.Direction.Z *= -1;

			// If dn is close to 0 then they don't intersect.  This should never happen
			float denominator = ray.Direction.Dot(Normal);
			if (Math.Abs(denominator) < 0.00001)
			{
				return false;
			}

			Vector3 v;
			v.X = Point.X - ray.Origin.X;
			v.Y = Point.Y - ray.Origin.Y;
			v.Z = -1 * (Point.Z - ray.Origin.Z);

			float numerator = v.Dot(Normal);

			// Compute the distance along the ray to the plane
			float d = numerator / denominator;
			if (d < 0)
			{
				return false;
			}

			// Extend the ray out this distance
			intersection.X = inRay.Origin.X + (inRay.Direction.X * d);
			intersection.Y = inRay.Origin.Y + (inRay.Direction.Y * d);
			intersection.Z = inRay.Origin.Z + (inRay.Direction.Z * d);
			return true;
		}
	};
}
