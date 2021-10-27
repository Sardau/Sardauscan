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
using System.Drawing.Imaging;
using Sardauscan.Core;

namespace Sardauscan.Gui.Controls
{
	public partial class LaserControl : UserControl
	{
		/// <summary>
		/// Default ctor
		/// </summary>
		public LaserControl()
		{
			InitializeComponent();
			OffImage = global::Sardauscan.Properties.Resources.Spot_Off;
			OnImage = global::Sardauscan.Properties.Resources.Spot_On;
			ColorizeLaser(this.button1);
			ColorizeLaser(this.button2);
			ColorizeLaser(this.button3);
			ColorizeLaser(this.RightButton);
			AlignControls();
		}
		public void ColorizeLaser(StatusImageButton button)
		{
			button.Image = SkinInfo.Colorize(OnImage,GetLaserColor(button));
		}
		Color GetLaserColor(StatusImageButton button)
		{
			int laserIndex = -1;
			try
			{
				laserIndex = int.Parse(button.Tag.ToString());
			}
			catch { laserIndex = -1; }
			Settings settings = Settings.Get<Settings>();
			if (settings != null && laserIndex >= 0)
				return settings.Read(Settings.LASER(laserIndex), Settings.DEFAULTCOLOR, LaserInfo.GetDefaultColor(laserIndex));
			else
				return LaserInfo.GetDefaultColor(laserIndex);
		}

		/// <summary>
		/// Dispose object
		/// </summary>
		public new void Dispose()
		{
			if (!this.IsDesignMode())
				Settings.UnRegisterInstance(this);
		}
		Image OffImage;
		Image OnImage;
		public void AlignSubControl(Control control)
		{
			if(control!=null)
				foreach (Control ctrl in control.Controls)
			{
				if (ctrl is StatusImageButton)
				{
					StatusImageButton button = (StatusImageButton)ctrl;
					int i = -1;
					try
					{
						i = int.Parse(ctrl.Tag.ToString());
					}
					catch { }
					//                    int i = ctrl.Tag == null ? -1 : (int)int.Parse(ctrl.Tag.ToString());
					ctrl.Visible = Proxy != null && i >= 0 && i < Proxy.Count;
					bool on = Proxy != null && Proxy.On(i);
					button.On = on;
				}
				else
					AlignSubControl(ctrl);
			}
		}
		public void AlignControls()
		{
			AlignSubControl(this);
		}

		protected ILaserProxy m_Proxy = null;
		public ILaserProxy Proxy
		{
			get { return m_Proxy; }
			set { m_Proxy = value; Enabled = (value != null); AlignControls(); }
		}

		private void button_Click(object sender, EventArgs e)
		{
			if (Proxy == null)
				return;
			int i = -1;
			try
			{
				i = int.Parse(((StatusImageButton)sender).Tag.ToString());
			}
			catch
			{ }

			if (i >= 0 && i < Proxy.Count)
			{
				Proxy.Turn(i, !Proxy.On(i));
				AlignControls();
			}

		}
	}
}
