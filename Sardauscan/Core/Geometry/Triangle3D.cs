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
        public Triangle3D(Point3D p1, Point3D p2, Point3D p3,Vector3 normal)
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
				public Triangle3D(Point3D p1, Point3D p2, Point3D p3, Vector3 normal, Color color)
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
				public Vector3 Normal;
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
        public static Vector3 CalculateNormal(Point3D a, Point3D b, Point3D c)
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
				public static Vector3 CalculateNormal(Vector3 a, Vector3 b, Vector3 c)
        {
            return Vector3.Cross(b - a, c - a).Normalized();
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
