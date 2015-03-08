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
	partial class CorrectionMatrix
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
			this.PreviewPanel = new System.Windows.Forms.Panel();
			this.LaserComboBox = new System.Windows.Forms.ComboBox();
			this.ProgressBar = new System.Windows.Forms.ProgressBar();
			this.LaserCommandPanel = new System.Windows.Forms.Panel();
			this.imageButton1 = new Sardauscan.Gui.Controls.ImageButton();
			this.pictureBox7 = new System.Windows.Forms.PictureBox();
			this.pictureBox6 = new System.Windows.Forms.PictureBox();
			this.pictureBox5 = new System.Windows.Forms.PictureBox();
			this.pictureBox4 = new System.Windows.Forms.PictureBox();
			this.pictureBox3 = new System.Windows.Forms.PictureBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.ReInitButton = new Sardauscan.Gui.Controls.ImageButton();
			this.BackgroundWorker = new System.ComponentModel.BackgroundWorker();
			this.panel1 = new System.Windows.Forms.Panel();
			this.QuickScanButton = new Sardauscan.Gui.Controls.ImageButton();
			this.LoadButton = new Sardauscan.Gui.Controls.ImageButton();
			this.LaserCommandPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// PreviewPanel
			// 
			this.PreviewPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PreviewPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.PreviewPanel.Location = new System.Drawing.Point(3, 3);
			this.PreviewPanel.Name = "PreviewPanel";
			this.PreviewPanel.Size = new System.Drawing.Size(418, 363);
			this.PreviewPanel.TabIndex = 1;
			this.PreviewPanel.SizeChanged += new System.EventHandler(this.PreviewPanel_SizeChanged);
			this.PreviewPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.PreviewPanel_Paint);
			this.PreviewPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PreviewPanel_MouseUp);
			// 
			// LaserComboBox
			// 
			this.LaserComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LaserComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.LaserComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.LaserComboBox.FormattingEnabled = true;
			this.LaserComboBox.Location = new System.Drawing.Point(3, 5);
			this.LaserComboBox.Name = "LaserComboBox";
			this.LaserComboBox.Size = new System.Drawing.Size(147, 21);
			this.LaserComboBox.TabIndex = 6;
			this.LaserComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.LaserComboBox_DrawItem);
			this.LaserComboBox.SelectedIndexChanged += new System.EventHandler(this.LaserComboBox_SelectedIndexChanged);
			// 
			// ProgressBar
			// 
			this.ProgressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.ProgressBar.Location = new System.Drawing.Point(0, 47);
			this.ProgressBar.Name = "ProgressBar";
			this.ProgressBar.Size = new System.Drawing.Size(151, 18);
			this.ProgressBar.TabIndex = 9;
			// 
			// LaserCommandPanel
			// 
			this.LaserCommandPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.LaserCommandPanel.Controls.Add(this.imageButton1);
			this.LaserCommandPanel.Controls.Add(this.pictureBox7);
			this.LaserCommandPanel.Controls.Add(this.pictureBox6);
			this.LaserCommandPanel.Controls.Add(this.pictureBox5);
			this.LaserCommandPanel.Controls.Add(this.pictureBox4);
			this.LaserCommandPanel.Controls.Add(this.pictureBox3);
			this.LaserCommandPanel.Controls.Add(this.pictureBox2);
			this.LaserCommandPanel.Controls.Add(this.ReInitButton);
			this.LaserCommandPanel.Controls.Add(this.LaserComboBox);
			this.LaserCommandPanel.Location = new System.Drawing.Point(427, 74);
			this.LaserCommandPanel.Name = "LaserCommandPanel";
			this.LaserCommandPanel.Size = new System.Drawing.Size(151, 283);
			this.LaserCommandPanel.TabIndex = 10;
			// 
			// imageButton1
			// 
			this.imageButton1.BackColor = System.Drawing.Color.Transparent;
			this.imageButton1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.imageButton1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.imageButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(55)))), ((int)(((byte)(85)))));
			this.imageButton1.Image = global::Sardauscan.Properties.Resources.Redo;
			this.imageButton1.Location = new System.Drawing.Point(0, 203);
			this.imageButton1.Name = "imageButton1";
			this.imageButton1.Size = new System.Drawing.Size(151, 40);
			this.imageButton1.TabIndex = 13;
			this.imageButton1.Text = "Clear this laser";
			this.imageButton1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.imageButton1.Click += new System.EventHandler(this.imageButton1_Click);
			// 
			// pictureBox7
			// 
			this.pictureBox7.Image = global::Sardauscan.Properties.Resources.Zoom;
			this.pictureBox7.Location = new System.Drawing.Point(74, 147);
			this.pictureBox7.Name = "pictureBox7";
			this.pictureBox7.Size = new System.Drawing.Size(74, 54);
			this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox7.TabIndex = 12;
			this.pictureBox7.TabStop = false;
			// 
			// pictureBox6
			// 
			this.pictureBox6.Image = global::Sardauscan.Properties.Resources.Corner_Arrow;
			this.pictureBox6.Location = new System.Drawing.Point(74, 90);
			this.pictureBox6.Name = "pictureBox6";
			this.pictureBox6.Size = new System.Drawing.Size(74, 53);
			this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox6.TabIndex = 11;
			this.pictureBox6.TabStop = false;
			// 
			// pictureBox5
			// 
			this.pictureBox5.Image = global::Sardauscan.Properties.Resources.Reload3;
			this.pictureBox5.Location = new System.Drawing.Point(74, 32);
			this.pictureBox5.Name = "pictureBox5";
			this.pictureBox5.Size = new System.Drawing.Size(74, 53);
			this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox5.TabIndex = 10;
			this.pictureBox5.TabStop = false;
			// 
			// pictureBox4
			// 
			this.pictureBox4.Image = global::Sardauscan.Properties.Resources.MouseZoom;
			this.pictureBox4.Location = new System.Drawing.Point(3, 148);
			this.pictureBox4.Name = "pictureBox4";
			this.pictureBox4.Size = new System.Drawing.Size(74, 53);
			this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox4.TabIndex = 9;
			this.pictureBox4.TabStop = false;
			// 
			// pictureBox3
			// 
			this.pictureBox3.Image = global::Sardauscan.Properties.Resources.MouseTranslate;
			this.pictureBox3.Location = new System.Drawing.Point(3, 90);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new System.Drawing.Size(74, 53);
			this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox3.TabIndex = 8;
			this.pictureBox3.TabStop = false;
			// 
			// pictureBox2
			// 
			this.pictureBox2.Image = global::Sardauscan.Properties.Resources.MouseRotate;
			this.pictureBox2.Location = new System.Drawing.Point(3, 32);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(74, 53);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox2.TabIndex = 7;
			this.pictureBox2.TabStop = false;
			// 
			// ReInitButton
			// 
			this.ReInitButton.BackColor = System.Drawing.Color.Transparent;
			this.ReInitButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.ReInitButton.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.ReInitButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(55)))), ((int)(((byte)(85)))));
			this.ReInitButton.Image = global::Sardauscan.Properties.Resources.Redo2;
			this.ReInitButton.Location = new System.Drawing.Point(0, 243);
			this.ReInitButton.Name = "ReInitButton";
			this.ReInitButton.Size = new System.Drawing.Size(151, 40);
			this.ReInitButton.TabIndex = 5;
			this.ReInitButton.Text = "Clear all lasers";
			this.ReInitButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.ReInitButton.Click += new System.EventHandler(this.ReInitButton_Click);
			// 
			// BackgroundWorker
			// 
			this.BackgroundWorker.WorkerReportsProgress = true;
			this.BackgroundWorker.WorkerSupportsCancellation = true;
			this.BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
			this.BackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorker_ProgressChanged);
			this.BackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.LoadButton);
			this.panel1.Controls.Add(this.QuickScanButton);
			this.panel1.Controls.Add(this.ProgressBar);
			this.panel1.Location = new System.Drawing.Point(426, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(151, 65);
			this.panel1.TabIndex = 13;
			// 
			// QuickScanButton
			// 
			this.QuickScanButton.BackColor = System.Drawing.Color.Transparent;
			this.QuickScanButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.QuickScanButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(55)))), ((int)(((byte)(85)))));
			this.QuickScanButton.Image = global::Sardauscan.Properties.Resources.Forward;
			this.QuickScanButton.Location = new System.Drawing.Point(0, 0);
			this.QuickScanButton.Name = "QuickScanButton";
			this.QuickScanButton.Size = new System.Drawing.Size(109, 44);
			this.QuickScanButton.TabIndex = 8;
			this.QuickScanButton.Text = "Quick Scan";
			this.QuickScanButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.QuickScanButton.Click += new System.EventHandler(this.QuickScanButton_Click);
			// 
			// LoadButton
			// 
			this.LoadButton.BackColor = System.Drawing.Color.Transparent;
			this.LoadButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.LoadButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(55)))), ((int)(((byte)(85)))));
			this.LoadButton.Image = global::Sardauscan.Properties.Resources.Load;
			this.LoadButton.Location = new System.Drawing.Point(112, 6);
			this.LoadButton.Name = "LoadButton";
			this.LoadButton.Size = new System.Drawing.Size(36, 35);
			this.LoadButton.TabIndex = 10;
			this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
			// 
			// CorrectionMatrix
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.LaserCommandPanel);
			this.Controls.Add(this.PreviewPanel);
			this.DoubleBuffered = true;
			this.Name = "CorrectionMatrix";
			this.Size = new System.Drawing.Size(581, 369);
			this.Load += new System.EventHandler(this.CorrectionMatrix_Load);
			this.LaserCommandPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel PreviewPanel;
		private Controls.ImageButton ReInitButton;
		private System.Windows.Forms.ComboBox LaserComboBox;
		private Controls.ImageButton QuickScanButton;
		private System.Windows.Forms.ProgressBar ProgressBar;
		private System.Windows.Forms.Panel LaserCommandPanel;
		private System.ComponentModel.BackgroundWorker BackgroundWorker;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.PictureBox pictureBox7;
		private System.Windows.Forms.PictureBox pictureBox6;
		private System.Windows.Forms.PictureBox pictureBox5;
		private System.Windows.Forms.PictureBox pictureBox4;
		private System.Windows.Forms.PictureBox pictureBox3;
		private System.Windows.Forms.PictureBox pictureBox2;
		private Controls.ImageButton imageButton1;
		private Controls.ImageButton LoadButton;
	}
}
