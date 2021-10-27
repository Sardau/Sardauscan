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
