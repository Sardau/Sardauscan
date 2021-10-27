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
		protected Vector3d CameraPosition;
		public override ScanData DoTask(ScanData source)
		{
			CameraPosition = new Vector3d();
			Settings settings = Settings.Get<Settings>();
			CameraPosition.X = settings.Read(Settings.CAMERA, Settings.X, 0f);
			CameraPosition.Y = settings.Read(Settings.CAMERA, Settings.Y, 270f);
			CameraPosition.Z = settings.Read(Settings.CAMERA, Settings.Z, 70f);
			ScanData ret = base.DoTask(source);
			ret.Sort();
			return ret;
		}
		public override ScanLine DoTask(ScanLine source)
		{
			ScanLine ret = new ScanLine(source);
			return ret;
		}

		public override string Name
		{
			get { return "Sort"; }
		}
	}
}
