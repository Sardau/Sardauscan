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
using System.Windows.Forms;
using Sardauscan.Core.Interface;

namespace Sardauscan.Hardware
{
	/// <summary>
	/// Abstract class for a T IHardwareProxy
	/// T must be a class that implement IHardwareInterface
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class AbstractProxyProvider<T> : IHardwareProxyProvider where T : IHardwareProxy 
	{
		/// <summary>
		/// Name of the Provider
		/// </summary>
		public abstract string Name {get;}

		/// <summary>
		/// Generated IHardwareProxy Type
		/// </summary>
		public Type GenerateType { get { return typeof(T); } }
		/// <summary>
		/// Function called for Selection of ressource, etc
		/// </summary>
		/// <param name="owner"></param>
		/// <returns></returns>
		public abstract object Select(IWin32Window owner);
	}
}
