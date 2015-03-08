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
namespace Sardauscan.Gui.CalibrationSteps
{
	/// <summary>
	/// Scanner Dimention configuration control
	/// </summary>
	partial class Dimention
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
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.CameraZ = new System.Windows.Forms.NumericUpDown();
			this.CameraY = new System.Windows.Forms.NumericUpDown();
			this.LaserAngle = new System.Windows.Forms.NumericUpDown();
			this.LaserLabel = new System.Windows.Forms.Label();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.LaserComboBox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.LaserY = new System.Windows.Forms.NumericUpDown();
			this.LaserZ = new System.Windows.Forms.NumericUpDown();
			this.LaserX = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.CameraZ)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.CameraY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.LaserAngle)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.LaserY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.LaserZ)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.LaserX)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::Sardauscan.Properties.Resources.Calib1;
			this.pictureBox1.Location = new System.Drawing.Point(6, 3);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(506, 258);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// CameraZ
			// 
			this.CameraZ.DecimalPlaces = 1;
			this.CameraZ.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.CameraZ.Location = new System.Drawing.Point(252, 72);
			this.CameraZ.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.CameraZ.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            -2147483648});
			this.CameraZ.Name = "CameraZ";
			this.CameraZ.Size = new System.Drawing.Size(63, 20);
			this.CameraZ.TabIndex = 2;
			this.CameraZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.CameraZ.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.CameraZ.ValueChanged += new System.EventHandler(this.CameraPos_Changed);
			// 
			// CameraY
			// 
			this.CameraY.DecimalPlaces = 1;
			this.CameraY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.CameraY.Location = new System.Drawing.Point(54, 102);
			this.CameraY.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.CameraY.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            -2147483648});
			this.CameraY.Name = "CameraY";
			this.CameraY.Size = new System.Drawing.Size(59, 20);
			this.CameraY.TabIndex = 3;
			this.CameraY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.CameraY.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.CameraY.ValueChanged += new System.EventHandler(this.CameraPos_Changed);
			// 
			// LaserAngle
			// 
			this.LaserAngle.DecimalPlaces = 5;
			this.LaserAngle.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.LaserAngle.Location = new System.Drawing.Point(403, 375);
			this.LaserAngle.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
			this.LaserAngle.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
			this.LaserAngle.Name = "LaserAngle";
			this.LaserAngle.Size = new System.Drawing.Size(94, 20);
			this.LaserAngle.TabIndex = 4;
			this.LaserAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.LaserAngle.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
			this.LaserAngle.ValueChanged += new System.EventHandler(this.LaserAngle_Changed);
			// 
			// LaserLabel
			// 
			this.LaserLabel.AutoSize = true;
			this.LaserLabel.Location = new System.Drawing.Point(400, 348);
			this.LaserLabel.Name = "LaserLabel";
			this.LaserLabel.Size = new System.Drawing.Size(82, 13);
			this.LaserLabel.TabIndex = 5;
			this.LaserLabel.Text = "Laser Angle ( ° )";
			// 
			// pictureBox2
			// 
			this.pictureBox2.Image = global::Sardauscan.Properties.Resources.Calib2;
			this.pictureBox2.Location = new System.Drawing.Point(6, 268);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(442, 249);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox2.TabIndex = 1;
			this.pictureBox2.TabStop = false;
			// 
			// LaserComboBox
			// 
			this.LaserComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.LaserComboBox.FormattingEnabled = true;
			this.LaserComboBox.Location = new System.Drawing.Point(403, 313);
			this.LaserComboBox.Name = "LaserComboBox";
			this.LaserComboBox.Size = new System.Drawing.Size(94, 21);
			this.LaserComboBox.TabIndex = 6;
			this.LaserComboBox.SelectedIndexChanged += new System.EventHandler(this.CurrentLaser_Changed);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Location = new System.Drawing.Point(37, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(403, 23);
			this.label1.TabIndex = 7;
			this.label1.Text = "Camera Position (mm)";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(228, 74);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(14, 13);
			this.label2.TabIndex = 8;
			this.label2.Text = "Z";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(34, 104);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(14, 13);
			this.label3.TabIndex = 9;
			this.label3.Text = "Y";
			// 
			// LaserY
			// 
			this.LaserY.DecimalPlaces = 1;
			this.LaserY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.LaserY.Location = new System.Drawing.Point(434, 461);
			this.LaserY.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.LaserY.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            -2147483648});
			this.LaserY.Name = "LaserY";
			this.LaserY.Size = new System.Drawing.Size(63, 20);
			this.LaserY.TabIndex = 10;
			this.LaserY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.LaserY.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.LaserY.ValueChanged += new System.EventHandler(this.LaserPosition_Changed);
			// 
			// LaserZ
			// 
			this.LaserZ.DecimalPlaces = 1;
			this.LaserZ.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.LaserZ.Location = new System.Drawing.Point(434, 490);
			this.LaserZ.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.LaserZ.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            -2147483648});
			this.LaserZ.Name = "LaserZ";
			this.LaserZ.Size = new System.Drawing.Size(63, 20);
			this.LaserZ.TabIndex = 11;
			this.LaserZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.LaserZ.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.LaserZ.ValueChanged += new System.EventHandler(this.LaserPosition_Changed);
			// 
			// LaserX
			// 
			this.LaserX.DecimalPlaces = 1;
			this.LaserX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.LaserX.Location = new System.Drawing.Point(434, 432);
			this.LaserX.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.LaserX.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            -2147483648});
			this.LaserX.Name = "LaserX";
			this.LaserX.Size = new System.Drawing.Size(63, 20);
			this.LaserX.TabIndex = 12;
			this.LaserX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.LaserX.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.LaserX.ValueChanged += new System.EventHandler(this.LaserPosition_Changed);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(400, 408);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(97, 13);
			this.label4.TabIndex = 13;
			this.label4.Text = "Laser position (mm)";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(400, 463);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(14, 13);
			this.label5.TabIndex = 14;
			this.label5.Text = "Y";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(400, 492);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(14, 13);
			this.label6.TabIndex = 14;
			this.label6.Text = "Z\r\n";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(400, 434);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(14, 13);
			this.label7.TabIndex = 14;
			this.label7.Text = "X";
			// 
			// Dimention
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.LaserX);
			this.Controls.Add(this.LaserZ);
			this.Controls.Add(this.LaserY);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.LaserComboBox);
			this.Controls.Add(this.LaserLabel);
			this.Controls.Add(this.LaserAngle);
			this.Controls.Add(this.CameraY);
			this.Controls.Add(this.CameraZ);
			this.Controls.Add(this.pictureBox2);
			this.Controls.Add(this.pictureBox1);
			this.Name = "Dimention";
			this.Size = new System.Drawing.Size(523, 527);
			this.Load += new System.EventHandler(this.Dimention_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.CameraZ)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.CameraY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.LaserAngle)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.LaserY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.LaserZ)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.LaserX)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.NumericUpDown CameraZ;
		private System.Windows.Forms.NumericUpDown CameraY;
		private System.Windows.Forms.NumericUpDown LaserAngle;
		private System.Windows.Forms.Label LaserLabel;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.ComboBox LaserComboBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown LaserY;
		private System.Windows.Forms.NumericUpDown LaserZ;
		private System.Windows.Forms.NumericUpDown LaserX;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
	}
}
