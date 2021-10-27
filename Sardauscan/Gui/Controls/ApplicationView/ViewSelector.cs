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
using Sardauscan.Core;

namespace Sardauscan.Gui.Controls.ApplicationView
{
	public partial class ViewSelector : UserControl
	{
		/// <summary>
		/// Default ctor
		/// </summary>
		public ViewSelector()
		{
			InitializeComponent();
		}

		ViewControler _Controler;
		public ViewControler Controler 
		{
			get { return _Controler; }
			set
			{
				if (value != _Controler)
				{
					if (_Controler != null)
						_Controler.OnLockChange -= AlignOnOff;
					_Controler = value;
					if(_Controler!=null)
						_Controler.OnLockChange += AlignOnOff;

				}
			}
		}
		public void AlignToControler()
		{
			if (this.IsDesignMode())
			{
				var values = Enum.GetValues(typeof(ViewType));
				foreach (ViewType type in values)
					CreateButton(type);
			}
			else
			{
				ViewControler controler = Controler;
				if (controler != null)
				{
					List<ViewType> list = controler.RegisterdViews;
					list.Sort();
					foreach (ViewType type in list)
						CreateButton(type, controler.IsCurrent(type));
				}
				AlignOnOff();
			}

		}
		public void AlignOnOff()
		{
			ViewControler controler = Controler;
			if (controler != null)
			{
				foreach (Control ctrl in this.flowLayoutPanel1.Controls)
				{
					if (ctrl is StatusImageButton && ctrl.Tag != null)
					{
						ViewType type = (ViewType)(ctrl.Tag);
						bool on = controler.IsCurrent(type);
						((StatusImageButton)ctrl).On = on;
						if (!on)
							ctrl.Enabled = !controler.Lock;
						ctrl.Enabled = ctrl.Enabled && controler.IsEnabled(type);
					}
				}
			}
		}
		private void ViewSelector_Load(object sender, EventArgs e)
		{
			this.ToolTip.ForeColor = SkinInfo.ForeColor;
			this.ToolTip.BackColor = SkinInfo.BackColor;
			if (!this.IsDesignMode())
			{
				ViewControler controler = Controler;
				if (controler != null)
				{
					controler.OnViewListChange += this.AlignToControler;
					controler.OnViewChanged += this.AlignOnOff;
				}
			}
			AlignToControler();
		}

		private void CreateButton(ViewType type, bool on = true)
		{
			int size = this.flowLayoutPanel1.Height;
			StatusImageButton button = new StatusImageButton();
			this.flowLayoutPanel1.Controls.Add(button);
			button.Image = type.Bitmap();
			button.Location = new System.Drawing.Point(0, 0);
			button.Name = type.ToString();
			button.Tag = type;
			button.OffImageType = Sardauscan.Gui.Controls.eOffButtonType.NotSelected;
			button.On = on;
			button.Size = new System.Drawing.Size(size, size);
			button.Click += OnButtonClick;
			button.Margin = new Padding(0);
			this.ToolTip.SetToolTip(button, type.ToString());
			flowLayoutPanel1.Width = size * flowLayoutPanel1.Controls.Count;
			flowLayoutPanel1.Height = size;
		}
		public void OnButtonClick(object sender, EventArgs e)
		{
			ViewControler controler = Controler;
			if (controler != null && !controler.Lock)
			{
				StatusImageButton ctrl = sender as StatusImageButton;
				if (ctrl != null && ctrl.Tag != null)
				{
					ViewType type = (ViewType)(ctrl.Tag);
					controler.ChangeView(type);
					AlignOnOff();
				}
			}
		}
	}
}
