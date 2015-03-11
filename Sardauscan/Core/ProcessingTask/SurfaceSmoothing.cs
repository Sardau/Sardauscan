using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sardauscan.Core.Geometry;
using OpenTK;
using System.ComponentModel;
using Sardauscan.Gui.PropertyGridEditor;
using System.Drawing.Design;
using System.Threading.Tasks;

namespace Sardauscan.Core.ProcessingTask
{
	public class SurfaceSmoothing : AbstractProcessingTask 
	{
		public override eTaskItem In{	get { return eTaskItem.ScanLines; }}

		public override eTaskItem Out { get { return eTaskItem.ScanLines; } }

		public override string Name { get { return "Surface smooth"; } }

		public override AbstractProcessingTask Clone()
		{
			SurfaceSmoothing ret = (SurfaceSmoothing)Activator.CreateInstance(this.GetType());
			ret.Iteration = this.Iteration;
			return ret;
		}



		private int iteration = 1;
		[Browsable(true)]
		[Description("Number of iteration")]
		[DisplayName("Iteration")]
		[TypeConverter(typeof(NumericUpDownTypeConverter))]
		[Editor(typeof(NumericUpDownTypeEditor), typeof(UITypeEditor)), MinMaxAttribute(1, 10)]
		public int Iteration { get { return iteration; } set { iteration = value; } }


		protected override ScanData DoTask(ScanData source)
		{
			ScanData ret = new ScanData(source.Count);
			UpdatePercent(0, ret);
			source.Sort();
			int count = source.Count;
			/*
			for (int i = 1; i <= count; i++)
			{
				if (this.CancelPending) return source;
				ScanLine current = source[i % count];
				ScanLine prev = source[(i-1) % count];
				ScanLine next = source[(i+1) % count];
				int lineCount = current.Count;

				ScanLine smoothed = new ScanSlice(current.Count);
				smoothed.DisplayAsLine = current.DisplayAsLine;

				for (int l = 0; l < lineCount; l++)
				{
					smoothed.Add(Smooth(l, prev, current, next));
				}
				ret.Add(smoothed);
				UpdatePercent((int)((100 * i) / count), ret);
			}
			 */
			ret = source;
			for (int i = 0; i < Iteration; i++)
			{
				float pctstart = i * 100f/Iteration;
				float pctend = pctstart + 100f / Iteration;
				ret = DoIteration(ret, pctstart, pctend);
			}
			UpdatePercent(100, ret);
			return ret;
		}

		protected ScanData DoIteration(ScanData source, float pctMin, float pctMax)
		{
			ScanData ret = new ScanData(source.Count);
			ret.AddRange(source);
			int count = source.Count;
			int doneCount = 0;
			int ParallelCount = Settings.Get<Settings>().Read(Settings.SYSTEM, Settings.MAXTHREADS, 8);
			Parallel.For(1, count+1, new ParallelOptions { MaxDegreeOfParallelism = ParallelCount }, i =>
				{//for (int i = 1; i <= count; i++)
					{
						if (this.CancelPending) return;
						ScanLine current = source[i % count];
						ScanLine prev = source[(i - 1) % count];
						ScanLine next = source[(i + 1) % count];
						int lineCount = current.Count;

						ScanLine smoothed = new ScanLine(current.Count);
						smoothed.DisplayAsLine = current.DisplayAsLine;

						for (int l = 0; l < lineCount; l++)
						{
							smoothed.Add(Smooth(l, prev, current, next));
						}
						ret[i % count] = smoothed;
						doneCount++;
						int pct = (int)(pctMin + ((pctMax - pctMin) * doneCount) / count);
						UpdatePercent(pct, ret);
					}
				});
			return ret;
		}

		protected Point3D Smooth(int index, ScanLine prev, ScanLine current, ScanLine next)
		{
			//http://paulbourke.net/geometry/polygonmesh/
			Point3DList nearPoints = new Point3DList();
			if (index > 0)
			{
				Point3D prevP = current[index - 1];
				nearPoints.Add(prevP);
				nearPoints.Add(prev.GetInterpolateByY(prevP.Position.Y));
				nearPoints.Add(next.GetInterpolateByY(prevP.Position.Y));
			}
			Point3D pt = current[index];
			nearPoints.Add(pt);
			nearPoints.Add(prev.GetInterpolateByY(pt.Position.Y));
			nearPoints.Add(next.GetInterpolateByY(pt.Position.Y));

			if (index <current.Count-1)
			{
				Point3D nextP = current[index + 1];
				nearPoints.Add(nextP);
				nearPoints.Add(prev.GetInterpolateByY(nextP.Position.Y));
				nearPoints.Add(next.GetInterpolateByY(nextP.Position.Y));
			}


			Point3D ret = Smooth(current[index], nearPoints);
			return ret;
		}


		protected Point3D Smooth(Point3D p, IList<Point3D> nearPts)
		{
			if(nearPts.Count<1)
				return p;
			Vector3 pos = new Vector3(0, 0, 0);
			Vector3 norm = new Vector3(0, 0, 0);

			int count =nearPts.Count;
			for (int i = 0; i < count; i++)
			{
				pos = pos + (nearPts[i].Position - p.Position);
				norm = norm + (nearPts[i].Normal - p.Normal);
			}

			pos = p.Position + pos / count;
			norm = p.Normal + norm / count;
			norm.Normalize();

			return new Point3D(pos,norm,p.Color);
		}



	}
}
