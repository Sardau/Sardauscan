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
using Sardauscan.Core;

namespace Sardauscan.Core.OpenGL
{
	/// <summary>
	/// OpenGL configuration
	/// </summary>
	public struct GLViewerConfig
	{
		/// <summary>
		/// Show Scene color
		/// </summary>
		public bool ShowSceneColor;
		/// <summary>
		/// Show boundingBox of scene
		/// </summary>
		public bool BoundingBox;
		/// <summary>
		/// Default object material
		/// </summary>
		public string DefaultMaterial;
		/// <summary>
		/// Scaner Table Radius
		/// </summary>
		public double TableRadius;
		/// <summary>
		/// Scaner Table Height
		/// </summary>
		public double TableHeight;
		/// <summary>
		/// Lignning enabled
		/// </summary>
		public bool Lightning;
		/// <summary>
		/// Face Smooth Enabled
		/// </summary>
		public bool Smooth;
		/// <summary>
		/// +Use perspective projection
		/// </summary>
		public bool Projection;
		/// <summary>
		/// Display WireFrame
		/// </summary>
		public bool Wireframe;
		/// <summary>
		/// Load Default
		/// </summary>
		public void LoadDefault()
		{
			ShowSceneColor = true;
			BoundingBox = false;
			DefaultMaterial = "Default";
			TableHeight = 150f;
			TableRadius = 100f;
			Lightning = true;
			Smooth = false;
			Wireframe = false;
			Settings settings = Settings.Get<Settings>();
			if (settings != null)
			{
				TableHeight = settings.Read(Settings.TABLE, Settings.HEIGHT, TableHeight);
				TableRadius = settings.Read(Settings.TABLE, Settings.DIAMETER, TableRadius * 2f) / 2f;
				ShowSceneColor = settings.Read(Settings.OPENGL, Settings.SHOWSCENECOLOR, ShowSceneColor);
				BoundingBox = settings.Read(Settings.OPENGL, Settings.SHOWBOUNDINGBOX, BoundingBox);
				DefaultMaterial = settings.Read(Settings.OPENGL, Settings.DEFAULTMATERIAL, DefaultMaterial);
				Lightning = settings.Read(Settings.OPENGL, Settings.LIGHTNING, Lightning);
				Smooth = settings.Read(Settings.OPENGL, Settings.SMOOTH, Smooth);
				Projection = settings.Read(Settings.OPENGL, Settings.PROJECTION, Projection);
				Wireframe = settings.Read(Settings.OPENGL, Settings.WIREFRAME, Wireframe);

			}
		}
		/// <summary>
		/// Save current as Default
		/// </summary>
		public void SaveDefault()
		{
			Settings settings = Settings.Get<Settings>();
			settings.Write(Settings.OPENGL, Settings.SHOWSCENECOLOR, ShowSceneColor);
			settings.Write(Settings.OPENGL, Settings.SHOWBOUNDINGBOX, BoundingBox);
			settings.Write(Settings.OPENGL, Settings.DEFAULTMATERIAL, DefaultMaterial);
			settings.Write(Settings.OPENGL, Settings.LIGHTNING, Lightning);
			settings.Write(Settings.OPENGL, Settings.SMOOTH, Smooth);
			settings.Write(Settings.OPENGL, Settings.PROJECTION, Projection);
			settings.Write(Settings.OPENGL, Settings.WIREFRAME, Wireframe);
		}



	}
}
