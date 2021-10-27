#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
namespace Sardauscan.Hardware.Gui
{
	/// <summary>
	/// Proxy For default sardauscan Firmware
	/// </summary>
	partial class SardauscanProxyControl
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
			this.m_LaserView = new Sardauscan.Gui.Controls.LaserControl();
			this.m_TurnTableView = new Sardauscan.Gui.Controls.TurnTableControl();
			this.SuspendLayout();
			// 
			// m_LaserView
			// 
			this.m_LaserView.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.m_LaserView.Proxy = null;
			this.m_LaserView.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.m_LaserView.Location = new System.Drawing.Point(0, 53);
			this.m_LaserView.Name = "m_LaserView";
			this.m_LaserView.Size = new System.Drawing.Size(199, 37);
			this.m_LaserView.TabIndex = 0;
			// 
			// m_TurnTableView
			// 
			this.m_TurnTableView.Proxy = null;
			this.m_TurnTableView.Cursor = System.Windows.Forms.Cursors.VSplit;
			this.m_TurnTableView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_TurnTableView.ForeColor = System.Drawing.SystemColors.Highlight;
			this.m_TurnTableView.Location = new System.Drawing.Point(0, 0);
			this.m_TurnTableView.Name = "m_TurnTableView";
			this.m_TurnTableView.Size = new System.Drawing.Size(199, 53);
			this.m_TurnTableView.TabIndex = 1;
			// 
			// DefaultArduinoProxyView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.m_TurnTableView);
			this.Controls.Add(this.m_LaserView);
			this.Name = "DefaultArduinoProxyView";
			this.Size = new System.Drawing.Size(199, 90);
			this.ResumeLayout(false);

		}

		#endregion

		private Sardauscan.Gui.Controls.LaserControl m_LaserView;
		private Sardauscan.Gui.Controls.TurnTableControl m_TurnTableView;
	}
}
