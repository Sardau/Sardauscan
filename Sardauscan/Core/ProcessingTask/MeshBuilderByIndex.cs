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

namespace Sardauscan.Core.ProcessingTask
{
	/// <summary>
	/// Reconstruc the Mesh using the Scanline points index
	/// </summary>
	public class MeshBuilderByIndex : AbstractMeshBuilder
	{
		/// <summary>
		/// Name of the Task
		/// </summary>
		public override string Name
		{
			get
			{
				return "By Index";
			}
		}
		protected override StripResult CreateStrip(ScanLine previous, ScanLine current)
		{
			int count1 = previous.Count;
			int count2 = current.Count;
			ScanLine ret1 = new ScanLine(previous.LaserID, previous.Count);
			ScanLine ret2 = new ScanLine(current.LaserID, current.Count);
			int i1 = 0;
			int i2 = 0;

			while (i1 < count1 && i2 < count2)
			{
				Point3D p1 = previous[i1];
				Point3D p2 = current[i2];
				ret1.Add(p1);
				ret2.Add(p2);
				if (i2 == 0 && p1.Position.Y > p2.Position.Y)
					i1++;
				else if (i1 == 0 && p1.Position.Y < p2.Position.Y)
					i2++;
				else if (i1 == count1 - 1 && i2 != count2 - 1)
					i2++;
				else if (i2 == count2 - 1 && i1 != count1 - 1)
					i1++;
				else
				{
					i1++;
					i2++;
				}
			}

			return new StripResult(ret1, ret2);
		}
	}
}
