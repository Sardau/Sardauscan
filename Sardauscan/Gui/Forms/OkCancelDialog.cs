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
using Sardauscan.Gui.Controls;

namespace Sardauscan.Gui.Forms
{
	public partial class OkCancelDialog : CustomAppForm, IDisposable
	{
		/// <summary>
		/// Default ctor
		/// </summary>
		public OkCancelDialog()
		{
			InitializeComponent();
			SkinInfo.ApplyColor(this);
			Application.Idle += OnIdle;

		}
		/// <summary>
		/// Dispose object
		/// </summary>
		public new void Dispose()
		{
			base.Dispose();
			Application.Idle -= OnIdle;
		}


		protected Control InnerControl = null;

		public DialogResult ShowDialog(Control innerControl, IWin32Window owner = null)
		{
			InnerControl = innerControl;

			Size contentSize = InnerControl.Size;
			Size pannelSize = this.ContentPanel.Size;

			int dx = contentSize.Width-pannelSize.Width;
			int dy = contentSize.Height-pannelSize.Height;

			Size windowSize = this.Size;
			windowSize.Width += dx;
			windowSize.Height += dy;
			this.Size = windowSize;

			InnerControl.Dock = DockStyle.Fill;
			this.ContentPanel.Controls.Add(InnerControl);
			return ShowDialog(owner);
		}

		public new DialogResult ShowDialog(IWin32Window owner = null)
		{
			DialogResult ret = base.ShowDialog(owner);
			if (InnerControl is IOKCancelView)
			{
				switch (ret)
				{
					case System.Windows.Forms.DialogResult.OK:
						((IOKCancelView)InnerControl).OnOk();
						break;
					case System.Windows.Forms.DialogResult.Cancel:
						((IOKCancelView)InnerControl).OnCancel();
						break;
				}
			}
			return ret;
		}


		private void OKCancelButtonClick(object sender, EventArgs e)
		{
			if (sender is ImageButton)
			{

				this.DialogResult = ((ImageButton)sender).DialogResult;
				this.Close();
			}
		}
		DateTime LastIdleCheck = DateTime.Now;
		private void OnIdle(object sender, EventArgs e)
		{
			try
			{
				bool ignore = false;
				if (!ignore && Visible)
				{
					DateTime now = DateTime.Now;
					bool expired = (now - LastIdleCheck).TotalMilliseconds > 100;
					if (expired)
					{
						if (InnerControl is IOKCancelView)
							this.OkButton.Enabled = ((IOKCancelView)InnerControl).IsValid();
					}
				}
			}
			catch
			{
			}
		}
	}
}
