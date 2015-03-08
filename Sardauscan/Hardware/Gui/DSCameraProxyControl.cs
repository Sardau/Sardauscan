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
using System.IO;
using System.Xml;

namespace Sardauscan.Hardware.Gui
{
	public partial class DSCameraProxyControl : UserControl, IDisposable
	{
		/// <summary>
		/// Default ctor
		/// </summary>
		public DSCameraProxyControl()
		{
			InitializeComponent();
			Application.Idle += OnIdle;
		}
		/// <summary>
		/// Dispose object
		/// </summary>
		public new void Dispose()
		{
			base.Dispose();
			Application.Idle -= OnIdle;
			SaveProxyConfig();
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
				DSCameraProxy cam = (DSCameraProxy)Proxy;
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

						List<DirectShowLib.CameraControlProperty> list = Enum.GetValues(typeof(DirectShowLib.CameraControlProperty)).Cast<DirectShowLib.CameraControlProperty>().ToList();
						foreach( DirectShowLib.CameraControlProperty prop in list)
						{
							DSCameraProxy.ControlPropertyInfo info = cam.GetControlPropertyInfo(prop, true);
							if (info != null)
							{
								writer.WriteStartElement("Prop");
								writer.WriteStartAttribute("Name");
								writer.WriteValue(prop.ToString());
								writer.WriteEndAttribute();
								writer.WriteStartAttribute("Value");
								writer.WriteValue(info.Value);
								writer.WriteEndAttribute();
								writer.WriteEndElement();
							}
						}
						writer.WriteEndDocument();
					}
				}


				//TODO
				// SAVE TO Program.HardwareConfigPath\[PROXYTYPE].HARDWARE.XML
			}
		}
		void LoadProxyConfig()
		{
			if (Proxy != null)
			{
				try
				{
					DSCameraProxy cam = (DSCameraProxy)Proxy;
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
							XmlNodeList proplist = node.SelectNodes("Prop");
							foreach(XmlNode popNode in proplist)
							{
								DirectShowLib.CameraControlProperty prop = (DirectShowLib.CameraControlProperty)Enum.Parse(typeof(DirectShowLib.CameraControlProperty), popNode.Attributes["Name"].Value);
								cam.SetControlProperty(prop, Int32.Parse(popNode.Attributes["Value"].Value));
							}
						}
					}
				}
				catch
				{
				}
			}
		}
		ICameraProxy _Proxy = null;
		public ICameraProxy Proxy
		{ 
			get { return _Proxy; } 
			set 
			{
				if (_Proxy != value)
				{
					SaveProxyConfig();
					_Proxy = value;
					LoadProxyConfig();
				}
				AlignSliders(); 
			} 
		}

		protected void AlignSliders()
		{
			foreach (Control c in Controls)
			{
				if (c is DSCam.DSCamPropertySlider)
				{
					DSCam.DSCamPropertySlider slider = (DSCam.DSCamPropertySlider)c;
					slider.Camera = (DSCameraProxy)Proxy;
				}
			}
		}

		DateTime lastImageTime = DateTime.Now;
		private void OnIdle(object sender, EventArgs e)
		{
			try
			{
				bool ignore = false;
				if (!ignore && Visible)
				{
					DateTime now = DateTime.Now;
					bool expired = (now - lastImageTime).TotalMilliseconds > 100;
					if (expired)
					{
						if (Proxy != null)
						{
							this.PreviewPictureBox.Image = Proxy.AcquireImage();
							lastImageTime = now;
							this.PreviewPictureBox.Invalidate();
						}
						else
						{
							bool lasthasImage = this.PreviewPictureBox.Image != null;
							this.PreviewPictureBox.Image = null;
							if (lasthasImage)
								this.PreviewPictureBox.Invalidate();
						}
					}
				}
			}
			catch
			{
			}
		}
		
	}
}
