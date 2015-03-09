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
using Sardauscan.Core.Geometry;
using System.IO;
using OpenTK;
using Sardauscan.Core;
using System.Globalization;

namespace Sardauscan.Core.IO
{
	/// <summary>
	/// STL file IO
	/// </summary>
	public class StlIO
	{
		/// <summary>
		/// Write ScanData (mesh) to STL
		/// </summary>
		/// <param name="file"></param>
		/// <param name="points"></param>
		public static void Write(string file, ScanData points)
		{
			/*
			solid
				 :
				 :
				 facet normal 0.0 0.0 1.0 
				 outer loop 
						vertex  1.0  1.0  0.0 
						vertex -1.0  1.0  0.0 
						vertex  0.0 -1.0  0.0 
				 endloop
				 endfacet
				 :
				 :
			endsolid
			*/

			using (StreamWriter w = System.IO.File.CreateText(file))
			{
				w.WriteLine("solid");
				for (int i = 0; i < points.Count; i++)
				{
					ScanLine slice = points[i];
					if (slice.IsMesh)
					{
						List<Triangle3D> tris = slice.GetTriangles();
						for (int t = 0; t < tris.Count; t++)
							WriteTriangle(w, tris[t]);
					}
				}
				w.WriteLine("endsolid");
			}
		}
		protected static void WriteTriangle(StreamWriter w, Triangle3D t)
		{
			w.WriteLine(string.Format("facet normal {0}", GetVectorSTR(t.Normal)));
			w.WriteLine("outer loop");
			w.WriteLine(string.Format(" vertex  {0} ", GetVectorSTR(t.Point1.Position)));
			w.WriteLine(string.Format(" vertex  {0} ", GetVectorSTR(t.Point2.Position)));
			w.WriteLine(string.Format(" vertex  {0} ", GetVectorSTR(t.Point3.Position)));
			w.WriteLine("endloop");
			w.WriteLine("endfacet");
		}

		public static string GetVectorSTR(Vector3 v)
		{
			return String.Format(CultureInfo.InvariantCulture, "{0} {1} {2}", v.X, -v.Z, v.Y);
		}

		public const string DefaultExtention = ".stl";
		public static string GetDialogFilter() { return String.Format("stl file(*{0})|*{0}", DefaultExtention); }

	}
}
