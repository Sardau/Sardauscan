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
