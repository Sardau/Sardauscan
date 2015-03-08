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
	partial class CrashReport
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
			this.underConstruction1 = new Sardauscan.Gui.Controls.UnderConstruction();
			this.ExceptionTextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// underConstruction1
			// 
			this.underConstruction1.Dock = System.Windows.Forms.DockStyle.Top;
			this.underConstruction1.Location = new System.Drawing.Point(0, 0);
			this.underConstruction1.Name = "underConstruction1";
			this.underConstruction1.Size = new System.Drawing.Size(612, 217);
			this.underConstruction1.TabIndex = 0;
			// 
			// ExceptionTextBox
			// 
			this.ExceptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ExceptionTextBox.Location = new System.Drawing.Point(15, 206);
			this.ExceptionTextBox.Multiline = true;
			this.ExceptionTextBox.Name = "ExceptionTextBox";
			this.ExceptionTextBox.Size = new System.Drawing.Size(581, 228);
			this.ExceptionTextBox.TabIndex = 1;
			// 
			// CrashReport
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ExceptionTextBox);
			this.Controls.Add(this.underConstruction1);
			this.Name = "CrashReport";
			this.Size = new System.Drawing.Size(612, 452);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private UnderConstruction underConstruction1;
		private System.Windows.Forms.TextBox ExceptionTextBox;
	}
}
