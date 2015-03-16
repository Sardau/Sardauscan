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
using Sardauscan.Gui.OpenGL;

namespace Sardauscan.Core
{
	/// <summary>
	/// Correction object for a laser
	/// </summary>
	public class LaserCorrection
	{
		public LaserCorrection() { Clear(); }

		public void LoadFromSettings(int laserindex)
		{
			Settings set = Settings.Get<Settings>();

			this.Rotation = set.Read(Settings.LASER(laserindex), Settings.ROTATION, this.Rotation);
			this.Scale = set.Read(Settings.LASER(laserindex), Settings.SCALE, this.Scale);
			this.Translation = new Vector2d(
				set.Read(Settings.LASER(laserindex), Settings.TRANSLATIONX, this.Translation.X),
				set.Read(Settings.LASER(laserindex), Settings.TRANSLATIONY, this.Translation.Y)
				);
		}
		public void SaveToSettings(int laserIndex)
		{
			Settings set = Settings.Get<Settings>();
			set.Write(Settings.LASER(laserIndex), Settings.ROTATION, this.Rotation);
			set.Write(Settings.LASER(laserIndex), Settings.SCALE, this.Scale);
			set.Write(Settings.LASER(laserIndex), Settings.TRANSLATIONX, this.Translation.X);
			set.Write(Settings.LASER(laserIndex), Settings.TRANSLATIONY, this.Translation.Y);
		}

		public void Clear()
		{
			Rotation = 0;
			Translation = new Vector2d(0, 0);
			Scale = 1f;
		}

		/// <summary>
		/// Ritation in degrees
		/// </summary>
		public double Rotation;
		public Vector2d Translation;
		public double Scale;


		public void Apply(DragBallNavigator dragball)
		{
			Translation.X += dragball.Pane.X / 2;
			Translation.Y += dragball.Pane.Y / 2;
			Scale /= dragball.Zoom;
			Rotation += dragball.Angle.X / 4;

		}

		public Matrix4d GetMatrix()
		{
			Matrix4d matrix = Matrix4d.CreateRotationY(Utils.DEGREES_TO_RADIANS(this.Rotation));
			matrix = Matrix4d.Mult(matrix,Matrix4d.CreateTranslation(Translation.X,0,Translation.Y));
			matrix = Matrix4d.Mult(matrix,Matrix4d.Scale(Scale,1f,Scale));
			return matrix;
		}
	
	}

}
