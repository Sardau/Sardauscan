using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sardauscan.Gui;
using Sardauscan.Core.Interface;
using Sardauscan.Core;
using Camera_NET;
using DirectShowLib;
using System.Runtime.InteropServices.ComTypes;

namespace Sardauscan.Hardware.Gui.NetCamera
{
	public partial class NetCameraSelectionControl : UserControl, IOKCancelView
	{
		public NetCameraSelectionControl()
		{
			InitializeComponent();
		}
		NetCameraProxy NetCamera { get; set; }
		public void DisposeCamera()
		{
			Settings.UnRegisterInstance(NetCamera);
			if (NetCamera != null)
				NetCamera.Dispose();
			NetCamera = null;
		}
		// Camera choice
		private CameraChoice _CameraChoice = new CameraChoice();
		IMoniker Moniker
		{
			get
			{
				if (this.CameraBox.SelectedIndex < 0)
					return null;

				return Camera.GetDeviceMoniker(this.CameraBox.SelectedIndex);
			}
		}

		Resolution Resolution
		{
			get
			{
				try
				{

					if (ResolutionBox.SelectedIndex < 0)
						return null;

					return Camera.GetResolutionList(Moniker)[ResolutionBox.SelectedIndex];
				}
				catch
				{
					return null;
				}

			}
		}

		void FillCameraComboBox()
		{
			_CameraChoice.UpdateDeviceList();
			this.CameraBox.Items.Clear();
			string lastCam = Settings.Get<Settings>().Read(Settings.LAST_USED, Settings.CAMERA_DEVICE, string.Empty);
			int found = -1;
			int i = 0;
			foreach (var camera_device in _CameraChoice.Devices)
			{
				this.CameraBox.Items.Add(camera_device.Name);
				if (camera_device.ClassID.ToString() == lastCam)
					found = i;
				i++;
			}
			try
			{
				if (found != -1)
					CameraBox.SelectedIndex = found;
			}
			catch { }
		}
		private void CameraComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			FillCameraResolutionComboBox();
		}
		private void FillCameraResolutionComboBox()
		{
			this.ResolutionBox.Items.Clear();

			if (Moniker == null)
				return;

			ResolutionList resolutions = Camera.GetResolutionList(Moniker);
			string lastRes = Settings.Get<Settings>().Read(Settings.LAST_USED, Settings.CAMERA_RESOLUTION, string.Empty);
			int found = -1;
			for (int i = 0; i < resolutions.Count; i++)
			{
				this.ResolutionBox.Items.Add(resolutions[i]);
				if (resolutions[i].ToString() == lastRes)
					found = i;
			}
			if (found != -1)
				this.ResolutionBox.SelectedIndex = found;

		}
		public ICameraProxy Proxy { get; set; }
		private void CameraResolutionComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			DisposeCamera();
			if (Moniker != null && Resolution != null)
			{
				try
				{
					NetCamera = new NetCameraProxy(Moniker, Resolution);
					Settings.Get<Settings>().Write(Settings.LAST_USED, Settings.CAMERA_RESOLUTION, Resolution.ToString());
					Guid id;
					Moniker.GetClassID(out id);
					Settings.Get<Settings>().Write(Settings.LAST_USED, Settings.CAMERA_DEVICE, id.ToString());
					Proxy = NetCamera;
					this.CameraControl.Proxy = Proxy;
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


		public bool IsValid()
		{
			return Proxy != null;

		}

		public void OnOk()
		{
		}

		public void OnCancel()
		{
		}

		private void NetCameraSelectionControl_Load(object sender, EventArgs e)
		{
			FillCameraComboBox();
		}
	}
}
