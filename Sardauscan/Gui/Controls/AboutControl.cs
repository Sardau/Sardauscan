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

namespace Sardauscan.Gui.Controls
{
	public partial class AboutControl : UserControl
	{
		public AboutControl()
		{
			InitializeComponent();
		}

		private void AboutControl_Load(object sender, EventArgs e)
		{
			TitleLabel.Text = "Sarcaukar V" + Application.ProductVersion;
			CopyrightBox.Text = System.Text.Encoding.Default.GetString(global::Sardauscan.Properties.Resources.LICENSE).Replace("\n","\r\n");
			CopyrightBox.Select(0, 0);
			CreditBox.Text = global::Sardauscan.Properties.Resources.CREDIT;
			CreditBox.Select(0, 0);
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://plus.google.com/+FabioFerretti3D");
		}
	}
}
