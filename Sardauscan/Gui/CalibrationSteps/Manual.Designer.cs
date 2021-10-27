#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
namespace Sardauscan.Gui.CalibrationSteps
{
	/// <summary>
	/// Scanner physical xonfiguration helper
	/// </summary>
	partial class Manual
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Manual));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.HelpText1 = new System.Windows.Forms.TextBox();
			this.HelpText2 = new System.Windows.Forms.TextBox();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.ViewPanel = new System.Windows.Forms.Panel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.LaserControl = new Sardauscan.Gui.Controls.LaserControl();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox1.Image = global::Sardauscan.Properties.Resources.Calib3;
			this.pictureBox1.Location = new System.Drawing.Point(3, 3);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(217, 104);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			// 
			// pictureBox2
			// 
			this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox2.Image = global::Sardauscan.Properties.Resources.Calib4;
			this.pictureBox2.Location = new System.Drawing.Point(3, 179);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(217, 104);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox2.TabIndex = 2;
			this.pictureBox2.TabStop = false;
			// 
			// HelpText1
			// 
			this.HelpText1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.HelpText1.Cursor = System.Windows.Forms.Cursors.Default;
			this.HelpText1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.HelpText1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.HelpText1.Location = new System.Drawing.Point(3, 113);
			this.HelpText1.Multiline = true;
			this.HelpText1.Name = "HelpText1";
			this.HelpText1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.HelpText1.Size = new System.Drawing.Size(217, 60);
			this.HelpText1.TabIndex = 3;
			this.HelpText1.Text = "- Physicaly move your Camera so that the vertical line pass through the center of" +
    " the turn table .\r\n\r\n- click te center of the turn table inthe preview window.";
			this.HelpText1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HelpText1_KeyPress);
			// 
			// HelpText2
			// 
			this.HelpText2.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.HelpText2.Cursor = System.Windows.Forms.Cursors.Default;
			this.HelpText2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.HelpText2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.HelpText2.Location = new System.Drawing.Point(3, 289);
			this.HelpText2.Multiline = true;
			this.HelpText2.Name = "HelpText2";
			this.HelpText2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.HelpText2.Size = new System.Drawing.Size(217, 150);
			this.HelpText2.TabIndex = 4;
			this.HelpText2.Text = resources.GetString("HelpText2.Text");
			this.HelpText2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HelpText1_KeyPress);
			// 
			// splitContainer1
			// 
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.LaserControl);
			this.splitContainer1.Panel1.Controls.Add(this.ViewPanel);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
			this.splitContainer1.Size = new System.Drawing.Size(615, 444);
			this.splitContainer1.SplitterDistance = 386;
			this.splitContainer1.TabIndex = 5;
			// 
			// ViewPanel
			// 
			this.ViewPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ViewPanel.Location = new System.Drawing.Point(2, 44);
			this.ViewPanel.Name = "ViewPanel";
			this.ViewPanel.Size = new System.Drawing.Size(379, 396);
			this.ViewPanel.TabIndex = 0;
			this.ViewPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.ViewPanel_Paint);
			this.ViewPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ViewPanel_MouseDown);
			this.ViewPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ViewPanel_MouseMove);
			this.ViewPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ViewPanel_MouseUp);
			this.ViewPanel.Resize += new System.EventHandler(this.ViewPanel_Resize);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.HelpText2, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.HelpText1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.pictureBox2, 0, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(223, 442);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// LaserControl
			// 
			this.LaserControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.LaserControl.Dock = System.Windows.Forms.DockStyle.Top;
			this.LaserControl.Location = new System.Drawing.Point(0, 0);
			this.LaserControl.Name = "LaserControl";
			this.LaserControl.Proxy = null;
			this.LaserControl.Size = new System.Drawing.Size(384, 38);
			this.LaserControl.TabIndex = 1;
			// 
			// Manual
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Name = "Manual";
			this.Size = new System.Drawing.Size(615, 444);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.TextBox HelpText1;
		private System.Windows.Forms.TextBox HelpText2;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel ViewPanel;
		private Controls.LaserControl LaserControl;
	}
}
