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
