#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sardauscan.Gui.Controls.ApplicationView
{
	/// <summary>
	/// Structure to store information of a main window View
	/// </summary>
	public struct View
	{
		/// <summary>
		/// Ctor
		/// </summary>
		public View(ViewType type, Control bigControl, Control smallControl)
		{
			this.Type = type;
			BigControl = bigControl;
			SmallControl = smallControl;
		}
		public ViewType Type;
		public Control BigControl;
		public Control SmallControl;

		public override string ToString()
		{
			return this.Type.ToString();
		}
		public bool Enable
		{
			get
			{
				if(BigControl==null &&  SmallControl==null)
					return false;
				return ViewAvailable(SmallControl) && ViewAvailable(BigControl);
			}
		}
		private bool ViewAvailable(Control ctrl)
		{
			if (ctrl == null)
				return true;
			if (!ctrl.Enabled)
				return false;
			if (ctrl is IMainView && !((IMainView)ctrl).Available)
				return false;
			return true;
		}
	}
}
