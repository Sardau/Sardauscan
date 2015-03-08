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
using Sardauscan.Core.Geometry;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Xml.Serialization;
using System.IO;

namespace Sardauscan.Core.ProcessingTask
{
	/// <summary>
	/// Task input output data
	/// </summary>
	public enum eTaskItem
	{
		None = 0,
		ScanLines = 1,
		Mesh = 2,
		File = 3
	}
	/// <summary>
	/// Task Status type
	/// </summary>
	public enum eTaskStatus
	{
		None,
		Working,
		Finished,
		Error
	}
	/// <summary>
	/// Abstract Processing Task
	/// </summary>
	[Browsable(true)]
	public abstract class AbstractProcessingTask : IComparable
	{
		/// <summary>
		/// Ctor
		/// </summary>
		public AbstractProcessingTask()
		{
			Status = eTaskStatus.None;
			Percent = 0;
		}

		/// <summary>
		/// Input Data Type
		/// </summary>
		[XmlIgnoreAttribute]
		[Browsable(false)]
		public abstract eTaskItem In { get; }
		/// <summary>
		/// Output Data type
		/// </summary>
		[XmlIgnoreAttribute]
		[Browsable(false)]
		public abstract eTaskItem Out { get; }

		/// <summary>
		/// Status of the task
		/// </summary>
		[XmlIgnoreAttribute]
		[Browsable(false)]
		public eTaskStatus Status { get; protected set; }
		/// <summary>
		/// Completion percent of the task
		/// </summary>
		[XmlIgnoreAttribute]
		[Browsable(false)]
		public int Percent;

		/// <summary>
		/// Clone this task
		/// </summary>
		/// <returns></returns>
		public abstract AbstractProcessingTask Clone();

		protected bool? hasBrowsableSettings = null;
		/// <summary>
		/// Check if this task has Browsable Settings
		/// </summary>
		[XmlIgnoreAttribute]
		[Browsable(false)]
		public bool HasBrowsableSettings
		{
			get
			{
				if (hasBrowsableSettings == null)
				{
					hasBrowsableSettings = false;
					var type = this.GetType();

					foreach (var prop in type.GetProperties())
					{
						BrowsableAttribute attribute = Attribute.GetCustomAttribute(prop, typeof(BrowsableAttribute)) as BrowsableAttribute;

						if (attribute != null)
						{
							if (attribute.Browsable)
							{
								hasBrowsableSettings = true;
								break;
							}
						}
					}
				}

				return (bool)hasBrowsableSettings;
			}
		}

		/// <summary>
		/// Does this task have Settings
		/// </summary>
		[Browsable(false)]
		public virtual bool HasSettings { get { return HasBrowsableSettings; } }
		/// <summary>
		/// Run the settings interface (return true if modified)
		/// </summary>
		/// <returns></returns>
		public virtual bool RunSettings() { return false; }
		/// <summary>
		/// Prepate task to run
		/// </summary>
		public void PrepareToRun()
		{
			Status = eTaskStatus.None;
			Percent = 0;
		}
		protected Control CallerControl = null;
		/// <summary>
		/// Run the task
		/// </summary>
		/// <param name="source">Source data</param>
		/// <param name="control">Control that launch</param>
		/// <param name="worker">background worker trhead if anny</param>
		/// <param name="e">DoWork Event argument</param>
		/// <param name="updateFunc"> Update function to call</param>
		/// <returns></returns>
		public ScanData Run(ScanData source, Control control = null, BackgroundWorker worker = null, DoWorkEventArgs e = null, ProgressChangedEventHandler updateFunc = null)
		{
			LastError = string.Empty;
			Worker = worker;
			WorkerArg = e;
			CallerControl = control;
			UpdateFunc = updateFunc;
			ScanData ret = null;
			Status = eTaskStatus.Working;
			UpdatePercent(0, ret);
			try
			{
				ret = DoTask(source);
			}
			catch (Exception exception)
			{
				Status = eTaskStatus.Error;
				LastError = exception.Message;
				return null;
			}
			if (Status != eTaskStatus.Error)
			{
				Status = eTaskStatus.Finished;
				UpdatePercent(100, ret);
			}
			UpdateFunc = null;
			CallerControl = null;
			Worker = null;
			WorkerArg = null;
			return ret;
		}

		[XmlIgnoreAttribute]
		[Browsable(false)]
		private ProgressChangedEventHandler UpdateFunc;

		/// <summary>
		/// Update percentage
		/// </summary>
		/// <param name="percent"></param>
		/// <param name="data"></param>
		public virtual void UpdatePercent(int percent, ScanData data)
		{
			if (Percent != percent)
			{
				Percent = percent;
				if (UpdateFunc != null)
					UpdateFunc(this, new ProgressChangedEventArgs(percent, data));
			}
		}

		/// <summary>
		/// Realy do the task
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		protected abstract ScanData DoTask(ScanData source);

		/// <summary>
		/// Name of the task
		/// </summary>
		[Browsable(false)]
		public abstract string Name { get; }
		/// <summary>
		/// Display name of the task
		/// </summary>
		[Browsable(false)]
		public virtual string DisplayName { get { return Name; } }


		/// <summary>
		/// Check if the task can be inserted between two other
		/// </summary>
		/// <param name="prev"></param>
		/// <param name="next"></param>
		/// <returns></returns>
		public bool CanInsert(AbstractProcessingTask prev, AbstractProcessingTask next)
		{
			bool inok = false;
			if (prev == null)
				inok = In == eTaskItem.None;
			else
				inok = In == prev.Out;
			bool outok = false;
			if (next == null)
				outok = true;
			else
				outok = Out == next.In;

			return inok && outok;
		}
		/// <summary>
		/// Check if the task can be inserted based on input output data Type
		/// </summary>
		/// <param name="prevOut"></param>
		/// <param name="nextIn"></param>
		/// <returns></returns>
		public bool CanInsert(eTaskItem prevOut, eTaskItem nextIn)
		{
			return In == prevOut && Out == nextIn;
		}
		/// <summary>
		/// Check if task can follow based on Data Type
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		public bool CanFollow(eTaskItem t)
		{
			return In == t;
		}
		/// <summary>
		/// Check if task can follow another task
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		public bool CanFolow(AbstractProcessingTask other)
		{
			return CanFollow(other.Out);
		}


		public override string ToString()
		{
			return DisplayName;
		}
		/// <summary>
		/// Compare a Task to another
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public int CompareTo(object obj)
		{
			AbstractProcessingTask other = (AbstractProcessingTask)obj;
			int c = this.In.CompareTo(other.In);
			if (c == 0)
				c = this.Out.CompareTo(other.Out);
			return c;
		}

		protected DoWorkEventArgs WorkerArg = null;
		protected BackgroundWorker Worker = null;
		protected bool CancelPending
		{
			get
			{
				bool cancel = false;
				if (Worker != null && Worker.CancellationPending)
				{
					if (WorkerArg != null)
						WorkerArg.Cancel = true;
					cancel = true;
				}
				return cancel;

			}
		}

		[Browsable(false)]
		///Get the associated ConfigFile
		public virtual string ConfigFileName { get { return this.GetType().ToString() + ".config.xml"; } }
		/// <summary>
		/// Load a task from Config
		/// </summary>
		/// <param name="settingsDirectory"></param>
		/// <returns></returns>
		public AbstractProcessingTask LoadFromFile(string settingsDirectory)
		{
			try
			{
				string filename = Path.Combine(settingsDirectory, ConfigFileName);
				if (File.Exists(filename))
				{
					using (StreamReader reader = File.OpenText(filename))
					{
						string xml = reader.ReadToEnd();
						return LoadFromXml(xml);
					}
				}

			}
			catch
			{
			}
			return this;
		}

		/// <summary>
		/// Load a task from XML
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		public virtual AbstractProcessingTask LoadFromXml(string xml)
		{
			try
			{
				using (StringReader reader = new StringReader(xml))
				{
					XmlSerializer serializer = new XmlSerializer(this.GetType());
					return (AbstractProcessingTask)serializer.Deserialize(reader);
				}
			}
			catch
			{
			}
			return this;
		}

		/// <summary>
		/// Save task to a file
		/// </summary>
		/// <param name="settingsDirectory"></param>
		public void SaveToFile(string settingsDirectory)
		{
			if (!Directory.Exists(settingsDirectory))
				Directory.CreateDirectory(settingsDirectory);
			string filename = Path.Combine(settingsDirectory, ConfigFileName);
			using (StreamWriter writer = File.CreateText(filename))
			{
				writer.Write(ToXml());
			}
		}
		/// <summary>
		/// Get the XML to save for this task
		/// </summary>
		/// <returns></returns>
		public virtual string ToXml()
		{
			using (StringWriter writer = new StringWriter())
			{
				XmlSerializer serializer = new XmlSerializer(this.GetType());
				serializer.Serialize(writer, this);
				return writer.ToString();
			}

		}
		/// <summary>
		/// Is the task ready
		/// </summary>
		[Browsable(false)]
		public virtual bool Ready { get { return true; } }

		/// <summary>
		/// Last Error of the task
		/// </summary>
		[Browsable(false)]
		protected String LastError { get; set; }
		/// <summary>
		/// Task tooltip
		/// </summary>
		[Browsable(false)]
		public virtual string ToolTip
		{
			get
			{
				switch (Status)
				{
					case eTaskStatus.Finished:
						return "Finished";
					case eTaskStatus.Working:
						return string.Format("Working : {0}%", Percent);
					case eTaskStatus.Error:
						return "Error :" + LastError;
					default:
						{
							if (Ready)
								return DisplayName;
							else
								return "Missings Ressource to run task";
						}
				}
			}
		}

	}
}
