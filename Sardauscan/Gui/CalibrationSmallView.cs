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
