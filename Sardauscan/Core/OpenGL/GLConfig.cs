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
using System.Xml;

namespace Sardauscan.Core.OpenGL
{
	/// <summary>
	/// OpenGL Confguration
	/// </summary>
	public class GLConfig
	{

		static Dictionary<string, GLMaterial> m_Materials = null;

		/// <summary>
		/// Dictionnary of predefined Materials
		/// </summary>
		protected static Dictionary<string, GLMaterial> Materials
		{
			get
			{
				if (m_Materials == null)
				{
					m_Materials = new Dictionary<string, GLMaterial>();
					m_Materials.Add("Default", GLMaterial.Default());

					XmlDocument doc = new XmlDocument();
					doc.LoadXml(global::Sardauscan.Properties.Resources.OPENGL);
					XmlNodeList list = doc.SelectNodes("/OpenGL/Materials/Material");
					foreach (XmlNode n in list)
					{
						string name = n.Attributes["Name"].Value;
						GLMaterial mat = GLMaterial.CreateFrom(n);
						if (m_Materials.ContainsKey(name))
							m_Materials[name] = mat;
						else
							m_Materials.Add(name, mat);
					}

				}
				return m_Materials;
			}
		}
		/// <summary>
		/// Get a OpenGL materail from it's name
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static GLMaterial Material(string key)
		{
			if (Materials.ContainsKey(key))
				return Materials[key];
			return GLMaterial.Default();
		}
		/// <summary>
		/// Get the list of All OpenGl materials names
		/// </summary>
		/// <returns></returns>
		public static List<String> MaterialNames()
		{
			return new List<string>(Materials.Keys);
		}
	}
}
