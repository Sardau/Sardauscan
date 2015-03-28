using Sardauscan.Gui.Forms;
using Sardauscan.Hardware.Gui.NetCamera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sardauscan.Hardware
{
	public class NetCameraProxyProvider : AbstractProxyProvider<NetCameraProxy>
	{
		/// <summary>
		/// Name of the provider
		/// </summary>
		public override string Name
		{
			get { return "Direct Show Cameras"; }
		}
		/// <summary>
		/// Selection of the Camera
		/// </summary>
		/// <param name="owner"></param>
		/// <returns></returns>
		public override object Select(System.Windows.Forms.IWin32Window owner)
		{
			OkCancelDialog dlg = new OkCancelDialog();
			dlg.Text = Name;
			NetCameraSelectionControl view = new NetCameraSelectionControl();
			if (dlg.ShowDialog(view, owner) == System.Windows.Forms.DialogResult.OK)
			{
				return view.Proxy;
			}
			return null;
		}
	}
}
