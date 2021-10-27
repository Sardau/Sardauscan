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
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Sardauscan.Core.Geometry
{
	/// <summary>
	/// Triangle in 3D
	/// </summary>
    [DebuggerDisplay("{Point1} {Point2} {Point3} {Normal} {Color}")]
    public class Triangle3D
    {
			/// <summary>
			/// Ctor
			/// </summary>
			/// <param name="p1"></param>
			/// <param name="p2"></param>
			/// <param name="p3"></param>
        public Triangle3D(Point3D p1, Point3D p2, Point3D p3)
            :this(p1,p2,p3,CalculateNormal(p1,p2,p3),CalculateColor(p1, p2, p3))
        {
        }
			/// <summary>
			/// Ctor
			/// </summary>
			/// <param name="p1"></param>
			/// <param name="p2"></param>
			/// <param name="p3"></param>
			/// <param name="normal"></param>
        public Triangle3D(Point3D p1, Point3D p2, Point3D p3,Vector3d normal)
            : this(p1, p2, p3, normal, CalculateColor(p1, p2, p3))
        {
        }
			/// <summary>
			/// Ctor
			/// </summary>
			/// <param name="p1"></param>
			/// <param name="p2"></param>
			/// <param name="p3"></param>
			/// <param name="color"></param>
        public Triangle3D(Point3D p1, Point3D p2, Point3D p3, Color color)
            : this(p1, p2, p3, CalculateNormal(p1, p2, p3), color)
        {
        }

				/// <summary>
				/// Ctor
				/// </summary>
				/// <param name="p1"></param>
				/// <param name="p2"></param>
				/// <param name="p3"></param>
				/// <param name="color"></param>
				public Triangle3D(Point3D p1, Point3D p2, Point3D p3, Vector3d normal, Color color)
        {
            Point1 = p1;
            Point2 = p2;
            Point3 = p3;
            Normal = normal;
            this.Color = color;
        }

			 /// <summary>
			 /// Point 1 of the triangle
			 /// </summary>
        public readonly Point3D Point1;
				/// <summary>
				/// Point 2 of the triangle
				/// </summary>
				public readonly Point3D Point2;
				/// <summary>
				/// Point 3 of the triangle
				/// </summary>
				public readonly Point3D Point3;

				/// <summary>
				/// Normal of the triangle
				/// </summary>
				public Vector3d Normal;
				/// <summary>
				/// Color of the triangle
				/// </summary>
				public Color Color;

			/// <summary>
			/// Is is a valid Triangle
			/// </summary>
			/// <returns></returns>
        public bool Valid()
        {
            if (Point1.Position == Point2.Position || Point2.Position == Point3.Position || Point1.Position == Point3.Position)
                return false;
            return true;

        }
			/// <summary>
			/// Draw the Triangle to OpenGL
			/// </summary>
			/// <param name="useNormal"></param>
			/// <param name="useColor"></param>
        public void ToGL(bool useNormal, bool useColor)
        {
            if (useColor)
                GL.Color3(Color);
            if (useNormal)
                GL.Normal3(Normal);
            GL.Vertex3(Point1.Position);
            GL.Vertex3(Point2.Position);
            GL.Vertex3(Point3.Position);
        }
			/// <summary>
			/// Calculate a normal for 3 points
			/// </summary>
			/// <param name="a"></param>
			/// <param name="b"></param>
			/// <param name="c"></param>
			/// <returns></returns>
        public static Vector3d CalculateNormal(Point3D a, Point3D b, Point3D c)
        {
            return CalculateNormal(a.Position, b.Position, c.Position);
        }

				/// <summary>
				/// Calculate a normal for 3 points position
				/// </summary>
				/// <param name="a"></param>
				/// <param name="b"></param>
				/// <param name="c"></param>
				/// <returns></returns>
				public static Vector3d CalculateNormal(Vector3d a, Vector3d b, Vector3d c)
        {
            return Vector3d.Cross(b - a, c - a).Normalized();
        }
				/// <summary>
				/// Calculate a color based on 3 color (average)
				/// </summary>
				/// <param name="a"></param>
				/// <param name="b"></param>
				/// <param name="c"></param>
				/// <returns></returns>
				public static Color CalculateColor(Color c1, Color c2, Color c3)
        {
            var c = (c1.ToVector() + c2.ToVector() + c3.ToVector()) / 3f;
            return ColorExtension.ColorFromVector(c);
        }
				/// <summary>
				/// Calculate a color based on 3 Points (average)
				/// </summary>
				/// <param name="p1"></param>
				/// <param name="p2"></param>
				/// <param name="p3"></param>
				/// <returns></returns>
				public static Color CalculateColor(Point3D p1, Point3D p2, Point3D p3)
        {
            return CalculateColor(p1.Color, p2.Color, p3.Color);
        }

    }
}
