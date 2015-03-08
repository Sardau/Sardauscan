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

namespace Sardauscan.Gui.Controls
{
	/// <summary>
	/// Control to display a Crash Report
	/// </summary>
	public partial class CrashReport : UserControl
	{
		/// <summary>
		/// Ctor
		/// </summary>
		public CrashReport()
		{
			InitializeComponent();
			ForeColor = SkinInfo.ForeColor;
			BackColor = SkinInfo.BackColor;
			this.ExceptionTextBox.ForeColor = SkinInfo.ForeColor;
			this.ExceptionTextBox.BackColor = SkinInfo.BackColor;

		}
		/// <summary>
		/// Set the Exception
		/// </summary>
		/// <param name="e"></param>
		public void SetException(Exception e)
		{


			string text = "";

			text+=string.Format("Error ---\n{0}\n", e.Message);
			text += string.Format("\nHelpLink ---\n{0}", e.HelpLink);
			text += string.Format("\nSource ---\n{0}", e.Source);
			text += string.Format("\nStackTrace ---\n{0}", e.StackTrace);
			text += string.Format("\nTargetSite ---\n{0}", e.TargetSite);
			this.ExceptionTextBox.Text = text.Replace("\n","\r\n");
		}
	}
}
