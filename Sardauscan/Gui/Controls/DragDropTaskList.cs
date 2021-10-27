#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
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
using System.Runtime.InteropServices;
using Sardauscan.Core.ProcessingTask;
using System.Diagnostics;
using System.Reflection;
using Sardauscan.Core.Geometry;
using Sardauscan.Gui;
using Sardauscan.Gui.Forms;
using System.IO;
using Sardauscan.Gui.Controls.ApplicationView;

namespace Sardauscan.Gui.Controls
{
	public partial class DragDropTaskList : UserControl,IDisposable
	{

		private int IndexOfItemUnderMouseToDrag;
		private int IndexOfItemUnderMouseToDrop;

		private Rectangle dragBoxFromMouseDown;
		private Point screenOffset;

		private Cursor MyNormalCursor;
		private Cursor MyNodropCursor;

		private TaskPropertiesForm PropertyForm;
		private eTaskItem m_In = eTaskItem.None;
		public eTaskItem In
		{
			get { return m_In; }
			set { m_In = value; AlignTaskEnable(); }
		}
		public DragDropTaskList()
		{
			InitializeComponent();
			typeof(TaskListBox).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
			null, this.ListDragTarget, new object[] { true });
			typeof(TaskListBox).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
			null, this.ListDragSource, new object[] { true });
			//DoubleBuffered = true;
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
			this.DoubleBuffered = true;
			In = eTaskItem.None;

			PropertyForm = new TaskPropertiesForm();
			Application.Idle += this.OnIdle;

			this.ToolTip.ForeColor = SkinInfo.ForeColor;
			this.ToolTip.BackColor = SkinInfo.BackColor;

		}
		/// <summary>
		/// Dispose object
		/// </summary>
		public new void Dispose()
		{
			Application.Idle -= OnIdle;
		}


		private void DragDropTaskList_Load(object sender, EventArgs e)
		{
			if (this.IsDesignMode())
				return;
			MessageLabel.ForeColor = SkinInfo.ForeColor.GetStepColor(SkinInfo.BackColor, 0.5);
			FileDialog.Filter = "Process File (*.process.xml)|*.process.xml";
			FileDialog.InitialDirectory = Program.ProcessPath;
			if (!Directory.Exists(Program.ProcessPath))
				Directory.CreateDirectory(Program.ProcessPath);

			try
			{
				/*
				var availabelTaskType = AppDomain.CurrentDomain.GetAssemblies()
																.SelectMany(assembly => assembly.GetTypes())
																.Where(type => type.IsSubclassOf(typeof(AbstractProcessingTask)));
				foreach (Type type in availabelTaskType)
				{
					if (!type.IsAbstract)
					{
						BrowsableAttribute attribute = Attribute.GetCustomAttribute(type, typeof(BrowsableAttribute)) as BrowsableAttribute;
						if (attribute == null || (attribute != null && attribute.Browsable))
						{
							AbstractProcessingTask task = (AbstractProcessingTask)Activator.CreateInstance(type);
							AbstractProcessingTask settingsTask = task.LoadFromFile(Program.TaskConfigPath);
							if (settingsTask != null)
								task = settingsTask;
							tasks.Add(task);
						}
					}
				}

				tasks.Sort();
				*/
				List<AbstractProcessingTask> tasks = new List<AbstractProcessingTask>();
				var availabelTaskType = Reflector.GetSubClassOf(typeof(AbstractProcessingTask));
				foreach (Type type in availabelTaskType)
				{
							AbstractProcessingTask task = (AbstractProcessingTask)Activator.CreateInstance(type);
							AbstractProcessingTask settingsTask = task.LoadFromFile(Program.TaskPath);
							if (settingsTask != null)
								task = settingsTask;
							tasks.Add(task);
				}
				this.ListDragSource.SetStartupTask(tasks);
				AlignTaskEnable();
			}
			catch { }
		}
		#region DRAG DROP
		private void ListDragSource_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			IndexOfItemUnderMouseToDrop = ListBox.NoMatches;
			// Get the index of the item the mouse is below.
			IndexOfItemUnderMouseToDrag = ((TaskListBox)sender).IndexFromPoint(e.X, e.Y);
			if (IndexOfItemUnderMouseToDrag != ListBox.NoMatches)
			{

				// Remember the point where the mouse down occurred. The DragSize indicates 
				// the size that the mouse can move before a drag event should be started.                
				Size dragSize = SystemInformation.DragSize;

				// Create a rectangle using the DragSize, with the mouse position being 
				// at the center of the rectangle.
				dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
																											 e.Y - (dragSize.Height / 2)), dragSize);
			}
			else
				// Reset the rectangle if the mouse is not over an item in the ListBox.
				dragBoxFromMouseDown = Rectangle.Empty;

		}

		private void ListDragSource_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			// Reset the drag rectangle when the mouse button is raised.
			dragBoxFromMouseDown = Rectangle.Empty;
			IndexOfItemUnderMouseToDrop = ListBox.NoMatches;
		}
		int hoveredIndex = -1;
		private void ListDragSource_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{

			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				if (!ListDragTarget.Lock)
				{
					bool draggable = true;
					if (sender == ListDragTarget)
					{
						draggable = ListDragTarget.RemovableForTaskFlow(IndexOfItemUnderMouseToDrag);
					}
					if (!draggable)
						Cursor.Current = Cursors.No;
					// If the mouse moves outside the rectangle, start the drag. 
					if (draggable && dragBoxFromMouseDown != Rectangle.Empty &&
							!dragBoxFromMouseDown.Contains(e.X, e.Y))
					{

						// Create custom cursors for the drag-and-drop operation. 
						try
						{
							TaskListBox ctrl = (TaskListBox)sender;

							MyNormalCursor = ctrl.CreateCursor(IndexOfItemUnderMouseToDrag, true);
							MyNodropCursor = ctrl.CreateCursor(IndexOfItemUnderMouseToDrag, false);

						}
						catch
						{
						}
						finally
						{

							// The screenOffset is used to account for any desktop bands  
							// that may be at the top or left side of the screen when  
							// determining when to cancel the drag drop operation.
							screenOffset = SystemInformation.WorkingArea.Location;
							object data = ((TaskListBox)sender).GetDragData(IndexOfItemUnderMouseToDrag);
							if (sender == ListDragTarget)
							{
								((TaskListBox)sender).RemoveAt(IndexOfItemUnderMouseToDrag);
								AlignHelpMessage();
							}
							// Proceed with the drag-and-drop, passing in the list item.                    
							DragDropEffects dropEffect = ((TaskListBox)sender).DoDragDrop(data, DragDropEffects.All | DragDropEffects.Copy);

							// Dispose of the cursors since they are no longer needed. 
							if (MyNormalCursor != null)
								MyNormalCursor.Dispose();
							if (MyNodropCursor != null)
								MyNodropCursor.Dispose();
						}
					}
				}
			}
			else
			{
				if (sender is TaskListBox)
				{
					TaskListBox listbox = (TaskListBox)sender;
					int newHoveredIndex = listbox.IndexFromPoint(e.Location);
					if(newHoveredIndex==-1)
						this.ToolTip.ShowAlways = false;
					if (hoveredIndex != newHoveredIndex)
					{
						hoveredIndex = newHoveredIndex;
						if (hoveredIndex > -1)
						{
							AbstractProcessingTask task = listbox.Get(hoveredIndex);
							if (task != null)
							{
								this.ToolTip.Active = false;
								this.ToolTip.SetToolTip(listbox, task.ToolTip);
								this.ToolTip.Active = true;
								this.ToolTip.ShowAlways = true;
							}
						}
					}
				}
			}
		}
		private void ListDragSource_GiveFeedback(object sender, System.Windows.Forms.GiveFeedbackEventArgs e)
		{
			e.UseDefaultCursors = false;
			if ((e.Effect & DragDropEffects.Copy) == DragDropEffects.Copy)
				Cursor.Current = MyNormalCursor;
			else
				Cursor.Current = MyNodropCursor;

		}
		private void ListDragTarget_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{


			// Determine whether string data exists in the drop data. If not, then 
			// the drop effect reflects that the drop cannot occur. 
			bool valid = TaskListBox.IsValidDragData(e.Data);
			if (!valid)
			{

				e.Effect = DragDropEffects.None;
				return;
			}

			int lastIndex = IndexOfItemUnderMouseToDrop;
			IndexOfItemUnderMouseToDrop = ListDragTarget.IndexFromPoint(ListDragTarget.PointToClient(new Point(e.X, e.Y)));

			if (sender == ListDragTarget)
			{
				if (IndexOfItemUnderMouseToDrop == -1)
					IndexOfItemUnderMouseToDrop = ListDragTarget.Count;

				ListDragTarget.Invalidate(lastIndex);
				ListDragTarget.Invalidate(IndexOfItemUnderMouseToDrop);

				if (ListDragTarget.IsValidInsertionIndex(IndexOfItemUnderMouseToDrop, TaskListBox.DragDataTask(e.Data), In))
				{
					e.Effect = DragDropEffects.Copy;
					return;
				}
				else
				{
					IndexOfItemUnderMouseToDrop = -1;
					e.Effect = DragDropEffects.Move;
					return;
				}
			}
			e.Effect = DragDropEffects.Move;
		}
		private void ListDragTarget_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			// Ensure that the list item index is contained in the data. 
			if (TaskListBox.IsValidDragData(e.Data))
			{

				// Perform drag-and-drop, depending upon the effect. 
				if (e.Effect == DragDropEffects.Copy ||
						e.Effect == DragDropEffects.Move)
				{
					AbstractProcessingTask task = null;
					if (TaskListBox.DragDataSource(e.Data) == ListDragTarget)
					{
						task = (AbstractProcessingTask)(TaskListBox.DragDataTask(e.Data));
					}
					else // if we are comming from source list => Create a new instance
						if (task == null)
						{
							task = TaskListBox.DragDataTask(e.Data).Clone();
						}

					if (ListDragTarget.IsValidInsertionIndex(IndexOfItemUnderMouseToDrop, task, In)) // if place is ok
					{
						((TaskListBox)sender).Insert(IndexOfItemUnderMouseToDrop, task);
					}
					else if (TaskListBox.DragDataSource(e.Data) == ListDragTarget) // if place is not ok, but we came from the destination listbox, we reinsert the task when she come from
					{
						((TaskListBox)sender).Insert(TaskListBox.DragDataIndex(e.Data), task);
					}
					ListDragTarget.Invalidate();
					IndexOfItemUnderMouseToDrop = ListBox.NoMatches;
				}
			}
			AlignTaskEnable();
			AlignHelpMessage();
		}
		private void ListDragSource_QueryContinueDrag(object sender, System.Windows.Forms.QueryContinueDragEventArgs e)
		{
			// Cancel the drag if the mouse moves off the form.
			ListBox lb = sender as ListBox;

			if (lb != null)
			{

				Form f = lb.FindForm();

				// Cancel the drag if the mouse moves off the form. The screenOffset 
				// takes into account any desktop bands that may be at the top or left 
				// side of the screen. 
				if (((Control.MousePosition.X - screenOffset.X) < f.DesktopBounds.Left) ||
						((Control.MousePosition.X - screenOffset.X) > f.DesktopBounds.Right) ||
						((Control.MousePosition.Y - screenOffset.Y) < f.DesktopBounds.Top) ||
						((Control.MousePosition.Y - screenOffset.Y) > f.DesktopBounds.Bottom))
				{

					e.Action = DragAction.Cancel;
				}
			}
		}
		private void DeleteBox_DragDrop(object sender, DragEventArgs e)
		{
			IndexOfItemUnderMouseToDrop = ListBox.NoMatches;
			AlignTaskEnable();
			AlignHelpMessage();
			ListDragTarget.Invalidate();
		}
		#endregion
		#region DRAW
		private void ListDragSource_DrawItem(object sender, DrawItemEventArgs e)
		{
			TaskListBox listbox = (TaskListBox)sender;

			listbox.DrawTask(e.Graphics, e.Index, e.Bounds, !this.Enabled, true);
			if (sender == ListDragTarget)
			{
				e.Graphics.Clip = new Region();
				using (Pen p = new Pen(ForeColor, 3))
				{
					if (IndexOfItemUnderMouseToDrop == e.Index)
						e.Graphics.DrawLine(p, e.Bounds.X, e.Bounds.Top + 2, e.Bounds.Right, e.Bounds.Top + 2);
					else if (IndexOfItemUnderMouseToDrop == listbox.Count && e.Index == listbox.Count - 1)
						e.Graphics.DrawLine(p, e.Bounds.X, e.Bounds.Bottom - 2, e.Bounds.Right, e.Bounds.Bottom - 2);
				}
			}
			e.DrawFocusRectangle();
		}
		#endregion

		void AlignTaskEnable()
		{
			this.ListDragSource.AlignTaskEnable(this.ListDragTarget);
			this.RunButton.Enabled = this.ListDragTarget.Count > 0;
		}
		private void List_Click(object sender, MouseEventArgs e)
		{
			bool isDefaultTask = sender == this.ListDragSource;
			int index = ((ListBox)sender).IndexFromPoint(e.Location);
			if (index != System.Windows.Forms.ListBox.NoMatches && !((TaskListBox)sender).Lock)
			{
				AbstractProcessingTask task = (AbstractProcessingTask)((ListBox)sender).Items[index];
				Rectangle bounds = ((ListBox)sender).GetItemRectangle(index);
				int settingsSize = Math.Min(bounds.Width, bounds.Height);
				Rectangle settingsRect = new Rectangle(bounds.Right - settingsSize, bounds.Top, settingsSize, settingsSize);
				if (settingsRect.Contains(e.Location))
				{

					if (task != null && task.HasSettings)
					{
						AbstractProcessingTask ret = PropertyForm.EditSettings(this, task, sender == ListDragSource);
						((ListBox)sender).Items[index] = ret;
					}
				}

			}

		}

		[Browsable(false)]
		public List<AbstractProcessingTask> SelectedTasks
		{
			get
			{
				List<AbstractProcessingTask> ret = new List<AbstractProcessingTask>(ListDragTarget.Count);
				for (int i = 0; i < ListDragTarget.Count; i++)
					ret.Add(ListDragTarget.Get(i));
				return ret;
			}
			set
			{
				ListDragTarget.SuspendLayout();
				ListDragTarget.Clear();
				if(value!=null)
				{
					for (int i = 0; i < value.Count; i++)
						ListDragTarget.Add(value[i]);
				}
				AlignHelpMessage();
				ListDragTarget.ResumeLayout();
				ListDragTarget.Invalidate();


			}
		}
		private void AlignHelpMessage()
		{
			bool visible = ListDragTarget.Items.Count == 0;
			MessagePanel.Visible = visible;
			int top = !visible ? MessagePanel.Top : MessagePanel.Bottom;
			int bottom = this.ListDragTarget.Bottom;
			this.ListDragTarget.Top = top;
			this.ListDragTarget.Height = bottom - top;
		}
		private void DeleteBox_Click(object sender, EventArgs e)
		{
			AlignTaskEnable();
			this.ListDragTarget.Clear();
			AlignHelpMessage();
		}


		void SetRunButtonState(bool start)
		{
			Image img = global::Sardauscan.Properties.Resources.Play;
			if (!start)
			{
				img = global::Sardauscan.Properties.Resources.Stop;
			}
			this.DeleteBox.Enabled = start;
			ListDragTarget.Lock = !start;
			ListDragSource.Lock = !start;
			this.RunButton.Image = img;
			ViewControler controler = Settings.Get<ViewControler>();
			if (controler != null)
			{
				controler.Lock = !start;
			}
			this.Refresh();

			if (start)
			{
				IScene3DViewer viewer = Settings.Get<IScene3DViewer>();
				if (viewer != null)
					viewer.Invalidate();
			}
		}
		private void RunButton_Click(object sender, EventArgs e)
		{
			foreach (AbstractProcessingTask task in SelectedTasks)
			{
				if (!task.Ready)
				{
					MessageBox.Show(string.Format("task {0} is not ready :\n{1}", task.DisplayName, task.ToolTip), "Can't run process", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}

			bool Async = true;
			if (Async)
			{
				if (this.TaskBackgroundWorker.IsBusy == true)
				{
					if (this.TaskBackgroundWorker.WorkerSupportsCancellation == true)
					{
						this.TaskBackgroundWorker.CancelAsync();
					}
					SetRunButtonState(true);
				}
				else
				{
					this.TaskBackgroundWorker.RunWorkerAsync();
					SetRunButtonState(false);
				}

			}
			else
			{
				RunButton.Enabled = false;
				SetRunButtonState(false);
				TaskBackgroundWorker_DoWork(null, null);
				RunButton.Enabled = true;
				SetRunButtonState(true);
			}
		}

		protected void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs e)
		{
			ScanData points = e.UserState as ScanData;
			UpdateViewer(e.UserState as ScanData);
		}
		protected void UpdateViewer(ScanData points)
		{
			IScene3DViewer viewer = Settings.Get<IScene3DViewer>();
			if (viewer != null)
			{
				viewer.Scene.Clear();
				if (points != null)
					viewer.Scene.Add(points);
				viewer.Invalidate();
			}
		}

		private void TaskBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			var proc = new Sardauscan.Core.ProcessingTask.Process();
			proc.AddRange(SelectedTasks);
			UpdateViewer(null);
			ScanData ret = proc.Run(null, ListDragTarget, sender as BackgroundWorker, e);
			if (e == null)
				UpdateViewer(ret);

		}

		private void TaskBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (e.UserState != null)
			{
				UpdateViewer(e.UserState as ScanData);
			}

		}

		private void TaskBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (!e.Cancelled)
			{
				UpdateViewer(e.Result as ScanData);
			}
			SetRunButtonState(true);
		}
		DateTime lastImageTime = DateTime.Now;
		private void OnIdle(object sender, EventArgs e)
		{
			if (Visible)
			{
				DateTime now = DateTime.Now;
				bool expired = (now - lastImageTime).TotalMilliseconds > 200;
				if (expired)
				{
					AlignTaskEnable();
					this.ListDragSource.InvalidateNotReady();
					this.ListDragTarget.InvalidateNotReady();
					LoadButton.Enabled = DeleteBox.Enabled || this.ListDragTarget.Items.Count == 0;
					SaveButton.Enabled = DeleteBox.Enabled && this.ListDragTarget.Items.Count >0;
					AlignHelpMessage();
				}
			}
		}

		private void LoadButton_Click(object sender, EventArgs e)
		{
			FileDialog.CheckFileExists = true;
			FileDialog.Title = "Load Process";
			FileDialog.FileName = "Process";
			if (FileDialog.ShowDialog(this) == DialogResult.OK)
			{
				var proc = new Sardauscan.Core.ProcessingTask.Process();
				proc.Load(FileDialog.FileName);
				SelectedTasks = proc;
			}

		}

		private void SaveButton_Click(object sender, EventArgs e)
		{
			FileDialog.CheckFileExists = false;
			FileDialog.Title = "Save Process";
			FileDialog.FileName = "Process";
			if (FileDialog.ShowDialog(this) == DialogResult.OK)
			{
				var proc = new Sardauscan.Core.ProcessingTask.Process();
				proc.AddRange(SelectedTasks);
				proc.Save(FileDialog.FileName);
			}
			
			

		}

	}
}
