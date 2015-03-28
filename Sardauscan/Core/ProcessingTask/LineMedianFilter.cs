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
using System.Drawing.Design;
using Sardauscan.Gui.PropertyGridEditor;

namespace Sardauscan.Core.ProcessingTask
{
	/// <summary>
	/// Apply a median Filter to the Scanline
	/// </summary>
    public class LineMedianFilter : AbstractLineTask
    {
        public override string Name
        {
            get
            {
                return "Filter Median";
            }
        }
        public override eTaskType TaskType { get { return eTaskType.Filter; } }
        private uint m_Order = 5;

        [Browsable(true)]
        [Description("Size of the average window")] 
        [DisplayName("Median Window Size")]
        [TypeConverter(typeof(NumericUpDownTypeConverter))]
        [Editor(typeof(NumericUpDownTypeEditor), typeof(UITypeEditor)), MinMaxAttribute(3, 10)] 
        public uint Order
        {
            get { return m_Order; }
            set
            {
                m_Order = Math.Max(3, value);
            }
        }


        public override ScanLine DoTask(ScanLine source)
        {
            int count = source.Count;
            if (count <= 2)
                return source;
            ScanLine ret = new ScanLine(source.LaserID, source.Count);
            ret.DisplayAsLine = source.DisplayAsLine;
            uint order = Order;
            for (int i = 0; i < count; i++)
            {
                if (this.CancelPending) return ret;
                //                Debug.Write("[");
                List<Point3D> sourceWindow = new List<Point3D>();
                for (int j = 0; j < order; j++)
                {
                    int indice =(int)( i + j - order / 2);
                    indice = Math.Max(0, Math.Min(indice, count - 1));
                    //                   Debug.Write(" "+indice);
                    sourceWindow.Add(source[indice]);
                }

                //                Debug.WriteLine("]");
                ret.Add(Utils.GetMedian(sourceWindow));
            }
            return ret;
        }

				/// <summary>
				/// Clone this
				/// </summary>
				/// <returns></returns>
				public override AbstractProcessingTask Clone()
        {
            LineMedianFilter ret = (LineMedianFilter)base.Clone();
            ret.Order = Order;
            return ret;
        }

    }
}
