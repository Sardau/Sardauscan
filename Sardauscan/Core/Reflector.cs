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
                try
                {
                    Assembly a = Assembly.LoadFile(allPluginsPath[i]);
                }
                catch { }
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
