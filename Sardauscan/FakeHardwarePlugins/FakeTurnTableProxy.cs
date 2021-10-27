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
