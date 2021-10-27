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
