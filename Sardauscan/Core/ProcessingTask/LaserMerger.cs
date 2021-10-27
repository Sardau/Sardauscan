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

namespace Sardauscan.Core.ProcessingTask
{
	/// <summary>
	/// Merge laser together by average
	/// </summary>
	public class LaserMerger : AbstractProcessingTask
	{

		public override string Name{get { return "Merge Lasers"; }}
        public override string DisplayName { get { return "Laser"; } }

		public override eTaskItem In {get { return eTaskItem.ScanLines; }}

		public override eTaskItem Out {get { return eTaskItem.ScanLines; }}
        public override eTaskType TaskType { get { return eTaskType.Smooth; } }
        /// <summary>
		/// Clone this
		/// </summary>
		/// <returns></returns>
		public override AbstractProcessingTask Clone()
		{
			LaserMerger ret = (LaserMerger)Activator.CreateInstance(this.GetType());
			return ret;
		}

		public override ScanData DoTask(ScanData source)
		{
			ScanData ret = source;
			Dictionary<int, ScanData> laserScanData = new Dictionary<int, ScanData>();
			UpdatePercent(0, ret);
			for (int i = 0; i < source.Count; i++)
			{
				ScanLine currentLine = source[i];
				ScanData data = laserScanData.ContainsKey(currentLine.LaserID)?laserScanData[currentLine.LaserID]: new ScanData();
				data.Add(currentLine);
				laserScanData[currentLine.LaserID] = data;
			}
			int masterLaserId = -1;
			foreach (int lId in laserScanData.Keys)
			{
				if (masterLaserId == -1)
					masterLaserId = lId;
				else if (laserScanData[masterLaserId].Count < laserScanData[lId].Count)
						masterLaserId = lId;
				laserScanData[lId].Sort();
			}
			ret = new ScanData();
			
			ScanData master = laserScanData[masterLaserId];
			double maxAngle = 180f / master.Count;
			for (int i = 0; i < master.Count; i++)
			{
				List<ScanLine> lines = new List<ScanLine>(laserScanData.Keys.Count);
				double currentAngle = master[i].Angle;
				int currentCount = master[i].Count;
				int biggestIndex = 0;
				lines.Add(master[i]);
				foreach (int key in laserScanData.Keys)
				{
					if (key != masterLaserId)
					{
						ScanLine currentLine = laserScanData[key].GetNearestLine(currentAngle);
						if (currentLine == null)
							continue;
						if (maxAngle > Math.Abs(Utils.DeltaAngle(currentAngle,currentLine.Angle)))
						{
							if (currentCount < currentLine.Count)
							{
								currentCount = currentLine.Count;
								biggestIndex = lines.Count;
							}
							lines.Add(currentLine);
						}
					}
				}
				ret.Add(AverageLines(currentCount,biggestIndex, lines));
			}
			UpdatePercent(100, ret);
			return ret;
		}
		protected ScanLine AverageLines(int count,int biggestIndex,List<ScanLine> scanlines)
		{
			int numLines = scanlines.Count;
			ScanLine ret = new ScanLine(-1, count);
			for (int i = 0; i < count; i++)
			{
				List<Point3D> list = new List<Point3D>(numLines);
				double refy = scanlines[biggestIndex][i].Position.Y;
				for (int line = 0; line < numLines; line++)
				{
					list.Add(scanlines[line].GetInterpolateByY(refy,i==0));
				}
				ret.Add(Point3D.Average(list));
			}
			return ret;
		}

	}
}
