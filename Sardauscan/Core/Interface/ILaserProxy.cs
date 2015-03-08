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

namespace Sardauscan.Core.Interface
{

	/// <summary>
	/// Basic Interface for a Laser Proxy
	/// </summary>
	public interface ILaserProxy : IDisposable, IHardwareProxy
	{

		/// <summary>
		/// Turn a laser on or off
		/// </summary>
		/// <param name="index"></param>
		/// <param name="on"></param>
		void Turn(int index, bool on);

		/// <summary>
		/// Ask if a laser is on or off
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		bool On(int index);
		/// <summary>
		/// Get the count of laser
		/// </summary>
		int Count { get; }

	}

	/// <summary>
	/// Extention of ILaserProxy
	/// </summary>
	public static class ILaserProxyExtention
	{
		/// <summary>
		/// Turn all the lasers  on or off together
		/// </summary>
		/// <param name="ctrl"></param>
		/// <param name="on"></param>
		public static void TurnAll(this ILaserProxy ctrl, bool on)
		{
			for (int i = 0; i < ctrl.Count; i++)
				ctrl.Turn(i, on);
		}
	}
}
