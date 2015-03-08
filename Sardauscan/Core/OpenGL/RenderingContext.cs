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
using System.Drawing;

namespace Sardauscan.Core.OpenGL
{
	/// <summary>
	/// OpenGL rendering Context
	/// </summary>
	public struct RenderingContext
	{
		/// <summary>
		/// Use Object color
		/// </summary>
		public bool UseObjectColor;
		/// <summary>
		/// Use object Normals
		/// </summary>
		public bool UseNormal;
		/// <summary>
		/// Display WireFrame of object
		/// </summary>
		public bool Wireframe;
		/// <summary>
		/// Default material for Faces
		/// </summary>
		private GLMaterial DefaultFaceMaterial;
		/// <summary>
		/// Default material for lines
		/// </summary>
		private GLMaterial DefaultLineMaterial;
		/// <summary>
		/// Lightning is on
		/// </summary>
		public bool Lightning;
		/// <summary>
		/// Get Default
		/// </summary>
		/// <returns></returns>
		public static RenderingContext Default()
		{
			RenderingContext ret = new RenderingContext();
			ret.UseNormal = true;
			ret.UseObjectColor = true;
			ret.Wireframe = false;
			ret.DefaultFaceMaterial = GLMaterial.Default();
			ret.DefaultLineMaterial = GLMaterial.Default();
			ret.DefaultLineMaterial.Ambiant = Color.DarkGray;
			ret.Lightning = true;
			return ret;
		}
		/// <summary>
		/// Create RenderingContext From ViewerConfig
		/// </summary>
		/// <param name="viewerConfig"></param>
		/// <returns></returns>
		public static RenderingContext From(GLViewerConfig viewerConfig)
		{
			RenderingContext ret = RenderingContext.Default();

			ret.Lightning = viewerConfig.Lightning;
			ret.UseNormal = true;
			ret.Wireframe = viewerConfig.Wireframe;
			ret.UseObjectColor = viewerConfig.ShowSceneColor;
			ret.DefaultFaceMaterial = GLConfig.Material(viewerConfig.DefaultMaterial);
			ret.DefaultLineMaterial = GLConfig.Material("Black plastic");

			return ret;
		}

		public void ApplyLineDefault()
		{
			ApplyMaterial(this.DefaultLineMaterial);
		}
		public void ApplyFaceDefault()
		{
			ApplyMaterial(this.DefaultFaceMaterial);
		}
		public void ApplyMaterial(GLMaterial mat)
		{
			mat.ToGL(Lightning);
		}
	}
}
