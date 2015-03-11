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
using Sardauscan.Core.IO;
using System.ComponentModel;
using System.IO;

namespace Sardauscan.Core.ProcessingTask
{
	/// <summary>
	/// Save Scan Lines As Line/point
	/// </summary>
	public class SavePoints : AbstractProcessingTask
	{
		private string m_FileName = string.Empty;
		[Browsable(false)]
		[Description("File Path")]
		[EditorAttribute(typeof(Point3dArrayIOOpenFileEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public string Filename { get { return m_FileName; } set { m_FileName = value; } }

		public override eTaskItem In
		{
			get { return eTaskItem.ScanLines; }
		}

		public override eTaskItem Out
		{
			get { return eTaskItem.File; }
		}
		/// <summary>
		/// Clone this
		/// </summary>
		/// <returns></returns>
		public override AbstractProcessingTask Clone()
		{
			AbstractProcessingTask ret = (AbstractProcessingTask)Activator.CreateInstance(this.GetType());
			return ret;
		}


		public virtual string DialogFilter
		{
			get { return ScanDataIO.GetDialogFilter(); ; }
		}
		string ShowDialog()
		{
			DialogResult result = DialogResult.Ignore;
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.Filter = DialogFilter;
			dlg.InitialDirectory = Program.UserDataPath;
			dlg.CheckFileExists = false;
			if (CallerControl != null)
				CallerControl.Invoke(new Action(() => result = dlg.ShowDialog()));
			else
				result = dlg.ShowDialog();
			if (result == DialogResult.OK)
				return dlg.FileName;
			return string.Empty;
		}

		protected override ScanData DoTask(ScanData source)
		{
			LastError = string.Empty;
			{
				if (String.IsNullOrEmpty(Filename))
				{
					string file = ShowDialog();
					if (!string.IsNullOrEmpty(file))
						this.Filename = file;
				}


				if (!String.IsNullOrEmpty(Filename))
				{
					this.Status = eTaskStatus.Working;
					UpdatePercent(0, source);
					Save(source);
					this.Status = eTaskStatus.Finished;
					UpdatePercent(100, source);
				}
				else
				{
					this.Status = eTaskStatus.Error;
					LastError = String.Format("Invalid File {0}", this.Filename);
				}
				return source;
			}
		}

		protected virtual void Save(ScanData source)
		{
			ScanDataIO.Write(Filename, source);
		}
		public override string Name
		{
			get { return "Save Points"; }
		}
		public override string DisplayName
		{
			get
			{
				if (!string.IsNullOrEmpty(Filename))
					return String.Format("Save: \"{0}\"", Path.GetFileName(Filename));
				return base.DisplayName;
			}
		}

		public override bool RunSettings()
		{
			string ret = ShowDialog();
			if (!string.IsNullOrEmpty(ret))
				Filename = ret;
			return true;
		}
		public override bool HasSettings
		{
			get
			{
				return true;
			}
		}

	}

	/// <summary>
	/// Save Mesh As MeshFile
	/// </summary>
	public class SaveMesh : SavePoints
	{
		public override eTaskItem In
		{
			get
			{
				return eTaskItem.Mesh;
			}
		}
		public override string Name
		{
			get
			{
				return "Save Mesh";
			}
		}
	}
	/// <summary>
	/// Save Mesh As STL
	/// </summary>
	[Browsable(true)]
	public class SaveStl : SaveMesh
	{
		public override eTaskItem In
		{
			get { return eTaskItem.Mesh; }
		}

		protected override void Save(ScanData source)
		{
			StlIO.Write(this.Filename, source);
		}
		public override string DialogFilter
		{
			get
			{
				return StlIO.GetDialogFilter();
			}
		}
		public override string Name
		{
			get { return "Save STL"; }
		}
	}


	/// <summary>
	/// Save Mesh As Ply
	/// </summary>
	[Browsable(true)]
	public class SavePly : SaveMesh
	{
		public override eTaskItem In
		{
			get { return eTaskItem.ScanLines; }
		}

		protected override void Save(ScanData source)
		{
			PLYIO.Write(this.Filename, source);
		}
		public override string DialogFilter
		{
			get
			{
				return PLYIO.GetDialogFilter();
			}
		}
		public override string Name
		{
			get { return "Save PLY"; }
		}
	}


	/// <summary>
	/// Save Mesh As XYZ
	/// </summary>
	[Browsable(true)]
	public class SaveXyz : SaveMesh
	{
		public override eTaskItem In
		{
			get { return eTaskItem.ScanLines; }
		}

		protected override void Save(ScanData source)
		{
			XYZIO.Write(this.Filename, source);
		}
		public override string DialogFilter
		{
			get
			{
				return XYZIO.GetDialogFilter();
			}
		}
		public override string Name
		{
			get { return "Save XYZ"; }
		}
	}

	/// <summary>
	/// Save Mesh As OBJ
	/// </summary>
	[Browsable(true)]
	public class SaveOBJ : SaveMesh
	{
		public override eTaskItem In
		{
			get { return eTaskItem.Mesh; }
		}

		protected override void Save(ScanData source)
		{
			WaveFormIO.Write(this.Filename, source);
		}
		public override string DialogFilter
		{
			get
			{
				return WaveFormIO.GetDialogFilter();
			}
		}
		public override string Name
		{
			get { return "Save OBJ"; }
		}
	}

}
