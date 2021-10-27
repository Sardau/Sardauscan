#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
using Sardauscan.Gui.Controls;
namespace Sardauscan.Gui.OpenGL
{
	/// <summary>
	/// 3D Scene Viewer
	/// </summary>
    partial class Scene3DControl
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
            this.imageButton1 = new Sardauscan.Gui.Controls.ImageButton();
            this.SettingsButton = new Sardauscan.Gui.Controls.ImageButton();
            this.imageButton2 = new Sardauscan.Gui.Controls.ImageButton();
            this.HomeButton = new Sardauscan.Gui.Controls.ImageButton();
            this.SuspendLayout();
            // 
            // imageButton1
            // 
            this.imageButton1.BackColor = System.Drawing.Color.Transparent;
            this.imageButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.imageButton1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.imageButton1.Image = global::Sardauscan.Properties.Resources.octicons_f06d_1__64;
            this.imageButton1.Location = new System.Drawing.Point(3, 43);
            this.imageButton1.Name = "imageButton1";
            this.imageButton1.Size = new System.Drawing.Size(41, 41);
            this.imageButton1.TabIndex = 8;
            this.imageButton1.Click += new System.EventHandler(this.imageButton1_Click);
            // 
            // SettingsButton
            // 
            this.SettingsButton.BackColor = System.Drawing.Color.Transparent;
            this.SettingsButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SettingsButton.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.SettingsButton.Image = global::Sardauscan.Properties.Resources.Gear3;
            this.SettingsButton.Location = new System.Drawing.Point(3, 125);
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.Size = new System.Drawing.Size(41, 41);
            this.SettingsButton.TabIndex = 7;
            this.SettingsButton.Click += new System.EventHandler(this.pictureBox4_Click);
            // 
            // imageButton2
            // 
            this.imageButton2.BackColor = System.Drawing.Color.Transparent;
            this.imageButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.imageButton2.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.imageButton2.Image = global::Sardauscan.Properties.Resources.Trash;
            this.imageButton2.Location = new System.Drawing.Point(3, 84);
            this.imageButton2.Name = "imageButton2";
            this.imageButton2.Size = new System.Drawing.Size(41, 41);
            this.imageButton2.TabIndex = 6;
            this.imageButton2.Click += new System.EventHandler(this.pictureBox5_Click);
            // 
            // HomeButton
            // 
            this.HomeButton.BackColor = System.Drawing.Color.Transparent;
            this.HomeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HomeButton.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.HomeButton.Image = global::Sardauscan.Properties.Resources.Home;
            this.HomeButton.Location = new System.Drawing.Point(3, 2);
            this.HomeButton.Name = "HomeButton";
            this.HomeButton.Size = new System.Drawing.Size(41, 41);
            this.HomeButton.TabIndex = 5;
            this.HomeButton.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // Scene3DControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScrollMargin = new System.Drawing.Size(42, 0);
            this.BackColor = System.Drawing.SystemColors.Highlight;
            this.Controls.Add(this.imageButton1);
            this.Controls.Add(this.SettingsButton);
            this.Controls.Add(this.imageButton2);
            this.Controls.Add(this.HomeButton);
            this.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.Name = "Scene3DControl";
            this.Size = new System.Drawing.Size(265, 190);
            this.Load += new System.EventHandler(this.Object3DView_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Render);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glSurface_MouseUp);
            this.Resize += new System.EventHandler(this.Scene3DView_Resize);
            this.ResumeLayout(false);

        }

        #endregion

				// private OpenTK.GLControl glControl1;
        private ImageButton HomeButton;
        private ImageButton imageButton2;
        private ImageButton SettingsButton;
        private ImageButton imageButton1;
    }
}
