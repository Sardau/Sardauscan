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
using Sardauscan.Core.Interface;
using Sardauscan.Core;
using System.Drawing.Drawing2D;

namespace Sardauscan.Gui.Controls
{
	public partial class TurnTableControl : UserControl
	{
		/// <summary>
		/// Default ctor
		/// </summary>
		public TurnTableControl()
		{
			InitializeComponent();
			SetCursor();
		}
		/// <summary>
		/// Dispose object
		/// </summary>
		public new void Dispose()
		{
		}
		private ITurnTableProxy m_Proxy = null;
		public ITurnTableProxy Proxy
		{
			get { return m_Proxy; }
			set
			{
				m_Proxy = value;
				this.Redraw();
			}
		}

		public bool Interactive
		{
			get { return Enabled && Proxy != null; }
		}
		private void button_Click(object sender, EventArgs e)
		{
			if (sender is Control)
			{
				double val = double.Parse(((Control)sender).Tag.ToString());
				this.Visible = false;
				Settings.Get<ITurnTableProxy>().Rotate(val, true);
				this.Visible = true;
			}
		}

		private void TableFrom_Paint(object sender, PaintEventArgs e)
		{
			SetCursor();
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			Graphics g = e.Graphics;
			Rectangle rect = new Rectangle(0, 0, Width, Height);
			using (SolidBrush bg = new SolidBrush(SkinInfo.BackColor))
				g.FillRectangle(bg, rect);
			rect.Inflate(-5, -5);
			DrawTable(g);
		}

		int OffsetHeight = 20;
		Rectangle GetEllipseRect()
		{
			Rectangle rect = new Rectangle(0, 0, Width, Height);
			rect.Inflate(-5, -5);
			rect.Height -= OffsetHeight;
			return rect;
		}

		private void DrawTable(Graphics g)
		{
			Color normalColor = Interactive ? SkinInfo.ActiveTitleBackColor : SkinInfo.InactiveTitleBackColor;
			int alpha = 255;
			if (Rotating)
				normalColor = normalColor.GetStepColor(SkinInfo.ActiveTitleBackColor, 0.75);
			Color shadowColor = normalColor;
			normalColor = normalColor.Lighter();
			Rectangle rect = GetEllipseRect();
			Rectangle bottom_rect = new Rectangle(rect.X, rect.Y + OffsetHeight, rect.Width, rect.Height);
			using (SolidBrush side_brush = new SolidBrush(Color.FromArgb(alpha, shadowColor)))
			{
				g.FillPie(side_brush, bottom_rect, 0, 360);
				g.FillRectangle(side_brush, new Rectangle(rect.X, rect.Y + rect.Height / 2, rect.Width, OffsetHeight));
			}
			using (LinearGradientBrush fg = new LinearGradientBrush(rect, Color.FromArgb(alpha, normalColor.Lighter()), Color.FromArgb(alpha, shadowColor), LinearGradientMode.ForwardDiagonal))
			{
				FillPie(g, rect, fg, 0, 360);
			}
			using (Pen p = new Pen(shadowColor, 1))
			{
				for (int i = 0; i < 180; i += 60)
				{
					g.DrawLine(p, GetPerifericalPoint(rect, i), GetPerifericalPoint(rect, i + 180));
				}
			}

			String text = string.Empty;
			int deltadrag = DeltaDragAngle();
			if (deltadrag != 0)
			{
				int start = GetAngle(rect, _StartDragPos);
				int delta = deltadrag;
				int end = start + delta;
				using (SolidBrush dragBrush = new SolidBrush(normalColor.ModifyLuminosity(-0.2)))
				{

					Point p1 = GetPerifericalPoint(rect, MinMaxValue(start, -90, 90));
					Point p2 = GetPerifericalPoint(rect, MinMaxValue(end, -90, 90));
					Point[] p = new Point[4];

					p[0] = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
					p[1] = new Point(p1.X, p1.Y);
					p[2] = new Point(p1.X, p1.Y + OffsetHeight);
					p[3] = new Point(p[0].X, p[0].Y + OffsetHeight);
					g.FillPolygon(dragBrush, p);

					p[0] = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
					p[1] = new Point(p2.X, p2.Y);
					p[2] = new Point(p2.X, p2.Y + OffsetHeight);
					p[3] = new Point(p[0].X, p[0].Y + OffsetHeight);
					g.FillPolygon(dragBrush, p);

					FillPie(g, bottom_rect, dragBrush, start, delta);
				}

				using (SolidBrush dragBrush = new SolidBrush(normalColor.ModifyLuminosity(0.2)))
				{
					FillPie(g, rect, dragBrush, start, delta);
				}
				text = String.Format("{0}°", delta);
			}
			if (Rotating)
				text = "Working...";
			if (Interactive)
			{
				double modi = 0.25;
				int a = 128;
				using (LinearGradientBrush b = new LinearGradientBrush(rect, Color.FromArgb(a, normalColor.ModifyLuminosity(modi)), Color.FromArgb(a, shadowColor.ModifyLuminosity(-modi)), LinearGradientMode.ForwardDiagonal))
				{
					b.WrapMode = WrapMode.TileFlipXY;
					using (Pen p = new Pen(b, 2.5f))
					{
						for (int i = 0; i < 180; i += 60)
						{
							g.DrawLine(p, GetPerifericalPoint(rect, i), GetPerifericalPoint(rect, i + 180));
						}
						g.DrawEllipse(p, rect);
						g.DrawLine(p, rect.X, rect.Y + rect.Height / 2, bottom_rect.X, bottom_rect.Y + bottom_rect.Height / 2);
						g.DrawLine(p, rect.Right, rect.Y + rect.Height / 2, bottom_rect.Right, bottom_rect.Y + bottom_rect.Height / 2);
						g.DrawArc(p, bottom_rect, 0, 180);
					}
				}
			}
			if (!string.IsNullOrEmpty(text))
				using (SolidBrush bb = Rotating ? new SolidBrush(ForeColor.Darker()) : new SolidBrush(Color.Black))
				{
					Rectangle text_rect = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);

					StringFormat stringFormat = new StringFormat();
					stringFormat.Alignment = StringAlignment.Center;
					stringFormat.LineAlignment = StringAlignment.Center;
					g.DrawString(text, new Font(this.Font.FontFamily, (text_rect.Height * 2) / 3, FontStyle.Bold), bb, text_rect, stringFormat);
				}

		}

		protected void FillPie(Graphics g, Rectangle r, Brush b, int start, int delta)
		{

			if (Math.Abs(delta) >= 360)
			{
				g.FillPie(b, r, 0, 360);
				return;
			}

			int s_ang = start;
			int s_end = start + delta;
			if (delta < 0) { int tmp = s_ang; s_ang = s_end; s_end = tmp; }
			List<Point> pts = new List<Point>();
			pts.Add(new Point(r.X + r.Width / 2, r.Y + r.Height / 2));
			for (int ang = s_ang; ang <= s_end; ang += 1)
			{
				pts.Add(GetPerifericalPoint(r, ang));
			}
			pts.Add(new Point(r.X + r.Width / 2, r.Y + r.Height / 2));
			g.FillPolygon(b, pts.ToArray());
		}
		protected string GetDeltaDebugString(string pref, float start, float current)
		{
			return string.Format("{0}: {1}=>{2} ={3}", pref, start, current, current - start);
		}
		protected int GetAngle(Rectangle r, Point p)
		{
			int wdemi = r.Width / 2;
			int dx = p.X - r.X + wdemi;
			double rad_ang = Math.Cos(((double)dx) / wdemi);
			int ang = (int)(rad_ang * (360 / System.Math.PI));
			return ang + 45;
		}
		System.Drawing.Point GetPerifericalPoint(Rectangle r, double deg)
		{
			double radian = Math.PI * (deg / 180.0);
			int radiusX = r.Width / 2;
			int radiusY = r.Height / 2;
			System.Drawing.Point centerPoint = new Point(r.X + radiusX, r.Y + radiusY);
			int x = (int)(-radiusX * Math.Sin(radian));
			int y = (int)(radiusY * Math.Cos(radian));
			return new System.Drawing.Point(centerPoint.X + x, centerPoint.Y + y);
		}
		int MinMaxValue(int val, int min, int max)
		{
			return Math.Min(max, Math.Max(min, val));
		}

		private void TableFrom_MouseEnter(object sender, EventArgs e)
		{
		}

		#region
		private void Redraw()
		{
			Invalidate();
			Update();
		}
		private bool IsDrag = false;
		private Point _StartDragPos;
		private Point _CurrentDragPos;
		private void StartDrag(object sender, MouseEventArgs e)
		{
			if (!Interactive)
				return;
			IsDrag = true;
			if (sender != null && (e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				_StartDragPos = e.Location;
				_CurrentDragPos = e.Location;
			}
			SetCursor();
			Redraw();
		}
		private void UpdateDrag(object sender, MouseEventArgs e)
		{
			if (!Interactive)
				return;
			if (!IsDrag)
				return;
			if (sender != null && (e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				_CurrentDragPos = e.Location;

			}
			SetCursor();
			Redraw();
		}
		bool Rotating = false;
		private void EndDrag()
		{
			if (!Interactive)
				return;
			if (!IsDrag)
				return;
			int ang = DeltaDragAngle();
			IsDrag = false;
			Rotating = true;
			Redraw();
			SetCursor();
			if (ang != 0)
				Proxy.Rotate(ang, true);
			Rotating = false;
			Redraw();
		}

		protected int DeltaDragLimitAngle = 90;
		private int DeltaDragAngle()
		{
			if (!IsDrag)
				return 0;
			Rectangle rect = this.GetEllipseRect();
			int start = GetAngle(rect, _StartDragPos);
			int end = GetAngle(rect, _CurrentDragPos);
			int delta = end - start;
			return Math.Max(-DeltaDragLimitAngle, Math.Min(DeltaDragLimitAngle, delta));
		}
		protected void SetCursor()
		{

			if (IsDrag)
			{
				if (DeltaDragAngle() < 0)
					this.Cursor = System.Windows.Forms.Cursors.PanEast;
				else if (DeltaDragAngle() > 0)
					this.Cursor = System.Windows.Forms.Cursors.PanWest;
				else
					this.Cursor = System.Windows.Forms.Cursors.VSplit;
			}
			else
				this.Cursor = System.Windows.Forms.Cursors.VSplit;
		}

		private void TableFrom_MouseDown(object sender, MouseEventArgs e)
		{
			StartDrag(sender, e);
		}

		private void TableFrom_MouseMove(object sender, MouseEventArgs e)
		{
			UpdateDrag(sender, e);
		}

		private void TableFrom_MouseUp(object sender, MouseEventArgs e)
		{
			EndDrag();
		}

		private void TableFrom_MouseLeave(object sender, EventArgs e)
		{
			EndDrag();
		}
		#endregion

		private void TurnTableView_Resize(object sender, EventArgs e)
		{
			Invalidate();
		}
	}
}
