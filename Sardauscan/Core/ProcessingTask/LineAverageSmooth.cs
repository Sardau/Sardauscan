#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
# endregion
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
