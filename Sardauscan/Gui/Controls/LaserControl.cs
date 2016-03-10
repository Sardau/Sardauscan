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
using Sardauscan.Core.Interface;
using System.Drawing.Imaging;
using Sardauscan.Core;

namespace Sardauscan.Gui.Controls
{
	public partial class LaserControl : UserControl
	{
		/// <summary>
		/// Default ctor
		/// </summary>
		public LaserControl()
		{
			InitializeComponent();
			OffImage = global::Sardauscan.Properties.Resources.Spot_Off;
			OnImage = global::Sardauscan.Properties.Resources.Spot_On;
			ColorizeLaser(this.button1);
			ColorizeLaser(this.button2);
			ColorizeLaser(this.button3);
			ColorizeLaser(this.RightButton);
			AlignControls();
		}
		public void ColorizeLaser(StatusImageButton button)
		{
			button.Image = SkinInfo.Colorize(OnImage,GetLaserColor(button));
		}
		Color GetLaserColor(StatusImageButton button)
		{
			int laserIndex = -1;
			try
			{
				laserIndex = int.Parse(button.Tag.ToString());
			}
			catch { laserIndex = -1; }
			Settings settings = Settings.Get<Settings>();
			if (settings != null && laserIndex >= 0)
				return settings.Read(Settings.LASER(laserIndex), Settings.DEFAULTCOLOR, LaserInfo.GetDefaultColor(laserIndex));
			else
				return LaserInfo.GetDefaultColor(laserIndex);
		}

		/// <summary>
		/// Dispose object
		/// </summary>
		public new void Dispose()
		{
			if (!this.IsDesignMode())
				Settings.UnRegisterInstance(this);
		}
		Image OffImage;
		Image OnImage;
		public void AlignSubControl(Control control)
		{
			if(control!=null)
				foreach (Control ctrl in control.Controls)
			{
				if (ctrl is StatusImageButton)
				{
					StatusImageButton button = (StatusImageButton)ctrl;
					int i = -1;
					try
					{
						i = int.Parse(ctrl.Tag.ToString());
					}
					catch { }
					//                    int i = ctrl.Tag == null ? -1 : (int)int.Parse(ctrl.Tag.ToString());
					ctrl.Visible = Proxy != null && i >= 0 && i < Proxy.Count;
					bool on = Proxy != null && Proxy.On(i);
					button.On = on;
				}
				else
					AlignSubControl(ctrl);
			}
		}
		public void AlignControls()
		{
			AlignSubControl(this);
		}

		protected ILaserProxy m_Proxy = null;
		public ILaserProxy Proxy
		{
			get { return m_Proxy; }
			set { m_Proxy = value; Enabled = (value != null); AlignControls(); }
		}

		private void button_Click(object sender, EventArgs e)
		{
			if (Proxy == null)
				return;
			int i = -1;
			try
			{
				i = int.Parse(((StatusImageButton)sender).Tag.ToString());
			}
			catch
			{ }

			if (i >= 0 && i < Proxy.Count)
			{
				Proxy.Turn(i, !Proxy.On(i));
				AlignControls();
			}

		}
	}
}
