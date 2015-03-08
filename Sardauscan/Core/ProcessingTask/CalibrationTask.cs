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
using System.Threading.Tasks;
using System.Windows.Threading;
using System.ComponentModel;

namespace Sardauscan.Core.ProcessingTask
{
	[Browsable(false)]
	internal class CalibrationTask : AbstractLineTask
	{
		protected override bool LaunchParallel
		{
			get
			{
				return true;
			}
		}

		protected override ScanData DoTask(ScanData source)
		{
			ScanData step1 = base.DoTask(source);
			step1.Sort();
			Dictionary<int, ScanLine> step2 = new Dictionary<int, ScanLine>();
			for (int i = 0; i < step1.Count; i++)
			{
				ScanLine line = step1[i];
				if (line.Count > 0)
				{
					if (!step2.ContainsKey(line.LaserID))
						step2[line.LaserID] = new ScanLine(line.LaserID, 24);
					step2[line.LaserID].Add(line[0]);
				}
			}

			ScanData step3 = new ScanData(step2.Keys.Count);
			for (int i = 0; i < step2.Keys.Count; i++)
				step3.Add(step2[i]);
			return step3;
		}
		protected override ScanLine DoTask(ScanLine source)
		{
			ScanLine ret = new ScanLine(source.LaserID, source.Count);
			if (source != null && source.Count > 0)
			{
				Point3D avg = Point3D.Average(source);
				ret.Add(avg);
			}
			return ret;
		}

		public override string Name
		{
			get { return "Calibration step Task"; }
		}
	}
}
