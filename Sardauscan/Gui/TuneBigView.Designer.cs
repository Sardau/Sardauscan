#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
namespace Sardauscan.Gui
{
	partial class TuneBigView
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.splitContainer3 = new System.Windows.Forms.SplitContainer();
			this.ResultPictureBox = new System.Windows.Forms.PictureBox();
			this.ReferencePictureBox = new System.Windows.Forms.PictureBox();
			this.LaserPictureBox = new System.Windows.Forms.PictureBox();
			this.DifferencePictureBox = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ResultPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ReferencePictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.LaserPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.DifferencePictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.ResultPictureBox);
			this.splitContainer1.Size = new System.Drawing.Size(147, 132);
			this.splitContainer1.SplitterDistance = 42;
			this.splitContainer1.TabIndex = 0;
			// 
			// splitContainer2
			// 
			this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.ReferencePictureBox);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
			this.splitContainer2.Size = new System.Drawing.Size(147, 42);
			this.splitContainer2.SplitterDistance = 49;
			this.splitContainer2.TabIndex = 0;
			// 
			// splitContainer3
			// 
			this.splitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer3.Location = new System.Drawing.Point(0, 0);
			this.splitContainer3.Name = "splitContainer3";
			// 
			// splitContainer3.Panel1
			// 
			this.splitContainer3.Panel1.Controls.Add(this.LaserPictureBox);
			// 
			// splitContainer3.Panel2
			// 
			this.splitContainer3.Panel2.Controls.Add(this.DifferencePictureBox);
			this.splitContainer3.Size = new System.Drawing.Size(94, 42);
			this.splitContainer3.SplitterDistance = 44;
			this.splitContainer3.TabIndex = 0;
			// 
			// ResultPictureBox
			// 
			this.ResultPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ResultPictureBox.Image = global::Sardauscan.Properties.Resources.Denied;
			this.ResultPictureBox.Location = new System.Drawing.Point(0, 0);
			this.ResultPictureBox.Name = "ResultPictureBox";
			this.ResultPictureBox.Size = new System.Drawing.Size(145, 84);
			this.ResultPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.ResultPictureBox.TabIndex = 0;
			this.ResultPictureBox.TabStop = false;
			// 
			// ReferencePictureBox
			// 
			this.ReferencePictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ReferencePictureBox.Image = global::Sardauscan.Properties.Resources.Denied;
			this.ReferencePictureBox.Location = new System.Drawing.Point(0, 0);
			this.ReferencePictureBox.Name = "ReferencePictureBox";
			this.ReferencePictureBox.Size = new System.Drawing.Size(47, 40);
			this.ReferencePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.ReferencePictureBox.TabIndex = 1;
			this.ReferencePictureBox.TabStop = false;
			// 
			// LaserPictureBox
			// 
			this.LaserPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LaserPictureBox.Image = global::Sardauscan.Properties.Resources.Denied;
			this.LaserPictureBox.Location = new System.Drawing.Point(0, 0);
			this.LaserPictureBox.Name = "LaserPictureBox";
			this.LaserPictureBox.Size = new System.Drawing.Size(42, 40);
			this.LaserPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.LaserPictureBox.TabIndex = 1;
			this.LaserPictureBox.TabStop = false;
			// 
			// DifferencePictureBox
			// 
			this.DifferencePictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DifferencePictureBox.Image = global::Sardauscan.Properties.Resources.Denied;
			this.DifferencePictureBox.Location = new System.Drawing.Point(0, 0);
			this.DifferencePictureBox.Name = "DifferencePictureBox";
			this.DifferencePictureBox.Size = new System.Drawing.Size(44, 40);
			this.DifferencePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.DifferencePictureBox.TabIndex = 1;
			this.DifferencePictureBox.TabStop = false;
			// 
			// TuneBigView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Name = "TuneBigView";
			this.Size = new System.Drawing.Size(147, 132);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
			this.splitContainer3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ResultPictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ReferencePictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.LaserPictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.DifferencePictureBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.PictureBox ReferencePictureBox;
		private System.Windows.Forms.SplitContainer splitContainer3;
		private System.Windows.Forms.PictureBox LaserPictureBox;
		private System.Windows.Forms.PictureBox DifferencePictureBox;
		private System.Windows.Forms.PictureBox ResultPictureBox;
	}
}
