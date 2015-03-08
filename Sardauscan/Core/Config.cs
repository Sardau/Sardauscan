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
using System.Text;
using System.Xml;
using System.Drawing;
using Sardauscan.Core.IO;

namespace Sardauscan.Core
{
	/// <summary>
	/// Represents a Configuration Node in the XML file
	/// http://www.codeproject.com/Articles/16953/XML-configuration-files-made-simple-at-last
	/// </summary>
	public class Config
	{
		/// <summary>
		/// The node from the XMLDocument, which it describes
		/// </summary>
		private XmlNode node;

		/// <summary>
		/// This class cannot be constructed directly. You will need to give a node to describe
		/// </summary>
		private Config()
		{
			throw new Exception("Cannot be created directly. Need a node parameter");
		}

		/// <summary>
		/// Creates an instance of the class
		/// </summary>
		/// <param name="node">
		/// the XmlNode to describe
		/// </param>
		public Config(XmlNode node)
		{
			if (node == null)
				throw new Exception("Node parameter can NOT be null!");
			this.node = node;
		}

		/// <summary>
		/// The Name of the element it describes
		/// </summary>
		/// <remarks>Read only property</remarks>        
		public string Name
		{
			get
			{
				return node.Name;
			}
		}

		/// <summary>
		/// Number of children nodes
		/// </summary>
		/// <remarks>Read only property</remarks>
		public int ChildCount
		{
			get
			{
				XmlNodeList Children = node.ChildNodes;
				if (Children != null)
					return Children.Count;
				else
					return 0;
			}
		}

		/// <summary>
		/// A string array with the names of the child nodes
		/// </summary>
		/// <remarks>Read only property</remarks>
		public string[] ChildrenNames
		{
			get
			{
				if (ChildCount == 0)
					return null;
				string[] ss = new string[ChildCount];
				for (int i = 0; i < ChildCount; i++)
				{
					ss[i] = node.ChildNodes[i].Name;
				}
				return ss;
			}
		}

		/// <summary>
		/// A ConfigSetting array describin each child node
		/// </summary>
		/// <remarks>Read only property</remarks>
		public Config[] Children
		{
			get
			{
				if (ChildCount == 0)
					return null;
				Config[] css = new Config[ChildCount];
				for (int i = 0; i < ChildCount; i++)
				{
					css[i] = new Config(node.ChildNodes[i]);
				}
				return css;
			}
		}


		public Type ValueType
		{
			get
			{
				XmlNode xmlattrib = node.Attributes.GetNamedItem("type");
				if (xmlattrib != null) return Type.GetType(xmlattrib.Value); else return typeof(String);
			}
			set
			{
				XmlNode xmlattrib = node.Attributes.GetNamedItem("type");
				if (xmlattrib == null)
					xmlattrib = node.Attributes.Append(node.OwnerDocument.CreateAttribute("type"));
				xmlattrib.Value = value.ToString();
			}
		}

		/// <summary>
		/// String value of the specific Configuration Node
		/// </summary>
		public string Value
		{
			get
			{
				XmlNode xmlattrib = node.Attributes.GetNamedItem("value");
				if (xmlattrib != null) return xmlattrib.Value; else return "";
			}

			set
			{
				XmlNode xmlattrib = node.Attributes.GetNamedItem("value");
				if (value != "")
				{
					if (xmlattrib == null) xmlattrib = node.Attributes.Append(node.OwnerDocument.CreateAttribute("value"));
					xmlattrib.Value = value;
				}
				else if (xmlattrib != null) node.Attributes.RemoveNamedItem("value");
				ValueType = value.GetType();
			}
		}

		public Color ColorValue
		{
			get { return XmlConfig.ColorFromString(Value); }
			set
			{
				Value = XmlConfig.ColorToString(value);
				ValueType = value.GetType();
			}
		}

		/// <summary>
		/// int value of the specific Configuration Node
		/// </summary>
		public int IntValue
		{
			get { int i; int.TryParse(Value, out i); return i; }
			set
			{
				Value = value.ToString();
				ValueType = value.GetType();
			}

		}
		/// <summary>
		/// bool value of the specific Configuration Node
		/// </summary>
		public bool BoolValue
		{
			get { bool b; bool.TryParse(Value, out b); return b; }
			set
			{
				Value = value.ToString();
				ValueType = value.GetType();
			}
		}

		/// <summary>
		/// float value of the specific Configuration Node
		/// </summary>
		public float FloatValue
		{
			get { float f; float.TryParse(Value, out f); return f; }
			set
			{
				Value = value.ToString();
				ValueType = value.GetType();
			}

		}

		/// <summary>
		/// Get a specific child node
		/// </summary>
		/// <param name="path">
		/// The path to the specific node. Can be either only a name, or a full path separated by '/' or '\'
		/// </param>
		/// <example>
		/// <code>
		/// XmlConfig conf = new XmlConfig("configuration.Xml");
		/// screenname = conf.Settings["screen"].Value;
		/// height = conf.Settings["screen/height"].IntValue;
		///  // OR
		/// height = conf.Settings["screen"]["height"].IntValue;
		/// </code>
		/// </example>
		/// <returns></returns>
		public Config this[string path]
		{
			get
			{
				char[] separators = { '/', '\\' };
				path.Trim(separators);
				String[] fullpath = path.Split(separators);

				XmlNode selectednode = node;
				XmlNode newnode;

				foreach (string nextname in fullpath)
				{
					// Verify name
					foreach (char c in nextname)
					{
						if ((!Char.IsLetterOrDigit(c)) && c != '_')
							return null;
					}

					newnode = selectednode.SelectSingleNode(nextname);

					if (newnode == null)
					{
						XmlElement newelement = selectednode.OwnerDocument.CreateElement(nextname);
						newnode = selectednode.AppendChild(newelement);
					}
					selectednode = newnode;
				}

				return new Config(selectednode);
			}
		}

		/// <summary>
		/// Check if the node conforms with the config xml restrictions
		/// 1. No nodes with two children of the same name
		/// 2. Only alphanumerical names
		/// </summary>
		/// <returns>
		/// True on success and false on failiure
		/// </returns>        
		public bool Validate()
		{
			if (ChildCount == 0)
				return true;
			string[] names = ChildrenNames;
			Array.Sort(names);
			for (int i = 0; i < names.Length - 1; i++)
			{
				foreach (Char c in names[i])
				{
					if (!Char.IsLetterOrDigit(c) && c != '_')
						return false;
				}
				if (names[i] == names[i + 1])
					return false;
				if (!this[names[i]].Validate())
					return false;
			}
			return true;
		}

		/// <summary>
		/// Remove the specific node from the tree
		/// </summary>
		public void Remove()
		{
			if (node.ParentNode == null) return;
			node.ParentNode.RemoveChild(node);
		}

		/// <summary>
		/// Remove all children of the node, but keep the node itself
		/// </summary>
		public void RemoveChildren()
		{
			node.RemoveAll();
		}


	}
}
