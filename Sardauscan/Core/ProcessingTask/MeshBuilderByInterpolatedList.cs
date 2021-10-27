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

namespace Sardauscan.Core.ProcessingTask
{
	/// <summary>
	/// Reconstruc the Mesh by interpolating the Scanline points 
	/// </summary>
	public class MeshBuilderByInterpolatedList : AbstractMeshBuilder
    {
        protected override StripResult CreateStrip(ScanLine previous, ScanLine current)
        {
            ScanLine interpolated = new ScanLine(current.LaserID, current.GetInterpolatedList(previous.Count));
            return new StripResult(previous, interpolated);
        }
		/// <summary>
		/// Name of the task
		/// </summary>
        public override string Name
        {
            get { return "Build Mesh"; }
        }
    }
}
