using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using Sardauscan.Core.Interface;
using Camera_NET;

namespace Sardauscan.Hardware.Gui.NetCamera
{
	public partial class NetCameraProxyControl : UserControl
	{
		public NetCameraProxyControl()
		{
			InitializeComponent();
		}
		protected string ConfigPath
		{
			get
			{
				if (!Directory.Exists(Program.HardwareConfigPath))
					Directory.CreateDirectory(Program.HardwareConfigPath);
				return Path.Combine(Program.HardwareConfigPath, "DSCamera.Xml");
			}
		}
		void SaveProxyConfig()
		{
			if (Proxy != null)
			{
				NetCameraProxy cam = (NetCameraProxy)Proxy;
				using (StreamWriter sw = File.CreateText(ConfigPath))
				{
					using (XmlWriter writer = XmlWriter.Create(sw))
					{
						writer.WriteStartDocument();
						writer.WriteStartElement("Config");
						writer.WriteStartAttribute("Type");
						writer.WriteValue(cam.GetType().ToString());
						writer.WriteEndAttribute();
						writer.WriteStartAttribute("HardwareId");
						writer.WriteValue(cam.HardwareId);
						writer.WriteEndAttribute();
						writer.WriteEndDocument();
					}
				}
			}
		}
		void LoadProxyConfig()
		{
			if (Proxy != null)
			{
				try
				{
                    NetCameraProxy cam = (NetCameraProxy)Proxy;
					XmlDocument doc = new XmlDocument();
					doc.Load(ConfigPath);
					XmlNode node = doc.SelectSingleNode("Config");
					if (node != null)
					{
						if (node.Attributes["Type"] != null)
						{
							string typestr = node.Attributes["Type"].Value;
							if (Proxy.GetType() != Type.GetType(typestr))
								return;
							if (Proxy.HardwareId != node.Attributes["HardwareId"].Value)
								return;
						}
					}
				}
				catch
				{
				}
			}
		}
		public ICameraProxy Proxy
		{
			get { return LivePreviewBox.Proxy; }
			set
			{
				if (LivePreviewBox.Proxy != value)
				{
					SaveProxyConfig();
					LivePreviewBox.Proxy = value;
					LoadProxyConfig();
				}
			}
		}


		private void Settings_Click(object sender, EventArgs e)
		{
			NetCameraProxy cam = (NetCameraProxy)Proxy;
			if (cam == null)
				return;
			if (cam.Moniker == null)
				return;
			Camera.DisplayPropertyPage_Device(cam.Moniker, this.Handle);
		}

	}
}
