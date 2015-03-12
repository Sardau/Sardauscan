#region COPYRIGHT
/****************************************************************************
 *  Copyright (c) 2015 Fabio Ferretti <https://plus.google.com/+FabioFerretti3D>                 *
 *  This file is part of Sardauscan.                                        *
 *                                                                          *
 *  Sardauscan is free software: you can redistribute it and/or modify      *
 *  it under the terms of the GNU General Public License as published by    *
 *  the Free Software Foundation, either version 3 of the License, or       *
 *  (at your option) any later version.                                     *
 *                                                                          *
 *  Sardauscan is distributed in the hope that it will be useful,           *
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of          *
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the           *
 *  GNU General Public License for more details.                            *
 *                                                                          *
 *  You are not allowed to Sell in any form this code                       * 
 *  or any compiled version. This code is free and for free purpose only    *
 *                                                                          *
 *  You should have received a copy of the GNU General Public License       *
 *  along with Sardaukar.  If not, see <http://www.gnu.org/licenses/>       *
 ****************************************************************************
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sardauscan.Core.Geometry;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Sardauscan.Core.ProcessingTask
{
	/// <summary>
	/// Abstract class for ScanLineTask
	/// </summary>
	public abstract class AbstractLineTask : AbstractProcessingTask
	{
		/// <summary>
		/// In is Scanlines
		/// </summary>
		public sealed override eTaskItem In
		{
			get { return eTaskItem.ScanLines; }
		}
		/// <summary>
		/// Out Is ScanLines
		/// </summary>
		public sealed override eTaskItem Out
		{
			get { return eTaskItem.ScanLines; }
		}
		/// <summary>
		/// Do the task
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public override ScanData DoTask(ScanData source)
		{
			ScanData ret = new ScanData(source.Count);
			source.Sort();
			int count = source.Count;
			int ParallelCount = Settings.Get<Settings>().Read(Settings.SYSTEM, Settings.MAXTHREADS, 8);
			UpdatePercent(0, ret);
			if (LaunchParallel)
			{
				int doneCount = 0;
				Parallel.For(0, count, new ParallelOptions { MaxDegreeOfParallelism = ParallelCount }, i =>
				{
					if (this.CancelPending) return;
					ScanLine line = DoTask(source[i]);
					if (line != null)
					{
						//line.DisplayAsLine = source[i].DisplayAsLine;
						lock (ret)
						{
							ret.Add(line);
						}
						doneCount++;
						{
							int percent = (int)((100 * doneCount) / count);
							if (percent % 10 == 0)
								UpdatePercent(percent, ret);
						}
					}

				}
							 );
			}
			else
			{

				for (int i = 0; i < count; i++)
				{
					if (this.CancelPending) return ret;
					ScanLine line = DoTask(source[i]);
					if (line != null)
						ret.Add(line);
					UpdatePercent((int)(100 * i / count), ret);
				}
			}
			UpdatePercent(100, ret);
			return ret;
		}


		/// <summary>
		/// Abstract function for work to do on a Scanline
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public abstract ScanLine DoTask(ScanLine source);
		/// <summary>
		/// Clone this task
		/// </summary>
		/// <returns></returns>
		public override AbstractProcessingTask Clone()
		{
			AbstractLineTask ret = (AbstractLineTask)Activator.CreateInstance(this.GetType());
			return ret;
		}


		/// <summary>
		/// Laucn the task in Parallel (threads / lines)
		/// </summary>
		protected virtual bool LaunchParallel { get { return true; } }

	}
}
