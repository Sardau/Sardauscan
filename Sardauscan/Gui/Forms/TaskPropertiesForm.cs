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
using Sardauscan.Core.ProcessingTask;
using System.Xml.Serialization;
using System.IO;
using Sardauscan.Gui.Controls;
using Sardauscan.Core;

namespace Sardauscan.Gui.Forms
{
	public partial class TaskPropertiesForm : CustomAppForm
	{
		public TaskPropertiesForm()
		{
			InitializeComponent();
			this.BackColor = SkinInfo.BackColor;
			this.ForeColor = SkinInfo.ForeColor;


			this.Grid.LineColor = SkinInfo.ActiveTitleBackColor.GetStepColor(this.BackColor, 0.5);
			this.Grid.CategoryForeColor = SkinInfo.ActiveTitleTextColor;

			this.Grid.HelpBackColor = this.BackColor;
			this.Grid.HelpForeColor= this.ForeColor;
			this.Grid.BackColor = this.BackColor;
			this.Grid.ViewBackColor = this.BackColor;

		}


		public AbstractProcessingTask EditSettings(IWin32Window owner, AbstractProcessingTask task, bool defaultsettings)
		{
			AbstractProcessingTask ret = task.Clone();
			if (ret != null)
			{
				if (!ret.RunSettings())
				{
					this.Grid.SelectedObject = task;
					if (this.ShowDialog(owner) == System.Windows.Forms.DialogResult.OK)
					{
						ret = (AbstractProcessingTask)this.Grid.SelectedObject;
						if (defaultsettings && task.HasBrowsableSettings)
						{
							task.SaveToFile(Program.TaskPath);
						}
					}
				}
			}
			return ret;
		}

		private void imageButton1_Click(object sender, EventArgs e)
		{
			this.DialogResult = ((ImageButton)sender).DialogResult;
			this.Close();
		}
	}
}
