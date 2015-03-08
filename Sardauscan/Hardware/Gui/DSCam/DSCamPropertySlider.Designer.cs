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
namespace Sardauscan.Hardware.Gui.DSCam
{
	/// <summary>
	/// Class used to ling a Directshow Camera control to a slider
	/// </summary>
	partial class DSCamPropertySlider
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
			this.TrackBar = new System.Windows.Forms.TrackBar();
			this.AutoCheckBox = new System.Windows.Forms.CheckBox();
			this.NameLabel = new System.Windows.Forms.Label();
			this.ValueLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.TrackBar)).BeginInit();
			this.SuspendLayout();
			// 
			// TrackBar
			// 
			this.TrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TrackBar.AutoSize = false;
			this.TrackBar.Location = new System.Drawing.Point(108, 5);
			this.TrackBar.Name = "TrackBar";
			this.TrackBar.Size = new System.Drawing.Size(155, 22);
			this.TrackBar.TabIndex = 0;
			this.TrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
			this.TrackBar.ValueChanged += new System.EventHandler(this.TrackBar_ValueChanged);
			// 
			// AutoCheckBox
			// 
			this.AutoCheckBox.AutoSize = true;
			this.AutoCheckBox.Location = new System.Drawing.Point(87, 8);
			this.AutoCheckBox.Name = "AutoCheckBox";
			this.AutoCheckBox.Size = new System.Drawing.Size(15, 14);
			this.AutoCheckBox.TabIndex = 1;
			this.AutoCheckBox.UseVisualStyleBackColor = true;
			this.AutoCheckBox.CheckedChanged += new System.EventHandler(this.AutoCheckBox_CheckedChanged);
			// 
			// NameLabel
			// 
			this.NameLabel.AutoSize = true;
			this.NameLabel.Location = new System.Drawing.Point(3, 8);
			this.NameLabel.Name = "NameLabel";
			this.NameLabel.Size = new System.Drawing.Size(0, 13);
			this.NameLabel.TabIndex = 2;
			// 
			// ValueLabel
			// 
			this.ValueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ValueLabel.AutoSize = true;
			this.ValueLabel.Location = new System.Drawing.Point(278, 8);
			this.ValueLabel.Name = "ValueLabel";
			this.ValueLabel.Size = new System.Drawing.Size(0, 13);
			this.ValueLabel.TabIndex = 3;
			// 
			// DSCamPropertySlider
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ValueLabel);
			this.Controls.Add(this.NameLabel);
			this.Controls.Add(this.AutoCheckBox);
			this.Controls.Add(this.TrackBar);
			this.Name = "DSCamPropertySlider";
			this.Size = new System.Drawing.Size(327, 34);
			((System.ComponentModel.ISupportInitialize)(this.TrackBar)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TrackBar TrackBar;
		private System.Windows.Forms.CheckBox AutoCheckBox;
		private System.Windows.Forms.Label NameLabel;
		private System.Windows.Forms.Label ValueLabel;
	}
}
