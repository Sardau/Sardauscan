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
		protected double RotationStep;

		protected Vector3d CameraLoc = new Vector3d();
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


		private double _Precision = 50f;

		[Browsable(true)]
		[Description("Acquire Precision")]
		[DisplayName("Precision")]
		[TypeConverter(typeof(NumericUpDownTypeConverter))]
		[Editor(typeof(NumericUpDownTypeEditor), typeof(UITypeEditor)), MinMaxAttribute(0f, 100f, 0.5f, 1)]
		public double Precision { get { return _Precision; } set { _Precision = value; } }

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
		public void PositionPostProcess(ref Point3DList list, double rotation)
		{
			// Build the 2D rotation matrix to rotate in the XZ plane
			double c = (double)Math.Cos(rotation);
			double s = (double)Math.Sin(rotation);
			double scale = 1f;

			for (int iPt = 0; iPt < list.Count; iPt++)
			{
				// Location
				Point3D p = list[iPt];
				double x = scale * (p.Position.X * c + p.Position.Z * -s);
				double y = scale * (p.Position.Y);
				double z = scale * (p.Position.X * s + p.Position.Z * c);


				// Normal
				double nx = p.Normal.X * c + p.Normal.Z * -s;
				double ny = p.Normal.Y;
				double nz = p.Normal.X * s + p.Normal.Z * c;


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



			RotationStep = (double)Math.Round(TurnTable.MinimumRotation() + (15f - TurnTable.MinimumRotation()) * ((100 - Precision) / 100f), 2);


			Settings settings = Settings.Get<Settings>();
			CameraLoc.X = settings.Read(Settings.CAMERA, Settings.X, 0f);
			CameraLoc.Y = settings.Read(Settings.CAMERA, Settings.Y, 270f);
			CameraLoc.Z = settings.Read(Settings.CAMERA, Settings.Z, 70f);

			double thres = settings.Read(Settings.LASER_COMMON, Settings.MAGNITUDE_THRESHOLD, 10);
			int min = settings.Read(Settings.LASER_COMMON, Settings.MIN_WIDTH, 1);
			int max = settings.Read(Settings.LASER_COMMON, Settings.MAX_WIDTH, 60);


			ICameraProxy camera = Settings.Get<ICameraProxy>();
			ImageProcessor = new ImageProcessor(thres, min, max);

			SizeF tableSize = new SizeF(
					(float)settings.Read(Settings.TABLE, Settings.DIAMETER, 20f),
                    (float)settings.Read(Settings.TABLE, Settings.HEIGHT, 15f)
					);

			Lasers = new List<LaserInfo>(LaserId.Length);
			for (int i = 0; i < LaserId.Length; i++)
			{
				Lasers.Add(new LaserInfo(LaserId[i], CameraLoc, tableSize));
			}

			ScanData ret = new ScanData();
			UpdatePercent(0, ret);
            int fadeTime = settings.Read(Settings.LASER_COMMON, Settings.FADE_DELAY, 100);

			TurnTable.InitialiseRotation();
			int laserCount = Lasers.Count;
			// Scan all laser location, 
			for (double currentAngle = 0; currentAngle < 360f; currentAngle += RotationStep)
			{
				if (this.CancelPending) return ret;

				Laser.TurnAll(false); // All laser off
                Thread.Sleep(fadeTime); // wait fade laser
                Bitmap imgoff = GetCapture();
				for (int laserIndex = 0; laserIndex < laserCount; laserIndex++)
				{
					Laser.Turn(Lasers[laserIndex].Id, true);
                    Thread.Sleep(fadeTime); // wait fade laser
					Bitmap imgon = GetCapture();
					Laser.Turn(Lasers[laserIndex].Id, false);

                    List<PointF> laserloc = ImageProcessor.Process(imgoff,imgon,null);

					Point3DList samplePoints = Lasers[laserIndex].MapPoints(laserloc, UseTexture ? imgoff : null, UseCorrectionMatrix);
					PositionPostProcess(ref samplePoints, -Utils.DEGREES_TO_RADIANS(currentAngle));
					ScanLine line = new ScanLine(laserIndex, samplePoints);
					line.DisplayAsLine = true;
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
        public override eTaskType TaskType { get { return eTaskType.Input; } }
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
