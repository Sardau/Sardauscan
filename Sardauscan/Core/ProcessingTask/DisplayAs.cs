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
using System.ComponentModel;

namespace Sardauscan.Core.ProcessingTask
{
	/// <summary>
	/// Change the rendering of ScanLines
	/// </summary>
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
