#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
namespace Sardauscan.Gui.Controls
{
	partial class MainToolBox
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
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.SettingsPanel = new System.Windows.Forms.Panel();
			this.viewSelector1 = new Sardauscan.Gui.Controls.ApplicationView.ViewSelector();
			this.hardwareStatusControl1 = new Sardauscan.Gui.Controls.HardwareStatusControl();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.viewSelector1);
			this.flowLayoutPanel1.Controls.Add(this.hardwareStatusControl1);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(236, 33);
			this.flowLayoutPanel1.TabIndex = 2;
			// 
			// SettingsPanel
			// 
			this.SettingsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.SettingsPanel.Location = new System.Drawing.Point(0, 33);
			this.SettingsPanel.Margin = new System.Windows.Forms.Padding(0);
			this.SettingsPanel.Name = "SettingsPanel";
			this.SettingsPanel.Size = new System.Drawing.Size(236, 10);
			this.SettingsPanel.TabIndex = 3;
			this.SettingsPanel.SizeChanged += new System.EventHandler(this.SettingsPanel_SizeChanged);
			// 
			// viewSelector1
			// 
			this.viewSelector1.AutoSize = true;
			this.viewSelector1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.viewSelector1.Controler = null;
			this.viewSelector1.Location = new System.Drawing.Point(0, 0);
			this.viewSelector1.Margin = new System.Windows.Forms.Padding(0);
			this.viewSelector1.Name = "viewSelector1";
			this.viewSelector1.Size = new System.Drawing.Size(96, 32);
			this.viewSelector1.TabIndex = 1;
			// 
			// hardwareStatusControl1
			// 
			this.hardwareStatusControl1.AutoSize = true;
			this.hardwareStatusControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.hardwareStatusControl1.Location = new System.Drawing.Point(96, 0);
			this.hardwareStatusControl1.Margin = new System.Windows.Forms.Padding(0);
			this.hardwareStatusControl1.Name = "hardwareStatusControl1";
			this.hardwareStatusControl1.Size = new System.Drawing.Size(103, 33);
			this.hardwareStatusControl1.TabIndex = 0;
			// 
			// MainToolBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.SettingsPanel);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Name = "MainToolBox";
			this.Size = new System.Drawing.Size(236, 45);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private HardwareStatusControl hardwareStatusControl1;
		private ApplicationView.ViewSelector viewSelector1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Panel SettingsPanel;
	}
}
