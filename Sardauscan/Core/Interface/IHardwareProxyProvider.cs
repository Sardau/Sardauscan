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

namespace Sardauscan.Core.Interface
{
	/// <summary>
	/// Basic interface fo a IHardwareProxy provider
	/// </summary>
	public interface IHardwareProxyProvider
	{
		 /// <summary>
		 /// Display name of the Provider, for the user to know what he select;)
		 /// </summary>
			string Name { get; }

		 /// <summary>
		 /// Return the typeof of the IHardwareproxy privided by this provider
		 /// </summary>
			Type GenerateType { get; }

			/// <summary>
			/// This function is call when the user request a instance of ther IHardwareProxy
			/// You can call winforms to as information ( Com port, configuration etc)
			/// </summary>
			/// <param name="owner">owner window</param>
			/// <returns>a IHardwareProxy if one is selected, Null in case of cancel or not disponible</returns>
			object Select(IWin32Window owner);

	}
}
