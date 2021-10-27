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
using Sardauscan.Gui;
using Sardauscan.Core.Geometry;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using Sardauscan.Gui.Controls;
using Sardauscan.Core.Interface;
using System.Windows.Forms;
using System.Xml;

namespace Sardauscan.Core.ProcessingTask
{
	/// <summary>
	/// Process (list of task)
	/// </summary>
	public class Process : List<AbstractProcessingTask>
	{

		private Control Control = null;
		public event ProgressChangedEventHandler ProgressChanged;
		public event RunWorkerCompletedEventHandler RunWorkerCompleted;

		private AbstractProcessingTask CurrentTask = null;

		BackgroundWorker Worker;
		public ScanData Run(ScanData indata, Control control = null, BackgroundWorker worker = null, DoWorkEventArgs e = null)
		{
			Worker = worker;


			CurrentTask = null;
			Control = control;
			int count = this.Count;
			for (int i = 0; i < count; i++)
				this[i].PrepareToRun();
			RefreshControl(CurrentTask);
			ScanData ret = indata;
			bool cancel = false;
			try
			{
				for (int i = 0; !cancel && i < count; i++)
				{

					if (worker != null && worker.CancellationPending)
					{
						if (e != null)
							e.Cancel = true;
						cancel = true;
						continue;
					}

					CurrentTask = this[i];
					RefreshControl(CurrentTask);
					if(ret!=null)
						ret = (new LinesRemoveEmpty()).Run(ret); // quick remove empty
					ret = CurrentTask.Run(ret, Control, worker, e, ProgressChangedEventHandler);
					if (CurrentTask.Status == eTaskStatus.Error)
					{
						RefreshControl(CurrentTask);
						if (e != null)
							e.Result = ret;
						return ret;
					}
					RefreshControl(CurrentTask);
					int percent = (int)((100 * (i + 1)) / count);
					OnProgressChange(worker, percent, ret);
				}
				OnCompleted(worker, ret, null, cancel);
				if (e != null)
					e.Result = ret;
			}
			catch (Exception error)
			{
				OnCompleted(worker, null, error, false);
				if (e != null)
					e.Result = null;
			}
			RefreshControl(CurrentTask);
			CurrentTask = null;
			Worker = null;
			return ret;
		}

		protected void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs e)
		{
			RefreshControl(sender as AbstractProcessingTask);
		}


		protected void RefreshControl(AbstractProcessingTask sender)
		{
			if (Control != null)
			{
				lock (Control)
				{
					if (Control.InvokeRequired)
						Control.BeginInvoke(new Action(() =>
						{
							if (Control is TaskListBox)
							{
								((TaskListBox)Control).Invalidate(sender);
								((TaskListBox)Control).Update();
							}
							else
								Control.Invalidate();
						}));
					else
					{
						if (Control is TaskListBox)
						{
							((TaskListBox)Control).Invalidate(sender);
							((TaskListBox)Control).Update();
						}
						else
							Control.Invalidate();
					}
				}

				//                    ListBox.Invoke(new Action(() => ListBox.Refresh()));
			}
		}
		protected void OnProgressChange(BackgroundWorker worker, int percent, object status)
		{
			ProgressChangedEventHandler(CurrentTask, new ProgressChangedEventArgs(percent, status));
			if (worker != null && worker.WorkerReportsProgress)
				worker.ReportProgress(percent, status);
			if (ProgressChanged != null)
				ProgressChanged(this, new ProgressChangedEventArgs(percent, status));
		}

		protected void OnCompleted(BackgroundWorker worker, object result, Exception error, bool cancel)
		{
			if (RunWorkerCompleted != null)
				RunWorkerCompleted(this, new RunWorkerCompletedEventArgs(result, error, cancel));
		}


		public void Save(string filename)
		{
			string data;
			using (StreamWriter sw = File.CreateText(filename))
			{
				using (XmlWriter writer = XmlWriter.Create(sw))
				{
					writer.WriteStartDocument();
					writer.WriteStartElement("Process");
					writer.WriteStartAttribute("Type");
					writer.WriteValue(this.GetType().ToString());

					for (int i = 0; i < Count; i++)
					{
						AbstractProcessingTask task = this[i];
						writer.WriteStartElement("Task");
						writer.WriteStartAttribute("Type");
						writer.WriteValue(task.GetType().ToString());
						writer.WriteEndAttribute();
						writer.WriteString(task.ToXml());
						writer.WriteEndElement();
					}
					writer.WriteEndDocument();
				}
				data = sw.ToString();
			}

		}
		public void Load(string filename)
		{
			this.Clear();
			XmlDocument doc = new XmlDocument();
			doc.Load(filename);
			XmlNodeList list = doc.SelectNodes("Process/Task");
			foreach (XmlNode node in list)
			{
				if (node.Attributes["Type"] != null)
				{
					string typestr = node.Attributes["Type"].Value;
					Type type = Type.GetType(typestr);
					AbstractProcessingTask task = (AbstractProcessingTask)Activator.CreateInstance(type);
					string taskstr = node.InnerText;
					task = task.LoadFromXml(taskstr);
					if (task != null)
						Add(task);
				}
			}
		}
	}
}
