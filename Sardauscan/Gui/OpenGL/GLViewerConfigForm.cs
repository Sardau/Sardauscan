#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sardauscan.Core.OpenGL;
using Sardauscan.Gui.Controls;

namespace Sardauscan.Gui.OpenGL
{
	/// <summary>
	/// OpenGl viewer configuration Control
	/// </summary>
    public partial class GLViewerConfigForm : Form
    {
			/// <summary>
			/// Default ctor
			/// </summary>
			public GLViewerConfigForm()
        {
            InitializeComponent();
						this.ForeColor = SkinInfo.View3DForeColor;
						this.BackColor = SkinInfo.View3DBackColor;
        }


        public GLViewerConfig Config;

				void CloseThis(DialogResult result= System.Windows.Forms.DialogResult.OK)
				{
					if (Loading)
						return;
					this.DialogResult = result;
					this.Close();
				}
        private void OK_CANCEL_Click(object sender, EventArgs e)
        {
					CloseThis( ((ImageButton)sender).DialogResult);
        }

        private void OKButton_Click(object sender, EventArgs e)
        {

        }
				bool Loading;
        private void RenderingConfigForm_Load(object sender, EventArgs e)
        {
					Loading = true;
            LigthCheckBox.Checked = this.Config.Lightning;
            TextureCheckBox.Checked = this.Config.ShowSceneColor;
            BoundingCheckBox.Checked = this.Config.BoundingBox;
            SmoothingCheckBox.Checked = this.Config.Smooth;
            ProjectionCheckBox.Checked = this.Config.Projection;
						WireframeCheckBox.Checked = this.Config.Wireframe;

            MaterialPresetComboBox.Items.Clear();
            List<string> presets = GLConfig.MaterialNames();
            MaterialPresetComboBox.Items.AddRange(presets.ToArray());
            MaterialPresetComboBox.SelectedItem = this.Config.DefaultMaterial;
						Loading = false;
				}
        private void LigthCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.Config.Lightning = this.LigthCheckBox.Checked;
						CloseThis();
        }

        private void TextyreCheckBox_Click(object sender, EventArgs e)
        {
            this.Config.ShowSceneColor = this.TextureCheckBox.Checked;
						CloseThis();
				}

        private void BoundingCheckBox_Click(object sender, EventArgs e)
        {
            this.Config.BoundingBox = BoundingCheckBox.Checked;
						CloseThis();
				}

        private void MaterialPresetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Config.DefaultMaterial = (string)MaterialPresetComboBox.SelectedItem;
						CloseThis();
				}

        private void SmoothingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.Config.Smooth = SmoothingCheckBox.Checked;
						CloseThis();
				}

        private void ProjectionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.Config.Projection = this.ProjectionCheckBox.Checked;
						CloseThis();
				}

				private void WireframeCheckBox_CheckedChanged(object sender, EventArgs e)
				{
						this.Config.Wireframe=WireframeCheckBox.Checked;
						CloseThis();
				}
		}
}
