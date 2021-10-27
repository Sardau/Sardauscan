#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
namespace Sardauscan.Hardware.Gui
{
	partial class SardauscanSelectionControl
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
			this.ComComboBox = new System.Windows.Forms.ComboBox();
			this.m_SardauscanProxyControl = new Sardauscan.Hardware.Gui.SardauscanProxyControl();
			this.StartStopButton = new Sardauscan.Gui.Controls.ImageButton();
			this.PlugButton = new Sardauscan.Gui.Controls.ImageButton();
			this.SuspendLayout();
			// 
			// ComComboBox
			// 
			this.ComComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ComComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ComComboBox.FormattingEnabled = true;
			this.ComComboBox.Location = new System.Drawing.Point(36, 3);
			this.ComComboBox.Name = "ComComboBox";
			this.ComComboBox.Size = new System.Drawing.Size(190, 21);
			this.ComComboBox.TabIndex = 9;
			this.ComComboBox.SelectedIndexChanged += new System.EventHandler(this.ComComboBox_SelectedIndexChanged);
			// 
			// m_SardauscanProxyControl
			// 
			this.m_SardauscanProxyControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.m_SardauscanProxyControl.Location = new System.Drawing.Point(0, 35);
			this.m_SardauscanProxyControl.Name = "m_SardauscanProxyControl";
			this.m_SardauscanProxyControl.Size = new System.Drawing.Size(259, 121);
			this.m_SardauscanProxyControl.TabIndex = 11;
			// 
			// StartStopButton
			// 
			this.StartStopButton.BackColor = System.Drawing.Color.Green;
			this.StartStopButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.StartStopButton.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.StartStopButton.Image = global::Sardauscan.Properties.Resources.Reload;
			this.StartStopButton.Location = new System.Drawing.Point(1, -2);
			this.StartStopButton.Name = "StartStopButton";
			this.StartStopButton.Size = new System.Drawing.Size(29, 31);
			this.StartStopButton.TabIndex = 10;
			this.StartStopButton.Click += new System.EventHandler(this.ReloadComPort);
			// 
			// PlugButton
			// 
			this.PlugButton.BackColor = System.Drawing.Color.Green;
			this.PlugButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.PlugButton.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.PlugButton.Image = global::Sardauscan.Properties.Resources.Usb;
			this.PlugButton.Location = new System.Drawing.Point(232, -2);
			this.PlugButton.Name = "PlugButton";
			this.PlugButton.Size = new System.Drawing.Size(29, 31);
			this.PlugButton.TabIndex = 12;
			this.PlugButton.Click += new System.EventHandler(this.PlugButton_Click);
			// 
			// DefaultArduinoSelectionView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.PlugButton);
			this.Controls.Add(this.m_SardauscanProxyControl);
			this.Controls.Add(this.StartStopButton);
			this.Controls.Add(this.ComComboBox);
			this.Name = "DefaultArduinoSelectionView";
			this.Size = new System.Drawing.Size(264, 159);
			this.Load += new System.EventHandler(this.DefaultArduinoSelectionView_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private Sardauscan.Gui.Controls.ImageButton StartStopButton;
		private System.Windows.Forms.ComboBox ComComboBox;
		private SardauscanProxyControl m_SardauscanProxyControl;
		private Sardauscan.Gui.Controls.ImageButton PlugButton;
	}
}
