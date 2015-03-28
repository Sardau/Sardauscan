using Emgu.CV;
using Sardauscan.Core.Interface;
using Sardauscan.Hardware;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenCVPluggin
{
	public class OpenCVCameraProxy : ICameraProxy
	{
		#region static
		protected static Dictionary<int, Capture> _Captures = new Dictionary<int, Capture>();

		public static Capture GetCapture(string hardwareId)
		{
			int id;
			if (int.TryParse(hardwareId, out id))
			{
				if (!_Captures.Keys.Contains(id))
				{
					_Captures[id] = new Capture(id);
					return _Captures[id];
				}
			}
			return null;
		}
		#endregion
		Capture _Capture;
		public OpenCVCameraProxy(string hid=null)
		{
			Capture capture = string.IsNullOrEmpty(hid) ? null : GetCapture(hid);
			if(capture!=null)
			{
				this.HardwareId = hid;
					_Capture = capture;
					_Capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 1920);
					_Capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 1080);
					_Capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FPS, 30);
 				_Capture.Start();
			
					Bitmap bmp = AcquireImage();
					this.ImageHeight = bmp.Height;
					this.ImageWidth = bmp.Width;

			}
		}

		public System.Drawing.Bitmap AcquireImage()
		{
			if (_Capture.Grab())
			{
				var frame = _Capture.QueryFrame();
				if (frame != null)
				{
					Bitmap f = frame.Bitmap;
					/*
					Bitmap ret = new Bitmap(f);
					using (Graphics g = Graphics.FromImage(ret))
						g.DrawImageUnscaled(f, 0, 0);
					f.Dispose();
					f = ret;*/
					_Capture.Stop(); 
					return f;
				}
			}
			return null;
		}


		public int ImageHeight { get; set; }

		public int ImageWidth { get; set; }

		/// <summary>
		/// Get the sensor width
		/// </summary>
		public double SensorWidth { get { return 3.629f; } }
		/// <summary>
		/// Get the sensor height
		/// </summary>
		public double SensorHeight { get { return 2.722f; } }
		/// <summary>
		/// Get the focal lenght
		/// </summary>
		public double FocalLength { get { return 3.6f; } }
		/// <summary>
		/// dispose your Camera proxy
		/// </summary>
		public void Dispose()
		{
            _Capture.Stop();
            _Capture = null;
		}

		protected string _HardwareId=null;
		public string HardwareId
		{
			get { return _HardwareId; }
			set { _HardwareId = value; }
		}

		public IHardwareProxy LoadFromHardwareId(string hardwareId)
		{
			try
			{
				if (!string.IsNullOrEmpty(hardwareId))
				{
					var cam = new OpenCVCameraProxy(hardwareId);
					if (!string.IsNullOrEmpty(cam.HardwareId))
						return new OpenCVCameraProxy(hardwareId);
				}
			}
			catch
			{

			}
			return null;
		}

		public System.Windows.Forms.Control GetViewer()
		{
			return null;
		}
	}

	public class OpenCVCameraProvider : AbstractProxyProvider<ICameraProxy>
	{
		public OpenCVCameraProvider()
		{

		}


		public override string Name
		{
			get { return "OpenCV"; }
		}

		public override object Select(System.Windows.Forms.IWin32Window owner)
		{
			return new OpenCVCameraProxy("0");
		}


	}
}
