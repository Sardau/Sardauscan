#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
# endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sardauscan.Core.Geometry;
using System.Windows.Forms;
using Sardauscan.Core.IO;
using System.ComponentModel;
using System.IO;

namespace Sardauscan.Core.ProcessingTask
{
	/// <summary>
	/// Open a file 
	/// </summary>
    public class LineOpenFileTask : AbstractProcessingTask
    {
        private string m_FileName = string.Empty;
        [Browsable(false)]
        [Description("Source file for thumbnail and web images")]
        [EditorAttribute(typeof(Point3dArrayIOOpenFileEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Filename { get { return m_FileName; } set { m_FileName = value; } }

        public override string Name
        {
            get
            {
                return "Open "+ScanDataIO.DefaultExtention;
            }
        }
        public override string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(Filename) && File.Exists(Filename))
                    return  String.Format("Open: \"{0}\"",Path.GetFileName(Filename));
                return base.DisplayName;
            }
        }
        public override eTaskItem In
        {
            get { return eTaskItem.None; }
        }

        public override eTaskItem Out
        {
            get { return eTaskItem.ScanLines; }
        }
        public override eTaskType TaskType { get { return eTaskType.IO; } }

				/// <summary>
				/// Clone this
				/// </summary>
				/// <returns></returns>
				public override AbstractProcessingTask Clone()
        {
            LineOpenFileTask ret = (LineOpenFileTask )Activator.CreateInstance(this.GetType());
            ret.Filename = Filename;
            return ret;
        }

				/// <summary>
				/// Clone this
				/// </summary>
				/// <returns></returns>
				protected bool ValidFile
        {
            get
            {
                return (!string.IsNullOrEmpty(Filename) && File.Exists(Filename));
            }
        }
				public override ScanData DoTask(ScanData source)
        {
					LastError = string.Empty;
            if (!ValidFile)
            {
               string file =ShowDialog();
               if(!string.IsNullOrEmpty(file))
                   this.Filename = file;
            }


						if (ValidFile)
						{
							this.Status = eTaskStatus.Working;
							UpdatePercent(0, null);
							ScanData ret = ScanDataIO.Read(this.Filename);
							this.Status = eTaskStatus.Finished;
							UpdatePercent(100, ret);
							return ret;
						}
						else
						{
							this.Status = eTaskStatus.Error;
							LastError = String.Format("Invalid File {0}", this.Filename);
						}
					return null;
        }

        string ShowDialog()
        {
            DialogResult result = DialogResult.Ignore;
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = ScanDataIO.GetDialogFilter();
						dlg.InitialDirectory = Program.UserDataPath;
            dlg.CheckFileExists = true;
            if (CallerControl != null)
                CallerControl.Invoke(new Action(() => result = dlg.ShowDialog()));
            else
                result = dlg.ShowDialog();
            if (result == DialogResult.OK)
                return dlg.FileName;
            return string.Empty;
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
}
