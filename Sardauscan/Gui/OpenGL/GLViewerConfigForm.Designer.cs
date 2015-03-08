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
using Sardauscan.Gui.Controls;
namespace Sardauscan.Gui.OpenGL
{
    partial class GLViewerConfigForm
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
			this.LigthCheckBox = new System.Windows.Forms.CheckBox();
			this.TextureCheckBox = new System.Windows.Forms.CheckBox();
			this.BoundingCheckBox = new System.Windows.Forms.CheckBox();
			this.MaterialPresetComboBox = new System.Windows.Forms.ComboBox();
			this.SmoothingCheckBox = new System.Windows.Forms.CheckBox();
			this.ProjectionCheckBox = new System.Windows.Forms.CheckBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.imageButton1 = new Sardauscan.Gui.Controls.ImageButton();
			this.OKButton = new Sardauscan.Gui.Controls.ImageButton();
			this.WireframeCheckBox = new System.Windows.Forms.CheckBox();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// LigthCheckBox
			// 
			this.LigthCheckBox.AutoSize = true;
			this.LigthCheckBox.Location = new System.Drawing.Point(5, 7);
			this.LigthCheckBox.Name = "LigthCheckBox";
			this.LigthCheckBox.Size = new System.Drawing.Size(49, 17);
			this.LigthCheckBox.TabIndex = 2;
			this.LigthCheckBox.Text = "Light";
			this.LigthCheckBox.UseVisualStyleBackColor = true;
			this.LigthCheckBox.CheckedChanged += new System.EventHandler(this.LigthCheckBox_CheckedChanged);
			// 
			// TextureCheckBox
			// 
			this.TextureCheckBox.AutoSize = true;
			this.TextureCheckBox.Location = new System.Drawing.Point(5, 30);
			this.TextureCheckBox.Name = "TextureCheckBox";
			this.TextureCheckBox.Size = new System.Drawing.Size(62, 17);
			this.TextureCheckBox.TabIndex = 3;
			this.TextureCheckBox.Text = "Texture";
			this.TextureCheckBox.UseVisualStyleBackColor = true;
			this.TextureCheckBox.Click += new System.EventHandler(this.TextyreCheckBox_Click);
			// 
			// BoundingCheckBox
			// 
			this.BoundingCheckBox.AutoSize = true;
			this.BoundingCheckBox.Location = new System.Drawing.Point(5, 53);
			this.BoundingCheckBox.Name = "BoundingCheckBox";
			this.BoundingCheckBox.Size = new System.Drawing.Size(91, 17);
			this.BoundingCheckBox.TabIndex = 4;
			this.BoundingCheckBox.Text = "Bounding box";
			this.BoundingCheckBox.UseVisualStyleBackColor = true;
			this.BoundingCheckBox.Click += new System.EventHandler(this.BoundingCheckBox_Click);
			// 
			// MaterialPresetComboBox
			// 
			this.MaterialPresetComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.MaterialPresetComboBox.FormattingEnabled = true;
			this.MaterialPresetComboBox.Location = new System.Drawing.Point(104, 3);
			this.MaterialPresetComboBox.Name = "MaterialPresetComboBox";
			this.MaterialPresetComboBox.Size = new System.Drawing.Size(177, 21);
			this.MaterialPresetComboBox.TabIndex = 5;
			this.MaterialPresetComboBox.SelectedIndexChanged += new System.EventHandler(this.MaterialPresetComboBox_SelectedIndexChanged);
			// 
			// SmoothingCheckBox
			// 
			this.SmoothingCheckBox.AutoSize = true;
			this.SmoothingCheckBox.Location = new System.Drawing.Point(5, 76);
			this.SmoothingCheckBox.Name = "SmoothingCheckBox";
			this.SmoothingCheckBox.Size = new System.Drawing.Size(76, 17);
			this.SmoothingCheckBox.TabIndex = 6;
			this.SmoothingCheckBox.Text = "Smoothing";
			this.SmoothingCheckBox.UseVisualStyleBackColor = true;
			this.SmoothingCheckBox.CheckedChanged += new System.EventHandler(this.SmoothingCheckBox_CheckedChanged);
			// 
			// ProjectionCheckBox
			// 
			this.ProjectionCheckBox.AutoSize = true;
			this.ProjectionCheckBox.Location = new System.Drawing.Point(5, 99);
			this.ProjectionCheckBox.Name = "ProjectionCheckBox";
			this.ProjectionCheckBox.Size = new System.Drawing.Size(82, 17);
			this.ProjectionCheckBox.TabIndex = 7;
			this.ProjectionCheckBox.Text = "Perspective";
			this.ProjectionCheckBox.UseVisualStyleBackColor = true;
			this.ProjectionCheckBox.CheckedChanged += new System.EventHandler(this.ProjectionCheckBox_CheckedChanged);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Transparent;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.WireframeCheckBox);
			this.panel1.Controls.Add(this.imageButton1);
			this.panel1.Controls.Add(this.ProjectionCheckBox);
			this.panel1.Controls.Add(this.OKButton);
			this.panel1.Controls.Add(this.SmoothingCheckBox);
			this.panel1.Controls.Add(this.MaterialPresetComboBox);
			this.panel1.Controls.Add(this.LigthCheckBox);
			this.panel1.Controls.Add(this.BoundingCheckBox);
			this.panel1.Controls.Add(this.TextureCheckBox);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(286, 151);
			this.panel1.TabIndex = 8;
			// 
			// imageButton1
			// 
			this.imageButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.imageButton1.BackColor = System.Drawing.Color.Transparent;
			this.imageButton1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.imageButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.imageButton1.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.imageButton1.Image = global::Sardauscan.Properties.Resources.Uncheck;
			this.imageButton1.Location = new System.Drawing.Point(253, 118);
			this.imageButton1.Name = "imageButton1";
			this.imageButton1.Size = new System.Drawing.Size(32, 31);
			this.imageButton1.TabIndex = 1;
			this.imageButton1.Click += new System.EventHandler(this.OK_CANCEL_Click);
			// 
			// OKButton
			// 
			this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.OKButton.BackColor = System.Drawing.Color.Transparent;
			this.OKButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OKButton.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.OKButton.Image = global::Sardauscan.Properties.Resources.Check;
			this.OKButton.Location = new System.Drawing.Point(217, 118);
			this.OKButton.Name = "OKButton";
			this.OKButton.Size = new System.Drawing.Size(32, 31);
			this.OKButton.TabIndex = 0;
			this.OKButton.Visible = false;
			this.OKButton.Click += new System.EventHandler(this.OK_CANCEL_Click);
			// 
			// WireframeCheckBox
			// 
			this.WireframeCheckBox.AutoSize = true;
			this.WireframeCheckBox.Location = new System.Drawing.Point(5, 118);
			this.WireframeCheckBox.Name = "WireframeCheckBox";
			this.WireframeCheckBox.Size = new System.Drawing.Size(74, 17);
			this.WireframeCheckBox.TabIndex = 8;
			this.WireframeCheckBox.Text = "Wireframe";
			this.WireframeCheckBox.UseVisualStyleBackColor = true;
			this.WireframeCheckBox.CheckedChanged += new System.EventHandler(this.WireframeCheckBox_CheckedChanged);
			// 
			// GLViewerConfigForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(286, 151);
			this.ControlBox = false;
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GLViewerConfigForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Rendering Settings";
			this.Load += new System.EventHandler(this.RenderingConfigForm_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private ImageButton OKButton;
        private ImageButton imageButton1;
        private System.Windows.Forms.CheckBox LigthCheckBox;
        private System.Windows.Forms.CheckBox TextureCheckBox;
        private System.Windows.Forms.CheckBox BoundingCheckBox;
        private System.Windows.Forms.ComboBox MaterialPresetComboBox;
        private System.Windows.Forms.CheckBox SmoothingCheckBox;
        private System.Windows.Forms.CheckBox ProjectionCheckBox;
				private System.Windows.Forms.Panel panel1;
				private System.Windows.Forms.CheckBox WireframeCheckBox;
    }
}