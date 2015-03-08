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
using Sardauscan.Core;
using Sardauscan.Core.Interface;
using Sardauscan.Gui;

namespace Sardauscan.Hardware.Gui
{
	public partial class DSCameraSelectionControl : UserControl, IOKCancelView
	{
		/// <summary>
		/// Default ctor
		/// </summary>
		public DSCameraSelectionControl()
		{
			InitializeComponent();
		}
		DSCameraProxy Camera { get; set; }
		public void DisposeCamera()
		{
			Settings.UnRegisterInstance(Camera);
			if (Camera != null)
				Camera.Dispose();
			Camera = null;
		}
		void FillCameraComboBox()
		{

			this.CameraComboBox.Items.Clear();
			string lastCam = Settings.Get<Settings>().Read(Settings.LAST_USED, Settings.CAMERA_DEVICE, string.Empty);
			List<DSCameraInfo> camInfos = DSCameraInfo.GetAvailableCamera();
			int found = -1;
			for (int i = 0; i < camInfos.Count; i++)
			{
				DSCameraInfo cam = camInfos[i];
				this.CameraComboBox.Items.Add(cam);
				if (cam.UniqueId == lastCam)
					found = i;
			}
			try
			{
				if (found != -1)
					CameraComboBox.SelectedIndex = found;
			}
			catch { }
		}
		private void CameraComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			DSCameraInfo camInfo = (DSCameraInfo)this.CameraComboBox.SelectedItem;

			FillCameraResolutionComboBox(camInfo);
		}
		private List<Sardauscan.Hardware.DSCameraProxy.Resolution> FilterResolutions(List<Sardauscan.Hardware.DSCameraProxy.Resolution> list)
		{
			List<Sardauscan.Hardware.DSCameraProxy.Resolution> ret = new List<DSCameraProxy.Resolution>();

			foreach (Sardauscan.Hardware.DSCameraProxy.Resolution res in list)
			{
				if (!ret.Contains(res))
				{
					//if (res.Width % 160 == 0)
					ret.Add(res);
				}
			}

			ret.Sort();
			return ret;
		}
		private void FillCameraResolutionComboBox(DSCameraInfo camInfo)
		{
			this.CameraResolutionComboBox.Items.Clear();
			List<Sardauscan.Hardware.DSCameraProxy.Resolution> list = camInfo.GetAvailableResolution();
			list = FilterResolutions(list);
			string lastRes = Settings.Get<Settings>().Read(Settings.LAST_USED, Settings.CAMERA_RESOLUTION, string.Empty);
			int found = -1;
			for (int i = 0; i < list.Count; i++)
			{
				this.CameraResolutionComboBox.Items.Add(list[i]);
				if (list[i].ToString() == lastRes)
					found = i;
			}
			if (found != -1)
				this.CameraResolutionComboBox.SelectedIndex = found;
	
		}
		public ICameraProxy Proxy { get;set; }
		private void CameraResolutionComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			//            const int VIDEOWIDTH = 1024; // Depends on video device caps
			//           const int VIDEOHEIGHT = 480; // Depends on video device caps
			//            const int VIDEOWIDTH = 1280; // Depends on video device caps
			//            const int VIDEOHEIGHT = 720; // Depends on video device caps
			//            const int VIDEOBITSPERPIXEL = 24; // BitsPerPixel values determined by device

			Sardauscan.Hardware.DSCameraProxy.Resolution res = (Sardauscan.Hardware.DSCameraProxy.Resolution)this.CameraResolutionComboBox.SelectedItem;
			DSCameraInfo camInfo = (DSCameraInfo)this.CameraComboBox.SelectedItem;
			DisposeCamera();
			if (res != null && camInfo != null)
			{
				try
				{
					Camera = new DSCameraProxy(camInfo, res, null/*PreviewPanel*/);
					Settings.Get<Settings>().Write(Settings.LAST_USED, Settings.CAMERA_RESOLUTION, res.ToString());
					Settings.Get<Settings>().Write(Settings.LAST_USED, Settings.CAMERA_DEVICE, camInfo.UniqueId);
					Proxy = Camera;
					this.PreviewControl.Proxy = Proxy ;
				}
				catch
				{
				}
			}
		}
		private void ReloadCameraCombo(object sender, EventArgs e)
		{
			FillCameraComboBox();
		}

		private void DirectShowLibCameraSelectionView_Load(object sender, EventArgs e)
		{
			FillCameraComboBox();
		}

		private void PlugButton_Click(object sender, EventArgs e)
		{
			CameraResolutionComboBox_SelectedIndexChanged(sender, e);
		}

		public bool IsValid()
		{
			return this.PreviewControl.Proxy != null;
		}

		public void DisposePreview()
		{
			if (this.PreviewControl is IDisposable)
				((IDisposable)this.PreviewControl).Dispose();
		}
		public void OnOk()
		{
			DisposePreview();
		}

		public void OnCancel()
		{
			DisposePreview();
		}
	}
}
