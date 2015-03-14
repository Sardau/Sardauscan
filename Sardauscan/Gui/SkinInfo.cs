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
using Sardauscan.Properties;
using Sardauscan.Gui.Forms.CustomForm;
using System.Windows.Forms;
using Sardauscan.Gui.Controls;
using System.Drawing.Imaging;
using Sardauscan.Gui.OpenGL;

namespace Sardauscan.Gui
{
    public class SkinInfo
    {
        public static Color ActiveTitleBackColor { get { return System.Drawing.Color.FromArgb((byte)85, (byte)170, (byte)255); } }
        public static Color ActiveTitleTextColor { get { return System.Drawing.Color.White; } }

        public static Color InactiveTitleBackColor { get { return System.Drawing.Color.FromArgb((byte)150, (byte)180, (byte)210); } }
        public static Color InactiveTitleTextColor { get { return System.Drawing.Color.LightGray; } }

        public static Color BackColor { get { return Color.White; } }
        public static Color ForeColor { get { return System.Drawing.Color.FromArgb((byte)26, (byte)55, (byte)85); } }
        public static Color BorderColor { get { return System.Drawing.Color.FromArgb((byte)150, (byte)160, (byte)200); } }

        public static ColorMatrix UnavailableColorMatrix
        {
            get
            {
                float r = 10f;
                float g = 0.4f;
                float b = g;
                float a = 0.8f;

                // Make the ColorMatrix.
                ColorMatrix cm = new ColorMatrix(new float[][]
					{
							new float[] {r,  g,  b,  0, 0},        
							new float[] {g,  g,  0,  0, 0},        
							new float[] {b,  0,  b,  0, 0},        
							new float[] {0,  0,  0,  a, 0},        
							new float[] {0, 0, 0, 0, 1}
					});
                return cm;
            }
        }
        public static ColorMatrix NotSelectedColorMatrix
        {
            get
            {
                float a = 0.60f;
                ColorMatrix cm = new ColorMatrix(new float[][]
					{
							new float[] {1,  0,  0,  0, 0},        
							new float[] {0,  1,  0,  0, 0},        
							new float[] {0,  0,  1,  0, 0},        
							new float[] {0,  0,  0,  a, 0},        
							new float[] {0, 0, 0, 0, 1}
					});
                return cm;
            }
        }
        public static ColorMatrix HoverColorMatrix
        {
            get
            {
                float brightness = 0.75f;
                //double contrast = 1.25f; // twice the contrast
                float contrast = 1 + 1 - brightness;

                float adjustedBrightness = brightness - 1.0f;
                // create matrix that will brighten and contrast the image
                ColorMatrix cm = new ColorMatrix(new float[][] 
				{
        new float[] {contrast, 0, 0, 0, 0}, // scale red
        new float[] {0, contrast, 0, 0, 0}, // scale green
        new float[] {0, 0, contrast, 0, 0}, // scale blue
        new float[] {0, 0, 0, 1.0f, 0}, // don't scale alpha
        new float[] {adjustedBrightness, adjustedBrightness, adjustedBrightness, 0, 1}}
                );
                return cm;
            }
        }
        public static ColorMatrix CaptionHoverColorMatrix
        {
            get
            {
                return HoverColorMatrix;
            }
        }


        public static Color View3DBackColor { get { return ActiveTitleBackColor; } }
        public static Color View3DForeColor { get { return ActiveTitleTextColor; } }

        public static void ApplyColor(Control c)
        {

            if (c is Scene3DControl)
                return;

            if (!(c is Panel))
            {
                c.ForeColor = SkinInfo.ForeColor;
                if (c.BackColor != Color.Transparent)
                    c.BackColor = SkinInfo.BackColor;
            }
            foreach (Control cc in c.Controls)
                ApplyColor(cc);
        }


        public int BorderWidth = System.Windows.Forms.SystemInformation.FrameBorderSize.Width;
        public int IconSize = System.Windows.Forms.SystemInformation.CaptionHeight + System.Windows.Forms.SystemInformation.FrameBorderSize.Width;

        public static Image GetImage(CaptionButtonKey key)
        {
            switch (key)
            {
                case CaptionButtonKey.Close: return Resources.Window_Close;
                case CaptionButtonKey.Restore: return Resources.Window_Maximize;
                case CaptionButtonKey.Maximize: return Resources.Window_Restore;
                case CaptionButtonKey.Minimize: return Resources.Window_Minimize;
                case CaptionButtonKey.Help: return Resources.Mark_Question;
                case CaptionButtonKey.Settings: return Resources.Gears;
                case CaptionButtonKey.About: return Resources.About;
                default: return Resources.Asterix;
            }
        }

    }
}
