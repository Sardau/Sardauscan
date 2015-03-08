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
	partial class MainToolBox
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
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.SettingsPanel = new System.Windows.Forms.Panel();
			this.viewSelector1 = new Sardauscan.Gui.Controls.ApplicationView.ViewSelector();
			this.hardwareStatusControl1 = new Sardauscan.Gui.Controls.HardwareStatusControl();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.viewSelector1);
			this.flowLayoutPanel1.Controls.Add(this.hardwareStatusControl1);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(236, 33);
			this.flowLayoutPanel1.TabIndex = 2;
			// 
			// SettingsPanel
			// 
			this.SettingsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.SettingsPanel.Location = new System.Drawing.Point(0, 33);
			this.SettingsPanel.Margin = new System.Windows.Forms.Padding(0);
			this.SettingsPanel.Name = "SettingsPanel";
			this.SettingsPanel.Size = new System.Drawing.Size(236, 10);
			this.SettingsPanel.TabIndex = 3;
			this.SettingsPanel.SizeChanged += new System.EventHandler(this.SettingsPanel_SizeChanged);
			// 
			// viewSelector1
			// 
			this.viewSelector1.AutoSize = true;
			this.viewSelector1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.viewSelector1.Controler = null;
			this.viewSelector1.Location = new System.Drawing.Point(0, 0);
			this.viewSelector1.Margin = new System.Windows.Forms.Padding(0);
			this.viewSelector1.Name = "viewSelector1";
			this.viewSelector1.Size = new System.Drawing.Size(96, 32);
			this.viewSelector1.TabIndex = 1;
			// 
			// hardwareStatusControl1
			// 
			this.hardwareStatusControl1.AutoSize = true;
			this.hardwareStatusControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.hardwareStatusControl1.Location = new System.Drawing.Point(96, 0);
			this.hardwareStatusControl1.Margin = new System.Windows.Forms.Padding(0);
			this.hardwareStatusControl1.Name = "hardwareStatusControl1";
			this.hardwareStatusControl1.Size = new System.Drawing.Size(103, 33);
			this.hardwareStatusControl1.TabIndex = 0;
			// 
			// MainToolBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.SettingsPanel);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Name = "MainToolBox";
			this.Size = new System.Drawing.Size(236, 45);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private HardwareStatusControl hardwareStatusControl1;
		private ApplicationView.ViewSelector viewSelector1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Panel SettingsPanel;
	}
}
