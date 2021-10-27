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
using Sardauscan.Gui.CalibrationSteps;
using Sardauscan.Gui.Controls;

namespace Sardauscan.Gui
{
	public partial class CalibrationSmallView : UserControl, IMainView
	{
		/// <summary>
		/// Default ctor
		/// </summary>
		public CalibrationSmallView()
		{
			InitializeComponent();
		}

		public CalibrationBigView Viewer { get; set; }

		protected ITurnTableProxy TableProxy { get { return Settings.Get<ITurnTableProxy>(); } }
		protected ICameraProxy CameraProxy { get { return Settings.Get<ICameraProxy>(); } }
		protected ILaserProxy LaserProxy { get { return Settings.Get<ILaserProxy>(); } }
		public bool Available
		{
			get
			{
				return CameraProxy != null && LaserProxy != null && TableProxy != null;
			}
		}

		private void CalibrationSmallView_Load(object sender, EventArgs e)
		{
			if (!this.IsDesignMode())
			{
				List<Type> typeList = Reflector.GetAssignableFrom(typeof(ICalibrationStepInfo));
				List<ICalibrationStepInfo> infos = new List<ICalibrationStepInfo>(typeList.Count);
				foreach (Type type in typeList)
				{
					ICalibrationStepInfo info = Reflector.CreateInstance<ICalibrationStepInfo>(type);
					if (info != null)
						infos.Add(info);
				}
				infos = infos.OrderBy(x => x.OrderId).ToList();
				foreach (ICalibrationStepInfo info in infos)
					CreateButton(info);
				if (_buttons.Count > 0)
					SetSelected(_buttons[0]);
			}

		}
		List<StatusImageButton> _buttons = new List<StatusImageButton>();
		void SetSelected(StatusImageButton button)
		{
			ICalibrationStepInfo info = button.Tag as ICalibrationStepInfo;
			if (info != null)
			{
				Control view = info.CreateControl();
				foreach (StatusImageButton b in _buttons)
					b.On = (b == button);
				Viewer.SetCurrentView(view);
			}
		}
		void CreateButton(ICalibrationStepInfo info)
		{
			StatusImageButton button = new StatusImageButton();
			button.Size= new Size(this.FlowPanel.Width-10,64);
			button.Text = info.Label;
			button.Tag = info;
			button.Image = info.Image();
			button.OffImageType = eOffButtonType.NotSelected;
			button.Click += new EventHandler(Button_Click);
			FlowPanel.Controls.Add(button);
			_buttons.Add(button);
		}

		void Button_Click(object sender, EventArgs e)
		{
			if (Viewer != null)
			{
				if (sender is StatusImageButton)
				{
					SetSelected((StatusImageButton)sender);
				}
			}
		}


		private void FlowSizeChanged(object sender, EventArgs e)
		{
			foreach (Control c in FlowPanel.Controls)
			{
				c.Width = FlowPanel.Width - 20;
			}
		}
	}
}
