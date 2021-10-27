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
using System.Reflection;
using Sardauscan.Core.Interface;
using Sardauscan.Core;

namespace Sardauscan.Gui.CalibrationSteps
{
	public partial class Manual : UserControl, IDisposable
	{
		public class StepInfo : ICalibrationStepInfo
		{
			public int OrderId { get { return 200; } }

			public string Label { get { return "Physical Calibration"; } }

			public Type ControlType {get { return typeof(Manual); }}
		}
		/// <summary>
		/// Default ctor
		/// </summary>
		public Manual()
		{
			InitializeComponent();
			Application.Idle += OnIdle;
			typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
			null, this.ViewPanel, new object[] { true });
			this.DoubleBuffered = true;
			HelpText1.ForeColor = SkinInfo.ForeColor;
			HelpText1.BackColor = SkinInfo.BackColor;
			HelpText2.ForeColor = SkinInfo.ForeColor;
			HelpText2.BackColor = SkinInfo.BackColor;
		}

		/// <summary>
		/// Dispose object
		/// </summary>
		public new void Dispose()
		{
			base.Dispose();
			Application.Idle -= OnIdle;
		}
		private Image CurrentImage = null;
		private double TableCenterYClamp = 0.5;
		public int ViewPanelCenterY
		{
			get
			{
				return (int)(ViewPanel.Height * TableCenterYClamp);
			}
			set
			{
				TableCenterYClamp = (value / (double)ViewPanel.Height);
			}
		}

		private void ViewPanel_Paint(object sender, PaintEventArgs e)
		{
			Control ctrl = this.ViewPanel;
			using (Bitmap bmp = new Bitmap(ctrl.Width, ctrl.Height))
			{
				Rectangle r = new Rectangle(0, 0, bmp.Width, bmp.Height);
				using (Pen pen = new Pen(Color.LightGreen, 2))
				{
					using (Graphics g = Graphics.FromImage(bmp))
					{
						if (CurrentImage != null)
							g.DrawImage(CurrentImage, r);

						g.DrawLine(pen, r.Width / 2, 0, r.Width / 2, r.Height);
						g.DrawLine(pen, 0, ViewPanelCenterY, r.Width, ViewPanelCenterY);
					}
				}
				e.Graphics.DrawImage(bmp, r);
			}
		}
		private void ViewPanel_Resize(object sender, EventArgs e)
		{
			this.ViewPanel.Invalidate();
		}

		DateTime lastImageTime = DateTime.Now;
		private void OnIdle(object sender, EventArgs e)
		{
			try
			{
				bool ignore = false;
				if (!ignore && Visible)
				{
					DateTime now = DateTime.Now;
					bool expired = (now - lastImageTime).TotalMilliseconds > 10;
					if (expired)
					{
						ICameraProxy camera = Settings.Get<ICameraProxy>();
						if (camera != null)
						{
							CurrentImage = camera.AcquireImage();
							lastImageTime = now;
							ViewPanel.Invalidate();
						}
						else
						{
							bool lasthasImage = CurrentImage != null;
							CurrentImage = null;
							if (lasthasImage)
								ViewPanel.Invalidate();
						}
						ILaserProxy laser = Settings.Get<ILaserProxy>();
						expired = (now - lastImageTime).TotalMilliseconds > 750;
						if (laser != null)
						{
							LaserControl.Proxy = laser;
							LaserControl.AlignControls();
						}
					}
				}
			}
			catch
			{
			}
		}

		bool CenterDefine = false;
		private void CenterFromMouse(MouseEventArgs e)
		{
			ViewPanelCenterY = e.Y - ViewPanel.Left;
			ViewPanel.Invalidate();
		}
		private void ViewPanel_MouseDown(object sender, MouseEventArgs e)
		{
			CenterDefine = true;
			CenterFromMouse(e);
		}

		private void ViewPanel_MouseUp(object sender, MouseEventArgs e)
		{
			CenterDefine = false;
			CenterFromMouse(e);
		}

		private void ViewPanel_MouseMove(object sender, MouseEventArgs e)
		{
			if (CenterDefine)
				CenterFromMouse(e);
		}

		private void HelpText1_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = true;
		}

	}

}
