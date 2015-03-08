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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using Sardauscan.Core;

namespace Sardauscan.Core.IO
{
	/// <summary>
	/// The class which represents a configuration xml file
	/// based on http://www.codeproject.com/Articles/16953/XML-configuration-files-made-simple-at-last
	/// </summary>
	public class XmlConfig : IDisposable
	{

		private XmlDocument Xmldoc;
		private string originalFile;
		private bool commitonunload = true;

		/// <summary>
		/// Check XML file if it conforms the config xml restrictions
		/// 1. No nodes with two children of the same name
		/// 2. Only alphanumerical names
		/// </summary>
		/// <param name="silent">
		/// Whether to return a true/false value, or throw an exception on failiure
		/// </param>
		/// <returns>
		/// True on success and in case of silent mode false on failiure
		/// </returns>
		public bool ValidateXML(bool silent)
		{
			if (!Settings.Validate())
			{
				if (silent)
					return false;
				else
					throw new Exception("This is not a valid configuration xml file! Probably duplicate children with the same names, or non-alphanumerical tagnames!");
			}
			else
				return true;
		}


		/*
		/// <summary>
		/// Create an XmlConfig from an empty xml file 
		/// containing only the rootelement named as 'xml'
		/// </summary>
		public XmlConfig()
		{
				xmldoc = new XmlDocument();
				LoadXmlFromString("<xml></xml>");
		}
		*/
		/// <summary>
		/// Create an XmlConfig from an existing file, or create a new one
		/// </summary>
		/// <param name="loadfromfile">
		/// Path and filename from where to load the xml file
		/// </param>
		/// <param name="create">
		/// If file does not exist, create it, or throw an exception?
		/// </param>
		public XmlConfig(string loadfromfile, bool create)
		{
			Xmldoc = new XmlDocument();
			LoadXmlFromFile(loadfromfile, create);
		}

		/// <summary>
		/// When unloading the current XML config file
		/// shold any changes be saved back to the file?
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>Only applies if it was loaded from a local file</item>
		/// <item>True by default</item>
		/// </list>
		/// </remarks>
		public bool CommitOnUnload
		{
			get { return commitonunload; }
			set { commitonunload = value; }
		}

		/// <summary>
		/// Save any modifications to the XML file before destruction
		/// if CommitOnUnload is true
		/// </summary>
		public void Dispose()
		{
			if (CommitOnUnload) Commit();
		}

		/// <summary>
		/// Load a new XmlDocument from a file
		/// </summary>
		/// <param name="filename">
		/// Path and filename from where to load the xml file
		/// </param>
		/// <param name="create">
		/// If file does not exist, create it, or throw an exception?
		/// </param>
		public void LoadXmlFromFile(string filename, bool create)
		{
			if (CommitOnUnload) Commit();
			try
			{
				Xmldoc.Load(filename);
			}
			catch
			{
				if (!create)
					throw new Exception("xmldoc.Load() failed! Probably file does NOT exist!");
				else
				{
					Xmldoc.LoadXml("<xml></xml>");
					Save(filename);
				}
			}
			ValidateXML(false);
			originalFile = filename;

		}

		/// <summary>
		/// Load a new XmlDocument from a file
		/// </summary>
		/// <param name="filename">
		/// Path and filename from where to load the xml file
		/// </param>
		/// <remarks>
		/// Throws an exception if file does not exist
		/// </remarks>
		public void LoadXmlFromFile(string filename)
		{
			LoadXmlFromFile(filename, false);
		}

		/// <summary>
		/// Load a new XmlDocument from a string
		/// </summary>
		/// <param name="xml">
		/// XML string
		/// </param>
		public void LoadXmlFromString(string xml)
		{
			if (CommitOnUnload) Commit();
			Xmldoc.LoadXml(xml);
			originalFile = null;
			ValidateXML(false);
		}

		/// <summary>
		/// Load an empty XmlDocument
		/// </summary>
		/// <param name="rootelement">
		/// Name of root element
		/// </param>
		public void NewXml(string rootelement)
		{
			if (CommitOnUnload) Commit();
			LoadXmlFromString(String.Format("<{0}></{0}>", rootelement));
		}

		/// <summary>
		/// Save configuration to an xml file
		/// </summary>
		/// <param name="filename">
		/// Path and filname where to save
		/// </param>
		public void Save(string filename)
		{
			ValidateXML(false);
			Xmldoc.Save(filename);
			originalFile = filename;
		}

		/// <summary>
		/// Save configuration to a stream
		/// </summary>
		/// <param name="stream">
		/// Stream where to save
		/// </param>
		public void Save(System.IO.Stream stream)
		{
			ValidateXML(false);
			Xmldoc.Save(stream);
		}

		/// <summary>
		/// If loaded from a file, commit any changes, by overwriting the file
		/// </summary>
		/// <returns>
		/// True on success
		/// False on failiure, probably due to the file was not loaded from a file
		/// </returns>

		public bool Commit()
		{
			if (originalFile != null) { Save(originalFile); return true; } else { return false; }
		}

		/// <summary>
		/// If loaded from a file, trash any changes, and reload the file
		/// </summary>
		/// <returns>
		/// True on success
		/// False on failiure, probably due to file was not loaded from a file
		/// </returns>
		public bool Reload()
		{
			if (originalFile != null) { LoadXmlFromFile(originalFile); return true; } else { return false; }
		}

		/// <summary>
		/// Gets the root ConfigSetting
		/// </summary>
		public Config Settings
		{
			get { return new Config(Xmldoc.DocumentElement); }
		}

		public static Color ColorFromString(string str)
		{
			return System.Drawing.ColorTranslator.FromHtml(str);
		}
		public static string ColorToString(Color col)
		{
			return System.Drawing.ColorTranslator.ToHtml(col);
		}


		/// <summary>
		/// Load the Xml Configuration document and populate our CustomClass with a dynamic property
		/// for each of the supported configuration sections. We're only supporting three sections here.
		/// The default appSettings, plus our standard ApplicationConfiguration
		/// and CommonConfiguration section handlers. These handlers are derived from IConfigurationSectionHandler.
		/// They have extended support for the Description attribute in addition to the Key, Value 
		/// pair attributes in the Xml configuration file.
		/// This could easily be extended to include support for any section under the configuration
		/// section that has the <add key="value" value="value"/> structure (assuming you haven't written a
		/// completely new Xml structure for your custom configuration section).
		/// </summary>
		public PropertyGridSettingsComponent GetPropertyGridSettingsComponent()
		{
			PropertyGridSettingsComponent customClass = new PropertyGridSettingsComponent();
			try
			{
				XmlDocument xmlDoc = this.Xmldoc;
				XmlNodeList nodes = xmlDoc.DocumentElement.ChildNodes;
				foreach (XmlNode section in nodes)
				{
					XmlNodeList itemList = section.ChildNodes;
					foreach (XmlNode item in itemList)
					{
						object value = string.Empty;
						if (item.Attributes["value"] != null)
							value = item.Attributes["value"].Value;
						Type type = typeof(String);
						if (item.Attributes["type"] != null)
						{
							try
							{
								if ("System.Drawing.Color" == item.Attributes["type"].Value)
								{
									type = typeof(System.Drawing.Color);
									value = XmlConfig.ColorFromString(value.ToString());
								}
								else if (typeof(Enum).IsAssignableFrom(Type.GetType(item.Attributes["type"].Value)))
								{
									type = Type.GetType(item.Attributes["type"].Value);
									value = Enum.Parse(type, value.ToString());
								}
								else
								{

									type = Type.GetType(item.Attributes["type"].Value);
								}
							}
							catch
							{
								type = typeof(String);
							}
							if (type == null)
								type = typeof(String);
						}

						customClass.AddProperty(item.Name, value, section.Name + "/" + item.Name + " => " + type.Name, section.Name, type, false, false);
					}

				}
				return customClass;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// We're only supporting three sections here at the moment.
		/// The default appSettings, plus our standard ApplicationConfiguration
		/// and CommonConfiguration section handlers. These handlers have extended support for
		/// the Description attribute in addition to the Key, Value pair attributes 
		/// in the Xml configuration file.
		/// </summary>
		public void UpdateConfiguration(PropertyGridSettingsComponent customClass, bool commit)
		{
			try
			{
				//Save a backup version
				if (!string.IsNullOrEmpty(originalFile))
					Xmldoc.Save(this.originalFile + "_bak");
				//Populate our property collection. 
				PropertyDescriptorCollection props = customClass.GetProperties();

				for (int i = 0; i < props.Count; i++)
				{
					PropertyDescriptor item = props[i];
					string value = item.GetValue(null).ToString();
					string category = item.Category;
					string key = item.DisplayName;
					Type type = item.PropertyType;
					if (type == typeof(System.Drawing.Color))
						Settings[category][key].ColorValue = (System.Drawing.Color)item.GetValue(null);
					else
						Settings[category][key].Value = value;
					Settings[category][key].ValueType = type;

				}
				Commit();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

	}
}
