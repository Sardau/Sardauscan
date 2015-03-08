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
using Sardauscan.Core;
using Sardauscan.Core.IO;
using OpenTK;
using System.Drawing;
using Sardauscan.Core.Geometry;

namespace Sardauscan.Core
{
	/// <summary>
	/// Utility class
	/// </summary>
	public static class Utils
	{
		/// <summary>
		/// Darian to degree
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		public static float RADIANS_TO_DEGREES(float r) { return (float)((r / (2.0 * System.Math.PI)) * 360.0); }
		/// <summary>
		/// Degree to Radian
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public static float DEGREES_TO_RADIANS(float d) { return (float)((d / 360.0) * (2.0 * System.Math.PI)); }
		/// <summary>
		/// Fast Round
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public static int ROUND(double d) { return ((int)(d + 0.5)); }
		/// <summary>
		/// Angle between 2 angle
		/// </summary>
		/// <param name="angle1"></param>
		/// <param name="angle2"></param>
		/// <returns></returns>
		public static float DeltaAngle(float angle1, float angle2)
		{
			float a = angle1 - angle2;
			a = (a + 180) % 360 - 180;
			return a;
		}


		/// <summary>
		/// Get median Point3D of a List of Point3D
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static Point3D GetMedian(List<Point3D> source)
		{
			if (source == null || source.Count == 0)
				return null;

			List<Vector3> pos = source.Select(p => p.Position).ToList();
			List<Vector3> norm = source.Select(p => p.Normal).ToList();
			List<Color> col = source.Select(p => p.Color).ToList();

			return new Point3D(GetMedian(pos), GetMedian(norm), GetMedian(col));
		}
		/// <summary>
		/// Get mediant Vector3
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static Vector3 GetMedian(List<Vector3> source)
		{
			if (source == null || source.Count == 0)
				return new Vector3();

			List<float> x = source.Select(vector => vector.X).ToList();
			List<float> y = source.Select(vector => vector.Y).ToList();
			List<float> z = source.Select(vector => vector.Z).ToList();

			return new Vector3(GetMedian(x), GetMedian(y), GetMedian(z));
		}
		/// <summary>
		/// Get median Color
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static Color GetMedian(List<Color> source)
		{

			List<byte> r = source.Select(col => col.R).ToList();
			List<byte> g = source.Select(col => col.G).ToList();
			List<byte> b = source.Select(col => col.B).ToList();
			List<byte> a = source.Select(col => col.A).ToList();

			return Color.FromArgb(GetMedian(a), GetMedian(r), GetMedian(g), GetMedian(b));
		}
		/// <summary>
		/// Templated Get Median
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sourceNumbers"></param>
		/// <returns></returns>
		public static T GetMedian<T>(List<T> sourceNumbers)
		{
			List<T> sortedPNumbers = new List<T>(sourceNumbers);
			sortedPNumbers.Sort();
			//get the median
			int size = sourceNumbers.Count;
			int mid = size / 2;
			return sortedPNumbers[mid];
		}
	}
}
