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
