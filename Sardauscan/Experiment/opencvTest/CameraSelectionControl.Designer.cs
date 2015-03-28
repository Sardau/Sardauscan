using Sardauscan.Gui.Controls;
namespace opencvTest
{
	partial class CameraSelectionControl
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
            this.ConnectButton = new System.Windows.Forms.Button();
            this.ResolutionBox = new System.Windows.Forms.ComboBox();
            this.CameraBox = new System.Windows.Forms.ComboBox();
            this.Preview = new System.Windows.Forms.PictureBox();
            this.SnapshotButton = new System.Windows.Forms.Button();
            this.Settings = new System.Windows.Forms.Button();
            this.LivePreview = new CameraLivePreviewControl();
            ((System.ComponentModel.ISupportInitialize)(this.Preview)).BeginInit();
            this.SuspendLayout();
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(6, 49);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(121, 25);
            this.ConnectButton.TabIndex = 6;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // ResolutionBox
            // 
            this.ResolutionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ResolutionBox.FormattingEnabled = true;
            this.ResolutionBox.Location = new System.Drawing.Point(6, 26);
            this.ResolutionBox.Name = "ResolutionBox";
            this.ResolutionBox.Size = new System.Drawing.Size(121, 21);
            this.ResolutionBox.TabIndex = 4;
            // 
            // CameraBox
            // 
            this.CameraBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CameraBox.FormattingEnabled = true;
            this.CameraBox.Location = new System.Drawing.Point(6, 3);
            this.CameraBox.Name = "CameraBox";
            this.CameraBox.Size = new System.Drawing.Size(121, 21);
            this.CameraBox.TabIndex = 5;
            this.CameraBox.SelectedIndexChanged += new System.EventHandler(this.CameraBox_SelectedIndexChanged);
            // 
            // Preview
            // 
            this.Preview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Preview.Location = new System.Drawing.Point(133, 3);
            this.Preview.Name = "Preview";
            this.Preview.Size = new System.Drawing.Size(334, 281);
            this.Preview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Preview.TabIndex = 3;
            this.Preview.TabStop = false;
            // 
            // SnapshotButton
            // 
            this.SnapshotButton.Location = new System.Drawing.Point(6, 185);
            this.SnapshotButton.Name = "SnapshotButton";
            this.SnapshotButton.Size = new System.Drawing.Size(75, 23);
            this.SnapshotButton.TabIndex = 7;
            this.SnapshotButton.Text = "Snapshot";
            this.SnapshotButton.UseVisualStyleBackColor = true;
            this.SnapshotButton.Click += new System.EventHandler(this.SnapshotButton_Click);
            // 
            // Settings
            // 
            this.Settings.Location = new System.Drawing.Point(6, 76);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(121, 23);
            this.Settings.TabIndex = 8;
            this.Settings.Text = "SettingsButton";
            this.Settings.UseVisualStyleBackColor = true;
            this.Settings.Click += new System.EventHandler(this.Settings_Click);
            // 
            // LivePreview
            // 
            this.LivePreview.InitialRefreshTime = 30;
            this.LivePreview.Location = new System.Drawing.Point(6, 105);
            this.LivePreview.Name = "LivePreview";
            this.LivePreview.Proxy = null;
            this.LivePreview.Size = new System.Drawing.Size(121, 77);
            this.LivePreview.TabIndex = 9;
            // 
            // CameraSelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LivePreview);
            this.Controls.Add(this.Settings);
            this.Controls.Add(this.SnapshotButton);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.ResolutionBox);
            this.Controls.Add(this.CameraBox);
            this.Controls.Add(this.Preview);
            this.Name = "CameraSelectionControl";
            this.Size = new System.Drawing.Size(470, 287);
            this.Load += new System.EventHandler(this.CameraSelctionControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Preview)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button ConnectButton;
		private System.Windows.Forms.ComboBox ResolutionBox;
		private System.Windows.Forms.ComboBox CameraBox;
		private System.Windows.Forms.PictureBox Preview;
		private System.Windows.Forms.Button SnapshotButton;
		private System.Windows.Forms.Button Settings;
		private CameraLivePreviewControl LivePreview;
	}
}
