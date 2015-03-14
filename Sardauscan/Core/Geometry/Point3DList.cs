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
using OpenTK.Graphics.OpenGL;
using Sardauscan.Core.Interface;
using System.Drawing;
using System.Diagnostics;
using Sardauscan.Core.OpenGL;

namespace Sardauscan.Core.Geometry
{
	/// <summary>
	/// Base class for a list of Point3D
	/// </summary>
	[DebuggerDisplay("{DrawAs} {Count}")]
	public class Point3DList : DirtyAvareList<Point3D>, I3DEntity
	{
		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="capacity"></param>
		public Point3DList(int capacity = 0)
			: base(capacity)
		{
		}
		/// <summary>
		/// Add another List
		/// </summary>
		/// <param name="points"></param>
		public void Add(IList<Point3D> points)
		{
			this.AddRange(points);
		}
		/// <summary>
		/// OpenGL primitive to use
		/// </summary>
		public virtual PrimitiveType DrawAs { get { return PrimitiveType.Points; } }
		private Vector3d m_Min;
		/// <summary>
		/// Minimal X Y Z position
		/// </summary>
		public Vector3d Min
		{
			get
			{
				if (Dirty)
					Update();
				return m_Min;
			}
		}
		private Vector3d m_Max;
		/// <summary>
		/// Maximal X Y Z position
		/// </summary>
		public Vector3d Max
		{
			get
			{
				if (Dirty)
					Update();
				return m_Max;
			}
		}
		/// <summary>
		/// Update the min-max
		/// </summary>
		public override void Update()
		{
			if (Count > 0)
			{
				Vector3d v = this[0].Position;
				m_Min = new Vector3d(v);
				m_Max = new Vector3d(v);
				for (int i = 1; i < Count; i++)
				{
					v = this[i].Position;
					m_Min.X = Math.Min(m_Min.X, v.X);
					m_Min.Y = Math.Min(m_Min.Y, v.Y);
					m_Min.Z = Math.Min(m_Min.Z, v.Z);
					m_Max.X = Math.Max(m_Max.X, v.X);
					m_Max.Y = Math.Max(m_Max.Y, v.Y);
					m_Max.Z = Math.Max(m_Max.Z, v.Z);
				}
			}
			else
			{
				m_Min = new Vector3d(-0.5f, -0.5f, -0.5f);
				m_Max = new Vector3d(0.5f, 0.5f, 0.5f);
			}
			base.Update();
		}

		bool useGLArray = false;
		/// <summary>
		/// Render the Points to OpenGL
		/// </summary>
		/// <param name="context"></param>
		public virtual void Render(ref RenderingContext context)
		{
			if (Dirty)
				Update();
			if (useGLArray)
			{
				int len = Count;
				Vector3d[] pos = new Vector3d[len];
				Vector3d[] nor = new Vector3d[context.UseNormal ? len : 0];
				int[] col = new int[context.UseObjectColor ? len : 0];
				for (int i = 0; i < Count; i++)
				{
					Point3D v = this[i];
					pos[i].X = v.Position.X;
					pos[i].Y = v.Position.Y;
					pos[i].Z = v.Position.Z;
					if (context.UseNormal)
						nor[i] = v.Normal;
					if (context.UseObjectColor)
						col[i] = v.Color.ToGLRgba32();
				}
				int posH = -1; ;
				int pos_Size = Vector3d.SizeInBytes;
				int norH = -1;
				int nor_Size = Vector3d.SizeInBytes;
				int colH = -1;
				int col_Size = sizeof(int);

				GL.GenBuffers(1, out posH);
				GL.BindBuffer(BufferTarget.ArrayBuffer, posH);
				GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(len * pos_Size), pos, BufferUsageHint.StaticDraw);
				GL.BindBuffer(BufferTarget.ArrayBuffer, posH);
				GL.VertexPointer(3, VertexPointerType.Double, pos_Size, IntPtr.Zero);
				GL.EnableClientState(ArrayCap.VertexArray);


				if (context.UseNormal)
				{
					GL.GenBuffers(1, out norH);
					GL.BindBuffer(BufferTarget.ArrayBuffer, norH);
					GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(len * nor_Size), nor, BufferUsageHint.StaticDraw);
					GL.BindBuffer(BufferTarget.ArrayBuffer, norH);
                    GL.NormalPointer(NormalPointerType.Double , pos_Size, IntPtr.Zero);
					GL.EnableClientState(ArrayCap.NormalArray);
				}

				if (context.UseObjectColor)
				{
					GL.GenBuffers(1, out colH);
					GL.BindBuffer(BufferTarget.ArrayBuffer, colH);
					GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(len * col_Size), col, BufferUsageHint.StaticDraw);
					GL.BindBuffer(BufferTarget.ArrayBuffer, colH);
					GL.ColorPointer(4, ColorPointerType.UnsignedByte, sizeof(int), IntPtr.Zero);
					GL.EnableClientState(ArrayCap.ColorArray);
				}

				GL.DrawArrays(DrawAs, 0, len);


				GL.DisableClientState(ArrayCap.VertexArray);
				if (context.UseNormal)
					GL.DisableClientState(ArrayCap.NormalArray);
				if (context.UseObjectColor)
					GL.DisableClientState(ArrayCap.ColorArray);

				if (posH != -1)
					GL.DeleteBuffers(1, ref posH);
				if (context.UseNormal && norH != -1)
					GL.DeleteBuffers(1, ref norH);
				if (context.UseObjectColor && colH != -1)
					GL.DeleteBuffers(1, ref colH);
			}
			else
			{
				GL.Begin(this.DrawAs);
				if (this.DrawAs == PrimitiveType.TriangleStrip)
				{
					for (int i = 0; i < Count; i++)
					{
						Point3D p = this[i];
						if (context.UseNormal && i < Count - 2)
							GL.Normal3(p.Normal);
						p.ToGL(ref context);
					}
				}
				else
				{
					for (int i = 0; i < Count; i++)
						this[i].ToGL(ref context);
				}
				GL.End();
#if false
                Vector4d currentColor = new Vector4d();
                GL.Getdouble(GetPName.CurrentColor, out currentColor);
                GL.Begin(PrimitiveType.Lines);
                GL.Color3(Color.Red);
                for (int i = 0; i < Count; i++)
                {
                    Point3D p = this[i];
                    GL.Vertex3(p.Position);
                    GL.Vertex3(p.Position + p.Normal);
                }
                GL.End();
                GL.Color4(currentColor);

#endif
			}
		}

		/// <summary>
		/// Create a Point3dList from Vector3d List and a color
		/// </summary>
		/// <param name="vertices"></param>
		/// <param name="color"></param>
		/// <returns></returns>
		public static Point3DList FromVertices(List<Vector3d> vertices, Color color)
		{
			Point3DList ret = new Point3DList();
			for (int i = 0; i < vertices.Count; i++)
				ret.Add(new Point3D(vertices[i], new Vector3d(0, 1, 0), color));
			return ret;
		}

		/// <summary>
		/// Get a interpolated list of this list composed od "count" point
		/// </summary>
		/// <param name="count"></param>
		/// <returns></returns>
		public Point3DList GetInterpolatedList(int count)
		{
			Point3DList ret = new Point3DList(count);
			if (count == 0)
				return ret;
			else if (count == Count)
			{
				ret.AddRange(this);
			}
			else
			{
				double max = Max.Y;
				double d = (max - Min.Y) / count;
				for (int i = 0; i < count; i++)
				{
					Point3D p = GetInterpolateByY(max - i * d, i == 0);
					ret.Add(p);
				}
			}
			return ret;
		}
		/// <summary>
		/// Get a interpolated list of this list composed od "count" point based on the Y position of another list of points
		/// </summary>
		/// <param name="count"></param>
		/// <returns></returns>
		public Point3DList GetYInterpolated(Point3DList reflist)
		{
			int count = reflist.Count;
			Point3DList ret = new Point3DList(count);
			if (count == 0)
				return ret;

			for (int i = 0; i < count; i++)
			{
				double y = reflist[i].Position.Y;
				Point3D p = GetInterpolateByY(reflist[i].Position.Y, i == 0);

				ret.Add(p);
			}

			return ret;


		}

		/// <summary>
		/// Get the point with the Closest Y position 
		/// </summary>
		/// <param name="y"></param>
		/// <returns></returns>
		public Point3D ClosestY(double y)
		{
			return this.OrderBy(n => Math.Abs(y - n.Position.Y)).First();
		}
		/// <summary>
		/// Get the point with the Closest Y position 
		/// </summary>
		/// <param name="y"></param>
		/// <returns></returns>
		public Point3D GetNearestY(double y, bool sort = false)
		{
			if (Count == 0)
				return null;
			if (sort)
				this.Sort();
			if (y < this.Min.Y)
				return this.Last();
			if (y > this.Max.Y)
				return this.First();

			int range = 0;
			for (int i = 0; i < Count - 1; i++)
			{
				double y0 = this[i].Position.Y;
				double y1 = this[i + 1].Position.Y;

				if (y0 == y)
					return this[i];
				else if (y1 == y)
					return this[i + 1];
				else if (y0 > y && y1 < y)
					break;
				else
					range++;
			}

			if (range >= Count - 1)
				return this.Last();

			Point3D pt1 = this[range];
			Point3D pt2 = this[range + 1];
			double d1 = y - pt1.Position.Y;
			double d = pt2.Position.Y - pt1.Position.Y;
			double clamp = d1 / (d);
			if (clamp > 0.5)
				return pt1;
			return pt2;
		}



		/// <summary>
		/// Get a interpolated list of this list composed od "count" point based on the Y position of points
		/// </summary>
		/// <param name="count"></param>
		/// <returns></returns>
		public Point3D GetInterpolateByY(double y, bool sort = false)
		{
			if (Count == 0)
				return null;
			if (Count == 1)
				return this[0];
			if (sort)
				this.Sort();
			Point3D first = this[0];
			if (y >= first.Position.Y)
				return first;
			Point3D last = this[Count - 1];
			if (y <= last.Position.Y)
				return last;

			for (int i = 0; i < Count - 1; i++)
			{
				Point3D p1 = this[i];
				if (p1.Position.Y == y)
					return p1;
				Point3D p2 = this[i + 1];
				if (p1.Position.Y > y && p2.Position.Y <= y)
					return InterpolateByY(p1, p2, y);
			}
			return InterpolateByY(first, last, y);// can't go here.
		}
		/// <summary>
		/// Interpolate by y 2 Point3D
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static Point3D InterpolateByY(Point3D start, Point3D end, double y)
		{
			double delta = start.Position.Y - end.Position.Y;
			double clamp = (start.Position.Y - y) / delta;
			return Interpolate(start, end, clamp);
		}
		/// <summary>
		/// Interpolate 2 Point3D baser on a [0-1] range
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="clamp"></param>
		/// <returns></returns>

		public static Point3D Interpolate(Point3D start, Point3D end, double clamp)
		{
			Point3D ret = new Point3D();

			ret.Position = Interpolate(start.Position, end.Position, clamp);
			ret.Normal = Interpolate(start.Normal, end.Normal, clamp);
			ret.Color = start.Color.GetStepColor(end.Color, clamp);

			return ret;
		}
		/// <summary>
		/// Interpolate 2 Vector3D on a [0-1] range
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="clamp"></param>
		/// <returns></returns>
		public static Vector3d Interpolate(Vector3d start, Vector3d end, double clamp)
		{
			return new Vector3d(
					Interpolate(start.X, end.X, clamp),
					Interpolate(start.Y, end.Y, clamp),
					Interpolate(start.Z, end.Z, clamp)
					);
		}

		/// <summary>
		/// Interpolate 2 double on a [0-1] range
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="clamp"></param>
		/// <returns></returns>
		public static double Interpolate(double start, double end, double clamp)
		{
			double t = Math.Min(Math.Max(clamp, 0), 1);
			double d = end - start;
			return start + d * t;
		}
		/// <summary>
		/// Is these point list a mesh 
		/// </summary>
		public bool IsMesh
		{
			get
			{
				return
						DrawAs == PrimitiveType.TriangleFan ||
						DrawAs == PrimitiveType.TriangleStrip ||
						DrawAs == PrimitiveType.Triangles
						;
			}
		}
		/// <summary>
		/// Get the list of Triangles in this
		/// </summary>
		/// <returns></returns>
		public List<Triangle3D> GetTriangles()
		{
			int count = Count;
			List<Triangle3D> ret = new List<Triangle3D>(count);
			if (count < 3)
				return ret;
			switch (DrawAs)
			{
				case PrimitiveType.Triangles:
					{
						for (int i = 0; i < count; i += 3)
							ret.Add(new Triangle3D(this[i], this[i + 1], this[i + 2]));
						break;
					}
				case PrimitiveType.TriangleFan:
					{
						for (int i = 1; i < count; i++)
							ret.Add(new Triangle3D(this[0], this[i], this[i + 1]));
						break;
					}
				case PrimitiveType.TriangleStrip:
					{
						for (int i = 0; i < count -2; i++)
							if (i % 2 == 0) // http://en.wikipedia.org/wiki/Triangle_strip => order point in ccw as some fileformat does not take care of normals
								ret.Add(new Triangle3D(this[i], this[i+1], this[i+2]));
							else
								ret.Add(new Triangle3D(this[i+1], this[i], this[i+2]));
						break;
					}
			}
			return ret;
		}

		public void Transform(Matrix4d mat)
		{
			double[,] matrixdouble = MathUtils.GetTransformMatrixAsArray(mat);
			for (int i = 0; i < Count; i++)
			{
				Point3D p1 = this[i];

				p1.Position = MathUtils.TransformPoint(p1.Position, matrixdouble);
				p1.Normal = MathUtils.TransformPoint(p1.Normal, matrixdouble);
			}
		}
	}
}
