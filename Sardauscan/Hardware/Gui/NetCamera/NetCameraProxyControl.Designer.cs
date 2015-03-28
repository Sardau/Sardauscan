namespace Sardauscan.Hardware.Gui.NetCamera
{
	partial class NetCameraProxyControl
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
			this.Settings = new System.Windows.Forms.Button();
			this.LivePreviewBox = new Sardauscan.Gui.Controls.CameraLivePreviewControl();
			this.SuspendLayout();
			// 
			// Settings
			// 
			this.Settings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.Settings.Location = new System.Drawing.Point(176, 3);
			this.Settings.Name = "Settings";
			this.Settings.Size = new System.Drawing.Size(121, 23);
			this.Settings.TabIndex = 10;
			this.Settings.Text = "SettingsButton";
			this.Settings.UseVisualStyleBackColor = true;
			this.Settings.Click += new System.EventHandler(this.Settings_Click);
			// 
			// LivePreviewBox
			// 
			this.LivePreviewBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LivePreviewBox.InitialRefreshTime = 100;
			this.LivePreviewBox.Location = new System.Drawing.Point(-1, 32);
			this.LivePreviewBox.Name = "LivePreviewBox";
			this.LivePreviewBox.Proxy = null;
			this.LivePreviewBox.Size = new System.Drawing.Size(301, 183);
			this.LivePreviewBox.TabIndex = 0;
			// 
			// NetCameraProxyControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.Settings);
			this.Controls.Add(this.LivePreviewBox);
			this.Name = "NetCameraProxyControl";
			this.Size = new System.Drawing.Size(300, 218);
			this.ResumeLayout(false);

		}

		#endregion

		private Sardauscan.Gui.Controls.CameraLivePreviewControl LivePreviewBox;
		private System.Windows.Forms.Button Settings;
	}
}
