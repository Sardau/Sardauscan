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
