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

		public static string GetVectorSTR(Vector3d v)
		{
			return String.Format(CultureInfo.InvariantCulture, "{0} {1} {2}", v.X, -v.Z, v.Y);
		}

		public const string DefaultExtention = ".stl";
		public static string GetDialogFilter() { return String.Format("stl file(*{0})|*{0}", DefaultExtention); }

	}
}
