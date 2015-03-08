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
using System.Drawing.Imaging;

namespace Sardauscan.Gui.Controls
{
	/// <summary>
	/// Class for a Status Image button : On Off different of Enable/Disale
	/// </summary>
	public partial class StatusImageButton : ImageButton
	{
		/// <summary>
		/// Default Ctor
		/// </summary>
		public StatusImageButton()
		{
		}
		private eOffButtonType _offImageType = eOffButtonType.Unavailable;
		[Category("Appearance")]
		[Browsable(true)]
		public eOffButtonType OffImageType { get { return _offImageType; } set { _offImageType = value; CreateOff(); } }
		private bool _OnState = true;
		public bool On
		{
			get { return _OnState; }
			set {
				bool last = _OnState;
				_OnState = value; 
				if(last!=value)
					Invalidate(); 
			}
		}
		private Image OffImage;
		public override Image Image
		{
			get { return On?base.Image:OffImage; }
			set
			{
				base.Image = value;
				CreateOff();
			}
		}

		void CreateOff()
		{
			Image img = base.Image;

			Bitmap bmp = new Bitmap(img.Width, img.Height);
			using (Graphics gaphics = Graphics.FromImage(bmp))
			{
				ImageAttributes ia = new ImageAttributes();

				if (OffImageType == eOffButtonType.Unavailable)
					ia.SetColorMatrix(SkinInfo.UnavailableColorMatrix);
				else
					ia.SetColorMatrix(SkinInfo.NotSelectedColorMatrix);
				gaphics.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);

			}
			OffImage=bmp;
			Invalidate();
		}

	}
	public enum eOffButtonType
	{
		Unavailable,
		NotSelected,
	}
}
