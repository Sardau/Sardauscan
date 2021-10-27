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
using System.ComponentModel;

namespace Sardauscan.Core.ProcessingTask
{
	/// <summary>
	/// Change the rendering of ScanLines
	/// </summary>
    [Browsable(false)]
    public class DisplayAs : AbstractLineTask
	{
#if DEBUG
//		protected override bool LaunchParallel{get{return false;}}
#endif
		private bool m_DisplayAsLine = true;
		[Browsable(true)]
		[Description("Change the rendering to point (false) or line(true)")]
		[DisplayName("Display as lines")]
		public bool DisplayAsLine { get { return m_DisplayAsLine; } set { m_DisplayAsLine = value; } }

		public override ScanLine DoTask(ScanLine source)
		{
			ScanLine ret = new ScanLine(source);
			ret.DisplayAsLine = this.DisplayAsLine;
			return ret;
		}

		public override string Name		{			get { return "Display"; }	}
	}
}
