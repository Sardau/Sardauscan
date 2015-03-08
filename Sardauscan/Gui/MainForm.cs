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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Management;
using System.Diagnostics;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using Sardauscan.Core;
using Sardauscan.Core.Interface;
using Sardauscan.Hardware;
using DirectShowLib;
using OpenTK;
using Sardauscan.Core.IO;
using Sardauscan.Hardware.Com;
using Sardauscan.Core.Geometry;
using Sardauscan.Gui.Controls;
using Sardauscan.Gui.Controls.ApplicationView;
using Sardauscan.Gui.Forms;

namespace Sardauscan.Gui
{
    public partial class MainForm : CustomAppForm/*/Form/**/, IDisposable
    {
        public MainForm()
        {
            InitializeComponent();
            Application.Idle +=OnIdle;
						SkinInfo.ApplyColor(this);
						object cust = this;
						if (cust is CustomAppForm)
						{
							((CustomAppForm)cust).SettingsClick += new System.EventHandler(this.MainForm_SettingsClick);
							((CustomAppForm)cust).AboutClick += new System.EventHandler(this.MainForm_AboutClick);
						}
							
						ViewControler controler = new ViewControler(this, this.BigViewPanel, this.SmallViewPanel);
						Settings.RegisterInstance(controler);
						controler.RegisterView(ViewType.Process, Scene3DView, ProcessView);
						TuneSmallView.Viewer = TuneBigView;
						controler.RegisterView(ViewType.Tune, TuneBigView, TuneSmallView);
						CalibrationSmallView.Viewer = CalibrationBigView;
						controler.RegisterView(ViewType.Calibrate, CalibrationBigView, CalibrationSmallView);
						controler.ChangeView(ViewType.Process);
						this.mainToolBox1.Controler = controler;
        }
				/// <summary>
				/// Dispose object
				/// </summary>
				public new void Dispose()
				{
					base.Dispose();
					Application.Idle -= OnIdle;
				}

        private void MainForm_Load(object sender, EventArgs e)
        {

            //FillCameraComboBox();
            AlignInterface();
            this.OpenPointsDlg.DefaultExt = ScanDataIO.GetDialogFilter();
						this.OpenPointsDlg.InitialDirectory = Program.UserDataPath;
        }

        protected void AlignInterface()
        {
            Settings.RegisterInstance(this.Scene3DView, true);
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            if (((TabControl)sender).SelectedTab == this.SettingsPage)
                settingsView1.LoadCuttentSetting();
            else
                settingsView1.SaveCurrentSettings();
             */
            
        }
        public ScanData OpenColoreddPointArray(IScene3DViewer viewer)
        {
            if (this.OpenPointsDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
							string path = Path.Combine(Program.UserDataPath, this.OpenPointsDlg.FileName);
                ScanData points = ScanDataIO.Read(path);

                if (viewer != null)
                {
                    viewer.Scene.Clear();
                    viewer.Scene.Add(points);
                    viewer.Invalidate();
                }
                return points;
            }
            return null;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            IScene3DViewer viewer = Settings.Get<IScene3DViewer>();
            if (viewer != null)
            {
                ScanData points = OpenColoreddPointArray(viewer);
                if (points != null)
                {
                    //viewer.Scene = Scene3D.MeshFromScan(points, this.object3DView1.Invalidate); ;
                    viewer.Invalidate();

                }
            }
        }

 
        private void PreviewPictureBox_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ViewTabControl_Selected(object sender, TabControlEventArgs e)
        {
            foreach (Control c in e.TabPage.Controls)
                c.Visible = true;
        }

        private void ViewTabControl_Deselected(object sender, TabControlEventArgs e)
        {
            foreach (Control c in e.TabPage.Controls)
                c.Visible = false;
        }

        DateTime lastImageTime = DateTime.Now;
        private void OnIdle(object sender, EventArgs e)
        {
            try
            {
                bool ignore = false;
                if (!ignore && Visible)
                {
									eHardware hardware = eHardware.None;
										HardwareStatusControl ctrl = Settings.Get<HardwareStatusControl>();
										if (ctrl != null)
											hardware = ctrl.Hardware;
									  //TODO enableDisable....
                }
            }
            catch
            {
            }
        }

				private void MainForm_SettingsClick(object sender, EventArgs e)
				{
					OkCancelDialog dlg = new OkCancelDialog();
					dlg.ShowDialog(new SettingsControl(),this);
				}
				private void MainForm_AboutClick(object sender, EventArgs e)
				{
					OkCancelDialog dlg = new OkCancelDialog();
					dlg.ShowDialog(new AboutControl(),this);
				}
			
				private void mainToolBox1_SizeChanged(object sender, EventArgs e)
				{
					this.SmallViewPanel.Location = new Point(0, this.mainToolBox1.Bottom);
					this.SmallViewPanel.Size = new Size(this.splitContainer1.Panel1.Width, this.splitContainer1.Panel1.Height - this.SmallViewPanel.Top);
//					this.splitContainer1.Panel1.PerformLayout();
				}

				private void MainForm_SizeChanged(object sender, EventArgs e)
				{
					mainToolBox1_SizeChanged(sender, e);
				}
    }

}
