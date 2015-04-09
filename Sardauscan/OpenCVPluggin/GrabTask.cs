using Sardauscan.Core;
using Sardauscan.Core.Interface;
using Sardauscan.Core.ProcessingTask;
using Sardauscan.Gui.PropertyGridEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenCVPluggin
{
    public class GrabTask : AbstractProcessingTask
    {
        public override eTaskItem In
        {
            get { return eTaskItem.None; }
        }

        public override eTaskItem Out
        {
            get { return eTaskItem.File; }
        }

        public override AbstractProcessingTask Clone()
        {
            return (AbstractProcessingTask)this.MemberwiseClone();
        }

        #region Proxy
        ITurnTableProxy TurnTable
        {
            get { return Settings.Get<ITurnTableProxy>(); }
        }
        ILaserProxy Laser
        {
            get { return Settings.Get<ILaserProxy>(); }
        }
        ICameraProxy Camera
        {
            get { return Settings.Get<ICameraProxy>(); }
        }

        bool HardwareAvailable
        {
            get
            {
                return TurnTable != null && Laser != null && Camera != null; ;
            }
        }
        public override bool Ready
        {
            get
            {
                return this.HardwareAvailable;
            }
        }
        public override string ToolTip
        {
            get
            {
                if (!Ready)
                {
                    return string.Format("HardWare missing : TURNTABLE:{0} LASER:{1} CAMERA:{2}", HardwarePresentTrace(TurnTable), HardwarePresentTrace(Laser), HardwarePresentTrace(Camera));
                }
                return base.ToolTip;
            }
        }
        private string HardwarePresentTrace(object obj)
        {
            return obj == null ? "FAILED" : "OK";
        }
        protected Bitmap GetCapture()
        {
            Bitmap img = null;


            if (CallerControl != null)
            {
                if (CallerControl.InvokeRequired)
                    CallerControl.Invoke(new Action(() => img = Camera.AcquireImage()));
                else
                    img = Camera.AcquireImage();
            }
            return img;
        }
        #endregion

        private int _NumberOfGrab = 70;

        [Browsable(true)]
        [Description("number of image")]
        [DisplayName("number of image")]
        [TypeConverter(typeof(NumericUpDownTypeConverter))]
        [Editor(typeof(NumericUpDownTypeEditor), typeof(UITypeEditor)), MinMaxAttribute(4f, 720f, 1f, 0)]
        public int NumberOfGrab { get { return _NumberOfGrab; } set { _NumberOfGrab = value; } }
        
        private string _Folder = "capture";
        [Browsable(true)]
        [Description("Save captures files")]
        [DisplayName("Folder")]
        [EditorAttribute(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public String Folder { get { return _Folder; } set { _Folder = value; } }

        public override Sardauscan.Core.ScanData DoTask(Sardauscan.Core.ScanData source)
        {
            if (!HardwareAvailable)
                throw new Exception(string.Format("HardWare missing : TURNTABLE:{0} LASER:{1} CAMERA:{2}", HardwarePresentTrace(TurnTable), HardwarePresentTrace(Laser), HardwarePresentTrace(Camera)));

            double RotationStep = (double)Math.Round(360.0 / (NumberOfGrab-1), 2);
            ScanData ret = new ScanData();
            UpdatePercent(0, ret);

            TurnTable.InitialiseRotation();
            Laser.TurnAll(false);
            int index = 0;    
            for (double currentAngle = 0; currentAngle < 360f; currentAngle += RotationStep)
            {
                if (this.CancelPending) return ret;
                Bitmap imgoff = GetCapture();
                string path = Path.Combine(Folder, string.Format("capture{0}.jpg", ++index,(int)currentAngle));
                imgoff.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

                int percent = (int)((currentAngle / 360f) * 100f);
                UpdatePercent(percent, ret);
                TurnTable.Rotate(currentAngle, false);
                Thread.Sleep(1000);
            }

            return null;
        }

        public override string Name
        {
            get { return "grab pictures"; }
        }
    }
}
