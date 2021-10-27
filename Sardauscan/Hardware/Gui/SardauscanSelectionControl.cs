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
using Sardauscan.Gui;
using Sardauscan.Hardware.Com;
using Sardauscan.Core;
using System.IO.Ports;
using System.Management;
using System.Diagnostics;

namespace Sardauscan.Hardware.Gui
{
	public partial class SardauscanSelectionControl : UserControl, IOKCancelView
	{
		public SardauscanSelectionControl()
		{
			InitializeComponent();
			this.ForeColor = SkinInfo.ForeColor;
			this.BackColor = SkinInfo.BackColor;
		}

		bool _loading;
		void FillComComboBox()
		{
			_loading = true;
			this.PlugButton.Enabled = false;
			this.ComComboBox.Items.Clear();
			List<PortInfo> list = PortInfo.GetPortsInfo();
			int found = -1;
			string lastport = Settings.Get<Settings>().Read(Settings.LAST_USED, Settings.COM_PORT, string.Empty);
			for (int i = 0; i < list.Count; i++)
			{
				PortInfo info = list[i];
				this.ComComboBox.Items.Add(info);
				if (info.Name == lastport && found == -1)
					found = i;
			}
			try
			{
				if (found != -1)
					ComComboBox.SelectedIndex = found;
			}
			catch { }
			_loading = false;
			this.PlugButton.Enabled = true;
		}

		private void AutodetectArduinoPort()
		{
			// Create a temporary dictionary to superimpose onto the SerialPorts property.
			Dictionary<string, SerialPort> dict = new Dictionary<string, SerialPort>();

			try
			{
				// Scan through each SerialPort registered in the WMI.
				foreach (ManagementObject device in
						new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_SerialPort").Get())
				{
					// Ignore all devices that do not have a relevant VendorID.
					if (!device["PNPDeviceID"].ToString().Contains("VID_2341") && // Arduino
							!device["PNPDeviceID"].ToString().Contains("VID_04d0")) continue; // Digi International (X-Bee)

					// Create a SerialPort to add to the collection.
					var port = new SerialPort();

					// Gather related configuration details for the Arduino Device.
					var config = device.GetRelated("Win32_SerialPortConfiguration")
														 .Cast<ManagementObject>().ToList().FirstOrDefault();

					// Set the SerialPort's PortName property.
					port.PortName = device["DeviceID"].ToString();

					// Set the SerialPort's BaudRate property. Use the devices maximum BaudRate as a fallback.
					port.BaudRate = (config != null)
															? int.Parse(config["BaudRate"].ToString())
															: int.Parse(device["MaxBaudRate"].ToString());

					// Add the SerialPort to the dictionary. Key = Arduino device description.
					dict.Add(device["Description"].ToString(), port);
				}

				// Return the dictionary.
				// SerialPorts = dict;
			}
			catch (ManagementException mex)
			{
				// Send a message to debug.
				Debug.WriteLine(@"An error occurred while querying for WMI data: " + mex.Message);
			}
		}

		public SardauscanHardwareProxy SardauscanHardwareProxy { get; private set; }

		private void DisposeArduino()
		{
			Settings.UnRegisterInstance(SardauscanHardwareProxy);
			if (SardauscanHardwareProxy != null)
				SardauscanHardwareProxy.Dispose();
			SardauscanHardwareProxy = null;
		}

		private void ComComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading)
				return;
			PlugButton_Click(sender, e);
		}
		public void OnOk()
		{
			if (SardauscanHardwareProxy != null)
				Settings.Get<Settings>().Write(Settings.LAST_USED, Settings.COM_PORT, SardauscanHardwareProxy.PortName);
		}

		public void OnCancel()
		{
			
		}

		private void ReloadComPort(object sender, EventArgs e)
		{
			FillComComboBox();
		}

		private void DefaultArduinoSelectionView_Load(object sender, EventArgs e)
		{
			FillComComboBox();
		}
		void AlignInterface()
		{
		}

		private void PlugButton_Click(object sender, EventArgs e)
		{
			this.PlugButton.Enabled = false;
			this.m_SardauscanProxyControl.Proxy = null;
			PortInfo portInfo = (PortInfo)this.ComComboBox.SelectedItem;
			try
			{
				DisposeArduino();
				SardauscanHardwareProxy = new SardauscanHardwareProxy(portInfo);
				this.m_SardauscanProxyControl.Proxy = SardauscanHardwareProxy;
			}
			catch
			{
			}
			finally
			{
				this.PlugButton.Enabled = true;
				AlignInterface();
			}
		}

		public bool IsValid()
		{
			return SardauscanHardwareProxy!=null;
		}
	}
}
