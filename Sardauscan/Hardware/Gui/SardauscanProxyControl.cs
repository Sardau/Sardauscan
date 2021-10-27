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
using Sardauscan.Core.Interface;

namespace Sardauscan.Hardware.Gui
{
	public partial class SardauscanProxyControl : UserControl
	{
		public SardauscanProxyControl()
		{
			InitializeComponent();
		}

		public IHardwareProxy Proxy
		{
			set
			{
				this.m_TurnTableView.Proxy = value as ITurnTableProxy;
				this.m_LaserView.Proxy = value as ILaserProxy;
			}
		}
	}
}
