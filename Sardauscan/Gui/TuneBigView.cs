#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
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
using Sardauscan.Core.Geometry;
using System.Threading;

namespace Sardauscan.Gui
{
	public partial class TuneBigView : UserControl, IMainView
	{
		/// <summary>
		/// Default ctor
		/// </summary>
		public TuneBigView()
		{
			InitializeComponent();
		}

		protected ICameraProxy CameraProxy { get { return Settings.Get<ICameraProxy>(); } }
		protected ILaserProxy LaserProxy { get { return Settings.Get<ILaserProxy>(); } }
		public bool Available
		{
			get 
			{
				return CameraProxy!=null  && LaserProxy!=null; 
			}
		}

		public void SetPicture(PictureBox pb, Bitmap bmp)
		{
			Image old = pb.Image;
			pb.Image = bmp;
			if (old != null)
				old.Dispose();
		}

		public void TestLaser(int laserIndex, double theshold, int minLaserWidth, int maxLaserWidth)
		{
			if (!Available)
			{
				this.ReferencePictureBox.Image = global::Sardauscan.Properties.Resources.Denied;
				this.LaserPictureBox.Image = global::Sardauscan.Properties.Resources.Denied;
				this.DifferencePictureBox.Image = global::Sardauscan.Properties.Resources.Denied;
				this.ResultPictureBox.Image = global::Sardauscan.Properties.Resources.Denied;
				return;
			}
			try
			{
                int fadeTime = Settings.Get<Settings>().Read(Settings.LASER_COMMON, Settings.FADE_DELAY, 100);
                // turn all laser off and take reference picture
				LaserProxy.TurnAll(false);
                Thread.Sleep(fadeTime); // wait fade laser
                Bitmap refImg = CameraProxy.AcquireImage();
				SetPicture(ReferencePictureBox,refImg);

				// take laser picture
				LaserProxy.Turn(laserIndex, true);
                Thread.Sleep(fadeTime); // wait fade laser
				Bitmap laserImg = CameraProxy.AcquireImage();
				SetPicture(LaserPictureBox,laserImg);

				Bitmap debuggingImage = new Bitmap(refImg.Width, refImg.Height);
				
				ImageProcessor imgproc = new ImageProcessor(theshold, minLaserWidth, maxLaserWidth);

                List<PointF> laserloc = imgproc.Process(refImg,laserImg,debuggingImage);

				SetPicture(DifferencePictureBox,debuggingImage);

				// Write the pixel image
				debuggingImage = BitmapExtention.SavePixels(laserloc, refImg.Width, refImg.Height);
				SetPicture(ResultPictureBox,debuggingImage);
			}
			catch { }
		}
	}
}
