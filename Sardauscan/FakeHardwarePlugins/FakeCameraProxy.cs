using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Sardauscan.Core.Interface;
using Sardauscan.Hardware;

namespace FakeHardwarePlugins
{
	public class FakeCameraProxy : AbstractProxyProvider<ICameraProxy>, ICameraProxy
	{
		Image bmp;

		public FakeCameraProxy()
		{
			bmp = new Bitmap(100, 100);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);
				StringFormat sf = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip);
				sf.Alignment = StringAlignment.Center;
				sf.LineAlignment = StringAlignment.Center;
				g.DrawString("Fake Camera", new Font("Arial", 12), Brushes.Red, new Rectangle(0, 0, bmp.Width, bmp.Height), sf);
			}
		}
		public Bitmap AcquireImage() { return (Bitmap)bmp; }

		public int ImageHeight { get { return bmp.Height; } }

		public int ImageWidth { get { return bmp.Width; } }

		public float SensorWidth { get { return 3.629f; } }
		public float SensorHeight { get { return 2.722f; } }
		public float FocalLength { get { return 3.6f; } }

		public void Dispose() { }

		public String HardwareId { get { return "Unique ID to reload th same"; } }

		public IHardwareProxy LoadFromHardwareId(string hardwareId)
		{
			// load a proxy using the hardwareid, for automatic reload of the last used
			return new FakeCameraProxy();
		}
		public System.Windows.Forms.Control GetViewer() { return null; }

		public override string Name
		{
			get { return "*FAKE CAMERA PROXY*"; }
		}

		public override object Select(System.Windows.Forms.IWin32Window owner)
		{
			return new FakeCameraProxy();
		}
	}
}
