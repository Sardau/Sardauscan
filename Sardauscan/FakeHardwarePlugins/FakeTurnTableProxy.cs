using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sardauscan.Gui.Controls;
using Sardauscan.Core.Interface;
using Sardauscan.Hardware;

namespace FakeHardwarePlugins
{
	public class FakeTurnTableProxy : AbstractProxyProvider<ITurnTableProxy>, ITurnTableProxy
	{

		public int Rotate(double theta, bool relative) { return 0; }

		public void InitialiseRotation() { }

		public float MinimumRotation() { return 1; }

		public bool MotorEnabled { set { } }

		public void Dispose() { }

		public String HardwareId { get { return "Unique ID to reload th same"; } }

		public IHardwareProxy LoadFromHardwareId(string hardwareId)
		{
			// load a proxy using the hardwareid, for automatic reload of the last used
			return new FakeTurnTableProxy();
		}
		 

		public System.Windows.Forms.Control GetViewer()
		{
			TurnTableControl view = new TurnTableControl();
			view.Height = 70;
			view.Proxy = this;
			return view;
		}

		public override string Name
		{
			get { return "*FAKE TURNTABLE PROXY*"; }
		}

		public override object Select(System.Windows.Forms.IWin32Window owner)
		{
			return new FakeTurnTableProxy();
		}
	}
}
