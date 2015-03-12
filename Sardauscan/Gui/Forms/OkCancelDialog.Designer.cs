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
namespace Sardauscan.Gui.Forms
{
	/// <summary>
	/// Dialog the show a control in a OK-Cancel Dialog
	/// </summary>
	partial class OkCancelDialog
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.ContentPanel = new System.Windows.Forms.Panel();
			this.CancelButton = new Sardauscan.Gui.Controls.ImageButton();
			this.OkButton = new Sardauscan.Gui.Controls.ImageButton();
			this.SuspendLayout();
			// 
			// ContentPanel
			// 
			this.ContentPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ContentPanel.Location = new System.Drawing.Point(3, 2);
			this.ContentPanel.Name = "ContentPanel";
			this.ContentPanel.Size = new System.Drawing.Size(325, 244);
			this.ContentPanel.TabIndex = 0;
			// 
			// CancelButton
			// 
			this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CancelButton.BackColor = System.Drawing.Color.Green;
			this.CancelButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelButton.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.CancelButton.Image = global::Sardauscan.Properties.Resources.Uncheck;
			this.CancelButton.Location = new System.Drawing.Point(237, 252);
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Size = new System.Drawing.Size(89, 35);
			this.CancelButton.TabIndex = 2;
			this.CancelButton.Text = "Cancel";
			this.CancelButton.Click += new System.EventHandler(this.OKCancelButtonClick);
			// 
			// OkButton
			// 
			this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.OkButton.BackColor = System.Drawing.Color.Green;
			this.OkButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkButton.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.OkButton.Image = global::Sardauscan.Properties.Resources.Check;
			this.OkButton.Location = new System.Drawing.Point(145, 252);
			this.OkButton.Name = "OkButton";
			this.OkButton.Size = new System.Drawing.Size(89, 35);
			this.OkButton.TabIndex = 1;
			this.OkButton.Text = "Ok";
			this.OkButton.Click += new System.EventHandler(this.OKCancelButtonClick);
			// 
			// OkCancelDialog
			// 
			this.AboutBox = false;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Green;
			this.ClientSize = new System.Drawing.Size(333, 292);
			this.Controls.Add(this.CancelButton);
			this.Controls.Add(this.OkButton);
			this.Controls.Add(this.ContentPanel);
			this.ForeColor = System.Drawing.Color.Blue;
			this.Name = "OkCancelDialog";
			this.SettingsBox = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel ContentPanel;
		private Controls.ImageButton OkButton;
		private new Controls.ImageButton CancelButton;
	}
}