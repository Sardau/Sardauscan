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

            Init();
        }


        Control Control;
        sData Data;
        public PointF Angle {get{return Data.Angle;}}
        public PointF Pane {get{return Data.Pane;}}
        public double Zoom { get { return Data.Zoom; } }


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
            else if (ev.Button == System.Windows.Forms.MouseButtons.Right)
                StartDrag(eDragMode.Zoom, pt);
            else if (ev.Button == System.Windows.Forms.MouseButtons.Middle)
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
					if(IsDragging)
            Invalidate();

        }
        #endregion

        public void Copy(DragBallNavigator other)
        {
            Data = other.Data;
        }
    }
}
