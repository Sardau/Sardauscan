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
using System.Drawing;

namespace Sardauscan.Core.Geometry
{
	/// <summary>
	/// Calss for a pixel location in a image
	/// </summary>
	public class PixelLocation
	{
		/// <summary>
		/// Ctor
		/// </summary>
		public PixelLocation()
			: this(-1, -1)
		{
		}
		/// <summary>
		///  CTor
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public PixelLocation(double x, double y)
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// X position
		/// </summary>
		public double X;
		/// <summary>
		/// Y position
		/// </summary>
		public double Y;
		/// <summary>
		/// Is default
		/// </summary>
		/// <returns></returns>
		public bool IsNull()
		{
			return X == -1 && Y == -1;
		}
		/// <summary>
		/// Convert to PointF
		/// </summary>
		/// <returns></returns>
		public PointF ToPointF()
		{
            return new PointF((float)X, (float)Y);
		}
	}
}
