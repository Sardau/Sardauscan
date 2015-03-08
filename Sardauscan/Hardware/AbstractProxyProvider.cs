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
