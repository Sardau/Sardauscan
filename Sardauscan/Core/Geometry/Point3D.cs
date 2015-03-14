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
using System.Drawing;
using Sardauscan.Core.Interface;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Sardauscan.Core.OpenGL;

namespace Sardauscan.Core.Geometry
{
	/// <summary>
	/// Point in 3D Space
	/// </summary>
	[DebuggerDisplay("{Position.X} {Position.Y} {Position.Z}")]
	public class Point3D : IComparable
	{
		/// <summary>
		/// Ctor
		/// </summary>
		public Point3D()
		{
		}
		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="normal"></param>
		/// <param name="color"></param>
		public Point3D(Vector3d pos, Vector3d normal, Color color)
			: this()
		{
			Position = pos;
			Normal = normal;
			Color = color;
		}
		/// <summary>
		/// Position of the Point in 3D
		/// </summary>
		public Vector3d Position;
		/// <summary>
		/// Normal to these point
		/// </summary>
		public Vector3d Normal;
		/// <summary>
		/// Color(texture) of this point
		/// </summary>
		public Color Color;
		/// <summary>
		/// Distance to another Point3D
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public double DistanceSquared(Point3D other)
		{
			double dx = Position.X - other.Position.X;
			double dy = Position.Y - other.Position.Y;
			double dz = Position.Z - other.Position.Z;
			return dx * dx + dy * dy + dz * dz;
		}
		/// <summary>
		/// Render this point to OpenGL
		/// </summary>
		/// <param name="context"></param>
		public void ToGL(ref RenderingContext context)
		{
			if (context.UseObjectColor)
				GL.Color3(Color);
			if (context.UseNormal)
				GL.Normal3(Normal);
			GL.Vertex3(Position);
		}
		/// <summary>
		/// Compare a point Y composant to another
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public int CompareTo(object obj)
		{
			Point3D o = (Point3D)obj;
			int c = -Position.Y.CompareTo(o.Position.Y);
			/* no realy needed we only sort to Y
									int c = Position.Y.CompareTo(o.Position.Y);
									 if (c != 0) return c;
									c = Position.Z.CompareTo(o.Position.Z);
									if (c != 0) return c;
									c = Position.X.CompareTo(o.Position.X);
									if (c != 0) return c;
									c = Normal.Z.CompareTo(o.Normal.Z);
									if (c != 0) return c;
									c = Normal.Y.CompareTo(o.Normal.Y);
									if (c != 0) return c;
									c = Normal.X.CompareTo(o.Normal.X);
									if (c != 0) return c;
									c = Color.ToArgb().CompareTo(o.Color.ToArgb());
			 */
			return c;
		}
		/// <summary>
		/// Average this point with another
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public Point3D Average(Point3D other)
		{
			Vector3d vp1 = this.Position;
			Vector3d vp2 = other.Position;

			Vector3d np1 = this.Normal;
			Vector3d np2 = other.Normal;

			Vector4 cp1 = this.Color.ToVector();
			Vector4 cp2 = other.Color.ToVector();

			vp1 = (vp2 + vp1) / 2f;
			np1 = (np2 + np1) / 2f;
			cp1 = (cp2 + cp1) / 2f;
			return new Point3D(vp1, np1, ColorExtension.ColorFromVector(cp1));
		}
		/// <summary>
		/// Average point3D List
		/// </summary>
		/// <param name="pts"></param>
		/// <returns></returns>
		public static Point3D Average(IList<Point3D> pts)
		{
			Vector3d pos = new Vector3d(0, 0, 0);
			Vector3d norm = new Vector3d(0, 0, 0);
			Vector4 col = new Vector4(0, 0, 0, 0);
			int count = pts.Count;
			for (int i = 0; i < count; i++)
			{
				pos += pts[i].Position;
				norm += pts[i].Normal;
				col += pts[i].Color.ToVector();
			}

			pos = pos / count;
			norm = (norm / count);
			norm.NormalizeFast();
			col = col / count;

			return new Point3D(pos, norm, ColorExtension.ColorFromVector(col));





		}
		/// <summary>
		/// Rotate this point around Y axe
		/// </summary>
		/// <param name="radian"></param>
		public void RotateAroundY(double radian)
		{
			Position.RotateAroundY(radian);
			Normal.RotateAroundY(radian);
		}

	}

	public static class Point3DExt
	{
		public static IList<Point3D> Copy(this IList<Point3D> pointsTarget)
		{

			List<Point3D> tempPoints = new List<Point3D>();


			for (int i = (pointsTarget.Count - 1); i >= 0; i--)
			{
				Point3D p = pointsTarget[i];
				tempPoints.Add(new Point3D(p.Position,p.Normal,p.Color));

			}
			return tempPoints;
		}
		private static Point3D CalculateCenterOfGravity(this IList<Point3D> vertices)
		{
			Point3D centerOfGravity = new Point3D();


			foreach (Point3D vr in vertices)
				centerOfGravity.Position += vr.Position;
			centerOfGravity.Position /= (double)vertices.Count;
			return centerOfGravity;
		}
		public static void ResetVerticesTo(this IList<Point3D> vertices, Point3D newOrigin)
		{
			//reset vertex so that it starts from 0,0,0
			for (int i = 0; i < vertices.Count; i++)
			{
				Vector3d v = vertices[i].Position;
				v.X -= newOrigin.Position.X;
				v.Y -= newOrigin.Position.Y;
				v.Z -= newOrigin.Position.Z;
				vertices[i].Position = v;

			}

		}
		public static void AddVector(this IList<Point3D> vertices, Point3D newOrigin)
		{
			//reset vertex so that it starts from 0,0,0
			for (int i = 0; i < vertices.Count; i++)
			{
				Vector3d v = vertices[i].Position;
				v.X += newOrigin.Position.X;
				v.Y += newOrigin.Position.Y;
				v.Z += newOrigin.Position.Z;
				vertices[i].Position = v;

			}

		}

		public static Point3D ResetToOrigin(this IList<Point3D> vertices)
		{
			Point3D newOrigin = vertices.CalculateCenterOfGravity();
			vertices.ResetVerticesTo(newOrigin);
			return newOrigin;

		}

		public static List<Vector3d> ConvertToVector3List(this IList<Point3D> listPoints)
		{
			List<Vector3d> listOfVectors = new List<Vector3d>();
			for (int i = 0; i < listPoints.Count; i++)
			{
				Point3D myPoint = listPoints[i];
				listOfVectors.Add(new Vector3d(myPoint.Position));
			}

			return listOfVectors;
		}
		public static void RemoveRange(this IList<Point3D> listPoints, int index, int count)
		{
			for (int i = 0; i < count; i++)
				listPoints.RemoveAt(index);

		}

		public static double CalculateTotalDistance(this IList<Point3D> a, IList<Point3D> b)
		{

			double totaldist = 0;
			for (int i = 0; i < Math.Min(a.Count,b.Count); i++)
			{
				Point3D p1 = a[i];
				Point3D p2 = b[i];
				double dist = (Vector3d.Subtract(p1.Position, p2.Position)).Length;

				totaldist += dist;

			}

			return (double)totaldist;
		}

	}
}
