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
namespace Sardauscan.Gui.Controls
{
	/// <summary>
	/// Control to Select/Remove/showStatus the hardware proxy
	/// </summary>
	partial class HardwareStatusControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.TableButton = new Sardauscan.Gui.Controls.StatusImageButton();
			this.LaserButton = new Sardauscan.Gui.Controls.StatusImageButton();
			this.CameraButton = new Sardauscan.Gui.Controls.StatusImageButton();
			this.HideSettings = new Sardauscan.Gui.Controls.ImageButton();
			this.ProxySettingPanel = new System.Windows.Forms.Panel();
			this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.TableButton);
			this.flowLayoutPanel1.Controls.Add(this.LaserButton);
			this.flowLayoutPanel1.Controls.Add(this.CameraButton);
			this.flowLayoutPanel1.Controls.Add(this.HideSettings);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(133, 32);
			this.flowLayoutPanel1.TabIndex = 1;
			// 
			// TableButton
			// 
			this.TableButton.BackColor = System.Drawing.Color.Transparent;
			this.TableButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.TableButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(55)))), ((int)(((byte)(85)))));
			this.TableButton.Image = global::Sardauscan.Properties.Resources.Busy;
			this.TableButton.Location = new System.Drawing.Point(0, 0);
			this.TableButton.Margin = new System.Windows.Forms.Padding(0);
			this.TableButton.Name = "TableButton";
			this.TableButton.OffImageType = Sardauscan.Gui.Controls.eOffButtonType.Unavailable;
			this.TableButton.On = true;
			this.TableButton.Size = new System.Drawing.Size(36, 32);
			this.TableButton.TabIndex = 0;
			this.TableButton.Click += new System.EventHandler(this.Button_Click);
			this.TableButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TableButton_MouseDown);
			// 
			// LaserButton
			// 
			this.LaserButton.BackColor = System.Drawing.Color.Transparent;
			this.LaserButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.LaserButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(55)))), ((int)(((byte)(85)))));
			this.LaserButton.Image = global::Sardauscan.Properties.Resources.Sun;
			this.LaserButton.Location = new System.Drawing.Point(36, 0);
			this.LaserButton.Margin = new System.Windows.Forms.Padding(0);
			this.LaserButton.Name = "LaserButton";
			this.LaserButton.OffImageType = Sardauscan.Gui.Controls.eOffButtonType.Unavailable;
			this.LaserButton.On = true;
			this.LaserButton.Size = new System.Drawing.Size(36, 32);
			this.LaserButton.TabIndex = 1;
			this.LaserButton.Click += new System.EventHandler(this.Button_Click);
			this.LaserButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TableButton_MouseDown);
			// 
			// CameraButton
			// 
			this.CameraButton.BackColor = System.Drawing.Color.Transparent;
			this.CameraButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.CameraButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(55)))), ((int)(((byte)(85)))));
			this.CameraButton.Image = global::Sardauscan.Properties.Resources.Camera;
			this.CameraButton.Location = new System.Drawing.Point(72, 0);
			this.CameraButton.Margin = new System.Windows.Forms.Padding(0);
			this.CameraButton.Name = "CameraButton";
			this.CameraButton.OffImageType = Sardauscan.Gui.Controls.eOffButtonType.Unavailable;
			this.CameraButton.On = true;
			this.CameraButton.Size = new System.Drawing.Size(31, 32);
			this.CameraButton.TabIndex = 2;
			this.CameraButton.Click += new System.EventHandler(this.Button_Click);
			this.CameraButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TableButton_MouseDown);
			// 
			// HideSettings
			// 
			this.HideSettings.BackColor = System.Drawing.Color.Transparent;
			this.HideSettings.Cursor = System.Windows.Forms.Cursors.Hand;
			this.HideSettings.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(55)))), ((int)(((byte)(85)))));
			this.HideSettings.Image = global::Sardauscan.Properties.Resources.Eye_Off;
			this.HideSettings.Location = new System.Drawing.Point(103, 0);
			this.HideSettings.Margin = new System.Windows.Forms.Padding(0);
			this.HideSettings.Name = "HideSettings";
			this.HideSettings.Size = new System.Drawing.Size(30, 32);
			this.HideSettings.TabIndex = 3;
			this.HideSettings.Visible = false;
			this.HideSettings.Click += new System.EventHandler(this.HideSettings_Click);
			// 
			// ProxySettingPanel
			// 
			this.ProxySettingPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ProxySettingPanel.BackColor = System.Drawing.Color.Transparent;
			this.ProxySettingPanel.Location = new System.Drawing.Point(0, 32);
			this.ProxySettingPanel.Margin = new System.Windows.Forms.Padding(0);
			this.ProxySettingPanel.Name = "ProxySettingPanel";
			this.ProxySettingPanel.Size = new System.Drawing.Size(1, 1);
			this.ProxySettingPanel.TabIndex = 2;
			// 
			// ToolTip
			// 
			this.ToolTip.AutomaticDelay = 100;
			// 
			// HardwareStatusControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.ProxySettingPanel);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "HardwareStatusControl";
			this.Size = new System.Drawing.Size(133, 33);
			this.Load += new System.EventHandler(this.HardwareStatusControl_Load);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private StatusImageButton TableButton;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private StatusImageButton LaserButton;
		private StatusImageButton CameraButton;
		private System.Windows.Forms.Panel ProxySettingPanel;
		private ImageButton HideSettings;
		private System.Windows.Forms.ToolTip ToolTip;
	}
}
