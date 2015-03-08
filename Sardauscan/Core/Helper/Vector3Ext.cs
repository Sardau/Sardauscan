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
using System.Globalization;

namespace Sardauscan.Core
{
	/// <summary>
	/// Vector3 Extention
	/// </summary>
	public static class Vector3Ext
	{
		/// <summary>
		/// Dot Product
		/// </summary>
		/// <param name="v"></param>
		/// <param name="a"></param>
		/// <returns></returns>
		public static float Dot(this Vector3 v, Vector3 a)
		{
			return v.X * a.X + v.Y * a.Y + v.Z * a.Z;
		}
		/// <summary>
		/// Is Vanid ( no NAN)
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static bool IsValid(this Vector3 v)
		{
			return v.X != float.NaN && v.Y != float.NaN && v.Z != float.NaN;
		}
		/// <summary>
		/// Dump as a string "X Y Z"
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static string Dump(this Vector3 v)
		{
			return String.Format(CultureInfo.InvariantCulture, "{0} {1} {2}", v.X, v.Y, v.Z);
		}
		/// <summary>
		/// Angle between 2 Vector in radian
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static float AngleInRadian(this Vector3 a, Vector3 b)
		{
			if (a.Equals(Vector3.Zero) || b.Equals(Vector3.Zero))
				return (float)(2 * Math.PI);

			Vector3 v = Vector3.Cross(a, b);
			double d1 = v.Length;//v.Norm();

			double d2 = Vector3.Dot(a, b);
			double angle = Math.Atan2(d1, d2);
			return (float)angle;
		}
		/// <summary>
		/// Angle between 2 vector in degree
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static float AngleInDegrees(this Vector3 a, Vector3 b)
		{
			return Utils.RADIANS_TO_DEGREES(a.AngleInRadian(b));
		}
		/// <summary>
		/// angle between 2 vector projected in XZ plane
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static float XZProjected_AngleInDegree(this Vector3 a, Vector3 b)
		{
			float cross = a.X * b.Z - a.Z * b.X;
			float dot = a.X * b.X + a.Z * b.Z;
			return Utils.RADIANS_TO_DEGREES((float)Math.Atan2(cross, dot));
		}

		/// <summary>
		/// Rotate a vector around Y Axe
		/// </summary>
		/// <param name="v"></param>
		/// <param name="radian"></param>
		public static void RotateAroundY(this Vector3 v, float radian)
		{
			float x = v.X;
			float z = v.Z;
			v.Z = (float)(z * Math.Cos(radian) - x * Math.Sin(radian));
			v.X = (float)(z * Math.Sin(radian) + x * Math.Cos(radian));
		}

	}
}
