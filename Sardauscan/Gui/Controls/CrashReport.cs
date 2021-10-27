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
