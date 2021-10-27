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
