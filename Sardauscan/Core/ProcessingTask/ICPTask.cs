using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Sardauscan.Core.Geometry;
using OpenTK;

namespace Sardauscan.Core.ProcessingTask
{
#if DEBUG
	[Browsable(true)]
#else
	[Browsable(false)]
#endif
	public class ICPTask : AbstractProcessingTask
	{
		public override eTaskItem In {get {return eTaskItem.ScanLines;}}

		public override eTaskItem Out {get {return eTaskItem.ScanLines;}}
        public override eTaskType TaskType { get { return eTaskType.Transform; } }

		public override AbstractProcessingTask Clone()
		{
			ICPTask ret = (ICPTask)Activator.CreateInstance(this.GetType());
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
				ScanData data = laserScanData.ContainsKey(currentLine.LaserID) ? laserScanData[currentLine.LaserID] : new ScanData();
				data.Add(currentLine);
				laserScanData[currentLine.LaserID] = data;
			}
			if (laserScanData.Keys.Count >= 1)
			{
					List<ScanData> datas = new List<ScanData>(laserScanData.Values);
					int maxSamplePoints = 10000;
					for (int i = 0; i < datas.Count; i++)
						maxSamplePoints = Math.Min(maxSamplePoints, datas[i].PointCount());
					//ICP.ICP icp = new ICP.ICP(maxSamplePoints/2, 0.001f, 10);
					ICP.IterativeClosestPointTransform icp = new ICP.IterativeClosestPointTransform();
					icp.NumberOfIterations = 1000;
					icp.NumberOfStartTrialPoints = maxSamplePoints/2;
					ICP.IterativeClosestPointTransform.SimulatedAnnealing = false;
					ICP.IterativeClosestPointTransform.DistanceOptimization = true;

					Point3DList refpoints = CreateListFromScanData(datas[0]);
					for (int i = 1; i < datas.Count; i++)
					{
						Point3DList points = CreateListFromScanData(datas[i]);
						Matrix4d mat =  icp.PerformICP(refpoints, points);
						datas[i].Transform(mat);
						
						//icp.Run(points, refpoints);
						UpdatePercent((int)(100f*i/datas.Count), ret);
					}
			}

			UpdatePercent(100, ret);
			return ret;

		}

		Point3DList CreateListFromScanData(ScanData source)
		{
			Point3DList ret = new Point3DList();
			IqrFilter filter = new IqrFilter();
			filter.Factor = 0.5f;
			ScanData data = filter.Run(source);
			for (int i = 0; i < data.Count; i++)
				ret.AddRange(data[i]);
			return ret;
		}

		public override string Name
		{
			get { return "ICP correction"; }
		}
	}
}
