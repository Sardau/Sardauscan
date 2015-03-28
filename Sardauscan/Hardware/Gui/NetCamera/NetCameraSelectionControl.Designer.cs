namespace Sardauscan.Hardware.Gui.NetCamera
{
	partial class NetCameraSelectionControl
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
			this.ResolutionBox = new System.Windows.Forms.ComboBox();
			this.CameraBox = new System.Windows.Forms.ComboBox();
			this.PlugButton = new Sardauscan.Gui.Controls.ImageButton();
			this.imageButton1 = new Sardauscan.Gui.Controls.ImageButton();
			this.CameraControl = new Sardauscan.Hardware.Gui.NetCamera.NetCameraProxyControl();
			this.SuspendLayout();
			// 
			// ResolutionBox
			// 
			this.ResolutionBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ResolutionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ResolutionBox.FormattingEnabled = true;
			this.ResolutionBox.Location = new System.Drawing.Point(232, 3);
			this.ResolutionBox.Name = "ResolutionBox";
			this.ResolutionBox.Size = new System.Drawing.Size(121, 21);
			this.ResolutionBox.TabIndex = 6;
			this.ResolutionBox.SelectedIndexChanged += new System.EventHandler(this.CameraResolutionComboBox_SelectedIndexChanged);
			// 
			// CameraBox
			// 
			this.CameraBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.CameraBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CameraBox.FormattingEnabled = true;
			this.CameraBox.Location = new System.Drawing.Point(29, 3);
			this.CameraBox.Name = "CameraBox";
			this.CameraBox.Size = new System.Drawing.Size(197, 21);
			this.CameraBox.TabIndex = 7;
			this.CameraBox.SelectedIndexChanged += new System.EventHandler(this.CameraComboBox_SelectedIndexChanged);
			// 
			// PlugButton
			// 
			this.PlugButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.PlugButton.BackColor = System.Drawing.Color.Green;
			this.PlugButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.PlugButton.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.PlugButton.Image = global::Sardauscan.Properties.Resources.Usb;
			this.PlugButton.Location = new System.Drawing.Point(359, 0);
			this.PlugButton.Name = "PlugButton";
			this.PlugButton.Size = new System.Drawing.Size(29, 31);
			this.PlugButton.TabIndex = 15;
			this.PlugButton.Click += new System.EventHandler(this.CameraResolutionComboBox_SelectedIndexChanged);
			// 
			// imageButton1
			// 
			this.imageButton1.BackColor = System.Drawing.Color.Green;
			this.imageButton1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.imageButton1.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.imageButton1.Image = global::Sardauscan.Properties.Resources.Reload;
			this.imageButton1.Location = new System.Drawing.Point(0, 0);
			this.imageButton1.Name = "imageButton1";
			this.imageButton1.Size = new System.Drawing.Size(29, 31);
			this.imageButton1.TabIndex = 13;
			this.imageButton1.Click += new System.EventHandler(this.ReloadCameraCombo);
			// 
			// CameraControl
			// 
			this.CameraControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.CameraControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.CameraControl.Location = new System.Drawing.Point(0, 30);
			this.CameraControl.Name = "CameraControl";
			this.CameraControl.Proxy = null;
			this.CameraControl.Size = new System.Drawing.Size(391, 261);
			this.CameraControl.TabIndex = 0;
			// 
			// NetCameraSelectionControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.PlugButton);
			this.Controls.Add(this.imageButton1);
			this.Controls.Add(this.ResolutionBox);
			this.Controls.Add(this.CameraBox);
			this.Controls.Add(this.CameraControl);
			this.Name = "NetCameraSelectionControl";
			this.Size = new System.Drawing.Size(391, 294);
			this.Load += new System.EventHandler(this.NetCameraSelectionControl_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private NetCameraProxyControl CameraControl;
		private System.Windows.Forms.ComboBox ResolutionBox;
		private System.Windows.Forms.ComboBox CameraBox;
		private Sardauscan.Gui.Controls.ImageButton imageButton1;
		private Sardauscan.Gui.Controls.ImageButton PlugButton;
	}
}
