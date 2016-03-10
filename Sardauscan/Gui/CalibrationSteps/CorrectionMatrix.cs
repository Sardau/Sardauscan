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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sardauscan.Core;
using Sardauscan.Core.Interface;
using Sardauscan.Core.ProcessingTask;
using System.IO;
using Sardauscan.Core.IO;
using Sardauscan.Core.Geometry;
using OpenTK;
using Sardauscan.Gui.OpenGL;
using System.Reflection;

namespace Sardauscan.Gui.CalibrationSteps
{
	/// <summary>
	/// Laser Correction Matrix Construction Control
	/// </summary>
	public partial class CorrectionMatrix : UserControl, IDisposable
	{
		public class StepInfo : ICalibrationStepInfo
		{
			public int OrderId { get { return 400; } }

			public string Label { get { return "Correction matrix"; } }

			public Type ControlType { get { return typeof(CorrectionMatrix); } }
		}

		DragBallNavigator Drag;

		public CorrectionMatrix()
		{
			InitializeComponent();
			Application.Idle += OnIdle;
			this.ProgressBar.Visible = false;
			this.BackColor = SkinInfo.BackColor;
			this.ForeColor = SkinInfo.ForeColor;
			this.PreviewPanel.BackColor = SkinInfo.BackColor.GetStepColor(Color.White, 0.75);
#if DEBUG
			LoadButton.Visible = true;
#else
			LoadButton.Visible = false;
#endif
		}
		/// <summary>
		/// Dispose object
		/// </summary>
		public new void Dispose()
		{
			base.Dispose();
			Application.Idle -= OnIdle;
		}
		ILaserProxy _LastLaserProxy = null;
		ILaserProxy LaserProxy
		{
			get
			{
				return _LastLaserProxy;
			}
			set
			{
				bool reload = _LastLaserProxy != value;
				_LastLaserProxy = value;
				if (reload)
					LoadFromSettings();
				EnableLaser(value != null);

			}
		}
		DateTime lastCheckTime = DateTime.Now;
		private void OnIdle(object sender, EventArgs e)
		{
			try
			{
				bool ignore = false;
				if (!ignore && Visible)
				{
					DateTime now = DateTime.Now;
					bool expired = (now - lastCheckTime).TotalMilliseconds > 500;
					if (expired)
					{
						LaserProxy = Settings.Get<ILaserProxy>();
					}
				}
			}
			catch
			{
			}
		}

		int CurrentLaserIndex = -1;
		bool loading = false;
		void LoadFromSettings()
		{
			loading = true;
			Settings settings = Settings.Get<Settings>();
			if (settings != null)
			{
				this.Enabled = true;
				ILaserProxy laser = Settings.Get<ILaserProxy>();
				if (laser != null)
				{
					int currentLaser = CurrentLaserIndex;
					int lasercount = laser.Count;
					this.LaserComboBox.Items.Clear();
					for (int i = 0; i < lasercount; i++)
					{
						int index = this.LaserComboBox.Items.Add(String.Format("Laser {0}", i));
					}
					this.LaserComboBox.SelectedIndex = Math.Max(0, currentLaser);
					CurrentLaserIndex=this.LaserComboBox.Items.Count == 0 ? -1 : this.LaserComboBox.SelectedIndex;
				}
			}
			else
				this.Enabled = false;


			loading = false;
		}
		void SaveToSettings()
		{
			Settings settings = Settings.Get<Settings>();
			if (settings != null)
			{
			}
		}
		Color GetLaserColor(int laserIndex)
		{
			Settings settings = Settings.Get<Settings>();
			if (settings != null && laserIndex >= 0)
				return settings.Read(Settings.LASER(laserIndex), Settings.DEFAULTCOLOR, LaserInfo.GetDefaultColor(laserIndex));
			else
				return LaserInfo.GetDefaultColor(laserIndex);
		}
        public void DrawGrid(Graphics g, PointF translation,float scale)
        {
            Settings set = Settings.Get<Settings>();
            float r = (float)(set.Read(Settings.TABLE, Settings.DIAMETER, 20f)*scale);

            Color color = ForeColor.GetStepColor(BackColor, 0.5);
            using (Pen p = new Pen(color, 2))
            {
                g.DrawLine(p, new PointF(translation.X, translation.Y - r), new PointF(translation.X, translation.Y + r));
                g.DrawLine(p, new PointF(translation.X - r, translation.Y), new PointF(translation.X + r, translation.Y));
            }

            using (Pen p = new Pen(color, 1))
            {
                for (float x = 10 * scale; x < r; x += 10 * scale)
                {
                    g.DrawLine(p, new PointF(translation.X - x, translation.Y - r), new PointF(translation.X - x, translation.Y + r));
                    g.DrawLine(p, new PointF(translation.X + x, translation.Y - r), new PointF(translation.X + x, translation.Y + r));

                    g.DrawLine(p, new PointF(translation.X - r, translation.Y - x), new PointF(translation.X + r, translation.Y - x));
                    g.DrawLine(p, new PointF(translation.X - r, translation.Y + x), new PointF(translation.X + r, translation.Y + x));

                }
            }
        }
		private void PreviewPanel_Paint(object sender, PaintEventArgs e)
		{
			//Graphics g = e.Graphics;
			using (Bitmap bmp = new Bitmap(PreviewPanel.Width, PreviewPanel.Height))
			{
				using (Graphics g = Graphics.FromImage(bmp))
				{
					g.Clip = new Region();
					using (SolidBrush b = new SolidBrush(PreviewPanel.BackColor))
						g.FillRectangle(b, 0, 0, PreviewPanel.Width, PreviewPanel.Height);

					if (ScanInfo != null)
					{
						try
						{
							ScanLine line=null;
							LaserCorrection corr = null;
							double size = ScanInfo.Size() * 0.66f;
							double factor = Math.Min(PreviewPanel.Width, PreviewPanel.Height) / size;
							Matrix4d baseMatrix = Matrix4d.Scale(factor);
							PointF center = new PointF(PreviewPanel.Width / 2, PreviewPanel.Height / 2);
                            DrawGrid(g, center, (float)factor);
							int currentLaser = CurrentLaserIndex;
							List<ScanLine> selectedLaserLine = new List<ScanLine>();
							for (int i = 0; i < ScanInfo.Count; i++)
							{
								line = ScanInfo[i];

								corr = new LaserCorrection();
								corr.LoadFromSettings(line.LaserID);
								bool selected = currentLaser == line.LaserID;
                                if (!selected)
                                {
                                    DrawScanLine(g, center, factor, line, corr, false);
                                }
                                else
                                    selectedLaserLine.Add(line);
							}

                            corr = new LaserCorrection();
                            corr.LoadFromSettings(selectedLaserLine[Math.Max(0,currentLaser)].LaserID);
                            corr.Apply(Drag);
                            for (int i = 0; i < selectedLaserLine.Count; i++)
							{
                                DrawScanLine(g, center, factor,selectedLaserLine[i], corr, true);
							}

						}
						catch
						{
						}
					}
				}
				e.Graphics.Clip = new Region();
				e.Graphics.DrawImageUnscaled(bmp, new Point(0, 0));
			}
		}
		protected void DrawScanLine(Graphics g, PointF translation, double scale, ScanLine line, LaserCorrection corr,bool selected)
		{
			List<PointF> points = new List<PointF>(line.Count);
			Matrix4d m = corr.GetMatrix();
			int laserid = line.LaserID;
            int count = line.Count;
            Color baseCol = GetLaserColor(laserid);
            Color col = Color.FromArgb(selected ? 128 / NumClass : 64 / NumClass, baseCol);
            for (int i = 0; i < line.Count; i++)
			{
				Point3D p = line[i];
				Vector3d v = Vector3d.Transform(p.Position, m);
                PointF pt = new PointF((float)(v.X * scale), (float)(v.Z * scale));
				pt.X += translation.X;
				pt.Y += translation.Y;
				points.Add(pt);
            }
            using (SolidBrush b = new SolidBrush(col))
                g.FillPolygon(b, points.ToArray());
            using (Pen b = new Pen(Color.FromArgb(selected ? 128 : 64 , baseCol)))
                g.DrawPolygon(b, points.ToArray());
        }

		private void QuickScanButton_Click(object sender, EventArgs e)
		{
			bool Async = true;
			if (Async)
			{
				if (this.BackgroundWorker.IsBusy == true)
				{
					if (this.BackgroundWorker.WorkerSupportsCancellation == true)
					{
						this.BackgroundWorker.CancelAsync();
					}
					EndScanning();
				}
				else
				{
					this.BackgroundWorker.RunWorkerAsync();
					StartScanning();
				}

			}
			else
			{
				QuickScanButton.Enabled = false;
				StartScanning();
				BackgroundWorker_DoWork(null, null);
				EndScanning();
				QuickScanButton.Enabled = true;
			}
		}

		protected void StartScanning()
		{
			EnableLaser(false);
			this.ProgressBar.Value = 0;
			this.ProgressBar.Visible = true;
		}
		protected void EndScanning()
		{
			EnableLaser(true);
			this.ProgressBar.Visible = false;
		}
		protected void UpdateScanning()
		{
		}

		public void EnableLaser(bool enable)
		{
			LaserCommandPanel.Enabled = enable;
		}


		private void LaserComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.CommitCorrection();
			CurrentLaserIndex = this.LaserComboBox.Items.Count == 0 ? -1 : this.LaserComboBox.SelectedIndex;
			if (loading)
				return;
			LoadFromSettings();
			PreviewPanel.Invalidate();
		}

		private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			Process proc = new Process();
			ScanTask scan = new ScanTask();
			scan.Precision = Settings.Get<Settings>().Read(Settings.SCANNER, Settings.CALIBRATIONPRECISION, 5);
			scan.UseTexture = false;
			scan.UseCorrectionMatrix = false;
			scan.FileName = QuickFileName;
			ScanData data = scan.Run(null, this, sender == null ? null : BackgroundWorker, e, UpdateScanEvent);
			if (sender != null)
				BackgroundWorker.ReportProgress(100, data);
			else
				ProcessScanData(data);
		}
		void UpdateScanEvent(object sender, ProgressChangedEventArgs e)
		{
			BackgroundWorker.ReportProgress(e.ProgressPercentage,e.UserState);
			ProcessScanData(e.UserState as ScanData);
		}
		private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			this.ProgressBar.Value = e.ProgressPercentage;
			UpdateScanning();

		}

		private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{

			EndScanning();
			if(!e.Cancelled && e.Error==null)
				ProcessScanData(e.Result as ScanData);
		}
        public int NumClass = 10;
		protected void ProcessScanData(ScanData data)
		{
			if (data != null)
			{
				IqrFilter task1 = new IqrFilter();
				task1.Factor = 0.1f;
				ScanData step1 = task1.Run(data);
				CalibrationTask task = new CalibrationTask();
                task.NumClass=NumClass;
				ScanInfo = task.Run(step1);
			}
			this.PreviewPanel.Invalidate();
		}

		ScanData ScanInfo;
		protected string QuickFileName { get { return Path.Combine(Program.UserDataPath, "CalibrationScan" + ScanDataIO.DefaultExtention); } }

		private void CorrectionMatrix_Load(object sender, EventArgs e)
		{
			try
			{
				if (File.Exists(QuickFileName))
					ProcessScanData(ScanDataIO.Read(QuickFileName));
				EnableLaser(true);
			}
			catch
			{
				EnableLaser(false);
			}
			Drag = new DragBallNavigator(PreviewPanel);
			Drag.Init();
			this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.Selectable	, true);
			typeof(Panel).InvokeMember("DoubleBuffered",
		BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
		null, PreviewPanel, new object[] { true });
			PreviewPanel.TabStop = true;
			this.MouseWheel += Drag.MouseWheel;


		}

		private void PreviewPanel_SizeChanged(object sender, EventArgs e)
		{
			PreviewPanel.Invalidate();
		}

		private void LaserComboBox_DrawItem(object sender, DrawItemEventArgs e)
		{

			Color col = this.GetLaserColor(e.Index);
			using (SolidBrush b = new SolidBrush(col))
			{
				if (e.Index >= 0)
				{
					e.DrawBackground();
					int sqareSize = e.Bounds.Height - 4;
					e.Graphics.FillRectangle(b, new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2, sqareSize, sqareSize));
					using (Pen p = new Pen(SkinInfo.ForeColor))
						e.Graphics.DrawRectangle(p, new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2, sqareSize, sqareSize));
					Rectangle textRect = new Rectangle(e.Bounds.Left + e.Bounds.Height, e.Bounds.Top, e.Bounds.Width - e.Bounds.Height, e.Bounds.Height);
					using (SolidBrush textB = new SolidBrush(SkinInfo.ForeColor))
					{
						e.Graphics.DrawString(LaserComboBox.Items[e.Index].ToString(), this.Font, textB, textRect);
					}
				}

			}
		}

		#region correction load/save
		void CommitCorrection()
		{
			if(CurrentLaserIndex>=0)
			{
				LaserCorrection cor = new LaserCorrection();
				cor.LoadFromSettings(CurrentLaserIndex);
				cor.Apply(Drag);
				cor.SaveToSettings(CurrentLaserIndex);
				Drag.Init();
			}
		}
		void ClearCurrentLaserCorrection()
		{
			if (CurrentLaserIndex >= 0)
				ClearCorrection(CurrentLaserIndex);
		}
		void ClearCorrection(int index)
		{
			LaserCorrection cor = new LaserCorrection();
			cor.SaveToSettings(index);
		}
		void ClearCorrection()
		{
			Settings set = Settings.Get<Settings>();
			if (LaserProxy != null)
			{
				int count = LaserProxy.Count;
				for (int i = 0; i < count; i++)
					ClearCorrection(i);
			}
		}
		#endregion

			private void imageButton2_Click(object sender, EventArgs e)
		{
			Drag.Init();
			PreviewPanel.Invalidate();
		}

		private void imageButton1_Click(object sender, EventArgs e)
		{
			ClearCurrentLaserCorrection();
			PreviewPanel.Invalidate();
		}
		private void ReInitButton_Click(object sender, EventArgs e)
		{
			ClearCorrection();
			PreviewPanel.Invalidate();
		}

		private void PreviewPanel_MouseUp(object sender, MouseEventArgs e)
		{
			Drag.EndDrag();
			CommitCorrection();
			Drag.Init();
			PreviewPanel.Invalidate();
		}

		private void LoadButton_Click(object sender, EventArgs e)
		{
			if (DialogResult.OK == MessageBox.Show(this, "Load only Scan with Correstion matrix off", "Attention", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
			{
				LineOpenFileTask task = new LineOpenFileTask();
				task.Filename = string.Empty;
				ProcessScanData(task.Run(null));
			}
		}
	}
}
