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
