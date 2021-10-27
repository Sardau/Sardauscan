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
