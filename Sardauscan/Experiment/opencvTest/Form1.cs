using Emgu.CV;
using Emgu.CV.Structure;
using Sardauscan.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace opencvTest
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
		}


		Image<Gray, Byte> thinningIteration(Image<Gray, Byte> im, int iter)
		{
				Image<Gray, Byte> marker =  new Image<Gray,byte>(im.Size);
				marker.SetZero();
 
				for (int i = 1; i < im.Rows-1; i++)
				{
						for (int j = 1; j < im.Cols-1; j++)
						{
							ushort p2 = (ushort)im[i - 1, j].Intensity;
							ushort p3 = (ushort)im[i - 1, j + 1].Intensity;
							ushort p4 = (ushort)im[i, j + 1].Intensity;
							ushort p5 = (ushort)im[i + 1, j + 1].Intensity;
							ushort p6 = (ushort)im[i + 1, j].Intensity;
							ushort p7 = (ushort)im[i + 1, j - 1].Intensity;
							ushort p8 = (ushort)im[i, j - 1].Intensity;
							ushort p9 = (ushort)im[i - 1, j - 1].Intensity;

							int A = ((p2 == 0 && p3 == 1) ? 1 : 0 )+ ((p3 == 0 && p4 == 1) ? 1 : 0) +
												 ((p4 == 0 && p5 == 1) ? 1 : 0) + ((p5 == 0 && p6 == 1) ? 1 : 0) +
												 ((p6 == 0 && p7 == 1) ? 1 : 0) + ((p7 == 0 && p8 == 1) ? 1 : 0) +
												 ((p8 == 0 && p9 == 1) ? 1 : 0) + ((p9 == 0 && p2 == 1) ? 1 : 0);
								int B  = p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9;
								int m1 = iter == 0 ? (p2 * p4 * p6) : (p2 * p4 * p8);
								int m2 = iter == 0 ? (p4 * p6 * p8) : (p2 * p6 * p8);
 
								if (A == 1 && (B >= 2 && B <= 6) && m1 == 0 && m2 == 0)
										marker[i,j] = new Gray(255);
						}
				}
				return im.And(marker.Not());
		}
		Image<Gray, Byte> thinning(Image<Gray, Byte> im)
			{

					im /= 255;
 
					Image<Gray, Byte> prev = new Image<Gray, Byte>(im.Size);
				  
					Image<Gray, Byte> diff;
 
					do {
							im = thinningIteration(im, 0);
							im = thinningIteration(im, 1);
						  diff = im.AbsDiff(prev);
						  im.CopyTo(prev);
					} 
					while (diff.CountNonzero()[0] > 0);
 				im *= 255;
				return im;
			}
		void Skeleton(Image<Gray, Byte> image)
		{
			Image<Gray, Byte> eroded = new Image<Gray, byte>(image.Size);
			Image<Gray, Byte> temp = new Image<Gray, byte>(image.Size);
			Image<Gray, Byte> skel = new Image<Gray, byte>(image.Size);
			CvInvoke.cvThreshold(image, image, 80, 255, 0);
			StructuringElementEx element = new StructuringElementEx(3, 3, 1, 1, Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_CROSS);
			bool done = false;

			while (!done)
			{
				CvInvoke.cvErode(image, eroded, element, 1);
				CvInvoke.cvDilate(eroded, temp, element, 1);
				temp = image - temp;
				skel = skel | temp;
				image = eroded;
				if (CvInvoke.cvCountNonZero(image) == 0) 
					done = true;
			}
		}
		private void DiffButton_Click(object sender, EventArgs e)
		{

			string offpath = @"D:\Dropbox\3D\Scanner\test data\off.bmp";
			string onpath = @"D:\Dropbox\3D\Scanner\test data\on.bmp";

			LockBitmap off = new LockBitmap((Bitmap)Bitmap.FromFile(offpath));
			Bitmap on = (Bitmap)Bitmap.FromFile(onpath);

			DateTime start = DateTime.Now;

			ImageProcessor ip = new ImageProcessor(15, 1, 60, ImageProcessor.eLaserDetectionMode.MassCenter);
            List<PointF> laserloc = ip.Process(off,on,null);

			DateTime end = DateTime.Now;
			double dt =  (end - start).TotalMilliseconds;
            Bitmap bmp = new Bitmap(on);
            using(Graphics g =Graphics.FromImage(bmp))
            {
                g.FillRectangle(Brushes.Black, new Rectangle(0, 0, off.Width, off.Height));
                g.DrawLines(Pens.Red, laserloc.ToArray());
            }
            DiffBox.Image = bmp;// BitmapExtention.SavePixels(laserloc, off.Width, off.Height);
		}
		private void DiffButton_Clickopencv(object sender, EventArgs e)
		{
			/*
			Image<Gray, Byte> off = new Image<Gray, Byte>(cameraSelctionControl1.CurrentImage());
			Bitmap snap = cameraSelctionControl1.Snaphot();
			SnapBox.Image = snap;
			Image<Gray, Byte> on = new Image<Gray, Byte>(snap);
			*/

			string offpath = @"D:\Fabio\Dropbox\3D\Scanner\test data\off.bmp";
			string onpath = @"D:\Fabio\Dropbox\3D\Scanner\test data\on.bmp";

			Image<Gray, Byte> off = new Image<Rgb, Byte>(offpath).Split()[0];
			Image<Gray, Byte> on = new Image<Rgb, Byte>(onpath).Split()[0];

			DateTime start = DateTime.Now;
			
			
			/*
			Image<Gray, Byte> off = new Image<Rgb, Byte>(offpath).Split()[0].SmoothMedian(3);
			Image<Gray, Byte> on = new Image<Rgb, Byte>(onpath).Split()[0].SmoothMedian(3);
			Image<Gray, Byte>[] diff = new Image<Rgb, Byte>(diffpath).Split();
			 * */
			Image<Gray, Byte> r = (on - off);

			int thresval = 15;
			r = r.ThresholdToZero(new Gray(thresval));
			StructuringElementEx stru = new StructuringElementEx(3,3, 1, 1, Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_ELLIPSE);
			r = r.MorphologyEx(stru, Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_OPEN, 2);


			r = r.ThresholdBinary(new Gray(thresval), new Gray(255));
			//# find contours of the binarized image

			Rectangle bbox = new Rectangle();
			using (MemStorage storage = new MemStorage())
			{
				for (Contour<Point> contours = r.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_EXTERNAL, storage); contours != null; contours = contours.HNext)
				{
					if (bbox.IsEmpty)
						bbox = contours.BoundingRectangle;
					else
					{
						bbox = Rectangle.Union(bbox, contours.BoundingRectangle);
					}
					//r.Draw(contours, new Gray(255),1);
				}
			}
			DateTime end = DateTime.Now;

			//r.ROI = bbox;
			Image<Rgb, Byte> res = r.Convert<Rgb, byte>();
			res.Draw(bbox, new Rgb(255,0,0), 1);

			/*
			LineSegment2D[][] lines = r.HoughLinesBinary(1, Math.PI / 2, 2, 1, 1);

			 
			for (int i = 0; i < lines.Length; i++)
			{
				for(int y=0;y<lines[i].Length;y++)
				{
					LineSegment2D seg = lines[i][y];
					Point[] l = new Point[2];
					l[0]=seg.P1;
					l[1]=seg.P2;
					res.DrawPolyline(l, false, new Rgb(Color.Green), 1);
				}
			}*/
			
			/*
			//Image<Gray, Byte> res = off.AbsDiff(on);
			Image<Gray, Byte> res = (on.Split()[0] - off.Split()[0]);//.AbsDiff(on);
			//Image<Gray, Byte> res = (on.AbsDiff(off)).Split()[0];
			//Image<Gray, Byte> res = (on.AbsDiff(off)).Split()[0];
			 */
			DiffBox.Image = res.ToBitmap();


			/*
 threshold the image using a threshold value 0
ret, bw = cv2.threshold(img, 0, 255, cv2.THRESH_BINARY)
# find contours of the binarized image
contours, heirarchy = cv2.findContours(bw, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_NONE)
# curves
curves = np.zeros((img.shape[0], img.shape[1], 3), np.uint8) 

for i in range(len(contours)):
    # for each contour, draw the filled contour
    draw = np.zeros((img.shape[0], img.shape[1]), np.uint8) 
    cv2.drawContours(draw, contours, i, (255,255,255), -1)
    # for each column, calculate the centroid
    for col in range(draw.shape[1]):
        M = cv2.moments(draw[:, col])
        if M['m00'] != 0:
            x = col
            y = int(M['m01']/M['m00'])
            curves[y, x, :] = (0, 0, 255)
			 */


			/*
			Image<Gray, Byte> img1 = new Image<Gray, Byte>(cameraSelctionControl1.CurrentImage());
			Bitmap snap = cameraSelctionControl1.Snaphot();
			SnapBox.Image = snap;
			Image<Gray, Byte> img2 = new Image<Gray, Byte>(snap);
			Image<Gray, Byte> img3 = img2.AbsDiff(img1);
			Image<Gray, Byte> img3 = img2.Xor(img1);
			//img3=img3.ThresholdTrunc(new Gray(128));
			Gray g = new Gray(128);
			img3 = img3.ThresholdTrunc(g);
			//Image<Gray, Byte> img3 = img2 - img1; //Here the difference is applied.
			DiffBox.Image = img3.ToBitmap();
			 * */
		}

	}
}
