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
using System.ComponentModel;

namespace Sardauscan.Core.ProcessingTask
{
	/// <summary>
	/// Reconstruc the Mesh using the Scanline points index
	/// </summary>
    [Browsable(false)]
	public class MeshBuilderByIndex : AbstractMeshBuilder
	{
		/// <summary>
		/// Name of the Task
		/// </summary>
		public override string Name
		{
			get
			{
				return "By Index";
			}
		}
		protected override StripResult CreateStrip(ScanLine previous, ScanLine current)
		{
			int count1 = previous.Count;
			int count2 = current.Count;
			ScanLine ret1 = new ScanLine(previous.LaserID, previous.Count);
			ScanLine ret2 = new ScanLine(current.LaserID, current.Count);
			int i1 = 0;
			int i2 = 0;

			while (i1 < count1 && i2 < count2)
			{
				Point3D p1 = previous[i1];
				Point3D p2 = current[i2];
				ret1.Add(p1);
				ret2.Add(p2);
				if (i2 == 0 && p1.Position.Y > p2.Position.Y)
					i1++;
				else if (i1 == 0 && p1.Position.Y < p2.Position.Y)
					i2++;
				else if (i1 == count1 - 1 && i2 != count2 - 1)
					i2++;
				else if (i2 == count2 - 1 && i1 != count1 - 1)
					i1++;
				else
				{
					i1++;
					i2++;
				}
			}

			return new StripResult(ret1, ret2);
		}
	}
}
