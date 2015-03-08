#region COPYRIGHT
/****************************************************************************
 *  Copyright (c) 2015 Fabio Ferretti <https://plus.google.com/+FabioFerretti3D>                 *
 *  This file is part of Sardauscan.                                        *
 *                                                                          *
 *  Sardauscan is free software: you can redistribute it and/or modify      *
 *  it under the terms of the GNU General Public License as published by    *
 *  the Free Software Foundation, either version 3 of the License, or       *
 *  (at your option) any later version.                                     *
 *                                                                          *
 *  Sardauscan is distributed in the hope that it will be useful,           *
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of          *
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the           *
 *  GNU General Public License for more details.                            *
 *                                                                          *
 *  You are not allowed to Sell in any form this code                       * 
 *  or any compiled version. This code is free and for free purpose only    *
 *                                                                          *
 *  You should have received a copy of the GNU General Public License       *
 *  along with Sardaukar.  If not, see <http://www.gnu.org/licenses/>       *
 ****************************************************************************
*/
#endregion
using Sardauscan.Gui.OpenGL;
using Sardauscan.Gui.Controls;

namespace Sardauscan.Gui.Forms
{
    partial class ShowFull3dObjectForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			Sardauscan.Core.Geometry.Scene3D scene3D1 = new Sardauscan.Core.Geometry.Scene3D();
			this.object3DView1 = new Scene3DControl();
			this.SaveButton = new Sardauscan.Gui.Controls.ImageButton();
			this.SuspendLayout();
			// 
			// object3DView1
			// 
			this.object3DView1.AutoScrollMargin = new System.Drawing.Size(42, 0);
			this.object3DView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(170)))), ((int)(((byte)(255)))));
			this.object3DView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.object3DView1.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.object3DView1.Location = new System.Drawing.Point(0, 0);
			this.object3DView1.Name = "object3DView1";
			this.object3DView1.Scene = scene3D1;
			this.object3DView1.ShowDefaultScene = false;
			this.object3DView1.ShowFullOnClick = false;
			this.object3DView1.ShowSettingsButton = false;
			this.object3DView1.Size = new System.Drawing.Size(784, 561);
			this.object3DView1.TabIndex = 0;
			this.object3DView1.VSync = false;
			// 
			// SaveButton
			// 
			this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.SaveButton.BackColor = System.Drawing.Color.Transparent;
			this.SaveButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.SaveButton.ForeColor = System.Drawing.Color.Transparent;
			this.SaveButton.Image = global::Sardauscan.Properties.Resources.Floppy;
			this.SaveButton.Location = new System.Drawing.Point(746, 528);
			this.SaveButton.Name = "SaveButton";
			this.SaveButton.Size = new System.Drawing.Size(36, 33);
			this.SaveButton.TabIndex = 2;
			// 
			// ShowFull3dObjectForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 561);
			this.Controls.Add(this.SaveButton);
			this.Controls.Add(this.object3DView1);
			this.Name = "ShowFull3dObjectForm";
			this.Text = "ShowFull3dObjectForm";
			this.ResumeLayout(false);

        }

        #endregion

        private Scene3DControl object3DView1;
        private ImageButton SaveButton;
    }
}