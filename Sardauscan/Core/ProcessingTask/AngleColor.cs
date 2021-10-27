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
using Sardauscan.Core.Geometry;
using OpenTK;
using System.ComponentModel;

namespace Sardauscan.Core.ProcessingTask
{
	/// <summary>
	/// Color scanlines based on the ScanLine Angle
	/// </summary>
	[Browsable(false)]
	public class AngleColor : AbstractLineTask
	{
		public override ScanLine DoTask(ScanLine source)
		{
			double clamp = Math.Abs(Utils.DeltaAngle(-180, source.Angle) / 360);
			ScanLine ret = new ScanLine(source.LaserID,source.Count);
			for (int i = 0; i < source.Count; i++)
			{
				Point3D p = source[i];
				Vector4 c = new Vector4(0.25f, 0.25f, 0.25f, 1);
				c[source.LaserID%3] = (float)clamp;
				ret.Add(new Point3D(p.Position,p.Normal,ColorExtension.ColorFromVector(c))); 
			}
			return ret;
		}

		public override string Name
		{
			get { return "Angle Color"; }
		}
	}
}
