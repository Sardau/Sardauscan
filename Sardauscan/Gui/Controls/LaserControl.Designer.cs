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
	partial class LaserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private StatusImageButton RightButton;
		private StatusImageButton button1;
		private StatusImageButton button2;
		private StatusImageButton button3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LaserControl));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.button1 = new Sardauscan.Gui.Controls.StatusImageButton();
			this.button2 = new Sardauscan.Gui.Controls.StatusImageButton();
			this.button3 = new Sardauscan.Gui.Controls.StatusImageButton();
			this.RightButton = new Sardauscan.Gui.Controls.StatusImageButton();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.27273F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.72727F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.72727F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.27273F));
			this.tableLayoutPanel1.Controls.Add(this.button1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.button2, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.button3, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.RightButton, 2, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(239, 38);
			this.tableLayoutPanel1.TabIndex = 3;
			// 
			// button1
			// 
			this.button1.BackColor = System.Drawing.Color.White;
			this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.button1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
			this.button1.Location = new System.Drawing.Point(3, 3);
			this.button1.Name = "button1";
			this.button1.OffImageType = Sardauscan.Gui.Controls.eOffButtonType.NotSelected;
			this.button1.On = true;
			this.button1.Size = new System.Drawing.Size(30, 32);
			this.button1.TabIndex = 0;
			this.button1.Tag = "0";
			this.button1.Click += new System.EventHandler(this.button_Click);
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.BackColor = System.Drawing.Color.White;
			this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
			this.button2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
			this.button2.Location = new System.Drawing.Point(206, 3);
			this.button2.Name = "button2";
			this.button2.OffImageType = Sardauscan.Gui.Controls.eOffButtonType.NotSelected;
			this.button2.On = true;
			this.button2.Size = new System.Drawing.Size(30, 32);
			this.button2.TabIndex = 1;
			this.button2.Tag = "3";
			this.button2.Click += new System.EventHandler(this.button_Click);
			// 
			// button3
			// 
			this.button3.BackColor = System.Drawing.Color.White;
			this.button3.Cursor = System.Windows.Forms.Cursors.Hand;
			this.button3.ForeColor = System.Drawing.SystemColors.ControlText;
			this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
			this.button3.Location = new System.Drawing.Point(68, 3);
			this.button3.Name = "button3";
			this.button3.OffImageType = Sardauscan.Gui.Controls.eOffButtonType.NotSelected;
			this.button3.On = true;
			this.button3.Size = new System.Drawing.Size(30, 32);
			this.button3.TabIndex = 2;
			this.button3.Tag = "1";
			this.button3.Click += new System.EventHandler(this.button_Click);
			// 
			// RightButton
			// 
			this.RightButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.RightButton.BackColor = System.Drawing.Color.White;
			this.RightButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.RightButton.ForeColor = System.Drawing.SystemColors.ControlText;
			this.RightButton.Image = ((System.Drawing.Image)(resources.GetObject("RightButton.Image")));
			this.RightButton.Location = new System.Drawing.Point(140, 3);
			this.RightButton.Name = "RightButton";
			this.RightButton.OffImageType = Sardauscan.Gui.Controls.eOffButtonType.NotSelected;
			this.RightButton.On = true;
			this.RightButton.Size = new System.Drawing.Size(30, 32);
			this.RightButton.TabIndex = 0;
			this.RightButton.Tag = "2";
			this.RightButton.Click += new System.EventHandler(this.button_Click);
			// 
			// LaserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "LaserControl";
			this.Size = new System.Drawing.Size(239, 38);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

	}
}
