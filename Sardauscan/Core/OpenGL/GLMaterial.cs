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
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using System.Xml;
using System.Globalization;
using System.Drawing;
using Sardauscan.Core;
using Sardauscan.Gui;

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
			/*
			ret.Ambiant = ColorExtension.ColorFromVector(new Vector4(0f, 0.25f, 0.5f, 0.0f));
			ret.Diffuse = ColorExtension.ColorFromVector(new Vector4(0f, 0.5f, 1f, 0.0f));
			*/
			ret.Ambiant = SkinInfo.View3DBackColor;// ColorExtension.ColorFromVector(new Vector4(0f, 0.25f, 0.5f, 0.0f));
			ret.Diffuse = ret.Ambiant;
			ret.Specular = ret.Ambiant.Darker();
			ret.Shininess = 1f;

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
