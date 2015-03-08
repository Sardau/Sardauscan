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
namespace Sardauscan.Gui
{
	/// <summary>
	/// Control for Calibration
	/// </summary>
	partial class CalibrationBigView
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
			this.StepContainerPanel = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// StepContainerPanel
			// 
			this.StepContainerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.StepContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StepContainerPanel.Location = new System.Drawing.Point(0, 0);
			this.StepContainerPanel.Name = "StepContainerPanel";
			this.StepContainerPanel.Size = new System.Drawing.Size(704, 498);
			this.StepContainerPanel.TabIndex = 0;
			// 
			// CalibrationBigView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.StepContainerPanel);
			this.Name = "CalibrationBigView";
			this.Size = new System.Drawing.Size(704, 498);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel StepContainerPanel;
	}
}
