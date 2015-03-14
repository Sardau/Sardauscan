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
#region BASED ON Custom Border Forms - Copyright (C) 2005 Szymon Kobalczyk

// Custom Border Forms
// Copyright (C) 2005 Szymon Kobalczyk
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.

// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//
// Szymon Kobalczyk (http://www.geekswithblogs.com/kobush)

#endregion
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Sardauscan.Gui.Controls;

namespace Sardauscan.Gui.Forms.CustomForm
{
	#region DRAWUTILS
	/// <summary>
	/// Helper class for basic drawing functions.
	/// </summary>
	public sealed class CustomDrawUtil
	{
		#region Constructor

		private CustomDrawUtil() { }

		#endregion

		#region DrawImageUnscaled

		public static void DrawImageUnscaled(Graphics g, Image image, int x, int y)
		{
			g.DrawImage(image, new Rectangle(x, y, image.Width, image.Height),
					new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
		}

		#endregion

		#region DrawImageTiled

		public static void DrawImageTiled(Graphics g, Image image, Rectangle destRect)
		{
			using (ImageAttributes attr = new ImageAttributes())
			{
				// initialize wrap mode to tile
				attr.SetWrapMode(System.Drawing.Drawing2D.WrapMode.Tile);

				// create the texture brush 
				using (TextureBrush b = new TextureBrush(image, new Rectangle(0, 0, image.Width, image.Height), attr))
				{
					// adjust the origin of the brush to coincide with the destination rect 
					b.TranslateTransform(destRect.Left, destRect.Top);

					// fill the area using the texture 
					g.FillRectangle(b, destRect);
				}
			}
		}

		#endregion

		#region DrawImage

		public static void DrawImage(Graphics g, Image image, Rectangle destRect, ImageSizeMode sizeMode, Padding margins)
		{
			switch (sizeMode)
			{
				case ImageSizeMode.Centered:
					DrawImageUnscaled(g, image, destRect.X, destRect.Y);
					break;
				case ImageSizeMode.Stretched:
					if (margins == Padding.Empty)
						g.DrawImage(image, destRect);
					else
						DrawImage(g, image, destRect, null, margins);
					break;
				case ImageSizeMode.Tiled:
					DrawImageTiled(g, image, destRect);
					break;
			}
		}

		#endregion

		#region DrawImage

		public static void DrawImage(Graphics g, Image image, Rectangle destRect, ImageAttributes imgAttributes, Padding margins)
		{
			if (null == image)
				return;

			// setup
			System.Drawing.Size imgSize = image.Size;
			Rectangle srcRect = new Rectangle(Point.Empty, imgSize);

			DrawImage(g, image, srcRect, destRect, imgAttributes, margins);
		}

		public static void DrawImage(Graphics g, Image image, Rectangle srcRect, Rectangle destRect, ImageAttributes imgAttributes, Padding margins)
		{
			if (null == image)
				return;

			int left = margins.Left;
			int top = margins.Top;
			int right = margins.Right;
			int bottom = margins.Bottom;

			// constants
			const int TopLeft = 0;
			const int TopCenter = 1;
			const int TopRight = 2;
			const int MiddleLeft = 3;
			const int MiddleCenter = 4;
			const int MiddleRight = 5;
			const int BottomLeft = 6;
			const int BottomCenter = 7;
			const int BottomRight = 8;

			// make sure the splits do not exceed the image being split
			int totalMarginWidth = left + right;
			if (totalMarginWidth >= srcRect.Width)
			{
				double hRatio = (double)left / (double)totalMarginWidth;
				left = (int)((double)srcRect.Width * hRatio);
				right = (srcRect.Width - left) - 1;
			}

			// make sure the splits do not exceed the image being split
			int totalMarginHeight = top + bottom;
			if (totalMarginHeight >= srcRect.Height)
			{
				double vRatio = (double)top / (double)totalMarginHeight;
				top = (int)((double)srcRect.Height * vRatio);
				bottom = (srcRect.Height - top) - 1;
			}

			// make sure the splits do not exceed the destination area
			if (left + right > destRect.Width)
			{
				left = destRect.Width / 2;
				right = destRect.Width - left;
			}

			// make sure the splits do not exceed the destination area
			if (top + bottom > destRect.Height)
			{
				top = destRect.Height / 2;
				bottom = destRect.Height - top;
			}

			// get the size of the center area
			int srcCenterWidth = srcRect.Width - (left + right);
			int srcCenterHeight = srcRect.Height - (top + bottom);

			// get the size of the center area for the destination
			int destCenterWidth = destRect.Width - (left + right);
			int destCenterHeight = destRect.Height - (top + bottom);

			Rectangle src = Rectangle.Empty;
			Rectangle dest = Rectangle.Empty;

			for (int i = 0; i < 9; i++)
			{
				#region Prepare Rects
				switch (i)
				{
					default:
					case TopLeft:
						src = new Rectangle(srcRect.Left, srcRect.Top, left, top);
						dest = new Rectangle(destRect.X, destRect.Y, left, top);
						break;
					case TopCenter:
						src = new Rectangle(srcRect.Left + left, srcRect.Top, srcCenterWidth, top);
						dest = new Rectangle(dest.Right, destRect.Y, destCenterWidth, top);
						break;
					case TopRight:
						src = new Rectangle(src.Right, srcRect.Top, right, top);
						dest = new Rectangle(dest.Right, destRect.Y, right, top);
						break;
					case MiddleLeft:
						src = new Rectangle(srcRect.Left, srcRect.Top + top, left, srcCenterHeight);
						dest = new Rectangle(destRect.X, destRect.Y + top, left, destCenterHeight);
						break;
					case MiddleCenter:
						src = new Rectangle(srcRect.Left + left, srcRect.Top + top, srcCenterWidth, srcCenterHeight);
						dest = new Rectangle(dest.Right, destRect.Y + top, destCenterWidth, destCenterHeight);
						break;
					case MiddleRight:
						src = new Rectangle(src.Right, srcRect.Top + top, right, srcCenterHeight);
						dest = new Rectangle(dest.Right, destRect.Y + top, right, destCenterHeight);
						break;
					case BottomLeft:
						src = new Rectangle(srcRect.Left, src.Bottom, left, bottom);
						dest = new Rectangle(destRect.X, dest.Bottom, left, bottom);
						break;
					case BottomCenter:
						src = new Rectangle(srcRect.Left + left, src.Top, srcCenterWidth, bottom);
						dest = new Rectangle(dest.Right, dest.Top, destCenterWidth, bottom);
						break;
					case BottomRight:
						src = new Rectangle(src.Right, src.Top, right, bottom);
						dest = new Rectangle(dest.Right, dest.Top, right, bottom);
						break;
				}
				#endregion //Prepare Rects

				if (src.Width > 0 && src.Height > 0 && dest.Width > 0 && dest.Height > 0)
				{
					g.DrawImage(image, dest,
							src.X, src.Y, src.Width, src.Height,
							GraphicsUnit.Pixel, imgAttributes);
				}
			}
		}

		#endregion

		#region IconToBitmap

		public static Bitmap IconToBitmap(Icon icon, Size size)
		{
			Bitmap bmp = new Bitmap(size.Width, size.Height);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.Clear(Color.Transparent);
				g.DrawIcon(icon, new Rectangle(0, 0, size.Width, size.Height));
			}
			return bmp;
		}

		#endregion

		#region ExcludePadding

		public static Rectangle ExcludePadding(Rectangle rectangle, Padding padding)
		{
			if (padding == Padding.Empty)
				return rectangle;

			return new Rectangle(rectangle.X + padding.Left,
					rectangle.Y + padding.Top,
					rectangle.Width - padding.Horizontal,
					rectangle.Height - padding.Vertical);
		}

		#endregion

		#region ExcludePadding

		public static Padding SubstractPadding(Padding inner, Padding outer)
		{
			return new Padding(
					Math.Max(0, inner.Left - outer.Left),
					Math.Max(0, inner.Top - outer.Top),
					Math.Max(0, inner.Right - outer.Right),
					Math.Max(0, inner.Bottom - outer.Bottom));
		}

		#endregion

		#region CopyFromGraphics

		public static void CopyFromGraphics(Graphics sourceGraphics, Bitmap destBitmap, Point upperLeftSource, Point upperLeftDestination, Size regionSize)
		{
			using (Graphics bmpGfx = Graphics.FromImage(destBitmap))
			{
				CopyFromGraphics(sourceGraphics, bmpGfx, upperLeftSource, upperLeftDestination, regionSize);
			}
		}

		/// <summary>
		/// Uses BitBlt to copy block from source to destination graphics.
		/// </summary>
		public static void CopyFromGraphics(Graphics sourceGraphics, Graphics destGraphics, Point upperLeftSource, Point upperLeftDestination, Size regionSize)
		{
			IntPtr src_hDC = (IntPtr)0;
			IntPtr dest_hDC = (IntPtr)0;
			try
			{
				src_hDC = sourceGraphics.GetHdc();
				dest_hDC = destGraphics.GetHdc();
				NativeMethods.BitBlt(dest_hDC, upperLeftDestination.X, upperLeftDestination.Y,
							regionSize.Width, regionSize.Height, src_hDC, upperLeftSource.X, upperLeftSource.Y,
							NativeMethods.TernaryRasterOperations.SRCCOPY);
			}
			finally
			{
				if ((int)src_hDC != 0)
					destGraphics.ReleaseHdc(dest_hDC);
				if ((int)dest_hDC != 0)
					sourceGraphics.ReleaseHdc(src_hDC);
			}
		}

		#endregion
	}
	#endregion
}
