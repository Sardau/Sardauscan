using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sardauscan.Core.ProcessingTask;
using System.Drawing;

namespace FakeHardwarePlugins
{
	public class FakeTask : AbstractProcessingTask
	{
		public override string Name { get { return "*FAKE TASK*"; } }

		public override eTaskItem In { get { return eTaskItem.ScanLines; } }

		public override eTaskItem Out{get { return eTaskItem.ScanLines; }}

		public override AbstractProcessingTask Clone(){return new FakeTask();}

		protected override Sardauscan.Core.ScanData DoTask(Sardauscan.Core.ScanData source)
		{
			// try somthing simple change Texture colors to RED
			Sardauscan.Core.ScanData returndata = new Sardauscan.Core.ScanData(source.Count);
			Status = eTaskStatus.Working;
			// avoid using foreach for multithreading 
			// cycle the scan line
			for (int scanlines = 0; scanlines < source.Count; scanlines++)
			{
				Sardauscan.Core.ScanLine oldLine = source[scanlines];
				// create a new line
				Sardauscan.Core.ScanLine newLine = new Sardauscan.Core.ScanLine(oldLine.LaserID,oldLine.Count);
				// copy the render type
				newLine.DisplayAsLine = oldLine.DisplayAsLine;
				// cycle the points
				for(int pointIndex = 0;pointIndex < oldLine.Count; pointIndex++)
				{
					Sardauscan.Core.Geometry.Point3D oldPoint = oldLine[pointIndex];
					// Add a new point3d to the new scanline
					newLine.Add( new Sardauscan.Core.Geometry.Point3D(oldPoint.Position,oldPoint.Normal,Color.Red));
				}
				// Add the ne scanline to the result
				returndata.Add(newLine);

				// update the viewer
				UpdatePercent((int)((100 * scanlines) / source.Count), returndata);
			}
			Status = eTaskStatus.Finished;
			return returndata;
		}

	}
}
