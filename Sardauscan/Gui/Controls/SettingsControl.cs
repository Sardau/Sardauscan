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
using Sardauscan.Core;
using System.IO;
using Sardauscan.Gui.Controls;

namespace Sardauscan.Gui.Controls
{
	public partial class SettingsControl : UserControl, IOKCancelView
	{
		/// <summary>
		/// Default ctor
		/// </summary>
		public SettingsControl()
		{
			InitializeComponent();
			this.BackColor = SkinInfo.BackColor;

			this.ForeColor = SkinInfo.ForeColor;
			this.Grid.LineColor = SkinInfo.ActiveTitleBackColor.GetStepColor(this.BackColor, 0.5);
			this.Grid.CategoryForeColor = SkinInfo.ActiveTitleTextColor;

			this.Grid.HelpBackColor = this.BackColor;
			this.Grid.HelpForeColor = this.ForeColor;
			this.Grid.BackColor = this.BackColor;
			this.Grid.ViewBackColor = this.BackColor;

		}


		public void LoadCuttentSetting()
		{
			if (!this.IsDesignMode())
				this.Grid.SelectedObject = Settings.Get<Settings>().GetPropertyGridSettingsComponent();
		}
		public void SaveCurrentSettings()
		{
			PropertyGridSettingsComponent compo = this.Grid.SelectedObject as PropertyGridSettingsComponent;
			if (compo != null)
				Settings.Get<Settings>().Update(compo);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			LoadCuttentSetting();
		}

		private void SettingsView_VisibleChanged(object sender, EventArgs e)
		{
			if (Visible)
				LoadCuttentSetting();
			else
			{
				SaveCurrentSettings();
				this.Grid.SelectedObject = null;
			}
		}

		public void OnOk()
		{
			SaveCurrentSettings();
			this.Grid.SelectedObject = null;
		}

		public void OnCancel()
		{
		}

		public bool IsValid(){return true;}
	}
}
