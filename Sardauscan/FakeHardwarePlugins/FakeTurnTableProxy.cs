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
using Sardauscan.Gui.Controls;
using Sardauscan.Core.Interface;
using Sardauscan.Hardware;

namespace FakeHardwarePlugins
{
	/// <summary>
	/// Fake TurnTable Hardware Proxy for educational purpose ( and debug too ;) )
	/// in this cas the plugin implement also the provider and the hardwareproxy
	/// it is interesting when you don't have any configuration to make 
	/// ( if you can only select one hardware, whitout any parameter)
	/// </summary>
	/// <remarks>
	/// In this cas the object derive from AbstractProxyProvider&lt;ITurnTableProxy&gt;
	/// a abstract class that implement some function for a ITurnTableProxy provider
	/// </remarks>

	public class FakeTurnTableProxy : AbstractProxyProvider<ITurnTableProxy>, ITurnTableProxy
	{
		/// <summary>
		/// Rotate the Table
		/// </summary>
		/// <param name="theta"></param>
		/// <param name="relative"></param>
		/// <returns></returns>
		public int Rotate(double theta, bool relative) { return 0; }

		/// <summary>
		/// Reinit the rotation (ie set 0° to the current position)
		/// </summary>
		public void InitialiseRotation() { }
		/// <summary>
		/// Retreave the minimum rotation of the table
		/// </summary>
		/// <returns></returns>
		public double MinimumRotation() { return 1; }

		/// <summary>
		/// Enable disable motor
		/// </summary>
		public bool MotorEnabled { set { } }

		/// <summary>
		/// Dispose your Hardware proxy
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
			return new FakeTurnTableProxy();
		}


		/// <summary>
		/// Get the associated Viewer of these IHardwareProxy
		/// the viewer allow the user to interact with or tweak the hardware.
		/// you can return null if there is no setting or viewer 
		/// </summary>
		/// <returns></returns>
		public System.Windows.Forms.Control GetViewer()
		{
			// Hey why not using the default turntable interface ?
			TurnTableControl view = new TurnTableControl();
			view.Height = 70;
			view.Proxy = this;
			return view;
		}

		
		/// <summary>
		///Display name of the Provider, for the user to know what he select;) 
		/// </summary>
		public override string Name
		{
			get { return "*FAKE TURNTABLE PROXY*"; }
		}

		/// <summary>
		/// This function is call when the user request a instance of ther IHardwareProxy
		/// You can call winforms to as information ( Com port, configuration etc)
		/// </summary>
		/// <param name="owner">owner window</param>
		/// <returns>a IHardwareProxy if one is selected, Null in case of cancle or not disponible</returns>
		public override object Select(System.Windows.Forms.IWin32Window owner)
		{

			// do whatever interface stuff to select/configure your proxy
			// return the selected one or null
			return new FakeTurnTableProxy();
		}
	}
}
