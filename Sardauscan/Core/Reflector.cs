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
using System.ComponentModel;
using Sardauscan.Core.Interface;
using System.Reflection;

namespace Sardauscan.Core
{
	/// <summary>
	/// Reflection based functions
	/// </summary>
	public static class Reflector
	{

		private static bool _pluginloaded = false;
		/// <summary>
		/// Load the plugins
		/// </summary>
		public static void LoadPlugins()
		{
			if (_pluginloaded)
				return;
			String[] allPluginsPath = System.IO.Directory.GetFiles(Program.PluginsPath, "*.dll", System.IO.SearchOption.TopDirectoryOnly);
			for (int i = 0; i < allPluginsPath.Length; i++)
			{
				Assembly a = Assembly.LoadFile(allPluginsPath[i]);
			}
			_pluginloaded = true;
		}


		private static List<Type> _AllTypes = null;
		/// <summary>
		/// List of all Type available 
		/// </summary>
		public static List<Type> AllTypes
		{
			get
			{
				if (_AllTypes == null)
				{
					LoadPlugins();
					List<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).ToList();
					_AllTypes = new List<Type>();
					foreach (Type type in types)
					{
						if (!type.IsAbstract)
						{
							BrowsableAttribute attribute = Attribute.GetCustomAttribute(type, typeof(BrowsableAttribute)) as BrowsableAttribute;
							if (attribute == null || (attribute != null && attribute.Browsable))
								_AllTypes.Add(type);
						}
					}
				}
				return _AllTypes;
			}
		}
		/// <summary>
		/// Get all the derived type
		/// </summary>
		/// <param name="baseType"></param>
		/// <returns></returns>
		public static List<Type> GetSubClassOf(Type baseType)
		{
			return AllTypes.Where(type => type.IsSubclassOf(baseType)).ToList();
		}
		/// <summary>
		/// Get all the ckass that implement a interface
		/// </summary>
		/// <param name="baseType"></param>
		/// <returns></returns>
		public static List<Type> GetAssignableFrom(Type baseType)
		{
			return AllTypes.Where(type => baseType.IsAssignableFrom(type)).ToList();
		}
		/// <summary>
		/// Get all the hardware providers
		/// </summary>
		/// <param name="baseType"></param>
		/// <returns></returns>
		public static List<IHardwareProxyProvider> GetProviders(Type baseType)
		{
			var availabelType = GetAssignableFrom(typeof(IHardwareProxyProvider));

			List<IHardwareProxyProvider> ret = new List<IHardwareProxyProvider>();
			foreach (Type type in availabelType)
			{
				if (!type.IsAbstract)
				{
					BrowsableAttribute attribute = Attribute.GetCustomAttribute(type, typeof(BrowsableAttribute)) as BrowsableAttribute;
					if (attribute == null || (attribute != null && attribute.Browsable))
					{
						IHardwareProxyProvider provider = (IHardwareProxyProvider)Activator.CreateInstance(type);
						if (provider != null)
						{
							if (baseType.IsAssignableFrom(provider.GenerateType))
								ret.Add(provider);
						}
					}
				}
			}
			ret = ret.OrderBy(x => x.Name).ToList();
			return ret;

		}
		public static T CreateInstance<T>(Type type) 
		{
			try
			{
				return (T)Activator.CreateInstance(type);
			}
			catch
			{
				return default(T); 
			}
		}

		public static Type GetType(string assemblyQualifiedName)
		{
			List<Type> all = AllTypes;
			foreach (Type t in all)
				if (t.AssemblyQualifiedName == assemblyQualifiedName)
					return t;
			return null;

		}

	}
}
