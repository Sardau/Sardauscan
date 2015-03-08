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
