namespace Sardauscan.Gui.Controls
{
	partial class CameraLivePreviewControl
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
			this.components = new System.ComponentModel.Container();
			this.PreviewBox = new System.Windows.Forms.PictureBox();
			this.RefreshTimer = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.PreviewBox)).BeginInit();
			this.SuspendLayout();
			// 
			// PreviewBox
			// 
			this.PreviewBox.BackColor = System.Drawing.Color.Black;
			this.PreviewBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PreviewBox.Location = new System.Drawing.Point(0, 0);
			this.PreviewBox.Name = "PreviewBox";
			this.PreviewBox.Size = new System.Drawing.Size(239, 215);
			this.PreviewBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.PreviewBox.TabIndex = 0;
			this.PreviewBox.TabStop = false;
			// 
			// RefreshTimer
			// 
			this.RefreshTimer.Tick += new System.EventHandler(this.RefreshTimer_Tick);
			// 
			// CameraLivePreviewControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.PreviewBox);
			this.Name = "CameraLivePreviewControl";
			this.Size = new System.Drawing.Size(239, 215);
			this.Load += new System.EventHandler(this.CameraLivePreviewControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.PreviewBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox PreviewBox;
		private System.Windows.Forms.Timer RefreshTimer;
	}
}
