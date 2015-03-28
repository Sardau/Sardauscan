using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices.ComTypes;
using Camera_NET;
using Sardauscan.Hardware;

namespace opencvTest
{
	public partial class CameraSelectionControl : UserControl
	{


		public Bitmap Snaphot()
		{
			return this.proxy.AcquireImage();
		}
		public Bitmap CurrentImage()
		{
			return Preview.Image as Bitmap;
		}


		// Camera choice
		private CameraChoice _CameraChoice = new CameraChoice();

		NetCameraProxy proxy = new NetCameraProxy();

		public CameraSelectionControl()
		{
			InitializeComponent();
		}

		private void CameraSelctionControl_Load(object sender, EventArgs e)
		{
			FillCameraCombo();
		}

		void FillCameraCombo()
		{
			_CameraChoice.UpdateDeviceList();
			this.CameraBox.Items.Clear();
			foreach (var camera_device in _CameraChoice.Devices)
			{
				this.CameraBox.Items.Add(camera_device.Name);
			}
		}

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
		void FillResolutionComboBox()
		{
			ResolutionBox.Items.Clear();

			if (Moniker == null)
				return;

			ResolutionList resolutions = Camera.GetResolutionList(Moniker);

			if (resolutions == null)
				return;


			int index_to_select = -1;

			for (int index = 0; index < resolutions.Count; index++)
			{
				ResolutionBox.Items.Add(resolutions[index].ToString());
			}

			// select current resolution
			if (index_to_select >= 0)
			{
				ResolutionBox.SelectedIndex = index_to_select;
			}

		}

		private void CameraBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			FillResolutionComboBox();
		}

		private void ConnectButton_Click(object sender, EventArgs e)
		{
			proxy.SetCamera(Moniker, Resolution);
			this.LivePreview.Proxy = proxy;
			SnapshotButton_Click(sender, e);
		}

		private void SnapshotButton_Click(object sender, EventArgs e)
		{
			Bitmap bitmap = null;
			try
			{
				bitmap = this.proxy.AcquireImage();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, @"Error while getting a snapshot");
			}

			if (bitmap == null)
				return;
			this.Preview.Image = bitmap;
			this.Preview.Update();

		}
		private void Settings_Click(object sender, EventArgs e)
		{
			Camera.DisplayPropertyPage_Device(Moniker, this.Handle);
		}
	}
}
