#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
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
