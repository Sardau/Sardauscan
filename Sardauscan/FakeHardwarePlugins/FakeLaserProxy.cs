/*
 ****************************************************************************
 *  Copyright (c) 2015 Fabio Ferretti <Fabio@ferretti.info>                 *
 *	This file is part of Sardauscan.                                        *
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
 *   You should have received a copy of the GNU General Public License      *
 *   along with Sardauscan .  If not, see <http://www.gnu.org/licenses/>.   *
 ****************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sardauscan.Core.Interface;
using Sardauscan.Hardware;
using System.Windows.Forms;
using Sardauscan.Gui.Controls;

/// Education and debug purpose sample plugin

/// a plusgin is composed of 2 parts
/// A IHardwareProxyProvider
/// and the IHardWareProxy (ILaserProxy in this case)

/// IHardwareProxyProvider is the creator of the IHardwareProxy
/// It main purpose is to create the instance of the IHardwareProxy 
/// by proposing a visual interface, or send directly the instance 
/// if no configuration is necessary

namespace FakeHardwarePlugins
{
	/// <summary>
	///  Sample of IHardwareProxyProvider
	/// </summary>
	public class FakeLaserProxyProvider : IHardwareProxyProvider 
	{
		// for the user to know ;)
		public string Name {get { return "*FAKE LASER PROVIDER*"; }}

		// tell what type of IHardwareProxy the provider deliver
		public Type GenerateType 
		{
			get {
				return typeof(FakeLaserProxy); //return the typeof of the associated proxy, It can have more than one interface (for exemple ITurnTableProxy and ILaserProxy)
			}
		}

		/// <summary>
		/// This function is call when the user request a instance of ther IHardwareProxy
		/// You can call winforms to as information ( Com port, configuration etc)
		/// </summary>
		/// <param name="owner">owner window</param>
		/// <returns>a IHardwareProxy if one is selected, Null in case of cancle or not disponible</returns>
		public object Select(IWin32Window owner)
		{
			// do whatever interface stuff to select/configure your proxy
			// return the selected one
			return new FakeLaserProxy();
		}
	}
	/// <summary>
	///  The Hardware proxy
	/// </summary>
	public class FakeLaserProxy : ILaserProxy
	{
		List<bool> State;
		/// <summary>
		/// Cto must be whitout parameter !!
		/// </summary>
		public FakeLaserProxy() { State = new List<bool>(Count); for (int i = 0; i < Count; i++)State.Add(false); }

		/// <summary>
		/// ILaserProxy.TurnOn
		/// Turn on or off a laser
		/// </summary>
		/// <param name="index"></param>
		/// <param name="on"></param>
		public void Turn(int index, bool on) { State[index] = on; }

		/// <summary>
		/// ILaserProxy.On
		/// as if a laser is on or off
		/// </summary>
		public bool On(int index) { return State[index]; }

		/// <summary>
		/// ILaserProxy.Count
		/// Number of Laser(s)
		/// </summary>
		public int Count { get { return 4; } }

		/// <summary>
		/// Czalled when the IHardwareProxy is released
		/// </summary>
		public void Dispose() { }

		/// <summary>
		/// A unique id to identify a specific instance of IHardwareProxy (mainly used for reload a IHardwareproxy, so store all the properties)
		/// </summary>
		public String HardwareId { get { return "Unique ID to reload th same"; } }

		/// <summary>
		///  Load a IHardwareProxy with a specific HardwareId 
		/// </summary>
		/// <param name="hardwareId"></param>
		/// <returns> the loaded IHardwareProxy or null if you can't reload it</returns>
		public IHardwareProxy LoadFromHardwareId(string hardwareId)
		{
			// load a proxy using the hardwareid, for automatic reload of the last used
			return new FakeLaserProxy();
		}

		/// <summary>
		/// Get the associated Viewer of these IHardwareProxy
		/// the viewer allow the user to interact with or tweak the hardware.
		/// you can return null.
		/// </summary>
		/// <returns></returns>
		public Control GetViewer()
		{
			// hey why not use the default build in Laser Viewer to let the user play with the lasers? ;)
			LaserControl view = new LaserControl();
			view.Proxy = this;
			return view;
		}
	}
}
