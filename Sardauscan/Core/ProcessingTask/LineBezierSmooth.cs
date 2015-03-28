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
