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
using Sardauscan.Core.Interface;
using Sardauscan.Hardware;
using System.Windows.Forms;
using Sardauscan.Gui.Controls;

namespace FakeHardwarePlugins
{
	/// <summary>
	///  Sample of IHardwareProxyProvider
	/// Education and debug purpose sample plugin
	/// 
	/// a plusgin is composed of 2 parts
	/// A IHardwareProxyProvider
	/// and the IHardWareProxy (ILaserProxy in this case)
	/// 
	/// IHardwareProxyProvider is the creator of the IHardwareProxy
	/// It main purpose is to create the instance of the IHardwareProxy 
	/// by proposing a visual interface, or send directly the instance 
	/// if no configuration is necessary
	/// </summary>
	public class FakeLaserProxyProvider : IHardwareProxyProvider 
	{
		
		/// <summary>
		/// for the user to know ;)
		/// </summary>
		public string Name {get { return "*FAKE LASER PROVIDER*"; }}

		
		/// <summary>
		/// tell what type of IHardwareProxy the provider deliver 
		/// </summary>
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
