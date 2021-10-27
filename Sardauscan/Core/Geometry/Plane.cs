#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
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
		public Plane(Vector3d point, Vector3d normal)
		{
			Point = point;
			Normal = normal;
			Normal.Normalize();
		}

		/** The plane normal */
		public Vector3d Normal;

		/** A point in the plane */
		public Vector3d Point;

		/// <summary>
		/// Is the a Ray intersecting this plane
		/// </summary>
		/// <param name="inRay"></param>
		/// <param name="intersection"></param>
		/// <returns></returns>
		public bool Intersect(Ray inRay, out Vector3d intersection)
		{
			intersection = new Vector3d();
			// Reference: http://www.scratchapixel.com/lessons/3d-basic-lessons/lesson-7-intersecting-simple-shapes/ray-plane-and-ray-disk-intersection/
			// d = ((p0 - l0) * n) / (l * n)

			Ray ray = inRay;

			// Negate Z since this is a RH coordinate system
			ray.Direction.Z *= -1;

			// If dn is close to 0 then they don't intersect.  This should never happen
			double denominator = ray.Direction.Dot(Normal);
			if (Math.Abs(denominator) < 0.00001)
			{
				return false;
			}

			Vector3d v;
			v.X = Point.X - ray.Origin.X;
			v.Y = Point.Y - ray.Origin.Y;
			v.Z = -1 * (Point.Z - ray.Origin.Z);

			double numerator = v.Dot(Normal);

			// Compute the distance along the ray to the plane
			double d = numerator / denominator;
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
