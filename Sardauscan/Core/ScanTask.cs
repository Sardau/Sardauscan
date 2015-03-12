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
using Sardauscan.Core.ProcessingTask;
using OpenTK;
using Sardauscan.Core.Interface;
using System.Drawing;
using System.ComponentModel;
using Sardauscan.Gui.PropertyGridEditor;
using System.Drawing.Design;
using Sardauscan.Core;
using System.Threading;
using Sardauscan.Core.Geometry;
using System.Windows.Forms;
using System.IO;
using Sardauscan.Core.IO;

namespace Sardauscan.Core
{
	/// <summary>
	/// Scan Task
	/// </summary>
	public class ScanTask : AbstractProcessingTask
	{
		protected int LaserCount { get { return Lasers.Count; } }
		protected List<LaserInfo> Lasers;
		protected float RotationStep;

		protected Vector3 CameraLoc = new Vector3();
		protected ImageProcessor ImageProcessor;
		protected bool _UseTexture = true;
		protected bool _UseCorrectionMatrix = true;

		private short[] _LaserId = new short[] { 0, 1, 2, 3 };
		[Description("Laser Selection [0,Num laser for yout configuration]")]
		[DisplayName("Laser Selection")]
		public short[] LaserId 
		{ 
			get { return _LaserId; } 
			set {
				List<short> newOne = new List<short>();
				if(value!=null)
				{
					for(int i=0;i<value.Length;i++)
					{
						if (!newOne.Contains(value[i]))
							newOne.Add(value[i]);
					}
				}
				_LaserId = newOne.ToArray(); 
			} 
		}


		[Browsable(true)]
		[Description("Acquire the Texture")]
		[DisplayName("Use Texture")]
		public bool UseTexture { get { return _UseTexture; } set { _UseTexture = value; } }
		[Browsable(true)]
		[Description("Use the calibration Correction matrix")]
		[DisplayName("Use Correction matrix")]
		public bool UseCorrectionMatrix { get { return _UseCorrectionMatrix; } set { _UseCorrectionMatrix = value; } }


		private float _Precision = 50f;

		[Browsable(true)]
		[Description("Acquire Precision")]
		[DisplayName("Precision")]
		[TypeConverter(typeof(NumericUpDownTypeConverter))]
		[Editor(typeof(NumericUpDownTypeEditor), typeof(UITypeEditor)), MinMaxAttribute(0f, 100f, 0.5f, 1)]
		public float Precision { get { return _Precision; } set { _Precision = value; } }

		private string _FileName = "last" + ScanDataIO.DefaultExtention;
		[Browsable(true)]
		[Description("Save result points to file")]
		[DisplayName("FileName")]
		public String FileName { get { return _FileName; } set { _FileName = value; } }


		#region Proxy
		ITurnTableProxy TurnTable
		{
			get { return Settings.Get<ITurnTableProxy>(); }
		}
		ILaserProxy Laser
		{
			get { return Settings.Get<ILaserProxy>(); }
		}
		ICameraProxy Camera
		{
			get { return Settings.Get<ICameraProxy>(); }
		}

		bool HardwareAvailable
		{
			get
			{
				return TurnTable != null && Laser != null && Camera != null; ;
			}
		}

		#endregion
		/// <summary>
		/// Clone this
		/// </summary>
		/// <returns></returns>
		public override AbstractProcessingTask Clone()
		{
			ScanTask ret = new ScanTask();
			ret.Precision = Precision;
			ret.UseTexture = UseTexture;
			ret.UseCorrectionMatrix = UseCorrectionMatrix;
			ret.LaserId = (new List<short>(LaserId)).ToArray();
			return ret;
		}

		/// <summary>
		/// Default ctor
		/// </summary>
		public ScanTask()
		{
		}


		private string HardwarePresentTrace(object obj)
		{
			return obj == null ? "FAILED" : "OK";
		}
		protected Bitmap GetCapture()
		{
			Bitmap img = null;


			if (CallerControl != null)
			{
				if (CallerControl.InvokeRequired)
					CallerControl.Invoke(new Action(() => img = Camera.AcquireImage()));
				else
					img = Camera.AcquireImage();
			}
			return img;
		}
		public void PositionPostProcess(ref Point3DList list, float rotation)
		{
			// Build the 2D rotation matrix to rotate in the XZ plane
			float c = (float)Math.Cos(rotation);
			float s = (float)Math.Sin(rotation);
			float scale = 1f;

			for (int iPt = 0; iPt < list.Count; iPt++)
			{
				// Location
				Point3D p = list[iPt];
				float x = scale * (p.Position.X * c + p.Position.Z * -s);
				float y = scale * (p.Position.Y);
				float z = scale * (p.Position.X * s + p.Position.Z * c);


				// Normal
				float nx = p.Normal.X * c + p.Normal.Z * -s;
				float ny = p.Normal.Y;
				float nz = p.Normal.X * s + p.Normal.Z * c;


				p.Position.X = x;
				p.Position.Y = y;
				p.Position.Z = z;

				p.Normal.X = nx;
				p.Normal.Y = ny;
				p.Normal.Z = nz;

				list[iPt] = p;
			}
		}
		public override ScanData DoTask(ScanData source)
		{
			if (!HardwareAvailable)
				throw new Exception(string.Format("HardWare missing : TURNTABLE:{0} LASER:{1} CAMERA:{2}", HardwarePresentTrace(TurnTable), HardwarePresentTrace(Laser), HardwarePresentTrace(Camera)));



			RotationStep = (float)Math.Round(TurnTable.MinimumRotation() + (15f - TurnTable.MinimumRotation()) * ((100 - Precision) / 100f), 2);


			Settings settings = Settings.Get<Settings>();
			CameraLoc.X = settings.Read(Settings.CAMERA, Settings.X, 0f);
			CameraLoc.Y = settings.Read(Settings.CAMERA, Settings.Y, 270f);
			CameraLoc.Z = settings.Read(Settings.CAMERA, Settings.Z, 70f);

			float thres = settings.Read(Settings.LASER_COMMON, Settings.MAGNITUDE_THRESHOLD, 10);
			int min = settings.Read(Settings.LASER_COMMON, Settings.MIN_WIDTH, 1);
			int max = settings.Read(Settings.LASER_COMMON, Settings.MAX_WIDTH, 60);


			ICameraProxy camera = Settings.Get<ICameraProxy>();
			ImageProcessor = new ImageProcessor(thres, min, max);

			SizeF tableSize = new SizeF(
					settings.Read(Settings.TABLE, Settings.DIAMETER, 20f),
					settings.Read(Settings.TABLE, Settings.HEIGHT, 15f)
					);

			Lasers = new List<LaserInfo>(LaserId.Length);
			for (int i = 0; i < LaserId.Length; i++)
			{
				Lasers.Add(new LaserInfo(LaserId[i], CameraLoc, tableSize));
			}

			ScanData ret = new ScanData();
			UpdatePercent(0, ret);

			TurnTable.InitialiseRotation();
			int laserCount = Lasers.Count;
			int numbadLaserLocation = 0;
			int numImageProcessingRetries = 0;
			// Scan all laser location, 
			for (float currentAngle = 0; currentAngle < 360f; currentAngle += RotationStep)
			{
				if (this.CancelPending) return ret;

				Laser.TurnAll(false); // All laser off
				Bitmap imgoff = GetCapture();
				for (int laserIndex = 0; laserIndex < laserCount; laserIndex++)
				{
					Laser.Turn(Lasers[laserIndex].Id, true);
					Thread.Sleep(100); // wait fade laser
					Bitmap imgon = GetCapture();
					Laser.Turn(Lasers[laserIndex].Id, false);

					List<PixelLocation> laserloc = ImageProcessor.Process(imgoff,
																													imgon,
																													null,
																													ref Lasers[laserIndex].FirstRowLaserCol,
																													ref numbadLaserLocation,
																													ref numImageProcessingRetries);

					Point3DList samplePoints = Lasers[laserIndex].MapPoints(laserloc, UseTexture ? imgoff : null, UseCorrectionMatrix);
					PositionPostProcess(ref samplePoints, -Utils.DEGREES_TO_RADIANS(currentAngle));
					ScanLine line = new ScanLine(laserIndex, samplePoints);
					line.DisplayAsLine = false;
					ret.Add(line);
				}
				int percent = (int)((currentAngle / 360f) * 100f);
				UpdatePercent(percent, ret);
				TurnTable.Rotate(currentAngle, false);
			}
			LineSort lineSort = new LineSort();
			ret = lineSort.Run(ret, CallerControl, this.Worker, this.WorkerArg);
			if (!string.IsNullOrEmpty(FileName))
			{
				string path = Path.Combine(Program.UserDataPath, FileName);
				ScanDataIO.Write(path, ret);
			}
			return ret;
		}
		public override void UpdatePercent(int percent, ScanData data)
		{
			base.UpdatePercent(percent, data);
			IScene3DViewer viewer = Settings.Get<IScene3DViewer>();
			if (viewer != null)
			{
				Control ctl = viewer as Control;
				if (ctl == null || !ctl.InvokeRequired)
				{
					viewer.Scene.Clear();
					if (data != null)
						viewer.Scene.Add(data);
					viewer.Invalidate();
				}
				else
					ctl.BeginInvoke(new Action(() =>
					{
						viewer.Scene.Clear();
						if (data != null)
							viewer.Scene.Add(data);
						viewer.Invalidate();
					}));
			}
		}
		#region Overrides
		public override eTaskItem In { get { return eTaskItem.None; } }
		public override eTaskItem Out { get { return eTaskItem.ScanLines; } }
		/// <summary>
		/// Name
		/// </summary>
		public override string Name { get { return "Scan"; } }
		public override bool Ready
		{
			get
			{
				return this.HardwareAvailable;
			}
		}
		public override string ToolTip
		{
			get
			{
				if (!Ready)
				{
					return string.Format("HardWare missing : TURNTABLE:{0} LASER:{1} CAMERA:{2}", HardwarePresentTrace(TurnTable), HardwarePresentTrace(Laser), HardwarePresentTrace(Camera));
				}
				return base.ToolTip;
			}
		}
		#endregion
	}
}
