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
	partial class DSCameraSelectionControl
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
			this.CameraResolutionComboBox = new System.Windows.Forms.ComboBox();
			this.CameraComboBox = new System.Windows.Forms.ComboBox();
			this.PreviewControl = new Sardauscan.Hardware.Gui.DSCameraProxyControl();
			this.PlugButton = new Sardauscan.Gui.Controls.ImageButton();
			this.imageButton1 = new Sardauscan.Gui.Controls.ImageButton();
			this.SuspendLayout();
			// 
			// CameraResolutionComboBox
			// 
			this.CameraResolutionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.CameraResolutionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CameraResolutionComboBox.FormattingEnabled = true;
			this.CameraResolutionComboBox.Location = new System.Drawing.Point(334, 3);
			this.CameraResolutionComboBox.Name = "CameraResolutionComboBox";
			this.CameraResolutionComboBox.Size = new System.Drawing.Size(105, 21);
			this.CameraResolutionComboBox.TabIndex = 11;
			this.CameraResolutionComboBox.SelectionChangeCommitted += new System.EventHandler(this.CameraResolutionComboBox_SelectedIndexChanged);
			// 
			// CameraComboBox
			// 
			this.CameraComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.CameraComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CameraComboBox.FormattingEnabled = true;
			this.CameraComboBox.Location = new System.Drawing.Point(34, 3);
			this.CameraComboBox.Name = "CameraComboBox";
			this.CameraComboBox.Size = new System.Drawing.Size(288, 21);
			this.CameraComboBox.TabIndex = 10;
			this.CameraComboBox.SelectedIndexChanged += new System.EventHandler(this.CameraComboBox_SelectedIndexChanged);
			// 
			// PreviewControl
			// 
			this.PreviewControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PreviewControl.BackColor = System.Drawing.Color.Transparent;
			this.PreviewControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.PreviewControl.Location = new System.Drawing.Point(3, 30);
			this.PreviewControl.Name = "PreviewControl";
			this.PreviewControl.Proxy = null;
			this.PreviewControl.Size = new System.Drawing.Size(468, 466);
			this.PreviewControl.TabIndex = 15;
			// 
			// PlugButton
			// 
			this.PlugButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.PlugButton.BackColor = System.Drawing.Color.Green;
			this.PlugButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.PlugButton.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.PlugButton.Image = global::Sardauscan.Properties.Resources.Usb;
			this.PlugButton.Location = new System.Drawing.Point(442, 0);
			this.PlugButton.Name = "PlugButton";
			this.PlugButton.Size = new System.Drawing.Size(29, 31);
			this.PlugButton.TabIndex = 14;
			this.PlugButton.Click += new System.EventHandler(this.PlugButton_Click);
			// 
			// imageButton1
			// 
			this.imageButton1.BackColor = System.Drawing.Color.Green;
			this.imageButton1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.imageButton1.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.imageButton1.Image = global::Sardauscan.Properties.Resources.Reload;
			this.imageButton1.Location = new System.Drawing.Point(-1, -2);
			this.imageButton1.Name = "imageButton1";
			this.imageButton1.Size = new System.Drawing.Size(29, 31);
			this.imageButton1.TabIndex = 12;
			this.imageButton1.Click += new System.EventHandler(this.ReloadCameraCombo);
			// 
			// DirectShowLibCameraSelectionControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.PreviewControl);
			this.Controls.Add(this.PlugButton);
			this.Controls.Add(this.imageButton1);
			this.Controls.Add(this.CameraResolutionComboBox);
			this.Controls.Add(this.CameraComboBox);
			this.Name = "DirectShowLibCameraSelectionControl";
			this.Size = new System.Drawing.Size(474, 500);
			this.Load += new System.EventHandler(this.DirectShowLibCameraSelectionView_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private Sardauscan.Gui.Controls.ImageButton imageButton1;
		private System.Windows.Forms.ComboBox CameraResolutionComboBox;
		private System.Windows.Forms.ComboBox CameraComboBox;
		private Sardauscan.Gui.Controls.ImageButton PlugButton;
		private DSCameraProxyControl PreviewControl;
	}
}
