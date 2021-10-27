#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sardauscan.Core.Interface;
using Sardauscan.Core;
using OpenTK;

namespace Sardauscan.Gui.CalibrationSteps
{
	public partial class Dimention : UserControl, IDisposable
	{
		public class StepInfo : ICalibrationStepInfo
		{
			public int OrderId { get { return 300; } }

			public string Label { get { return "Build Dimention"; } }

			public Type ControlType { get { return typeof(Dimention); } }
		}

		/// <summary>
		/// Default ctor
		/// </summary>
		public Dimention()
		{
			InitializeComponent();
			Application.Idle += OnIdle;
		}
		/// <summary>
		/// Dispose object
		/// </summary>
		public new void Dispose()
		{
			base.Dispose();
			Application.Idle -= OnIdle;
		}

		ILaserProxy _LastLaserProxy=null;
		ILaserProxy LaserProxy
		{
			get 
			{
				return _LastLaserProxy; 
			}
			set
			{
				bool reload = _LastLaserProxy != value;
				_LastLaserProxy = value;
				if(reload)
					LoadFromSettings();
			}
		}

		bool loading = false;
		void SetNumericUpDownValue(NumericUpDown updown, decimal value )
		{
			updown.Value = Math.Max(updown.Minimum, Math.Min(updown.Maximum, value));
		}
		void LoadFromSettings()
		{
			loading = true;
			Settings settings = Settings.Get<Settings>();
			if(settings!=null)
			{
				this.Enabled=true;
				Vector3d cam = new Vector3d(settings.Read(Settings.CAMERA, Settings.X, 0f), settings.Read(Settings.CAMERA, Settings.Y, 50f), settings.Read(Settings.CAMERA, Settings.Z, 220f));

				SetNumericUpDownValue(this.CameraZ,(decimal)cam.Z);
				SetNumericUpDownValue(this.CameraY,(decimal)cam.Y);
				ILaserProxy laser = Settings.Get<ILaserProxy>();
				if (laser != null)
				{
					int currentLaser = this.LaserComboBox.Items.Count == 0 ? -1 : this.LaserComboBox.SelectedIndex;
					int lasercount = laser.Count;
					this.LaserComboBox.Items.Clear();
					for (int i = 0; i < lasercount; i++)
					{
						this.LaserComboBox.Items.Add(String.Format("Laser {0}", i));
					}
					currentLaser = Math.Max(0, currentLaser);
					this.LaserComboBox.SelectedIndex = currentLaser;
					Vector3d laserLoc = new Vector3d(
					settings.Read(Settings.LASER(currentLaser), Settings.X, 50f),
					settings.Read(Settings.LASER(currentLaser), Settings.Y, (double)this.CameraY.Value),
					settings.Read(Settings.LASER(currentLaser), Settings.Z, (double)this.CameraZ.Value)
				  );

					double angle = laserLoc.AngleInDegrees(cam);
					if(cam.X < laserLoc.X)
						angle = -angle;

					SetNumericUpDownValue(this.LaserX,(decimal)laserLoc.X);
					this.LaserYLabel.Text = string.Format("{0:.0}",laserLoc.Y);
					SetNumericUpDownValue(this.LaserZ,(decimal)laserLoc.Z);
					SetNumericUpDownValue(this.LaserAngle,(decimal) angle);
				}
			}
			else
				this.Enabled=false;


			loading = false;
		}
		void SaveToSettings(bool saveLaser,bool fromLaserPos)
		{
			Settings settings = Settings.Get<Settings>();
			if (settings != null)
			{
				settings.Write(Settings.CAMERA, Settings.Z, (double)this.CameraZ.Value);
				settings.Write(Settings.CAMERA, Settings.Y, (double)this.CameraY.Value);
				if (!saveLaser)
				{
					for (int i = 0; i < this.LaserComboBox.Items.Count;i++ )
						settings.Write(Settings.LASER(i), Settings.Y, (double)this.CameraY.Value);
				}
				else
				{
				int currentLaser = this.LaserComboBox.Items.Count == 0 ? -1 : this.LaserComboBox.SelectedIndex;
				if(currentLaser>=0)
				{
					if (fromLaserPos)
					{
						settings.Write(Settings.LASER(currentLaser), Settings.X, (double)this.LaserX.Value);
						settings.Write(Settings.LASER(currentLaser), Settings.Z, (double)this.LaserZ.Value);
					}
					else
					{
						Vector3d cam = new Vector3d(settings.Read(Settings.CAMERA, Settings.X, 0f), settings.Read(Settings.CAMERA, Settings.Y, 50f), settings.Read(Settings.CAMERA, Settings.Z, 220f));
						Vector3d laserLoc = RotateY(cam, (double)LaserAngle.Value);
						double angle = laserLoc.AngleInDegrees(cam);

						settings.Write(Settings.LASER(currentLaser), Settings.X, laserLoc.X);
						settings.Write(Settings.LASER(currentLaser), Settings.Z, laserLoc.Z);
					}
				}
				}
			}
		}
		Vector3d RotateY(Vector3d v, double angle)
		{
			Vector3d ret = new Vector3d(0, v.Y, 0);
			double rad = Utils.DEGREES_TO_RADIANS(angle);
			ret.X = (double)(v.X * Math.Cos(rad) - v.Z * Math.Sin(rad));
			ret.Z = (double)(v.X * Math.Sin(rad) + v.Z * Math.Cos(rad));
			return ret;
		}

		void UpdateSettings(bool savelaser, bool fromLaserPos)
		{
			SaveToSettings(savelaser,fromLaserPos);
			LoadFromSettings();
		}

		private void CameraPos_Changed(object sender, EventArgs e)
		{
			if (loading)
				return;
			UpdateSettings(false,false);
		}

		private void LaserPosition_Changed(object sender, EventArgs e)
		{
			if (loading)
				return;
			UpdateSettings(true,true);
		}

		private void LaserAngle_Changed(object sender, EventArgs e)
		{
			if (loading)
				return;
			UpdateSettings(true,false);
		}

		private void CurrentLaser_Changed(object sender, EventArgs e)
		{
			if (loading)
				return;
			LoadFromSettings();
		}

		DateTime lastCheckTime = DateTime.Now;
		private void OnIdle(object sender, EventArgs e)
		{
			try
			{
				bool ignore = false;
				if (!ignore && Visible)
				{
					DateTime now = DateTime.Now;
					bool expired = (now - lastCheckTime).TotalMilliseconds > 500;
					if (expired)
					{
						LaserProxy = Settings.Get<ILaserProxy>();
					}
				}
			}
			catch
			{
			}
		}

		private void Dimention_Load(object sender, EventArgs e)
		{
			LoadFromSettings();
		}

	}
}
