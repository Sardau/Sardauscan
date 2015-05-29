using Camera_NET;
using DirectShowLib;
using Sardauscan.Core.Interface;
using Sardauscan.Hardware;
using Sardauscan.Hardware.Gui.NetCamera;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Sardauscan.Hardware
{
	public class NetCameraProxy : IDisposable, ICameraProxy
	{
		private NetCamera _Camera = null;
		public IMoniker Moniker
		{
			get
			{
				if (_Camera != null)
					return _Camera.Moniker;
				return null;
			}
		}

		private RotateFlipType _Rotation = 0;
		public RotateFlipType Rotation
		{
			get
			{
				return _Rotation;
			}
			set
			{
				_Rotation = value;
			}
		}
		public NetCameraProxy()
            : this(null,null)
        { }

		public NetCameraProxy(IMoniker moniker , Resolution resolution )
		{
			if(moniker!=null && resolution!=null)
				SetCamera(moniker, resolution);
		}



		/// <summary>
		/// Initializes camera, builds and runs graph for control.
		/// </summary>
		/// <param name="moniker">Moniker (device identification) of camera.</param>
		/// <param name="resolution">Resolution of camera's output.</param>
		public void SetCamera(IMoniker moniker, Resolution resolution)
		{
			// Close current if it was opened
			CloseCamera();

			if (moniker == null)
				return;

			// Create camera object
			_Camera = new NetCamera();

			if (resolution != null)
			{
				_Camera.Resolution = resolution;
			}

			// Initialize
			_Camera.Initialize(moniker);

			// Build and Run graph
			_Camera.BuildGraph();
			_Camera.RunGraph();
		}

		public Bitmap AcquireImage()
		{
			Bitmap bmp = _Camera.SnapshotSourceImage();
			if(Rotation!= RotateFlipType.RotateNoneFlipNone)
				bmp.RotateFlip(Rotation);
			return bmp;
		}


		public void Dispose()
		{
			CloseCamera();
		}
		/// <summary>
		/// Close and dispose all camera and DirectX stuff.
		/// </summary>
		public void CloseCamera()
		{
			if (_Camera != null)
			{
				_Camera.StopGraph();
				_Camera.CloseAll();
				_Camera.Dispose();
				_Camera = null;
			}
		}



		public int ImageHeight
		{
			get
			{
				if (_Camera != null && _Camera.Resolution != null)
					return _Camera.Resolution.Height;
				return 0;
			}
		}

		public int ImageWidth
		{
			get
			{
				if (_Camera != null && _Camera.Resolution != null)
					return _Camera.Resolution.Width;
				return 0;
			}
		}

		public double SensorWidth
		{
			get
			{
				return 0.4111f;
			}
		}

		public double SensorHeight
		{
			get
			{
				return 0.37f;
			}
		}
		public double FocalLength
		{
			get
			{
				return 0.5f;
			}
		}

		public string HardwareId
		{
			get {

				string ret = string.Empty;
				if(_Camera!=null && _Camera.Moniker!=null)
				{
					Guid id;
					_Camera.Moniker.GetClassID(out id);
					ret += string.Format("{0}", id);
					if(_Camera.Resolution!=null)
						ret += string.Format("|{0}", _Camera.Resolution);
					else
						ret += "|";
					ret += string.Format("|{0}", Rotation);
				}
				return ret;
			}
		}

		public IHardwareProxy LoadFromHardwareId(string hardwareId)
		{
			if(string.IsNullOrEmpty(hardwareId))
				return null;
			string[] parts = hardwareId.Split("|".ToCharArray());
			if (parts.Length < 2)
				return null;

			Guid id = new Guid(parts[0]);
			string resstr = parts[1];

			CameraChoice cams = new CameraChoice();
            cams.UpdateDeviceList();
			IMoniker moniker= null;
			foreach (var camera_device in cams.Devices)
			{
				if(camera_device.Mon!=null)
				{
					Guid camid;
					camera_device.Mon.GetClassID(out camid);
					if(camid == id)
					{
						moniker = camera_device.Mon;
						break;
					}
				}
			}
			if (moniker == null)
				return null;
			ResolutionList resolutions = Camera.GetResolutionList(moniker);
			if (resolutions == null)
				return null;
			RotateFlipType rotation = RotateFlipType.RotateNoneFlipNone;
			if (parts.Length >= 3)
				rotation = (RotateFlipType)Enum.Parse(typeof(RotateFlipType),parts[2],true);

			for (int index = 0; index < resolutions.Count; index++)
			{
				if (resstr == resolutions[index].ToString())
				{
					NetCameraProxy proxy = new NetCameraProxy();
					proxy.SetCamera(moniker, resolutions[index]);
					proxy.Rotation = rotation;
					return proxy;
				}
			}
			return null;
		}

		public System.Windows.Forms.Control GetViewer()
		{
			return new NetCameraProxyControl();
		}
	}
}
