using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Sardauscan.Core.Geometry;
using OpenTK;
using System.Windows.Forms;

namespace Sardauscan.Core.IO
{

	/// <summary>
	/// OBJ IO
	/// </summary>
	public class WaveFormIO
	{
		struct FaceInfo
		{
			public long v1;
			public long v2;
			public long v3;
		}
		public static void Write(string file, ScanData data)
		{
			using (StreamWriter w = System.IO.File.CreateText(file))
			{
				w.WriteLine(string.Format("# Create By Sardauscan V{0}",Application.ProductVersion));
				w.WriteLine("#");
				w.WriteLine("# Object");
				List<FaceInfo> faces = new List<FaceInfo>();
				List<Vector3> vertex = new List<Vector3>();
				for (int i = 0; i < data.Count; i++) // collect vertices and face
				{
					ScanLine slice = data[i];
					if (slice.IsMesh)
					{
						List<Triangle3D> tris = slice.GetTriangles();
						for (int t = 0; t < tris.Count; t++)
						{
							Triangle3D triangle = tris[t];
							FaceInfo face = new FaceInfo();
							face.v1 = AddToList(vertex, triangle.Point1.Position);
							face.v2 = AddToList(vertex, triangle.Point2.Position);
							face.v3 = AddToList(vertex, triangle.Point3.Position);
							faces.Add(face);
						}
					}
				}
				w.WriteLine(string.Format("# Vertices {0}", vertex.Count));
				for (int i = 0; i < vertex.Count; i++)
					w.WriteLine(string.Format("v {0}",vertex[i].Dump()));
				w.WriteLine(string.Format("# Triangles {0}", faces.Count));
				for (int i = 0; i < faces.Count; i++)
					w.WriteLine(string.Format("f {0} {1} {2}", faces[i].v1 + 1, faces[i].v2 + 1, faces[i].v3 + 1));
			}
		}


		private static long AddToList(List<Vector3> list, Vector3 vector)
		{
			Vector3 val = new Vector3(vector.X, -vector.Z, vector.Y);
			int ret = list.IndexOf(val);
			if (ret < 0)
			{
				list.Add(val);
				return list.Count - 1;
			}
			return ret;
		}
		public const string DefaultExtention = ".obj";
		public static string GetDialogFilter() { return String.Format("Wave form Obj file(*{0})|*{0}", DefaultExtention); }

	}
}
