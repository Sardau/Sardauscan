#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
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
