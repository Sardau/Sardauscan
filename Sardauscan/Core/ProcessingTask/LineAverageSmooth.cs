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
	/// Smooth the scanline with a average
	/// </summary>
    [Browsable(false)]
    public class LineAverageSmooth : AbstractLineTask
    {
        public override string Name
        {
            get
            {
                return "Smooth Average";
            }
        }

        public override ScanLine DoTask(ScanLine source)
        {
            int count = source.Count;
            ScanLine ret = new ScanLine(source.LaserID, count);
            ret.DisplayAsLine = source.DisplayAsLine;
            if (count < 3)
                ret.AddRange(source);
            else
            {
                ret.Add(source[0]);
                for (int i = 1; i < count - 1; i++)
                {
                    if (CancelPending) return source;
                    ret.Add(source[i - 1].Average(source[i + 1]));
                }
                ret.Add(source[count - 1]);
            }
            return ret;
        }
    }
}
