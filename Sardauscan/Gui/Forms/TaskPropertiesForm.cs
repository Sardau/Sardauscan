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
