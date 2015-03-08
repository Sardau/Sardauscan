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
namespace Sardauscan.Hardware.Gui
{
	/// <summary>
	/// Proxy class for a directshow Camera
	/// </summary>
	partial class DSCameraProxyControl
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
			this.PreviewPictureBox = new System.Windows.Forms.PictureBox();
			this.dsCamPropertySlider1 = new Sardauscan.Hardware.Gui.DSCam.DSCamPropertySlider();
			this.dsCamPropertySlider2 = new Sardauscan.Hardware.Gui.DSCam.DSCamPropertySlider();
			this.dsCamPropertySlider3 = new Sardauscan.Hardware.Gui.DSCam.DSCamPropertySlider();
			this.dsCamPropertySlider4 = new Sardauscan.Hardware.Gui.DSCam.DSCamPropertySlider();
			this.dsCamPropertySlider5 = new Sardauscan.Hardware.Gui.DSCam.DSCamPropertySlider();
			this.dsCamPropertySlider6 = new Sardauscan.Hardware.Gui.DSCam.DSCamPropertySlider();
			this.dsCamPropertySlider7 = new Sardauscan.Hardware.Gui.DSCam.DSCamPropertySlider();
			((System.ComponentModel.ISupportInitialize)(this.PreviewPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// PreviewPictureBox
			// 
			this.PreviewPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PreviewPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.PreviewPictureBox.Image = global::Sardauscan.Properties.Resources.Denied;
			this.PreviewPictureBox.Location = new System.Drawing.Point(0, 3);
			this.PreviewPictureBox.Name = "PreviewPictureBox";
			this.PreviewPictureBox.Size = new System.Drawing.Size(591, 239);
			this.PreviewPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.PreviewPictureBox.TabIndex = 0;
			this.PreviewPictureBox.TabStop = false;
			// 
			// dsCamPropertySlider1
			// 
			this.dsCamPropertySlider1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dsCamPropertySlider1.Camera = null;
			this.dsCamPropertySlider1.CamProperty = DirectShowLib.CameraControlProperty.Exposure;
			this.dsCamPropertySlider1.Location = new System.Drawing.Point(3, 248);
			this.dsCamPropertySlider1.Name = "dsCamPropertySlider1";
			this.dsCamPropertySlider1.Size = new System.Drawing.Size(584, 34);
			this.dsCamPropertySlider1.TabIndex = 1;
			// 
			// dsCamPropertySlider2
			// 
			this.dsCamPropertySlider2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dsCamPropertySlider2.Camera = null;
			this.dsCamPropertySlider2.CamProperty = DirectShowLib.CameraControlProperty.Focus;
			this.dsCamPropertySlider2.Location = new System.Drawing.Point(3, 310);
			this.dsCamPropertySlider2.Name = "dsCamPropertySlider2";
			this.dsCamPropertySlider2.Size = new System.Drawing.Size(584, 34);
			this.dsCamPropertySlider2.TabIndex = 1;
			// 
			// dsCamPropertySlider3
			// 
			this.dsCamPropertySlider3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dsCamPropertySlider3.Camera = null;
			this.dsCamPropertySlider3.CamProperty = DirectShowLib.CameraControlProperty.Zoom;
			this.dsCamPropertySlider3.Location = new System.Drawing.Point(3, 279);
			this.dsCamPropertySlider3.Name = "dsCamPropertySlider3";
			this.dsCamPropertySlider3.Size = new System.Drawing.Size(584, 34);
			this.dsCamPropertySlider3.TabIndex = 1;
			// 
			// dsCamPropertySlider4
			// 
			this.dsCamPropertySlider4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dsCamPropertySlider4.Camera = null;
			this.dsCamPropertySlider4.CamProperty = DirectShowLib.CameraControlProperty.Pan;
			this.dsCamPropertySlider4.Location = new System.Drawing.Point(3, 372);
			this.dsCamPropertySlider4.Name = "dsCamPropertySlider4";
			this.dsCamPropertySlider4.Size = new System.Drawing.Size(584, 34);
			this.dsCamPropertySlider4.TabIndex = 2;
			// 
			// dsCamPropertySlider5
			// 
			this.dsCamPropertySlider5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dsCamPropertySlider5.Camera = null;
			this.dsCamPropertySlider5.CamProperty = DirectShowLib.CameraControlProperty.Tilt;
			this.dsCamPropertySlider5.Location = new System.Drawing.Point(3, 403);
			this.dsCamPropertySlider5.Name = "dsCamPropertySlider5";
			this.dsCamPropertySlider5.Size = new System.Drawing.Size(584, 34);
			this.dsCamPropertySlider5.TabIndex = 3;
			// 
			// dsCamPropertySlider6
			// 
			this.dsCamPropertySlider6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dsCamPropertySlider6.Camera = null;
			this.dsCamPropertySlider6.CamProperty = DirectShowLib.CameraControlProperty.Roll;
			this.dsCamPropertySlider6.Location = new System.Drawing.Point(3, 434);
			this.dsCamPropertySlider6.Name = "dsCamPropertySlider6";
			this.dsCamPropertySlider6.Size = new System.Drawing.Size(584, 34);
			this.dsCamPropertySlider6.TabIndex = 4;
			// 
			// dsCamPropertySlider7
			// 
			this.dsCamPropertySlider7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dsCamPropertySlider7.Camera = null;
			this.dsCamPropertySlider7.CamProperty = DirectShowLib.CameraControlProperty.Iris;
			this.dsCamPropertySlider7.Location = new System.Drawing.Point(3, 341);
			this.dsCamPropertySlider7.Name = "dsCamPropertySlider7";
			this.dsCamPropertySlider7.Size = new System.Drawing.Size(584, 34);
			this.dsCamPropertySlider7.TabIndex = 5;
			// 
			// DSCameraProxyControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.dsCamPropertySlider6);
			this.Controls.Add(this.dsCamPropertySlider7);
			this.Controls.Add(this.dsCamPropertySlider1);
			this.Controls.Add(this.dsCamPropertySlider2);
			this.Controls.Add(this.dsCamPropertySlider3);
			this.Controls.Add(this.dsCamPropertySlider4);
			this.Controls.Add(this.PreviewPictureBox);
			this.Controls.Add(this.dsCamPropertySlider5);
			this.Name = "DSCameraProxyControl";
			this.Size = new System.Drawing.Size(590, 471);
			((System.ComponentModel.ISupportInitialize)(this.PreviewPictureBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox PreviewPictureBox;
		private DSCam.DSCamPropertySlider dsCamPropertySlider1;
		private DSCam.DSCamPropertySlider dsCamPropertySlider2;
		private DSCam.DSCamPropertySlider dsCamPropertySlider3;
		private DSCam.DSCamPropertySlider dsCamPropertySlider4;
		private DSCam.DSCamPropertySlider dsCamPropertySlider5;
		private DSCam.DSCamPropertySlider dsCamPropertySlider6;
		private DSCam.DSCamPropertySlider dsCamPropertySlider7;
	}
}
