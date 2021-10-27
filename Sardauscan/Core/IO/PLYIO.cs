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
using Sardauscan.Core;
using System.IO;
using Sardauscan.Core.Geometry;
using System.ComponentModel;

namespace Sardauscan.Core.IO
{
	/// <summary>
	/// PLY IO
	/// </summary>
	public class PLYIO
	{
		/// <summary>
		/// Write ScanData To PLY file
		/// </summary>
		/// <param name="file"></param>
		/// <param name="points"></param>
		public static void Write(string file, ScanData points)
		{
			using (StreamWriter w = System.IO.File.CreateText(file))
			{
				w.WriteLine("ply");
				w.WriteLine("format ascii 1.0");
				w.WriteLine(string.Format("element vertex {0}", points.PointCount()));  // Leave space for updating the vertex count
				w.WriteLine("property double x");
				w.WriteLine("property double y");
				w.WriteLine("property double z");
				w.WriteLine("property double nx");
				w.WriteLine("property double ny");
				w.WriteLine("property double nz");
				w.WriteLine("property uchar red");
				w.WriteLine("property uchar green");
				w.WriteLine("property uchar blue");
				w.WriteLine("element face 0");
				w.WriteLine("property list uchar int vertex_indices");
				w.WriteLine("end_header");
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
			string line = string.Format("{0} {1} {2} {3} {4}", point.Position.Dump(),
																																						point.Normal.Dump(),
																																						point.Color.R, point.Color.G, point.Color.B);
			w.WriteLine(line);
		}
		public const string DefaultExtention = ".ply";
		public static string GetDialogFilter() { return String.Format("ply file(*{0})|*{0}", DefaultExtention); }
	}
}
