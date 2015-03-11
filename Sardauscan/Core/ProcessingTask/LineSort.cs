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
using OpenTK;
using Sardauscan.Core.Interface;
using System.ComponentModel;

namespace Sardauscan.Core.ProcessingTask
{
	/// <summary>
	/// Sort the Scanlines by angle
	/// </summary>
	[Browsable(false)]
	public class LineSort : AbstractLineTask
	{
		protected Vector3 CameraPosition;
		protected override ScanData DoTask(ScanData source)
		{
			CameraPosition = new Vector3();
			Settings settings = Settings.Get<Settings>();
			CameraPosition.X = settings.Read(Settings.CAMERA, Settings.X, 0f);
			CameraPosition.Y = settings.Read(Settings.CAMERA, Settings.Y, 270f);
			CameraPosition.Z = settings.Read(Settings.CAMERA, Settings.Z, 70f);
			ScanData ret = base.DoTask(source);
			ret.Sort();
			return ret;
		}
		protected override ScanLine DoTask(ScanLine source)
		{
			ScanLine ret = new ScanLine(source);
			return ret;
		}

		public override string Name
		{
			get { return "Angle Sort"; }
		}
	}
}
