#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
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

namespace Sardauscan.Gui.Forms.CustomForm
{
    #region Enum ImageSizeMode
    public enum ImageSizeMode
    {
        Centered = 0,
        Stretched = 1,
        Tiled = 2
    }
    #endregion

    #region Enum: CaptionButtonState

    public enum CaptionButtonState
    {
        Normal,
        Pressed,
        Over
    }

    #endregion
    #region Enum CaptionButtonKey
    public enum CaptionButtonKey
    {
        Close,
        Restore,
        Maximize,
        Minimize,
        Help,
				About,
			  Settings
    }
    #endregion

    
    #region CaptionButton
    /// <summary>
    /// A button that can appear in the window caption bar
    /// </summary>
    public class CustomCaptionButton
    {
        public class CaptionButtonSkin
        {
            public CaptionButtonSkin(Image image)
            {
                Image = image;
            }
						public Image _Image;
						public Image Image { get { return _Image; } set { _Image = value; ClearImageCache(); } }


					
					
					  protected void ClearImageCache()
						{
						}

            public void NormalState(Graphics g, Rectangle bounds, Color bgColor)
            {
							g.DrawImage(Image, bounds);
						}
						public void ActiveState(Graphics g, Rectangle bounds, Color bgColor)
            {
                g.DrawImage(Image, bounds);
								ControlPaint.DrawBorder3D(g, bounds, Border3DStyle.SunkenInner, Border3DSide.All & ~Border3DSide.Middle);
						}
						public void HoverState(Graphics g, Rectangle bounds, Color bgColor)
            {
							ImageAttributes ia = new ImageAttributes();
							ia.SetColorMatrix(SkinInfo.CaptionHoverColorMatrix);
							g.DrawImage(Image, bounds, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, ia);
						}
						public void DisabledState(Graphics g, Rectangle bounds, Color bgColor)
            {
								using (Bitmap bmp = new Bitmap(Image,bounds.Size))
								{
									ControlPaint.DrawImageDisabled(g, bmp, bounds.X, bounds.Y, Color.Red);
								}
            }
            public Size Size { get { return Image.Size; } }
        }
        #region Variables

        private Rectangle _bounds;
        private CaptionButtonState _state;
        private CaptionButtonSkin _Skin;
        private CaptionButtonKey _key;
        private bool _visible = true;
        private int _hitTestCode = -1;
        private int _systemCommand = -1;
        private bool _enabled = true;

        #endregion

        public CaptionButtonSkin Skin { get { return _Skin; } set { _Skin = value; } }


        #region ToString

        public override string ToString()
        {
            return this.Key.ToString();
        }

        #endregion

        #region Properties

        public CaptionButtonState State
        {
            get { return _state; }
            set { _state = value; }
        }

        public Rectangle Bounds
        {
            get { return _bounds; }
            set
            {
                _bounds = value;
            }
        }


        public CaptionButtonKey Key
        {
            get { return _key; }
            set
            {
                _key = value;
            }
        }

        public int HitTestCode
        {
            get { return _hitTestCode; }
            set { _hitTestCode = value; }
        }

        public int SystemCommand
        {
            get { return _systemCommand; }
            set { _systemCommand = value; }
        }

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        #endregion


        #region DrawButton

        public virtual void DrawButton(Graphics g, bool paintBackground, Color bgColor)
        {
            if (!Visible || Skin == null)
                return;

            // paint buffered background image
						if (paintBackground)
						{
							using (SolidBrush bg = new SolidBrush(bgColor))
								g.FillRectangle(bg, Bounds);
						}

            if (this.Enabled)
            {
                switch (this.State)
                {
                    case CaptionButtonState.Normal:
										Skin.NormalState(g, Bounds, bgColor);
                        break;
                    case CaptionButtonState.Pressed:
												Skin.ActiveState(g, Bounds, bgColor);
                        break;
                    case CaptionButtonState.Over:
												Skin.HoverState(g, Bounds, bgColor);
                        break;
                }
            }
            else
            {
							Skin.DisabledState(g, Bounds, bgColor);
            }
        }

        #endregion
    }

    #endregion
}
