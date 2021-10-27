#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sardauscan.Gui.Controls.ApplicationView;

namespace Sardauscan.Gui.Controls
{
	/// <summary>
	/// Main toolbox of the application
	/// </summary>
	public partial class MainToolBox : UserControl
	{
		/// <summary>
		/// Default Ctor
		/// </summary>
		public MainToolBox()
		{
			InitializeComponent();
			this.hardwareStatusControl1.SettingsPanel = this.SettingsPanel;
			int h = Math.Max(this.hardwareStatusControl1.Height, this.viewSelector1.Height);
			flowLayoutPanel1.Height = h;
			this.viewSelector1.Height = h;
			this.hardwareStatusControl1.Height=h;
		}
		private void SettingsPanel_SizeChanged(object sender, EventArgs e)
		{
			this.Height = SettingsPanel.Top + SettingsPanel.Height;
		}

		public ViewControler Controler { get { return this.viewSelector1.Controler; } set { this.viewSelector1.Controler = value; } }
	}
}
