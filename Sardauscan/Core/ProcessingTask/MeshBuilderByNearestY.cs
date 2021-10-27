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
	/// Reconstruc the Mesh by finding nearest Y points
	/// </summary>

     [Browsable(false)]
    public class MeshBuilderByNearestY : AbstractMeshBuilder
    {
        protected override StripResult CreateStrip(ScanLine previous, ScanLine current)
        {
            ScanLine ret1 = new ScanLine(previous.LaserID);
            ScanLine ret2 = new ScanLine(current.LaserID);

            Point3DList all = new Point3DList();
            all.AddRange(previous);
            all.AddRange(current.Where(p2 => previous.All(p1 => p1.Position.Y != p2.Position.Y)));
            all.Sort();
            all.Reverse();
            //            all.AddRange(list1.Union(list2, ));


            for (int i = 0; i < all.Count; i++)
            {
                double y = all[i].Position.Y;
                Point3D p1 = previous.GetNearestY(y);
                Point3D p2 = current.GetNearestY(y);
                ret1.Add(p1);
                ret2.Add(p2);
            }
            return new StripResult(ret1, ret2);

        }

        public override string Name
        {
            get { return "By Nearest"; }
        }
    }
}
