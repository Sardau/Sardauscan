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
using System.IO;
using Sardauscan.Core.Geometry;
using Sardauscan.Core;

namespace Sardauscan.Core.IO
{
	/// <summary>
	/// XYZ File
	/// </summary>
	public class XYZIO
	{
		/// <summary>
		/// Write ScanData to XYZ File
		/// </summary>
		/// <param name="file"></param>
		/// <param name="points"></param>
		public static void Write(string file, ScanData points)
		{
			using (StreamWriter w = System.IO.File.CreateText(file))
			{
				for (int i = 0; i < points.Count; i++)
					Write(w, points[i]);
			}
		}
		private static void Write(StreamWriter w, Point3DList points)
		{
			for (int i = 0; i < points.Count; i++)
				Write(w, points[i]);
		}
		private static void Write(StreamWriter w, Point3D point)
		{
			//			string line = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8}", point.Position.X, point.Position.Y, point.Position.Z,
			//																																						point.Normal.X, point.Normal.Y, point.Normal.Z,
			//																																						point.Color.R, point.Color.G, point.Color.B);
			string line = point.Position.Dump();
			w.WriteLine(line);
		}
		public const string DefaultExtention = ".xyz";
		public static string GetDialogFilter() { return String.Format("xyz file(*{0})|*{0}", DefaultExtention); }

	}
}
