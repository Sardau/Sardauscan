using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sardauscan.Core.Interface;
using Sardauscan.Core;
using Sardauscan.Gui;

namespace Sardauscan.Gui.Controls
{
	public partial class CameraLivePreviewControl : UserControl
	{

		private ICameraProxy _Proxy;
		public ICameraProxy Proxy {
			get
			{
				if (_Proxy!=null)
					return _Proxy;
				return Settings.Get<ICameraProxy>();
			}
			set
			{
				_Proxy = value;
			}
		}

		public int InitialRefreshTime
		{
			get {	return RefreshTimer.Interval;	}
			set { RefreshTimer.Interval = value; }
		}

		public CameraLivePreviewControl()
		{
			InitializeComponent();
		}


		private void RefreshTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				RefreshTimer.Stop();
				if (Visible && Proxy!=null)
				{
						DateTime start = DateTime.Now;
						this.PreviewBox.Image = Proxy.AcquireImage();
						DateTime end = DateTime.Now;
						double acqTime = (end - start).TotalMilliseconds;
						// take a little room
						acqTime *= 1.2;
						if (acqTime > InitialRefreshTime || acqTime * 2 < InitialRefreshTime)
							InitialRefreshTime = (int)Math.Ceiling(acqTime);

				}
			}
			catch
			{
			}
			finally
			{
				RefreshTimer.Start();
			}
		}

		private void CameraLivePreviewControl_Load(object sender, EventArgs e)
		{
			if (UserControlExt.IsDesignMode(this))
				RefreshTimer.Stop();
			else
				RefreshTimer.Start();

		}


	}
}
