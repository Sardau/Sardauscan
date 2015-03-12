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
using Sardauscan.Core.Geometry;
using OpenTK;
using System.ComponentModel;

namespace Sardauscan.Core.ProcessingTask
{
	/// <summary>
	/// Color scanlines based on the ScanLine Angle
	/// </summary>
	[Browsable(false)]
	public class AngleColor : AbstractLineTask
	{
		public override ScanLine DoTask(ScanLine source)
		{
			float clamp = Math.Abs(Utils.DeltaAngle(-180, source.Angle) / 360);
			ScanLine ret = new ScanLine(source.LaserID,source.Count);
			for (int i = 0; i < source.Count; i++)
			{
				Point3D p = source[i];
				Vector4 c = new Vector4(0.25f, 0.25f, 0.25f, 1);
				c[source.LaserID%3] = clamp;
				ret.Add(new Point3D(p.Position,p.Normal,ColorExtension.ColorFromVector(c))); 
			}
			return ret;
		}

		public override string Name
		{
			get { return "Angle Color"; }
		}
	}
}
