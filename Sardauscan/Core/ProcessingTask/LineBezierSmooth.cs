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
using Sardauscan.Gui.PropertyGridEditor;
using System.Drawing.Design;

namespace Sardauscan.Core.ProcessingTask
{
	/// <summary>
	/// Smooth the Scanline with a Bezier courbe interpolation
	/// </summary>
    public class LineBezierSmooth : AbstractLineTask
    {
        public override string Name
        {
            get
            {
                return "Smooth Bezier";
            }
        }
        public override string DisplayName { get { return "Bezier"; } }
        public override eTaskType TaskType { get { return eTaskType.Smooth; } }

        public enum eMode
        {
            [Description("Mode 0")]
            Mode0,
            [Description("Mode 3")]
            Mode1,
            [Description("Mode 2")]
            Mode2
        }

        private eMode m_Mode = eMode.Mode2;
        [Browsable(true)]
        [Description("Mode of the Interpolation")]
        [DisplayName("Interpolation mode")]
        public eMode Mode { get { return m_Mode; } set { m_Mode = value; } }

        private int m_SegmentPerCurve = 4;
        [Browsable(true)]
        [Description("Number of Segment per curve")]
        [DisplayName("Segment per curve")]
        [TypeConverter(typeof(NumericUpDownTypeConverter))]
        [Editor(typeof(NumericUpDownTypeEditor), typeof(UITypeEditor)), MinMaxAttribute(2, 10)]
        public int SegmentPerCurve { get { return m_SegmentPerCurve; } set { m_SegmentPerCurve = value; } }

        private  double m_MinSquareDistance = 0.2f;
        [Browsable(true)]
        [Description("Minimum size of a segment (squared)")]
        [DisplayName("Minimum square distance")]
        [TypeConverter(typeof(NumericUpDownTypeConverter))]
        [Editor(typeof(NumericUpDownTypeEditor), typeof(UITypeEditor)), MinMaxAttribute(0.01f, 50.0f, 0.005f,3)]
        public double MinSquareDistance { get { return m_MinSquareDistance; } set { m_MinSquareDistance = value; } }

        public LineBezierSmooth()
        {
        }

        public override ScanLine DoTask(ScanLine source)
        {
            int count = source.Count;
            ScanLine ret = new ScanLine(source.LaserID, count);
            ret.DisplayAsLine = source.DisplayAsLine;
            if (count < 4)
                ret.AddRange(source);
            else
            {
                BezierBuilder bez = new BezierBuilder(SegmentPerCurve, MinSquareDistance);
                bez.SetControlPoints(source);
                switch (Mode)
                {
                    case eMode.Mode0:
                        ret.AddRange(bez.GetDrawingPoints0());
                        break;
                    case eMode.Mode1:
                        ret.AddRange(bez.GetDrawingPoints1());
                        break;
                    default:
                        ret.AddRange(bez.GetDrawingPoints2());
                        break;
                }
            }
            return ret;
        }

				/// <summary>
				/// Clone this
				/// </summary>
				/// <returns></returns>
				public override AbstractProcessingTask Clone()
        {
            LineBezierSmooth ret = (LineBezierSmooth)base.Clone();
            ret.Mode = this.Mode;
            ret.SegmentPerCurve = this.SegmentPerCurve;
            ret.MinSquareDistance = this.MinSquareDistance;
            return ret;
        }

    }
}
