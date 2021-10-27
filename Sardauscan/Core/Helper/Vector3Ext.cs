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
using System.Globalization;

namespace Sardauscan.Core
{
	/// <summary>
	/// Vector3d Extention
	/// </summary>
	public static class Vector3Ext
	{
		/// <summary>
		/// Dot Product
		/// </summary>
		/// <param name="v"></param>
		/// <param name="a"></param>
		/// <returns></returns>
		public static double Dot(this Vector3d v, Vector3d a)
		{
			return v.X * a.X + v.Y * a.Y + v.Z * a.Z;
		}
		/// <summary>
		/// Is Vanid ( no NAN)
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static bool IsValid(this Vector3d v)
		{
			return v.X != double.NaN && v.Y != double.NaN && v.Z != double.NaN;
		}
		/// <summary>
		/// Dump as a string "X Y Z"
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static string Dump(this Vector3d v)
		{
			return String.Format(CultureInfo.InvariantCulture, "{0} {1} {2}", v.X, v.Y, v.Z);
		}
		/// <summary>
		/// Angle between 2 Vector in radian
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static double AngleInRadian(this Vector3d a, Vector3d b)
		{
			if (a.Equals(Vector3d.Zero) || b.Equals(Vector3d.Zero))
				return (double)(2 * Math.PI);

			Vector3d v = Vector3d.Cross(a, b);
			double d1 = v.Length;//v.Norm();

			double d2 = Vector3d.Dot(a, b);
			double angle = Math.Atan2(d1, d2);
			return (double)angle;
		}
		/// <summary>
		/// Angle between 2 vector in degree
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static double AngleInDegrees(this Vector3d a, Vector3d b)
		{
			return Utils.RADIANS_TO_DEGREES(a.AngleInRadian(b));
		}
		/// <summary>
		/// angle between 2 vector projected in XZ plane
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static double XZProjected_AngleInDegree(this Vector3d a, Vector3d b)
		{
			double cross = a.X * b.Z - a.Z * b.X;
			double dot = a.X * b.X + a.Z * b.Z;
			return Utils.RADIANS_TO_DEGREES((double)Math.Atan2(cross, dot));
		}

		/// <summary>
		/// Rotate a vector around Y Axe
		/// </summary>
		/// <param name="v"></param>
		/// <param name="radian"></param>
		public static void RotateAroundY(this Vector3d v, double radian)
		{
			double x = v.X;
			double z = v.Z;
			v.Z = (double)(z * Math.Cos(radian) - x * Math.Sin(radian));
			v.X = (double)(z * Math.Sin(radian) + x * Math.Cos(radian));
		}

	}
}
