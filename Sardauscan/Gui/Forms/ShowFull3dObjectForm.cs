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
using Sardauscan.Gui.OpenGL;
using Sardauscan.Core.OpenGL;

namespace Sardauscan.Gui.Forms
{
	/// <summary>
	/// Form for the 3D viewer
	/// </summary>
    public partial class ShowFull3dObjectForm : Form
    {
        public ShowFull3dObjectForm()
        {
            InitializeComponent();
        }
        public Scene3DControl View
        {
            get { return this.object3DView1; }
        }
        public GLViewerConfig ViewerConfig { get { return this.object3DView1.ViewerConfig; } set { this.object3DView1.ViewerConfig = value; this.object3DView1.Invalidate(); } }
    }
}
