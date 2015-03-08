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
using Sardauscan.Gui;
using Sardauscan.Hardware.Gui;
using Sardauscan.Gui.Forms;
using System.IO;

namespace Sardauscan.Hardware
{
	/// <summary>
	/// Sardaukar Default firmware proxy provider
	/// </summary>
	public class SardauscanProxyProvider : AbstractProxyProvider<SardauscanHardwareProxy>
	{
		/// <summary>
		/// Name of the harware/provider
		/// </summary>
		public override string Name	{	get { return "Sardauscan"; } }
		protected string ConfigPath
		{
			get
			{
				if (!Directory.Exists(Program.HardwareConfigPath))
					Directory.CreateDirectory(Program.HardwareConfigPath);
				return Path.Combine(Program.HardwareConfigPath, "Sardauscan.txt");
			}
		}

		public override object Select(System.Windows.Forms.IWin32Window owner)
		{
			/*
			if (autoselect && File.Exists(ConfigPath))
			{
				try
				{
					using(StreamReader sw = File.OpenText(ConfigPath))
					{
						string hardwareId = sw.ReadToEnd();
						SardauscanHardwareProxy proxy = SardauscanHardwareProxy.FromHardwareId(hardwareId);
						return proxy;
					}
				}
				catch { }
			}
			*/
			OkCancelDialog dlg = new OkCancelDialog();
			dlg.Text = Name;
			SardauscanSelectionControl view = new SardauscanSelectionControl();
			if(dlg.ShowDialog(view, owner)== System.Windows.Forms.DialogResult.OK)
			{
				try
				{
					if (view.SardauscanHardwareProxy as SardauscanHardwareProxy != null)
					{
						using (StreamWriter sw = File.CreateText(ConfigPath))
							sw.Write(((SardauscanHardwareProxy)view.SardauscanHardwareProxy).HardwareId);
					}
				}
				catch { }
				return view.SardauscanHardwareProxy;
			}
			return null;
		}
	}
}
