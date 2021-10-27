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
using OpenTK;
using Sardauscan.Core.Geometry;
using System.Drawing;
using System.ComponentModel;
using Sardauscan.Gui.PropertyGridEditor;
using System.Drawing.Design;

namespace Sardauscan.Core.ProcessingTask
{
	/// <summary>
	/// Make a IQR Filter to remove Scanline noise
	/// </summary>
    public class IqrFilter : AbstractLineTask
    {
        public override string Name
        {
            get { return "Filter IQR"; }
        }
        public override eTaskType TaskType { get { return eTaskType.Filter; } }

        public struct ValidityRange
        {
            public double Min;
            public double Max;

            public bool IsValid(double v)
            {
                return v <= Max && v >= Min;
            }
        }
        public double GetVal(Point3D pt)
        {
            return pt.Position.Xz.Length;
        }
        public ValidityRange GetRange(Point3DList source,double factor)
            {
                List<int> indexes = new List<int>(source.Count);
                for (int i = 0; i < source.Count; i++)
                    indexes.Add(i);
                indexes.Sort(delegate(int x, int y)
                {
                    return  GetVal(source[x]).CompareTo( GetVal(source[y]));
                }
                );

                double first = GetQuartile(indexes, source, 0.25);
                double third = GetQuartile(indexes, source, 0.75);
                double iqr = third - first;
                ValidityRange ret = new ValidityRange();
                ret.Min = first - factor * iqr;
                ret.Max = third + factor * iqr;
                return ret ; 
            }
        private static double GetQuartile(List<int> list, Point3DList pts,double quartile)
            {
                double result;

                // Get roughly the index
                double index = quartile * (list.Count() + 1);

                // Get the remainder of that index value if exists
                double remainder = index % 1;

                // Get the integer value of that index
                index = Math.Floor(index) - 1;

                if (remainder.Equals(0))
                {
                    // we have an integer value, no interpolation needed
                    result = pts[list.ElementAt((int)index)].Position.Xz.Length;
                }
                else
                {
                    // we need to interpolate
                    double v1 = pts[list.ElementAt((int)index)].Position.Xz.Length;
                    double v2 = pts[list.ElementAt((int)(index + 1))].Position.Xz.Length;
                    result = v1 + ((v2 - v1) * remainder);
                }
                return result;
            }
#if DEBUG
        private bool m_ColoriseOnly = false;
        [Browsable(true)]
        [Description("only colorise rejected")]
        [DisplayName("ColoriseOnly")]
        public bool ColoriseOnly { get { return m_ColoriseOnly; } set { m_ColoriseOnly = value; } }
#endif        
        private double factor = 1f;
        [Browsable(true)]
        [Description("IDR factor (1.5 = default)")]
        [DisplayName("IDR factor")]
        [TypeConverter(typeof(NumericUpDownTypeConverter))]
        [Editor(typeof(NumericUpDownTypeEditor), typeof(UITypeEditor)), MinMaxAttribute(0.001f, 3f, 0.005f, 3)]
        public double Factor { get { return factor; } set { factor = value; } }

        public override ScanLine DoTask(ScanLine source)
        {
					if (source.Count < 5)
					{ //not enough points, less than 5 points surely a false positive
						return new ScanLine(source.LaserID, 0);
					}
            ValidityRange range = GetRange(source, Factor);

						ScanLine ret = new ScanLine(source.LaserID, source.Count);

            for (int i = 0; i < source.Count; i++)
            {
                Point3D sp = source[i];
                bool valid = range.IsValid(GetVal(sp));
#if DEBUG
                if (ColoriseOnly)
                {
                    Color col = valid ? Color.WhiteSmoke : Color.Red;
                    ret.Add(new Point3D(sp.Position, sp.Normal, col));
                }
                else
#endif
                    if(valid)
                        ret.Add(new Point3D(sp.Position, sp.Normal, sp.Color));
            }
            return ret;
        }
     }
}
