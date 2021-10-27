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
