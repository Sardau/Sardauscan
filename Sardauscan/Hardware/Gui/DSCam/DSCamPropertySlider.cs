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
using Sardauscan.Hardware;
using Sardauscan.Gui;

namespace Sardauscan.Hardware.Gui.DSCam
{
	public partial class DSCamPropertySlider : UserControl
	{
		public DSCamPropertySlider()
		{
			InitializeComponent();
			this.TrackBar.BackColor = SkinInfo.BackColor;
		}

		DirectShowLib.CameraControlProperty? _CamProperty;
		public DirectShowLib.CameraControlProperty? CamProperty
		{
			get { return _CamProperty; }
			set
			{
				_CamProperty = value;
				this.NameLabel.Text = CamProperty.ToString();
				AlignFromCamera();
			}
		}
		DSCameraProxy _Camera = null;
		public DSCameraProxy Camera
		{
			get { return _Camera; }
			set
			{
				_Camera = value;
				AlignFromCamera();
			}
		}

		DSCameraProxy.ControlPropertyInfo GetPropertyInfo(bool getData=true) 
		{
			try
			{
				if (Camera != null && CamProperty!=null)
				{
					return Camera.GetControlPropertyInfo((DirectShowLib.CameraControlProperty)CamProperty, getData);
				}
			}
			catch
			{
				return null;
			}
			return null;
		} 

		public bool LoadingProps = false;
		private void AlignFromCamera()
		{
			DSCameraProxy.ControlPropertyInfo info = GetPropertyInfo();
			this.Enabled = info != null;
			if (info != null)
			{
				LoadingProps = true;
				try
				{
					this.TrackBar.Minimum = info.Min;
					this.TrackBar.Maximum = info.Max;
					this.TrackBar.SmallChange = info.Delta;
					this.TrackBar.LargeChange = info.Delta * 2;
					this.TrackBar.Value = info.Value;
					this.AutoCheckBox.Checked = info.ValueFlags == DirectShowLib.CameraControlFlags.Auto;
					this.TrackBar.Enabled = !this.AutoCheckBox.Checked;
					if (this.AutoCheckBox.Checked)
						this.ValueLabel.Text = "Auto";
					else
						this.ValueLabel.Text = info.Value.ToString();
				}
				catch
				{
					this.Enabled = false;
				}
				LoadingProps = false;
			}

			if(!this.Enabled)
				this.ValueLabel.Text = string.Empty;
		}
		private void AlignCamera()
		{
			if (this.Enabled && !LoadingProps)
			{
				if (Camera != null && CamProperty!=null)
				{
					if (this.AutoCheckBox.Checked)
						Camera.SetControlPropertyDefault((DirectShowLib.CameraControlProperty)CamProperty);
					else
						Camera.SetControlProperty((DirectShowLib.CameraControlProperty)CamProperty, TrackBar.Value);
				}
				AlignFromCamera(); // make sure interface stick to camera props
			}
		}

		private void AutoCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			AlignCamera();
		}

		private void TrackBar_ValueChanged(object sender, EventArgs e)
		{
			AlignCamera();
		}

	}
}
