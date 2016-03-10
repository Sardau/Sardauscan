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
using Sardauscan.Gui.Controls;
namespace Sardauscan.Gui
{
	/// <summary>
	/// Tune Laser Detection command control
	/// </summary>
    partial class TuneSmallView
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
			this.ThresholdUpDown = new System.Windows.Forms.NumericUpDown();
			this.LaserComboBox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.MinWidthUpDown = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.MaxWidthUpDown = new System.Windows.Forms.NumericUpDown();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.panel4 = new System.Windows.Forms.Panel();
			this.StartStopButton = new Sardauscan.Gui.Controls.ImageButton();
			((System.ComponentModel.ISupportInitialize)(this.ThresholdUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.MinWidthUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.MaxWidthUpDown)).BeginInit();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.panel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// ThresholdUpDown
			// 
			this.ThresholdUpDown.DecimalPlaces = 2;
			this.ThresholdUpDown.Location = new System.Drawing.Point(68, 4);
			this.ThresholdUpDown.Name = "ThresholdUpDown";
			this.ThresholdUpDown.Size = new System.Drawing.Size(49, 20);
			this.ThresholdUpDown.TabIndex = 2;
			this.ThresholdUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.ThresholdUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.ThresholdUpDown.ValueChanged += new System.EventHandler(this.ThresholdUpDown_ValueChanged);
			// 
			// LaserComboBox
			// 
			this.LaserComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.LaserComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.LaserComboBox.FormattingEnabled = true;
			this.LaserComboBox.Location = new System.Drawing.Point(68, 4);
			this.LaserComboBox.Margin = new System.Windows.Forms.Padding(0);
			this.LaserComboBox.Name = "LaserComboBox";
			this.LaserComboBox.Size = new System.Drawing.Size(98, 21);
			this.LaserComboBox.TabIndex = 3;
			this.LaserComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.LaserComboBox_DrawItem);
			this.LaserComboBox.SelectedIndexChanged += new System.EventHandler(this.LaserComboBox_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(54, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Threshold";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 7);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(55, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Min Width";
			// 
			// MinWidthUpDown
			// 
			this.MinWidthUpDown.Location = new System.Drawing.Point(68, 3);
			this.MinWidthUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.MinWidthUpDown.Name = "MinWidthUpDown";
			this.MinWidthUpDown.Size = new System.Drawing.Size(49, 20);
			this.MinWidthUpDown.TabIndex = 5;
			this.MinWidthUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.MinWidthUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.MinWidthUpDown.ValueChanged += new System.EventHandler(this.ThresholdUpDown_ValueChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 5);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(58, 13);
			this.label3.TabIndex = 8;
			this.label3.Text = "Max Width";
			// 
			// MaxWidthUpDown
			// 
			this.MaxWidthUpDown.Location = new System.Drawing.Point(68, 3);
			this.MaxWidthUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.MaxWidthUpDown.Name = "MaxWidthUpDown";
			this.MaxWidthUpDown.Size = new System.Drawing.Size(49, 20);
			this.MaxWidthUpDown.TabIndex = 7;
			this.MaxWidthUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.MaxWidthUpDown.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
			this.MaxWidthUpDown.ValueChanged += new System.EventHandler(this.ThresholdUpDown_ValueChanged);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.ThresholdUpDown);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(170, 26);
			this.panel1.TabIndex = 19;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.label2);
			this.panel2.Controls.Add(this.MinWidthUpDown);
			this.panel2.Location = new System.Drawing.Point(0, 26);
			this.panel2.Margin = new System.Windows.Forms.Padding(0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(170, 26);
			this.panel2.TabIndex = 20;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.MaxWidthUpDown);
			this.panel3.Controls.Add(this.label3);
			this.panel3.Location = new System.Drawing.Point(0, 52);
			this.panel3.Margin = new System.Windows.Forms.Padding(0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(170, 26);
			this.panel3.TabIndex = 21;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 7);
			this.label4.Margin = new System.Windows.Forms.Padding(0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(33, 13);
			this.label4.TabIndex = 22;
			this.label4.Text = "Laser";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(229, 163);
			this.tableLayoutPanel1.TabIndex = 23;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.panel1);
			this.flowLayoutPanel1.Controls.Add(this.panel2);
			this.flowLayoutPanel1.Controls.Add(this.panel3);
			this.flowLayoutPanel1.Controls.Add(this.panel4);
			this.flowLayoutPanel1.Controls.Add(this.StartStopButton);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(17, 3);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(194, 157);
			this.flowLayoutPanel1.TabIndex = 24;
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.label4);
			this.panel4.Controls.Add(this.LaserComboBox);
			this.panel4.Location = new System.Drawing.Point(0, 78);
			this.panel4.Margin = new System.Windows.Forms.Padding(0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(170, 30);
			this.panel4.TabIndex = 24;
			// 
			// StartStopButton
			// 
			this.StartStopButton.BackColor = System.Drawing.Color.Transparent;
			this.StartStopButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.StartStopButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(55)))), ((int)(((byte)(85)))));
			this.StartStopButton.Image = global::Sardauscan.Properties.Resources.Thunder;
			this.StartStopButton.Location = new System.Drawing.Point(3, 111);
			this.StartStopButton.Name = "StartStopButton";
			this.StartStopButton.Size = new System.Drawing.Size(167, 46);
			this.StartStopButton.TabIndex = 25;
			this.StartStopButton.Text = "Test Settings";
			// 
			// TuneSmallView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "TuneSmallView";
			this.Size = new System.Drawing.Size(229, 163);
			this.Load += new System.EventHandler(this.DebugImageControl_Load);
			this.VisibleChanged += new System.EventHandler(this.TuneView_VisibleChanged);
			((System.ComponentModel.ISupportInitialize)(this.ThresholdUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.MinWidthUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.MaxWidthUpDown)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

				private System.Windows.Forms.NumericUpDown ThresholdUpDown;
        private System.Windows.Forms.ComboBox LaserComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown MinWidthUpDown;
        private System.Windows.Forms.Label label3;
				private System.Windows.Forms.NumericUpDown MaxWidthUpDown;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
				private System.Windows.Forms.Panel panel3;
				private System.Windows.Forms.Label label4;
				private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
				private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
				private System.Windows.Forms.Panel panel4;
				private ImageButton StartStopButton;
    }
}
