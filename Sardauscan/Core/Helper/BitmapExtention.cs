#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Sardauscan.Core;
using Sardauscan.Core.Geometry;

namespace Sardauscan.Core
{
	/// <summary>
	/// BitMap Extention
	/// </summary>
	public static class BitmapExtention
	{

		/// <summary>
		/// Write Pixel location as a PNG
		/// </summary>
		/// <param name="pixels"></param>
		/// <param name="imageWidth"></param>
		/// <param name="imageHeight"></param>
		/// <param name="pngFilename"></param>
		/// <returns></returns>
		public static Bitmap SavePixels(List<PointF> pixels,
										 int imageWidth,
										 int imageHeight,
										 string pngFilename = "")
		{

			Console.WriteLine("Writing " + pngFilename + "...");
			Bitmap bmp = new Bitmap(imageWidth, imageHeight, PixelFormat.Format24bppRgb);
			using (var graphics = Graphics.FromImage(bmp))
			{
				using (SolidBrush bg = new SolidBrush(Color.Black))
					graphics.FillRectangle(bg, new Rectangle(0, 0, bmp.Width, bmp.Height));
			}
			OverlayPixels(bmp, pixels);
			if (!String.IsNullOrEmpty(pngFilename))
				bmp.Save(pngFilename);
			return bmp;
		}

		/// <summary>
		/// Overlay Pixel Location to a bitmap
		/// </summary>
		/// <param name="image"></param>
		/// <param name="pixels"></param>
        public static void OverlayPixels(this Bitmap image, List<PointF> pixels)
		{
			LockBitmap lbmp = new LockBitmap(image);
			for (int iPx = 0; iPx < pixels.Count; iPx++)
			{
				int x = (int)(pixels[iPx].X);
				int y = (int)(pixels[iPx].Y);
				lbmp.SetPixel(x, y, Color.Red);
			}
			lbmp.UnlockBits();
		}
		/// <summary>
		/// Get All the pixels of a Bitmap
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		public static List<List<Color>> GetPixels(this Bitmap image)
		{
			LockBitmap lbmp = new LockBitmap(image);
			List<List<Color>> ret = new List<List<Color>>(lbmp.Height);
			for (int y = 0; y < lbmp.Height; y++)
			{
				List<Color> line = new List<Color>(lbmp.Width);
				for (int x = 0; x < lbmp.Width; x++)
					line.Add(lbmp.GetPixel(x, y));
				ret.Add(line);
			}
			lbmp.UnlockBits();
			return ret;
		}
		/// <summary>
		/// Set the row color of a Bitmap
		/// </summary>
		/// <param name="image"></param>
		/// <param name="y"></param>
		/// <param name="colors"></param>
		public static void SetRowColors(this Bitmap image, int y, Color[] colors)
		{
			LockBitmap lbmp = new LockBitmap(image);
			for (int x = 0; x < colors.Length; x++)
				lbmp.SetPixel(x, y, colors[x]);
			lbmp.UnlockBits();
		}

	}
}
