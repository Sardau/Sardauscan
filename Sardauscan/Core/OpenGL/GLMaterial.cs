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
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using System.Xml;
using System.Globalization;
using System.Drawing;
using Sardauscan.Core;

namespace Sardauscan.Core.OpenGL
{
	/// <summary>
	/// Clas representing a OpenGl material
	/// </summary>
	public struct GLMaterial
	{
		/// <summary>
		/// Ambiant Color
		/// </summary>
		public Color Ambiant;
		/// <summary>
		/// Diffuse Color
		/// </summary>
		Color Diffuse;
		/// <summary>
		/// Specular Color
		/// </summary>
		Color Specular;
		/// <summary>
		/// Shininess
		/// </summary>
		double Shininess;
		/// <summary>
		/// specify this material to OpenGL
		/// </summary>
		/// <param name="lighting"></param>
		/// <param name="face"></param>
		public void ToGL(bool lighting, MaterialFace face = MaterialFace.Front)
		{
			if (lighting)
			{
				GL.Color4(Diffuse);
				GL.Material(MaterialFace.Back, MaterialParameter.Ambient, Color.Red.ToVector());
				GL.Material(MaterialFace.Back, MaterialParameter.Diffuse, Color.Red.ToVector());
				GL.Material(MaterialFace.Back, MaterialParameter.Specular, Color.Red.ToVector());
				GL.Material(face, MaterialParameter.Ambient, Ambiant.ToVector());
				GL.Material(face, MaterialParameter.Diffuse, Diffuse.ToVector());
				GL.Material(face, MaterialParameter.Specular, Specular.ToVector());
				GL.Material(face, MaterialParameter.Shininess, (float)(Shininess * 128f));
			}
			else
				GL.Color4(Ambiant);
		}
		/// <summary>
		/// Get the default MAterial
		/// </summary>
		/// <returns></returns>
		public static GLMaterial Default()
		{
			GLMaterial ret = new GLMaterial();
			ret.Ambiant = ColorExtension.ColorFromVector(new Vector4(0.5f, 0.5f, 0.5f, 0.0f));
			ret.Diffuse = ColorExtension.ColorFromVector(new Vector4(0.75f, 0.75f, 0.75f, 0.0f));
			ret.Specular = ColorExtension.ColorFromVector(new Vector4(1.0f, 1f, 1f, 0.0f));
			ret.Shininess = 0.9f;

			return ret;
		}
		/// <summary>
		/// Create from XmlNode
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public static GLMaterial CreateFrom(XmlNode node)
		{
			GLMaterial ret = new GLMaterial();
			ret.Ambiant = ColorFromString(node.SelectSingleNode("Ambient").InnerText);
			ret.Diffuse = ColorFromString(node.SelectSingleNode("Diffuse").InnerText);
			ret.Specular = ColorFromString(node.SelectSingleNode("Specular").InnerText);
			ret.Shininess = double.Parse(node.SelectSingleNode("Shininess").InnerText, NumberStyles.Any, CultureInfo.InvariantCulture);
			return ret;
		}

		private static Color ColorFromString(string val)
		{
			string[] part = val.Split("|".ToCharArray());
			float r = 0f;
            float g = 0f;
            float b = 0f;
            float a = 0f;
            if (float.TryParse(part[0], NumberStyles.Any, CultureInfo.InvariantCulture, out r))
                if (float.TryParse(part[1], NumberStyles.Any, CultureInfo.InvariantCulture, out g))
                    if (float.TryParse(part[2], NumberStyles.Any, CultureInfo.InvariantCulture, out b))
					{
						if (part.Length == 4)
                            float.TryParse(part[3], NumberStyles.Any, CultureInfo.InvariantCulture, out a);

					}
			return ColorExtension.ColorFromVector(new Vector4(r, g, b, a));
		}

		public static Dictionary<string, GLMaterial> CreateMaterialDictionnary()
		{
			Dictionary<string, GLMaterial> ret = new Dictionary<string, GLMaterial>();
			ret.Add("", Default());

			XmlDocument doc = new XmlDocument();
			doc.LoadXml(global::Sardauscan.Properties.Resources.OPENGL);
			XmlNodeList list = doc.SelectNodes("");
			foreach (XmlNode n in list)
			{
				string name = n.Attributes["Name"].Value;
				GLMaterial mat = CreateFrom(n);
				ret.Add(name, mat);
			}
			return ret;
		}
	}
}
