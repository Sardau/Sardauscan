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
