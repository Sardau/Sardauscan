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
using OpenTK.Graphics.OpenGL;
using Sardauscan.Core.Geometry;
using System.Drawing;
using Sardauscan.Core.OpenGL;

namespace Sardauscan.Core
{
	/// <summary>
	/// Object repesenting a Slice of a mesh
	/// </summary>
    public class ScanSlice : ScanLine
    {
			/// <summary>
			/// Ctor
			/// </summary>
			/// <param name="capacity"></param>
         public ScanSlice(int capacity=0)
            : base(-1,capacity)
        {
        }
			/// <summary>
			/// Ctor
			/// </summary>
			/// <param name="points"></param>
        public ScanSlice(Point3DList points)
            : base(-1,points)
        {
        }

        protected PrimitiveType m_Primitive = PrimitiveType.TriangleStrip;
			/// <summary>
			/// Primitive to use
			/// </summary>
        public override PrimitiveType DrawAs { get { return m_Primitive; } }

			/// <summary>
			/// Render this Slice to OpenGL
			/// </summary>
			/// <param name="context"></param>
        public override void Render(ref RenderingContext context)
        {
					if (context.Wireframe)
					{
						GL.PolygonOffset(-1, -1);
						RenderingContext linecontext = context;
						linecontext.Lightning = false;
						linecontext.UseObjectColor = context.UseObjectColor;
						RenderAsWireFrame(ref linecontext);
					}
					GL.PolygonOffset(0, 0);
					RenderAsFace(ref context);
				}
			/// <summary>
			/// Render thes Slice WireFrame
			/// </summary>
			/// <param name="context"></param>
        protected void RenderAsWireFrame(ref RenderingContext context)
        {
            PrimitiveType oldPrim = m_Primitive;
            m_Primitive = PrimitiveType.LineStrip;
            context.ApplyLineDefault();
	           base.Render(ref context);

            ScanLine l1 = new ScanLine(-1, Count / 2);
            ScanLine l2 = new ScanLine(-1, Count / 2);
            for (int i = 0; i < Count; i++)
                if (i % 2 == 0)
                    l1.Add(this[i]);
                else
                    l2.Add(this[i]);
            l1.Render(ref context);
            l2.Render(ref context);

            m_Primitive = oldPrim;
        }
			/// <summary>
			/// Render this slice Faces
			/// </summary>
			/// <param name="context"></param>
        protected void RenderAsFace(ref RenderingContext context)
        {
            PrimitiveType oldPrim = m_Primitive;
            context.ApplyFaceDefault();
            m_Primitive = PrimitiveType.TriangleStrip;
            base.Render(ref context);
            m_Primitive=oldPrim;
        }
			/// <summary>
			/// this "ScanLine" is part of mesh: it is composed of Face
			/// </summary>
        public override bool IsFaces {get{   return true;  }}
    }
}
