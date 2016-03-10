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
using Sardauscan.Core;
using Sardauscan.Core.Interface;
using Sardauscan.Hardware;
using Sardauscan.Core.Geometry;
using Sardauscan.Gui.Forms;

namespace Sardauscan.Gui
{
    public partial class TuneSmallView : UserControl, IDisposable
    {
			/// <summary>
			/// Default ctor
			/// </summary>
			public TuneSmallView()
        {
            InitializeComponent();
            Application.Idle +=this.OnIdle;
        }
			/// <summary>
			/// Dispose object
			/// </summary>
			public new void Dispose()
				{
					base.Dispose();
					Application.Idle -= OnIdle;
				}

				public TuneBigView Viewer { get; set; }
        private void TestButton_Click(object sender, EventArgs e)
        {
					UpdateViewer();
        }

        void UpdateViewer()
        {
					if (Viewer == null || loading)
						return;
					int laserIndex = Math.Max(0,this.LaserComboBox.SelectedIndex);
					double thres = (double)this.ThresholdUpDown.Value;
					int min = (int)this.MinWidthUpDown.Value;
					int max = (int)this.MaxWidthUpDown.Value;
					Viewer.TestLaser(laserIndex, thres, min, max);
        }

        private void DebugImageControl_Load(object sender, EventArgs e)
        {
					LoadInterface();
        }
				ILaserProxy LastlaserProxy = null;
        public void LoadInterface()
        {
					loading = true;
            ILaserProxy laser = Settings.Get<ILaserProxy>();
            if (laser != null && LastlaserProxy!=laser)
            {
							LastlaserProxy=laser;
							int currentLaser = this.LaserComboBox.SelectedIndex;
                int lasercount = laser.Count;
                this.LaserComboBox.Items.Clear();
                LoadFromSettings();
                for (int i = 0; i < lasercount; i++)
                {
                    this.LaserComboBox.Items.Add(String.Format("Laser {0}", i));
                }
								this.LaserComboBox.SelectedIndex = currentLaser;
            }
					loading=false;
        }
        private bool loading = false;
        public void LoadFromSettings()
        {
            loading = true;
            Settings set = Settings.Get<Settings>();
            this.ThresholdUpDown.Value = (decimal)set.Read(Settings.LASER_COMMON, Settings.MAGNITUDE_THRESHOLD, 10);
            this.MinWidthUpDown.Value = set.Read(Settings.LASER_COMMON, Settings.MIN_WIDTH,1);
            this.MaxWidthUpDown.Value = set.Read(Settings.LASER_COMMON, Settings.MAX_WIDTH,60);
            loading = false;
        }
        public void SaveToSettings()
        {
            Settings set = Settings.Get<Settings>();
            set.Write(Settings.LASER_COMMON, Settings.MAGNITUDE_THRESHOLD, (int)this.ThresholdUpDown.Value);
            set.Write(Settings.LASER_COMMON, Settings.MIN_WIDTH, (int)this.MinWidthUpDown.Value);
            set.Write(Settings.LASER_COMMON, Settings.MAX_WIDTH, (int)this.MaxWidthUpDown.Value);
        }
        private void LaserComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int laserIndex = this.LaserComboBox.SelectedIndex;
						UpdateViewer();
        }


        private void ThresholdUpDown_ValueChanged(object sender, EventArgs e)
        {
            SaveToSettings();
            UpdateViewer();
        }

        private void TuneView_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
                LoadInterface();
        }

				DateTime lastImageTime = DateTime.Now;
				private void OnIdle(object sender, EventArgs e)
        {
            try
            {
                if (Visible)
                {
										DateTime now = DateTime.Now;
										bool expired = (now - lastImageTime).TotalMilliseconds > 1000;
										if (expired)
										{
											LoadFromSettings();
											LoadInterface();
											UpdateViewer();
										}
								}
            }
            catch
            {
            }
        }

				Color GetLaserColor(int laserIndex)
				{
					Settings settings = Settings.Get<Settings>();
					if (settings != null && laserIndex >= 0)
						return settings.Read(Settings.LASER(laserIndex), Settings.DEFAULTCOLOR, LaserInfo.GetDefaultColor(laserIndex));
					else
						return LaserInfo.GetDefaultColor(laserIndex);
				}
				private void LaserComboBox_DrawItem(object sender, DrawItemEventArgs e)
				{

					Color col = this.GetLaserColor(e.Index);
					using (SolidBrush b = new SolidBrush(col))
					{
						if (e.Index >= 0)
						{
							e.DrawBackground();
							int sqareSize = e.Bounds.Height - 4;
							e.Graphics.FillRectangle(b, new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2, sqareSize, sqareSize));
							using (Pen p = new Pen(SkinInfo.ForeColor))
								e.Graphics.DrawRectangle(p, new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2, sqareSize, sqareSize));
							Rectangle textRect = new Rectangle(e.Bounds.Left + e.Bounds.Height, e.Bounds.Top, e.Bounds.Width - e.Bounds.Height, e.Bounds.Height);
							using (SolidBrush textB = new SolidBrush(SkinInfo.ForeColor))
							{
								e.Graphics.DrawString(LaserComboBox.Items[e.Index].ToString(), this.Font, textB, textRect);
							}
						}

					}
				}

    }
}
