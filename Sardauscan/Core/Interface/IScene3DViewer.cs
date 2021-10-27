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
using Sardauscan.Core;
using Sardauscan.Gui;
using Sardauscan.Core.Geometry;
using Sardauscan.Gui.OpenGL;

namespace Sardauscan.Core.Interface
{
	/// <summary>
	/// Interface for a Scene3D viewer
	/// </summary>
	public interface IScene3DViewer
	{
		/// <summary>
		/// Get set the scene
		/// </summary>
		Scene3D Scene { get; set; }

		/// <summary>
		/// Invalidate the render ( ask redraw)
		/// </summary>
		void Invalidate();
		/// <summary>
		/// the dragBall navidator associated with the Render
		/// </summary>
		DragBallNavigator Drag { get; }

	}
}
