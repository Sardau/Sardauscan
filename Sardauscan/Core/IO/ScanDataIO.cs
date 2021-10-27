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
using OpenTK;
using System.Drawing;
using Sardauscan.Core.Geometry;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using Sardauscan.Core;
using System.Globalization;

namespace Sardauscan.Core.IO
{
	/// <summary>
	/// Class for PropertyGrid open file
	/// </summary>
	class Point3dArrayIOOpenFileEditor : FileNameEditor
	{
		///<summary>
		///Initializes the open file dialog when it is created.
		///</summary>
		///
		///<param name="openFileDialog">The <see cref="T:System.Windows.Forms.OpenFileDialog"></see> to use to select a file name. </param>
		protected override void InitializeDialog(OpenFileDialog openFileDialog)
		{
			openFileDialog.Title = "Open";
			openFileDialog.CheckFileExists = true;
			openFileDialog.Filter = ScanDataIO.GetDialogFilter();
		}
	}
	/// <summary>
	/// Class for PropertyGrid Save file
	/// </summary>
	class Point3dArrayIOSaveFileEditor : FileNameEditor
	{
		///<summary>
		///Initializes the open file dialog when it is created.
		///</summary>
		///
		///<param name="openFileDialog">The <see cref="T:System.Windows.Forms.OpenFileDialog"></see> to use to select a file name. </param>
		protected override void InitializeDialog(OpenFileDialog openFileDialog)
		{
			openFileDialog.Title = "Save";
			openFileDialog.CheckFileExists = false;
			openFileDialog.Filter = ScanDataIO.GetDialogFilter();
		}
	}
	/// <summary>
	/// ScanData IO
	/// </summary>
	public class ScanDataIO
	{
		/// <summary>
		/// Write ScanData File
		/// </summary>
		/// <param name="file"></param>
		/// <param name="points"></param>
		public static void Write(string file, ScanData points)
		{
			using (StreamWriter w = System.IO.File.CreateText(file))
			{
				w.WriteLine(string.Format("slice count:{0}", points.Count));
				for (int i = 0; i < points.Count; i++)
				{
					ScanLine slice = points[i];
					w.WriteLine(string.Format("data:{0}", slice.LaserID.ToString()));
					w.WriteLine(string.Format("DrawAs:{0}", slice.DrawAs));
					w.WriteLine(string.Format("vertice count:{0}", slice.Count));
					for (int j = 0; j < slice.Count; j++)
					{
						Point3D p = slice[j];
						w.Write(string.Format("{0}|", p.Position.Dump()));
						w.Write(string.Format("{0}|", p.Normal.Dump()));
						w.WriteLine(string.Format("{0}", System.Drawing.ColorTranslator.ToHtml(p.Color)));
					}
				}
			}
		}
		/// <summary>
		/// Read ScanData File
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static ScanData Read(string file)
		{
			ScanData ret = new ScanData();
			using (StreamReader r = System.IO.File.OpenText(file))
			{
				string line = r.ReadLine();
				string[] part = line.Split(":".ToArray());
				int slicecount = int.Parse(part[1]);
				for (int i = 0; i < slicecount; i++)
				{
					line = r.ReadLine();
					part = line.Split(":".ToArray());
					ScanLine slice = new ScanLine((int)double.Parse(part[1]));

					line = r.ReadLine();
					part = line.Split(":".ToArray());
					if (part[0] == "DrawAs")
					{
						OpenTK.Graphics.OpenGL.PrimitiveType primitive = OpenTK.Graphics.OpenGL.PrimitiveType.Points;
						Enum.TryParse<OpenTK.Graphics.OpenGL.PrimitiveType>(part[1], true, out primitive);
						switch (primitive)
						{
							case OpenTK.Graphics.OpenGL.PrimitiveType.TriangleStrip:
								{
									slice = new ScanSlice(10000);
									break;
								}
							case OpenTK.Graphics.OpenGL.PrimitiveType.LineStrip:
								{
									slice.DisplayAsLine = true;
									break;
								}
							default:
								{
									slice.DisplayAsLine = false;
									break;
								}
						}
						line = r.ReadLine();
						part = line.Split(":".ToArray());
					}
					int pointcount = int.Parse(part[1]);

					for (int j = 0; j < pointcount; j++)
					{
						line = r.ReadLine();
						part = line.Split("|".ToArray());

						Vector3d pos = GetVector(part[0]);
						Vector3d normal = pos.Normalized();
						try
						{
							normal = GetVector(part[1]);
						}
						catch { }
						Color color = System.Drawing.ColorTranslator.FromHtml(part[2]);
						Point3D p = new Point3D(pos, normal, color);
						slice.Add(p);
					}
					ret.Add(slice);
				}
			}
			return ret;
		}
		private static Vector3d GetVector(string vectorStr)
		{
			string[] part = vectorStr.Split(" ".ToArray());
			Vector3d ret = new Vector3d();
            double x = double.NaN;
            double y = double.NaN;
            double z = double.NaN;
            if (double.TryParse(part[0], NumberStyles.Any, CultureInfo.InvariantCulture, out x) && 
                double.TryParse(part[1], NumberStyles.Any, CultureInfo.InvariantCulture, out y) && 
                double.TryParse(part[2], NumberStyles.Any, CultureInfo.InvariantCulture, out z))
			{
                if( !double.IsNaN(x) &&
                    !double.IsNaN(x) &&
                    !double.IsNaN(x)
                  )
                {
                    ret.X = x;
                    ret.Y = y;
                    ret.Z = z;
                }
			}
			return ret;
		}

		public const string DefaultExtention = ".Sar";
		public static string GetDialogFilter() { return String.Format("Sardauscan File(*{0})|*{0}", DefaultExtention); }
	}
}
