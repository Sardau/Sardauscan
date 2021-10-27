#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
using Sardauscan.Gui.Controls;
using Sardauscan.Gui.OpenGL;
namespace Sardauscan.Gui
{
	/// <summary>
	/// Main Form of the application
	/// </summary>
    partial class MainForm
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
			Sardauscan.Core.Geometry.Scene3D scene3D1 = new Sardauscan.Core.Geometry.Scene3D();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.OpenPointsDlg = new System.Windows.Forms.OpenFileDialog();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.mainToolBox1 = new Sardauscan.Gui.Controls.MainToolBox();
			this.SmallViewPanel = new System.Windows.Forms.Panel();
			this.CalibrationSmallView = new Sardauscan.Gui.CalibrationSmallView();
			this.TuneSmallView = new Sardauscan.Gui.TuneSmallView();
			this.ProcessView = new Sardauscan.Gui.ToolBoxView();
			this.BigViewPanel = new System.Windows.Forms.Panel();
			this.CalibrationBigView = new Sardauscan.Gui.CalibrationBigView();
			this.TuneBigView = new Sardauscan.Gui.TuneBigView();
			this.Scene3DView = new Sardauscan.Gui.OpenGL.Scene3DControl();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SmallViewPanel.SuspendLayout();
			this.BigViewPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// OpenPointsDlg
			// 
			this.OpenPointsDlg.DefaultExt = "cpa";
			this.OpenPointsDlg.Filter = "Point Array(*.cpa)|*.cpa";
			// 
			// splitContainer1
			// 
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.mainToolBox1);
			this.splitContainer1.Panel1.Controls.Add(this.SmallViewPanel);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.BigViewPanel);
			this.splitContainer1.Size = new System.Drawing.Size(861, 570);
			this.splitContainer1.SplitterDistance = 305;
			this.splitContainer1.SplitterWidth = 5;
			this.splitContainer1.TabIndex = 13;
			// 
			// mainToolBox1
			// 
			this.mainToolBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.mainToolBox1.Controler = null;
			this.mainToolBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.mainToolBox1.Location = new System.Drawing.Point(0, 0);
			this.mainToolBox1.Name = "mainToolBox1";
			this.mainToolBox1.Size = new System.Drawing.Size(303, 43);
			this.mainToolBox1.TabIndex = 1;
			this.mainToolBox1.SizeChanged += new System.EventHandler(this.mainToolBox1_SizeChanged);
			// 
			// SmallViewPanel
			// 
			this.SmallViewPanel.Controls.Add(this.CalibrationSmallView);
			this.SmallViewPanel.Controls.Add(this.TuneSmallView);
			this.SmallViewPanel.Controls.Add(this.ProcessView);
			this.SmallViewPanel.Location = new System.Drawing.Point(3, 43);
			this.SmallViewPanel.Name = "SmallViewPanel";
			this.SmallViewPanel.Size = new System.Drawing.Size(299, 524);
			this.SmallViewPanel.TabIndex = 2;
			// 
			// CalibrationSmallView
			// 
			this.CalibrationSmallView.AutoSize = true;
			this.CalibrationSmallView.Location = new System.Drawing.Point(19, 221);
			this.CalibrationSmallView.Name = "CalibrationSmallView";
			this.CalibrationSmallView.Size = new System.Drawing.Size(80, 69);
			this.CalibrationSmallView.TabIndex = 2;
			this.CalibrationSmallView.Viewer = null;
			// 
			// TuneSmallView
			// 
			this.TuneSmallView.BackColor = System.Drawing.Color.White;
			this.TuneSmallView.Location = new System.Drawing.Point(19, 112);
			this.TuneSmallView.Name = "TuneSmallView";
			this.TuneSmallView.Size = new System.Drawing.Size(105, 78);
			this.TuneSmallView.TabIndex = 1;
			this.TuneSmallView.Viewer = null;
			// 
			// ProcessView
			// 
			this.ProcessView.Location = new System.Drawing.Point(8, 3);
			this.ProcessView.Name = "ProcessView";
			this.ProcessView.Size = new System.Drawing.Size(288, 258);
			this.ProcessView.TabIndex = 0;
			// 
			// BigViewPanel
			// 
			this.BigViewPanel.Controls.Add(this.CalibrationBigView);
			this.BigViewPanel.Controls.Add(this.TuneBigView);
			this.BigViewPanel.Controls.Add(this.Scene3DView);
			this.BigViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BigViewPanel.Location = new System.Drawing.Point(0, 0);
			this.BigViewPanel.Name = "BigViewPanel";
			this.BigViewPanel.Size = new System.Drawing.Size(549, 568);
			this.BigViewPanel.TabIndex = 12;
			// 
			// CalibrationBigView
			// 
			this.CalibrationBigView.Location = new System.Drawing.Point(378, 17);
			this.CalibrationBigView.Name = "CalibrationBigView";
			this.CalibrationBigView.Size = new System.Drawing.Size(135, 132);
			this.CalibrationBigView.TabIndex = 13;
			// 
			// TuneBigView
			// 
			this.TuneBigView.Location = new System.Drawing.Point(190, 17);
			this.TuneBigView.Name = "TuneBigView";
			this.TuneBigView.Size = new System.Drawing.Size(147, 132);
			this.TuneBigView.TabIndex = 12;
			// 
			// Scene3DView
			// 
			this.Scene3DView.AutoScrollMargin = new System.Drawing.Size(42, 0);
			this.Scene3DView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(170)))), ((int)(((byte)(255)))));
			this.Scene3DView.ForeColor = System.Drawing.Color.White;
			this.Scene3DView.Location = new System.Drawing.Point(17, 17);
			this.Scene3DView.Name = "Scene3DView";
			this.Scene3DView.Scene = scene3D1;
			this.Scene3DView.ShowDefaultScene = false;
			this.Scene3DView.ShowFullOnClick = false;
			this.Scene3DView.ShowSettingsButton = true;
			this.Scene3DView.Size = new System.Drawing.Size(147, 132);
			this.Scene3DView.TabIndex = 10;
			this.Scene3DView.VSync = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(861, 570);
			this.Controls.Add(this.splitContainer1);
			this.DoubleBuffered = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Sardauscan";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.SmallViewPanel.ResumeLayout(false);
			this.SmallViewPanel.PerformLayout();
			this.BigViewPanel.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

				private System.Windows.Forms.OpenFileDialog OpenPointsDlg;
				private ToolBoxView ProcessView;
				private System.Windows.Forms.SplitContainer splitContainer1;
				private Scene3DControl Scene3DView;
				private System.Windows.Forms.Panel BigViewPanel;
				private MainToolBox mainToolBox1;
				private System.Windows.Forms.Panel SmallViewPanel;
				private TuneBigView TuneBigView;
				private TuneSmallView TuneSmallView;
				private CalibrationSmallView CalibrationSmallView;
				private CalibrationBigView CalibrationBigView;
    }
}

