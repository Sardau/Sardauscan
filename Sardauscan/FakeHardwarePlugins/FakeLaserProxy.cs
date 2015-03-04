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
	/// 
	/// </summary>
	public class FakeLaserProxyProvider : IHardwareProxyProvider 
	{
		// for the user to know ;)
		public string Name {get { return "*FAKE LASER PROVIDER*"; }}

		// tell what the provider deliver
		public Type GenerateType 
		{
			get { 
				return typeof(FakeLaserProxy); //return the typeof of the associated proxy, can have more than one interface
			}
		}

		public object Select(IWin32Window owner)
		{
			// do whatever interface stuff to select/configure your proxy
			// return the selected one
			return new FakeLaserProxy();
		}
	}
	/// <summary>
	/// 
	/// </summary>
	public class FakeLaserProxy : ILaserProxy
	{
		List<bool> State;
		public FakeLaserProxy() { State = new List<bool>(Count); for (int i = 0; i < Count; i++)State.Add(false); }

		public void Turn(int index, bool on) { State[index] = on; }

		public bool On(int index) { return State[index]; }

		public int Count { get { return 4; } }

		public void Dispose() { }

		public String HardwareId { get { return "Unique ID to reload th same"; } }

		public IHardwareProxy LoadFromHardwareId(string hardwareId)
		{
			// load a proxy using the hardwareid, for automatic reload of the last used
			return new FakeLaserProxy();
		}

		public Control GetViewer()
		{
			LaserControl view = new LaserControl();
			view.Proxy = this;
			return view;
		}
	}
}
