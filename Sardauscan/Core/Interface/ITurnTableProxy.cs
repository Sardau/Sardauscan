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
	/// Basic interface for a Turn Table Proxy
	/// </summary>
	public interface ITurnTableProxy : IDisposable, IHardwareProxy
	{
		/// <summary>
		/// Rotate the Table
		/// </summary>
		/// <param name="theta"></param>
		/// <param name="relative">use relative(true) or absolute(false) rotation</param>
		/// <returns></returns>
		int Rotate(double theta, bool relative);
		/// <summary>
		/// Set the current position to 0° (for absolute rotation)
		/// </summary>
		void InitialiseRotation();
		/// <summary>
		/// Get the minimum Rotation angle for the table
		/// </summary>
		/// <returns></returns>
		float MinimumRotation();
		/// <summary>
		/// Enable of disable the motors
		/// </summary>
		bool MotorEnabled { set; }
	}
}
