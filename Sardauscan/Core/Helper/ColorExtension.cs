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
using OpenTK;

namespace Sardauscan.Core
{
	/// <summary>
	/// Color Extention
	/// </summary>
	public static class ColorExtension
	{
		/// <summary>
		/// Convert to a in RGBA
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static int ToGLRgba32(this Color c)
		{
			return (int)((c.A << 24) | (c.B << 16) | (c.G << 8) | c.R);
		}

		/// <summary>
		/// Convert to a Vector4d
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static Vector4 ToVector(this Color c)
		{
			return new Vector4(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
		}
		/// <summary>
		/// Create From a Vector4d
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static Color ColorFromVector(Vector4 v)
		{
			int r = (int)Math.Max(0, Math.Min(255, v[0] * 255f));
			int g = (int)Math.Max(0, Math.Min(255, v[1] * 255f));
			int b = (int)Math.Max(0, Math.Min(255, v[2] * 255f));
			int a = (int)Math.Max(0, Math.Min(255, v[3] * 255f));
			return Color.FromArgb(a, r, g, b);
		}

		private static double m_LightningLevel = 0.4;

		/// <summary>
		/// Get the lighter Color of a specified color
		/// </summary>
		/// <param name="color"></param>
		/// <returns>a brighter color</returns>
		public static Color Lighter(this Color color)
		{
			return color.ModifyLuminosity(m_LightningLevel);
		}
		/// <summary>
		/// Get the lighter Color of a specified coloe
		/// </summary>
		/// <param name="color"></param>
		/// <returns>a brighter color</returns>
		public static Color HeavyLighter(this Color color)
		{
			return color.ModifyLuminosity(2.0 * m_LightningLevel);
		}
		/// <summary>
		/// Get the lighter Color of a specified coloe
		/// </summary>
		/// <param name="color"></param>
		/// <returns>a brighter color</returns>
		public static Color MediumLighter(this Color color)
		{
			return color.ModifyLuminosity(1.5 * m_LightningLevel);
		}
		/// <summary>
		/// Get the Darker Color of a specified color
		/// </summary>
		/// <param name="color"></param>
		/// <returns>a Darker Color</returns>
		public static Color Darker(this Color color)
		{
			return color.ModifyLuminosity(-m_LightningLevel);
		}
		/// <summary>
		/// Get the Darker Color of a specified color
		/// </summary>
		/// <param name="color"></param>
		/// <returns>a Darker Color</returns>
		public static Color HeavyDarker(this Color color)
		{
			return color.ModifyLuminosity(-2.0 * m_LightningLevel);
		}
		/// <summary>
		/// Get the Darker Color of a specified color
		/// </summary>
		/// <param name="color"></param>
		/// <returns>a Darker Color</returns>
		public static Color MediumDarker(this Color color)
		{
			return color.ModifyLuminosity(-1.5 * m_LightningLevel);
		}
		/// <summary> 
		/// Sets the absolute brightness of a colour 
		/// </summary> 
		/// <param name="c">Original colour</param> 
		/// <param name="brightness">The luminance level to impose</param> 
		/// <returns>an adjusted colour</returns> 
		public static Color SetBrightness(this Color c, double brightness)
		{
			ColorHSB hsb = ColorHSB.FromRGB(c);
			hsb.Brightness = brightness;
			return hsb.ToColor();
		}
		/// <summary> 
		/// Modifies an existing brightness level 
		/// </summary> 
		/// <param name="c">The original colour</param> 
		/// <param name="brightness">The luminance delta</param> 
		/// <returns>An adjusted colour</returns> 
		public static Color ModifyBrightness(this Color c, double brightness)
		{
			ColorHSB hsb = ColorHSB.FromRGB(c);
			hsb.Brightness *= (1 + brightness);
			return hsb.ToColor();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="c"></param>
		/// <param name="factor"></param>
		/// <returns></returns>
		public static Color ModifyLuminosity(this Color c, double factor)
		{
			return c.ModifyBrightness(factor);
		}

		/// <summary> 
		/// Sets the absolute saturation level 
		/// </summary> 
		/// <remarks>Accepted values 0-1</remarks> 
		/// <param name="c">An original colour</param> 
		/// <param name="Saturation">The saturation value to impose</param> 
		/// <returns>An adjusted colour</returns> 
		public static Color SetSaturation(this Color c, double Saturation)
		{
			ColorHSB hsb = ColorHSB.FromRGB(c);
			hsb.Saturation = Saturation;
			return hsb.ToColor();
		}


		/// <summary> 
		/// Modifies an existing Saturation level 
		/// </summary> 
		/// <remarks> 
		/// To reduce Saturation use a number smaller than 1. To increase Saturation use a number larger tnan 1 
		/// </remarks> 
		/// <param name="c">The original colour</param> 
		/// <param name="Saturation">The saturation delta</param> 
		/// <returns>An adjusted colour</returns> 
		public static Color ModifySaturation(this Color c, double Saturation)
		{
			ColorHSB hsb = ColorHSB.FromRGB(c);
			hsb.Saturation *= Saturation;
			return hsb.ToColor();
		}

		/// <summary> 
		/// Sets the absolute Hue level 
		/// </summary> 
		/// <remarks>Accepted values 0-1</remarks> 
		/// <param name="c">An original colour</param> 
		/// <param name="Hue">The Hue value to impose</param> 
		/// <returns>An adjusted colour</returns> 
		public static Color SetHue(this Color c, double Hue)
		{
			ColorHSB hsb = ColorHSB.FromRGB(c);
			hsb.Hue = Hue;
			return hsb.ToColor();
		}


		/// <summary> 
		/// Modifies an existing Hue level 
		/// </summary> 
		/// <remarks> 
		/// To reduce Hue use a number smaller than 1. To increase Hue use a number larger tnan 1 
		/// </remarks> 
		/// <param name="c">The original colour</param> 
		/// <param name="Hue">The Hue delta</param> 
		/// <returns>An adjusted colour</returns> 
		public static Color ModifyHue(this Color c, double Hue)
		{
			ColorHSB hsb = ColorHSB.FromRGB(c);
			hsb.Hue *= Hue;
			return hsb.ToColor();
		}

		/// <summary>
		/// invert the color
		/// </summary>
		/// <param name="c">the color</param>
		/// <returns></returns>
		public static Color Invert(this Color c)
		{
			return Color.FromArgb(c.A, 255 - c.R, 255 - c.G, 255 - c.B);
		}






		/// <summary>
		/// Get a color between 2 color (ie gradiant step)
		/// </summary>
		/// <param name="s">Start Color </param>
		/// <param name="e">End Color</param>
		/// <param name="clamp">Interpolation factor : clamp [0,1]</param>
		/// <returns>interpolated color</returns>
		public static Color GetStepColor(this Color s, Color e, double clamp)
		{
			return s.GetStepColor(e, clamp, false);
		}


		/// <summary>
		/// Get a color between 2 color (ie gradiant step)
		/// </summary>
		/// <param name="s">Start Color </param>
		/// <param name="e">End Color</param>
		/// <param name="clamp">Intyerpolation factor : clamp [0,1]</param>
		/// <param name="useHSB">Use USB convertion to compute interpolation</param>
		/// <returns></returns>
		public static Color GetStepColor(this Color s, Color e, double clamp, bool useHSB)
		{
			double t = Math.Min(Math.Max(clamp, 0), 1);
			if (useHSB)
			{
				ColorHSB sHSB = ColorHSB.FromRGB(s);
				ColorHSB eHSB = ColorHSB.FromRGB(e);
				double dH = sHSB.Hue - eHSB.Hue;
				double dS = sHSB.Saturation - eHSB.Saturation;
				double dB = sHSB.Brightness - eHSB.Brightness;
				ColorHSB mHSB = new ColorHSB();
				mHSB.Hue = sHSB.Hue - t * dH;
				mHSB.Saturation = sHSB.Saturation - t * dS;
				mHSB.Brightness = sHSB.Brightness - t * dB;
				Color ret = mHSB.ToColor();
				return ret;
			}
			else
			{
				double dR = s.R - e.R;
				double dG = s.G - e.G;
				double dB = s.B - e.B;
				double r = s.R - t * dR;
				double g = s.G - t * dG;
				double b = s.B - t * dB;
				Color ret = Color.FromArgb((int)r, (int)g, (int)b);
				return ret;

			}
		}
	}

	/// <summary>
	/// class to represent color as HSB
	/// </summary>
	public class ColorHSB
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public ColorHSB()
		{
			m_Hue = 0;
			m_Saturation = 0;
			m_Brightness = 0;
		}

		/// <summary>
		/// Hue
		/// </summary>
		private double m_Hue;
		/// <summary>
		/// Saturation
		/// </summary>
		private double m_Saturation;
		/// <summary>
		/// Brightness
		/// </summary>
		private double m_Brightness;
		/// <summary>
		/// Hue property
		/// </summary>
		public double Hue
		{
			get { return m_Hue; }
			set
			{
				m_Hue = value;
				m_Hue = m_Hue > 1 ? 1 : m_Hue < 0 ? 0 : m_Hue;
			}
		}
		/// <summary>
		/// Saturation property
		/// </summary>
		public double Saturation
		{
			get { return m_Saturation; }
			set
			{
				m_Saturation = value;
				m_Saturation = m_Saturation > 1 ? 1 : m_Saturation < 0 ? 0 : m_Saturation;
			}
		}
		/// <summary>
		/// Brightness property
		/// </summary>
		public double Brightness
		{
			get { return m_Brightness; }
			set
			{
				m_Brightness = value;
				m_Brightness = m_Brightness > 1 ? 1 : m_Brightness < 0 ? 0 : m_Brightness;
			}
		}

		/// <summary> 
		/// Converts a colour from ColorHSB to RGB 
		/// </summary> 
		/// <remarks>Adapted from the algoritm in Foley and Van-Dam</remarks> 
		/// <param name="hsb">The ColorHSB value</param> 
		/// <returns>A Color structure containing the equivalent RGB values</returns> 
		public Color ToColor()
		{
			double r = 0, g = 0, b = 0;
			double temp1, temp2;

			if (Brightness == 0)
			{
				r = g = b = 0;
			}
			else
			{
				if (Saturation == 0)
				{
					r = g = b = Brightness;
				}
				else
				{
					temp2 = ((Brightness <= 0.5) ? Brightness * (1.0 + Saturation) : Brightness + Saturation - (Brightness * Saturation));
					temp1 = 2.0 * Brightness - temp2;

					double[] t3 = new double[] { Hue + 1.0 / 3.0, Hue, Hue - 1.0 / 3.0 };
					double[] clr = new double[] { 0, 0, 0 };
					for (int i = 0; i < 3; i++)
					{
						if (t3[i] < 0)
							t3[i] += 1.0;
						if (t3[i] > 1)
							t3[i] -= 1.0;

						if (6.0 * t3[i] < 1.0)
							clr[i] = temp1 + (temp2 - temp1) * t3[i] * 6.0;
						else if (2.0 * t3[i] < 1.0)
							clr[i] = temp2;
						else if (3.0 * t3[i] < 2.0)
							clr[i] = (temp1 + (temp2 - temp1) * ((2.0 / 3.0) - t3[i]) * 6.0);
						else
							clr[i] = temp1;
					}
					r = clr[0];
					g = clr[1];
					b = clr[2];
				}
			}
			return Color.FromArgb((int)(255 * r), (int)(255 * g), (int)(255 * b));
		}

		/// <summary> 
		/// Converts RGB to ColorHSB 
		/// </summary> 
		/// <remarks>Takes advantage of whats already built in to .NET by using the Color.GetHue, Color.GetSaturation and Color.GetBrightness methods</remarks> 
		/// <param name="c">A Color to convert</param> 
		/// <returns>An ColorHSB value</returns> 
		internal static ColorHSB FromRGB(Color c)
		{
			ColorHSB hsb = new ColorHSB();

			hsb.Hue = c.GetHue() / 360.0; // we store hue as 0-1 as opposed to 0-360 
			hsb.Brightness = c.GetBrightness();
			hsb.Saturation = c.GetSaturation();

			return hsb;
		}

	}



	/// <summary>
	/// class to represent color as HSV
	/// </summary>
	public class ColorHSV
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public ColorHSV()
		{
			m_Hue = 0;
			m_Saturation = 0;
			m_Value = 0;
		}
		internal ColorHSV(double h, double s, double v)
		{
			Hue = h;
			Saturation = s;
			Value = v;
		}

		/// <summary>
		/// Hue
		/// </summary>
		private double m_Hue;
		/// <summary>
		/// Saturation
		/// </summary>
		private double m_Saturation;
		/// <summary>
		/// Value
		/// </summary>
		private double m_Value;
		/// <summary>
		/// Hue property
		/// </summary>
		public double Hue
		{
			get { return m_Hue; }
			set
			{
				m_Hue = value;
				if (m_Hue < 0)
					m_Hue = 0;
				if (m_Hue > 6)
					m_Hue = 6;
			}
		}
		/// <summary>
		/// Saturation property
		/// </summary>
		public double Saturation
		{
			get { return m_Saturation; }
			set
			{
				m_Saturation = value;
				m_Saturation = m_Saturation > 1 ? 1 : m_Saturation < 0 ? 0 : m_Saturation;
			}
		}
		/// <summary>
		/// Value property
		/// </summary>
		public double Value
		{
			get { return m_Value; }
			set
			{
				m_Value = value;
				m_Value = m_Value > 1 ? 1 : m_Value < 0 ? 0 : m_Value;
			}
		}

		private static double ClampRGB(int c)
		{
			return c / 255.0;
		}
		private static int UnClampRGB(double c)
		{
			return (int)(c * 255);
		}
		public static ColorHSV FromColor(Color col)
		{
			// RGB are each on [0, 1]. S and V are returned on [0, 1] and H is  
			// returned on [0, 6]. Exception: H is returned UNDEFINED if S==0.  
			double R = ClampRGB(col.R);
			double G = ClampRGB(col.G);
			double B = ClampRGB(col.B);
			double v, x, f;
			int i;
			x = Math.Min(R, Math.Min(G, B));
			v = Math.Max(R, Math.Max(G, B));
			if (v == x)
			{
				return new ColorHSV(-1, 0, v);
			}
			f = (R == x) ? G - B : ((G == x) ? B - R : R - G);
			i = (R == x) ? 3 : ((G == x) ? 5 : 1);
			return new ColorHSV(i - f / (v - x), (v - x) / v, v);
		}
		public Color ToColor()
		{
			// H is given on [0, 6] or UNDEFINED. S and V are given on [0, 1].  
			// RGB are each returned on [0, 1].  
			double h = Hue;
			double s = Saturation;
			double v = Value;
			int i;


			if (h == -1)
				return Color.FromArgb(UnClampRGB(v), UnClampRGB(v), UnClampRGB(v));
			i = (int)Math.Floor(h);
			double f = h - i;
			if ((i % 2) == 1)
				f = 1 - f; // if i is even  
			double m = v * (1 - s);
			double n = v * (1 - s * f);
			switch (i)
			{
				case 1:
					return Color.FromArgb(UnClampRGB(n), UnClampRGB(v), UnClampRGB(m));
				case 2:
					return Color.FromArgb(UnClampRGB(m), UnClampRGB(v), UnClampRGB(n));
				case 3:
					return Color.FromArgb(UnClampRGB(m), UnClampRGB(n), UnClampRGB(v));
				case 4:
					return Color.FromArgb(UnClampRGB(n), UnClampRGB(m), UnClampRGB(v));
				case 5:
					return Color.FromArgb(UnClampRGB(v), UnClampRGB(m), UnClampRGB(n));
				default: // case 6:   case 0: 
					return Color.FromArgb(UnClampRGB(v), UnClampRGB(n), UnClampRGB(m));
			}
		}
	}

}
