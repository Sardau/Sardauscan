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
using System.Windows.Forms;
using System.Drawing;
using Sardauscan.Core.ProcessingTask;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using Sardauscan.Core;

namespace Sardauscan.Gui.Controls
{
	/// <summary>
	/// Task list box
	/// </summary>
	public class TaskListBox : System.Windows.Forms.ListBox
	{
		#region Drag DATA
		public class DragData
		{
			public DragData(TaskListBox source, int index)
			{
				Source = source;
				Index = index;
				Task = source.Get(index);
			}
			public readonly TaskListBox Source;
			public readonly int Index;
			public readonly AbstractProcessingTask Task;
		}


		public object GetDragData(int index)
		{
			return new DragData(this, index);
		}
		public static bool IsValidDragData(IDataObject data)
		{
			if (data == null)
				return false;
			return data.GetDataPresent(typeof(DragData));
		}
		public static TaskListBox DragDataSource(IDataObject data)
		{
			if (data == null)
				return null;
			return ((DragData)data.GetData(typeof(DragData))).Source;
		}
		public static AbstractProcessingTask DragDataTask(IDataObject data)
		{
			if (data == null)
				return null;
			return ((DragData)data.GetData(typeof(DragData))).Task;
		}
		public static int DragDataIndex(IDataObject data)
		{
			if (data == null)
				return ListBox.NoMatches;
			return ((DragData)data.GetData(typeof(DragData))).Index;
		}
		#endregion


		public bool _Lock = false;
		public bool Lock { get { return _Lock; } set { _Lock = value; Invalidate(); } }

		public TaskListBox()
		{
			this.SetStyle(
					ControlStyles.OptimizedDoubleBuffer |
					ControlStyles.ResizeRedraw |
					ControlStyles.UserPaint,
					true);
			this.DrawMode = DrawMode.OwnerDrawFixed;
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			Region iRegion = new Region(e.ClipRectangle);
			e.Graphics.FillRegion(new SolidBrush(SkinInfo.BackColor), iRegion);
			if (this.Items.Count > 0)
			{
				for (int i = 0; i < this.Items.Count; ++i)
				{
					System.Drawing.Rectangle irect = this.GetItemRectangle(i);
					if (e.ClipRectangle.IntersectsWith(irect))
					{
						if ((this.SelectionMode == SelectionMode.One && this.SelectedIndex == i)
						|| (this.SelectionMode == SelectionMode.MultiSimple && this.SelectedIndices.Contains(i))
						|| (this.SelectionMode == SelectionMode.MultiExtended && this.SelectedIndices.Contains(i)))
						{
							OnDrawItem(new DrawItemEventArgs(e.Graphics, this.Font,
									irect, i,
									DrawItemState.Selected, SkinInfo.ForeColor,
									SkinInfo.BackColor));
						}
						else
						{
							OnDrawItem(new DrawItemEventArgs(e.Graphics, this.Font,
									irect, i,
									DrawItemState.Default, SkinInfo.ForeColor,
									SkinInfo.BackColor));
						}
						iRegion.Complement(irect);
					}
				}
			}
			base.OnPaint(e);
		}

		public void Clear()
		{
			m_SourceTaskEnableStatus.Clear();
			Items.Clear();
		}
		public bool IsValidInsertionIndex(int index, AbstractProcessingTask newTask, eTaskItem emptyTaskItem)
		{
			if (newTask == null || index == -1)
				return false;
			AbstractProcessingTask prev = Get(index - 1);
			AbstractProcessingTask next = Get(index);
			if (prev == null && next == null)
				return newTask.In == emptyTaskItem;
			if (prev == null)
				return newTask.In == emptyTaskItem && newTask.Out == next.In;
			else if (next == null)
				return newTask.In == prev.Out;
			return newTask.In == prev.Out && newTask.Out == next.In;
		}
		public bool RemovableForTaskFlow(int index)
		{
			AbstractProcessingTask prev = Get(index - 1);
			AbstractProcessingTask next = Get(index + 1);
			if (next == null)
				return true;

			if (prev == null)
				return next.In == eTaskItem.None;
			return next.CanFolow(prev);
		}
		private List<AbstractProcessingTask> AllTask = new List<AbstractProcessingTask>();
		public bool Add(AbstractProcessingTask task)
		{
			if (task != null)
			{
				if (IsValidInsertionIndex(Count, task, eTaskItem.None))
				{
					SuspendLayout();
					Items.Add(task);
					ResumeLayout();
					return true;
				}
			}
			return false;
		}
		public void SetStartupTask(List<AbstractProcessingTask> tasks)
		{
			AllTask = new List<AbstractProcessingTask>(tasks);
			Clear();
			foreach (AbstractProcessingTask task in tasks)
				Items.Add(task);
		}
		public void Insert(int index, AbstractProcessingTask task)
		{
			SuspendLayout();
			if (index < 0 || index >= this.Items.Count)
				Items.Add(task);
			else
				Items.Insert(index, task);
			ResumeLayout();
		}
		public AbstractProcessingTask Get(int index)
		{
			if (index < 0 || index >= Items.Count)
				return null;
			return (AbstractProcessingTask)Items[index];
		}
		protected AbstractProcessingTask Change(AbstractProcessingTask oldTask, AbstractProcessingTask newTask)
		{
			bool enabled = GetTaskEnabled(oldTask);
			m_SourceTaskEnableStatus.Remove(oldTask);
			SetTaskEnabled(newTask, enabled);
			return newTask;
		}
		public void Remove(AbstractProcessingTask task)
		{
			SuspendLayout();
			if (task == null)
				return;
			m_SourceTaskEnableStatus.Remove(task);
			int index = Items.IndexOf(task);
			Items.RemoveAt(index);
			ResumeLayout();
		}
		public void RemoveAt(int index)
		{
			AbstractProcessingTask task = Get(index);
			Remove(task);
		}
		public int Count { get { return Items.Count; } }

		#region Enabling
		private Dictionary<AbstractProcessingTask, bool> m_SourceTaskEnableStatus = new Dictionary<AbstractProcessingTask, bool>();
		protected bool GetTaskEnabled(AbstractProcessingTask task)
		{
			if (!m_SourceTaskEnableStatus.ContainsKey(task))
				m_SourceTaskEnableStatus.Add(task, true);
			return m_SourceTaskEnableStatus[task];
		}
		protected void SetTaskEnabled(AbstractProcessingTask task, bool enable)
		{
			if (m_SourceTaskEnableStatus.ContainsKey(task))
				m_SourceTaskEnableStatus[task] = enable;
			else
				m_SourceTaskEnableStatus.Add(task, enable);
		}
		bool lastOnNotReady = false;
		public void AlignTaskEnable(TaskListBox destListBox)
		{
			List<AbstractProcessingTask> alreadySelectedTask = new List<AbstractProcessingTask>();
			foreach (AbstractProcessingTask task in destListBox.Items)
				alreadySelectedTask.Add(task);

			bool somethingchange = false;
			bool oneNotReady=false;
			List<AbstractProcessingTask> candidateTask = new List<AbstractProcessingTask>();
			if (this != destListBox)
				candidateTask = new List<AbstractProcessingTask>(this.AllTask);
			else
				candidateTask = new List<AbstractProcessingTask>(Items.OfType<AbstractProcessingTask>());


			for (int s = 0; s < candidateTask.Count; s++)
			{
				AbstractProcessingTask task = (AbstractProcessingTask)candidateTask[s];
				int count = alreadySelectedTask.Count;
				bool insertionPointFound = false;
				for (int d = 0; d <= count && !insertionPointFound; d++)
				{
					AbstractProcessingTask prev = d == 0 ? null : alreadySelectedTask[d - 1];
					AbstractProcessingTask next = d == count ? null : alreadySelectedTask[d];
					insertionPointFound = task.CanInsert(prev, next);

				}
				somethingchange |= this.GetTaskEnabled(task) != insertionPointFound;
				this.SetTaskEnabled(task, insertionPointFound);
				oneNotReady |= !task.Ready;
			}
			bool notreadyChange = lastOnNotReady != oneNotReady;
			lastOnNotReady = oneNotReady;
			if (somethingchange || notreadyChange)
			{
				SuspendLayout();
				bool sort = this != destListBox;
				if (sort)
				{
					List<AbstractProcessingTask> enabled = new List<AbstractProcessingTask>();
					List<AbstractProcessingTask> disabled = new List<AbstractProcessingTask>();
					for (int i = 0; i < candidateTask.Count; i++)
					{
						AbstractProcessingTask task = candidateTask[i];
						if (GetTaskEnabled(task))
							enabled.Add(task);
						else
							disabled.Add(task);
					}
					enabled.Sort();
					disabled.Sort();
					Items.Clear();
					Items.AddRange(enabled.ToArray());
					//Items.AddRange(disabled.ToArray());
				}
				Invalidate();
				ResumeLayout();
			}
			if (oneNotReady)
				InvalidateNotReady();
		}
		public void InvalidateNotReady()
		{
			SuspendLayout();
			for (int i = 0; i < this.Items.Count; i++)
			{
				if (!Get(i).Ready)
					Invalidate(i);
			}
			ResumeLayout();
		}
		#endregion
		#region DRAWING
		public void Invalidate(AbstractProcessingTask task)
		{
			if (task != null)
			{
				Invalidate(Items.IndexOf(task));
			}
			else
				Invalidate();
			this.Update();
		}

		public void Invalidate(int index)
		{
			if (index != -1)
			{
				int count = Items.Count;
				if (index >= 0 && index < count)
				{
					Rectangle bounds = GetItemRectangle(index);
					Invalidate(bounds);
				}
				else if (index == count && index > 0)
				{
					Rectangle bounds = GetItemRectangle(index - 1);
					Invalidate(bounds);
					Update();
				}

			}
		}

		private void DrawImage(Graphics g, Image image, Point pt, bool enabled)
		{
			if (enabled)
			{
				ImageAttributes ia = new ImageAttributes();
				if (this.Lock)
				{
					ColorMatrix cm = new ColorMatrix();
					cm.Matrix33 = 0.3f;
					ia.SetColorMatrix(cm);
				}
				g.DrawImage(image, new Rectangle(pt, new Size(image.Width, image.Height)), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, ia);

				//						g.DrawImage(image, pt);
			}
			else
				ControlPaint.DrawImageDisabled(g, image, pt.X, pt.Y, SkinInfo.BackColor);

		}

		private void DrawScaleImage(Graphics g, Point p, Image image, int bitmapSize, bool enabled)
		{
			double factor = Math.Min(((double)bitmapSize) / image.Width, ((double)bitmapSize) / image.Height);
			using (Bitmap img = new Bitmap(image, new System.Drawing.Size((int)(factor * image.Width), (int)(factor * image.Height))))
			{
				DrawImage(g, img, p, enabled);
			}
		}
		public void DrawTask(Graphics g, int itemIndex, Rectangle r, bool forceDisabled, bool showsettings = false)
		{

			if (itemIndex > -1 && itemIndex < Items.Count)
			{
				AbstractProcessingTask task = Items[itemIndex] as AbstractProcessingTask;
				if (task == null)
					return;
				bool disabled = !GetTaskEnabled(task) || forceDisabled;
				using (SolidBrush backgroundBrush = new SolidBrush(SkinInfo.BackColor))
				{
					// Draw the background
					g.FillRectangle(backgroundBrush, r);
					int bitmapSize = Math.Min(r.Width, r.Height);
					int imageOffset = 0;
					using (Image inImage = GetImage(task.In))
					{
						using (Image outImage = GetImage(task.Out))
						{
							DrawScaleImage(g, r.Location, inImage, bitmapSize, !disabled);
							r.X += bitmapSize / 3;
							r.Width -= bitmapSize / 3;
							/*
								DrawScaleImage(g, r.Location, global::Sardauscan.Properties.Resources.Swap, bitmapSize, !disabled);
								r.X += bitmapSize;
								r.Width -=bitmapSize;
							*/
							DrawScaleImage(g, r.Location, outImage, bitmapSize, !disabled);
							r.X += bitmapSize;
							r.Width -= bitmapSize;
						}
						if (r.Width <= 0)
							return;
						if (showsettings && task.HasSettings)
						{
							DrawScaleImage(g, new Point(r.Right - bitmapSize, r.Top), global::Sardauscan.Properties.Resources.Gears, bitmapSize, true);
						}
						r.Width -= bitmapSize;
						if (r.Width <= 0)
							return;

						r = task.DrawStatus(g, r, SkinInfo.BackColor, SkinInfo.ForeColor);
						if (r.Width <= 0)
							return;
						Color textColor = SkinInfo.ForeColor.GetStepColor(SkinInfo.BackColor, this.Lock ? 0.5 : 0);
						if (!task.Ready)
							textColor = Color.Red;
						using (SolidBrush TextBrush = new SolidBrush(textColor))
						{
							StringFormat sf = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip);
							sf.Alignment = StringAlignment.Near;
							sf.LineAlignment = StringAlignment.Center;
							Rectangle textRect = new Rectangle(r.X + imageOffset, r.Y + 0, r.Width - imageOffset, r.Height);
							string text = task.DisplayName;
							if (this.Enabled)
								g.DrawString(text, this.Font, TextBrush, textRect, sf);
							else
								ControlPaint.DrawStringDisabled(g, text, this.Font, backgroundBrush.Color, textRect, sf);
						}
					}
				}
			}
		}
		#endregion
		#region STATICS HELPER
		private static Image GetImage(eTaskItem inType)
		{
			switch (inType)
			{
				case eTaskItem.None:
					return global::Sardauscan.Properties.Resources.Pulse;
				case eTaskItem.File:
					return global::Sardauscan.Properties.Resources.Floppy;
				case eTaskItem.ScanLines:
					return global::Sardauscan.Properties.Resources.Lines;
				case eTaskItem.Mesh:
					return global::Sardauscan.Properties.Resources.Meshes;
			}
			return global::Sardauscan.Properties.Resources.Mark_Question;
		}

		public struct IconInfo
		{
			public bool fIcon;
			public int xHotspot;
			public int yHotspot;
			public IntPtr hbmMask;
			public IntPtr hbmColor;
		}

		[DllImport("user32.dll")]
		public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

		private static Cursor CreateCursor(Bitmap bmp, int xHotSpot, int yHotSpot)
		{
			IconInfo tmp = new IconInfo();
			GetIconInfo(bmp.GetHicon(), ref tmp);
			tmp.xHotspot = xHotSpot;
			tmp.yHotspot = yHotSpot;
			tmp.fIcon = true;
			return new Cursor(CreateIconIndirect(ref tmp));
		}


		public Cursor CreateCursor(int index, bool enable)
		{
			int bitmapSize = Math.Min(Width, ItemHeight);
			Bitmap bmp = new Bitmap((int)(5 * bitmapSize), ItemHeight);
			Rectangle r = new Rectangle(0, 0, bmp.Width, bmp.Height);
			using (Graphics g = Graphics.FromImage(bmp))
			{

				DrawTask(g, index, r, !enable, false);
			}
			bmp.MakeTransparent(SkinInfo.BackColor);
			return CreateCursor(bmp, r.X, 0);

		}
		#endregion

		private void InitializeComponent()
		{
			this.SuspendLayout();
			this.ResumeLayout(false);

		}
	}

	public static class AbstractProcessingTaskExt
	{
		public static Rectangle DrawStatus(this AbstractProcessingTask task, Graphics g, Rectangle bound, Color backColor, Color frontColor)
		{
			eTaskStatus status = task.Status;
			int percent = task.Percent;

			if (status == eTaskStatus.Working)
			{
				return DrawProgress(g, bound, percent, backColor, frontColor);
			}
			else if (status == eTaskStatus.Error)
				return DrawScaleImage(g, global::Sardauscan.Properties.Resources.Bug, bound);
			else if (status == eTaskStatus.Finished)
				return DrawScaleImage(g, global::Sardauscan.Properties.Resources.Check, bound);

			return bound;
		}
		private static Rectangle DrawProgress(Graphics g, Rectangle bound, int percent, Color backColor, Color frontColor)
		{
			int pct = Math.Min(100, Math.Max(0, percent));

			if (pct < 25)
				return DrawScaleImage(g, global::Sardauscan.Properties.Resources.Battery_0, bound);
			else if (pct < 50)
				return DrawScaleImage(g, global::Sardauscan.Properties.Resources.Battery_1, bound);
			else if (pct < 75)
				return DrawScaleImage(g, global::Sardauscan.Properties.Resources.Battery_2, bound);
			return DrawScaleImage(g, global::Sardauscan.Properties.Resources.Battery_3, bound);
		}

		private static Rectangle DrawScaleImage(Graphics g, Image image, Rectangle bound)
		{
			int bitmapSize = Math.Min(bound.Width, bound.Height);
			double factor = Math.Min(((double)bitmapSize) / image.Width, ((double)bitmapSize) / image.Height);
			using (Bitmap img = new Bitmap(image, new System.Drawing.Size((int)(factor * image.Width), (int)(factor * image.Height))))
			{
				Point p = new Point(bound.Right - bitmapSize, bound.Top);
				g.DrawImage(img, p);
			}
			return new Rectangle(bound.X, bound.Y, bound.Width - bitmapSize, bound.Height);
		}

	}

}
