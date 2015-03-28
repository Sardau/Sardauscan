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
using System.ComponentModel;
using Sardauscan.Gui.PropertyGridEditor;
using System.Drawing.Design;
using Sardauscan.Core.Geometry;
using OpenTK;

namespace Sardauscan.Core.ProcessingTask
{
	/// <summary>
	/// Scale the Scanlines
	/// </summary>
	public class LineScaleTask : AbstractLineTask
	{
		private double xFactor = 1;
		[Browsable(true)]
		[Description("X scale factor (1 = no scale)")]
		[DisplayName("X")]
		[TypeConverter(typeof(NumericUpDownTypeConverter))]
		[Editor(typeof(NumericUpDownTypeEditor), typeof(UITypeEditor)), MinMaxAttribute(-1000f, 1000f, 0.005f, 3)]
		public double XFactor { get { return xFactor; } set { xFactor = value; } }

		private double yFactor = 1;
		[Browsable(true)]
		[Description("Y scale factor (1 = no scale)")]
		[DisplayName("Y")]
		[TypeConverter(typeof(NumericUpDownTypeConverter))]
		[Editor(typeof(NumericUpDownTypeEditor), typeof(UITypeEditor)), MinMaxAttribute(-1000f, 1000f, 0.005f, 3)]
		public double YFactor { get { return yFactor; } set { yFactor = value; } }

		private double zFactor = 1;
		[Browsable(true)]
		[Description("Z scale factor (1 = no scale)")]
		[DisplayName("Z")]
		[TypeConverter(typeof(NumericUpDownTypeConverter))]
		[Editor(typeof(NumericUpDownTypeEditor), typeof(UITypeEditor)), MinMaxAttribute(-1000f, 1000f, 0.005f, 3)]
		public double ZFactor { get { return zFactor; } set { zFactor = value; } }

		public override ScanLine DoTask(ScanLine source)
		{
			ScanLine ret = new ScanLine(source.LaserID, source.Count);
			for (int i = 0; i < source.Count; i++)
			{
				Point3D p = source[i];
				Vector3d pos = p.Position;
				pos.X *= XFactor;
				pos.Y *= YFactor;
				pos.Z *= ZFactor;
				ret.Add(new Point3D(pos, p.Normal, p.Color));
			}
			return ret;
		}

		public override string Name
		{
			get { return "Scale"; }
		}
        public override eTaskType TaskType { get { return eTaskType.Transform; } }

		/// <summary>
		/// Clone this
		/// </summary>
		/// <returns></returns>
		public override AbstractProcessingTask Clone()
		{
			LineScaleTask clone = (LineScaleTask)base.Clone();
			clone.XFactor = XFactor;
			clone.YFactor = YFactor;
			clone.ZFactor = ZFactor;
			return clone;
		}
		public override string DisplayName
		{
			get
			{
				if (XFactor != 1 || YFactor != 1 || ZFactor != 1)
					return base.DisplayName + String.Format(" ({0:0.##},{1:0.##},{2:0.##})", XFactor, YFactor, ZFactor);
				return base.DisplayName;
			}
		}
	}
}
