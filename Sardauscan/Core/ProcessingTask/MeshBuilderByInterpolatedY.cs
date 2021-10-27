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
	/// Reconstruc the Mesh byt interpolating the Scanline points by Y position
	/// </summary>
	[Browsable(false)]
	class MeshBuilderByInterpolatedY : AbstractMeshBuilder
	{
		protected override StripResult CreateStrip(ScanLine previous, ScanLine current)
		{
			ScanLine interpolated = new ScanLine(current.LaserID, current.GetYInterpolated(previous));
			return new StripResult(previous, interpolated);
		}

		public override string Name
		{
			get { return "By Y Aproximation"; }
		}
	}
}
