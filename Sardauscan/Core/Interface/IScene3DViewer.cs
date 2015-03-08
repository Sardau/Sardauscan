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
