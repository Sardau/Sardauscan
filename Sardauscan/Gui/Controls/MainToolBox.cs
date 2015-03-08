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
