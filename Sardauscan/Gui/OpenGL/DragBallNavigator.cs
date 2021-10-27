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
using System.Drawing;
using System.Windows.Forms;

namespace Sardauscan.Gui.OpenGL
{
	/// <summary>
	/// Drag ball for the OpenGL Viewer
	/// </summary>
	public class DragBallNavigator
	{
		public enum eDragMode
		{
			None,
			Rotation,
			Pane,
			Zoom
		};

		struct sData
		{
			public void Default()
			{
				StartPosition = new Point(0, 0);
				Angle = new Point(0, 0);
				_Angle = new PointF(0, 0);
				Pane = new PointF(0, 0);
				_Pane = new PointF(0, 0);
				RotationSpeed = 1;
				_Zoom = 1;
				Zoom = 1;
			}

			public Point StartPosition;
			public PointF Angle;
			public PointF _Angle;
			public PointF Pane;
			public PointF _Pane;
			public double RotationSpeed;
			public double _Zoom;
			public double Zoom;
		}

		public DragBallNavigator(Control ctrl)
		{
			Control = ctrl;
			ctrl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown);
			ctrl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMove);
			ctrl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUp);
			ctrl.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.MouseWheel);

			Init();
		}


		Control Control;
		sData Data;
		public PointF Angle { get { return Data.Angle; } }
		public PointF Pane { get { return Data.Pane; } }
		public double Zoom { get { return Data.Zoom; } }

		public void SetPane(PointF pane)
		{
			Data.Pane=pane;
			Data._Pane=pane;
		}
		public void SetAngleY(float value)
		{
			//Init();
			Data.Angle.Y = value;
			Data._Angle.Y = value;
			Data.Angle.X = 0;
			Data._Angle.X = 0;
		}

		public void Init()
		{
			Data.Default();
		}



		private eDragMode m_Mode = eDragMode.None;
		public bool IsDragging { get; protected set; }
		protected void StartDrag(eDragMode mode, Point pos)
		{
			m_Mode = mode;
			Data.StartPosition = new Point(pos.X, pos.Y);
		}
		protected void MoveDrag(Point pos)
		{
			if (m_Mode == eDragMode.Rotation)
			{
				Data.Angle.X = (float)(Data._Angle.X - (pos.X - Data.StartPosition.X) * Data.RotationSpeed);
				Data.Angle.Y = (float)(Data._Angle.Y - (pos.Y - Data.StartPosition.Y) * Data.RotationSpeed);
			}
			else if (m_Mode == eDragMode.Zoom)
			{
				Data.Zoom = (double)(Math.Max(0.00000001, Data._Zoom + (pos.Y - Data.StartPosition.Y) / 200.0f));
			}
			else if (m_Mode == eDragMode.Pane)
			{
				Data.Pane.X = Data._Pane.X + (pos.X - Data.StartPosition.X) / 10.0f;
				Data.Pane.Y = Data._Pane.Y + (pos.Y - Data.StartPosition.Y) / 10.0f;
			}

			IsDragging = m_Mode != eDragMode.None;

		}
		public void EndDrag()
		{
			if (m_Mode == eDragMode.Rotation)
			{
				Data._Angle.X = Data.Angle.X;
				Data._Angle.Y = Data.Angle.Y;
			}
			else if (m_Mode == eDragMode.Zoom)
			{
				Data._Zoom = Data.Zoom;
			}
			else if (m_Mode == eDragMode.Pane)
			{
				Data._Pane.X = Data.Pane.X;
				Data._Pane.Y = Data.Pane.Y;
			}
			m_Mode = eDragMode.None;
			IsDragging = false;
		}

		void Invalidate()
		{
			if (Control != null)
				Control.Invalidate();
		}
		#region event
		private void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			System.Windows.Forms.MouseEventArgs ev = (e as System.Windows.Forms.MouseEventArgs);
			Point pt = new Point(ev.X, ev.Y);
			if (ev.Button == MouseButtons.Left)
				StartDrag(eDragMode.Rotation, pt);
			else if (ev.Button == System.Windows.Forms.MouseButtons.Middle)
				StartDrag(eDragMode.Zoom, pt);
			else if (ev.Button == System.Windows.Forms.MouseButtons.Right)
				StartDrag(eDragMode.Pane, pt);
			else
			{
				EndDrag();
				Invalidate();
			}

		}

		private void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			System.Windows.Forms.MouseEventArgs ev = (e as System.Windows.Forms.MouseEventArgs);
			if (IsDragging)
			{
				EndDrag();
				Invalidate();
			}
			else if (ev.Button == MouseButtons.Left)
			{
				/*
						if (ShowFullOnClick)
						{
								ShowFull3dObjectForm dlg = new ShowFull3dObjectForm();
								dlg.View.Scene = this.Scene;
								dlg.ShowDialog();
						}
				*/
				EndDrag();
			}
		}

		private void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			System.Windows.Forms.MouseEventArgs ev = (e as System.Windows.Forms.MouseEventArgs);
			MoveDrag(new Point(ev.X, ev.Y));
			if (IsDragging)
				Invalidate();

		}
		public void MouseWheel(object sender, MouseEventArgs e)
		{
			if (e.Delta > 0)
				AddZoomFacor(0.95);
			else
				AddZoomFacor(1.05);
			Invalidate();
		}
		#endregion

		public void Copy(DragBallNavigator other)
		{
			Data = other.Data;
		}
		public void AddZoomFacor(double factor)
		{
			Data.Zoom*= factor;
			Data._Zoom *= factor;
			EndDrag();
		}
	}
}
