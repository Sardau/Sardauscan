#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
namespace Sardauscan.Gui
{
	partial class CalibrationSmallView
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
			this.FlowPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.SuspendLayout();
			// 
			// FlowPanel
			// 
			this.FlowPanel.AutoSize = true;
			this.FlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.FlowPanel.Location = new System.Drawing.Point(0, 0);
			this.FlowPanel.Name = "FlowPanel";
			this.FlowPanel.Size = new System.Drawing.Size(189, 75);
			this.FlowPanel.TabIndex = 5;
			this.FlowPanel.SizeChanged += new System.EventHandler(this.FlowSizeChanged);
			// 
			// CalibrationSmallView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.Controls.Add(this.FlowPanel);
			this.Name = "CalibrationSmallView";
			this.Size = new System.Drawing.Size(189, 75);
			this.Load += new System.EventHandler(this.CalibrationSmallView_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel FlowPanel;

	}
}
