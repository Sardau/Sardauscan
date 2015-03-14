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
using System.IO.Ports;
using Sardauscan.Core.Interface;
using System.Xml;
using System.Threading;
using System.Diagnostics;
using Sardauscan.Core;
using Sardauscan.Hardware.Com;
using Sardauscan.Hardware.Gui;

namespace Sardauscan.Hardware
{
	public class SardauscanHardwareProxy : ITurnTableProxy, ILaserProxy
	{

		public const int BAUD_RATE = 115200;
		public SardauscanHardwareProxy()
		{
			// for load from HardwareID (it use a instance to call LoadFromHardwareId
		}
		public SardauscanHardwareProxy(PortInfo info)
			: this(info.Name)
		{
		}

		Settings Settings { get { return Settings.Get<Settings>(); } }
		protected int LaserTimeout { get { return Settings.Read(Settings.COM, Settings.LASER_TIMEOUT, 500); } }
		protected int TableTimeout { get { return Settings.Read(Settings.COM, Settings.TABLE_TIMEOUT, 500); } }
		protected int InfoTimeout { get { return Settings.Read(Settings.COM, Settings.INFO_TIMEOUT, 1000); } }
		protected int ConnectionTimeout { get { return Settings.Read(Settings.COM, Settings.CONNECTION_TIMEOUT, 5000); } }
		public SardauscanHardwareProxy(string comName)
		{

			PortName = comName;
			Serial = Provider.Create(PortName);
			Serial.BaudRate = BAUD_RATE;
			Serial.ReadTimeout = 500;
			Serial.NewLine = "\r\n";
			Serial.Open();
			Thread.Sleep(1000);
			string ex = Serial.ReadExisting();
			Debug.WriteLine("<==" + ex);
			Serial.DiscardInBuffer();
			try
			{
				string ret = string.Empty;
				try
				{
					ret = SendCommand("sardauscan", this.ConnectionTimeout);
				}
				catch (TimeoutException)
				{
					Thread.Sleep(this.ConnectionTimeout / 2);
					ret = SendCommand("sardauscan", this.ConnectionTimeout);
				}
				if (ret.ToLower() != "yes")
					throw new Exception("Invalid com Port :" + comName);
				else
					Configure();
			}
			catch (Exception e)
			{
				if (Serial != null && Serial.IsOpen)
					Serial.Close();
				throw e;
			}
		}

		protected void Configure()
		{
			/// ASK STEP
			/// T S
			Int32 steps = 4072; // default
			try
			{
				string ret = SendCommand("T S", this.InfoTimeout);
				string[] val = ret.Trim().Split(" ".ToArray());
				Int32 v = -1;

				for (int i = 0; i < val.Length && v == -1; i++)
					if (!Int32.TryParse(val[i], out v))
						v = -1;
				if (v != -1)
					steps = v;

				LaserStatus = new List<bool>();
				ret = SendCommand("L", this.InfoTimeout);
				val = ret.Trim().Split(" ".ToArray());
				int count = 1;
				if (!(val.Length > 1 && Int32.TryParse(val[1], out count)))
					count = 1;
				for (int i = 0; i < count; i++)
				{
					ret = SendCommand(string.Format("L {0} 0", i), this.LaserTimeout); // off the laser
					LaserStatus.Add(false);
				}
			}
			catch
			{
			}

			RevolutionStep = steps;
		}

		protected List<bool> LaserStatus { get; set; }

		protected double StepsToDegree(int steps)
		{
			return steps * 360.0f / RevolutionStep;
		}
		protected int DegreeToSteps(double degree)
		{
			return (int)Math.Round((degree / 360.0) * RevolutionStep);
		}

		protected Int32 RevolutionStep = -1;

		public double MinimumRotation()
		{
			return StepsToDegree(1);
		}

		protected string SendCommand(string command, int timeout)
		{
			try
			{
				Serial.ReadTimeout = timeout;
				try
				{
					string ex = Serial.ReadExisting();
					if (!string.IsNullOrEmpty(ex))
						Debug.WriteLine("<==*" + ex);
				}
				catch { }
				Debug.WriteLine("==>" + command + ":");
				Serial.WriteLine(command);
				//Thread.Sleep(timeout/4);
				string ret = Serial.ReadLine().TrimEnd("\r\n".ToCharArray());
				Debug.WriteLine("<==" + ret);
				return ret;
			}
			catch
			{
				return string.Empty;
			}
		}

		private SerialPort Serial;

		public int Rotate(double theta, bool relative)
		{
			SendRotateCommand(theta, relative);
			return 0;
		}
		public void InitialiseRotation()
		{
			SendCommand("T C", this.InfoTimeout);
			int maxError = 100;
			int count = 0;
			int pos = GetAbosuteRotation();
			while (count < maxError && pos != 0)
			{
				Rotate(0, false);
				int val = GetAbosuteRotation();
				if (val == -1)
					count++;
				pos = val;
			}
			if (maxError == count)
				throw new Exception("Too much request Absolute position error");
		}
		int GetAbosuteRotation()
		{
			string ret = SendCommand("T", this.InfoTimeout);
			string[] val = ret.Trim().Split(" ".ToArray());
			int pos = -1;
			if (!(val.Length > 1 && Int32.TryParse(val[1], out pos)))
				pos = -1;
			return pos;
		}
		void SendRotateCommand(double angle, bool relative)
		{
			string ret = SendCommand(string.Format("T {0} {1}", relative ? "R" : "A", DegreeToSteps((double)angle)), this.TableTimeout);
			return;
		}

		public bool MotorEnabled
		{
			set { ;}
		}

		/// <summary>
		/// Dispose object
		/// </summary>
		public void Dispose()
		{
			Serial.Close();
			Serial = null;
		}

		public string PortName { get; private set; }
		#region IProxy
		public void SaveSettings(XmlWriter writer)
		{
			writer.WriteStartElement(this.GetType().ToString());
			/*........*/
			writer.WriteEndElement();
		}

		#endregion

		public void Turn(int index, bool on)
		{
			if (index >= 0 && index < Count)
			{
				LaserStatus[index] = on;
				SendCommand(string.Format("L {0} {1}", index, on ? 1 : 0), this.LaserTimeout);
			}
		}

		public bool On(int index)
		{
			if (index >= 0 && index < Count)
				return LaserStatus[index];
			return false;
		}

		public int Count
		{
			get { return LaserStatus.Count; ; }
		}
		#region IHardwareProxy
		public string HardwareId
		{
			get { return PortName; }
		}
		public static SardauscanHardwareProxy FromHardwareId(string hardwareId)
		{
				return new SardauscanHardwareProxy(hardwareId);
		}
		public IHardwareProxy LoadFromHardwareId(string hardwareId)
		{
			try
			{
				return FromHardwareId(hardwareId);
			}
			catch
			{
				return null;
			}
		}
		public System.Windows.Forms.Control GetViewer()
		{
			SardauscanProxyControl viewer = new SardauscanProxyControl();
			viewer.Proxy = (IHardwareProxy)Settings.Get<SardauscanHardwareProxy>();
			return viewer;
		}
		#endregion
	}
}
