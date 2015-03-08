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
using System.ComponentModel;

namespace Sardauscan.Core.ProcessingTask
{
	/// <summary>
	/// Reconstruc the Mesh by finding nearest Y points
	/// </summary>

     [Browsable(false)]
    public class MeshBuilderByNearestY : AbstractMeshBuilder
    {
        protected override StripResult CreateStrip(ScanLine previous, ScanLine current)
        {
            ScanLine ret1 = new ScanLine(previous.LaserID);
            ScanLine ret2 = new ScanLine(current.LaserID);

            Point3DList all = new Point3DList();
            all.AddRange(previous);
            all.AddRange(current.Where(p2 => previous.All(p1 => p1.Position.Y != p2.Position.Y)));
            all.Sort();
            all.Reverse();
            //            all.AddRange(list1.Union(list2, ));


            for (int i = 0; i < all.Count; i++)
            {
                float y = all[i].Position.Y;
                Point3D p1 = previous.GetNearestY(y);
                Point3D p2 = current.GetNearestY(y);
                ret1.Add(p1);
                ret2.Add(p2);
            }
            return new StripResult(ret1, ret2);

        }

        public override string Name
        {
            get { return "By Nearest"; }
        }
    }
}
