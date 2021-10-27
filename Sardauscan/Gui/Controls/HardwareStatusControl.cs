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
using Sardauscan.Core.Interface;
using Sardauscan.Gui.Controls.ApplicationView;

namespace Sardauscan.Gui.Controls
{
	public partial class HardwareStatusControl : UserControl, IDisposable
	{
		/// <summary>
		/// Default ctor
		/// </summary>
		public HardwareStatusControl()
		{
			InitializeComponent();
			Application.Idle +=OnIdle;
			if (!this.IsDesignMode())
				Settings.RegisterInstance(this, true);
			LoadHardware<ILaserProxy>();
			LoadHardware<ITurnTableProxy>();
			LoadHardware<ICameraProxy>();

		}
		/// <summary>
		/// Dispose object
		/// </summary>
		public new void Dispose()
		{
			base.Dispose();
			Application.Idle -= OnIdle;

		}
		void LoadHardware<T>()
		{
			Settings set = Settings.Get<Settings>();
			if (set != null)
			{
				try
				{
					string proxystr = set.Read(Settings.LAST_USED, Settings.PROXY<T>(), string.Empty);
					if(String.IsNullOrEmpty(proxystr))
						return;
					Type proxyType = Reflector.GetType(proxystr);
					if (proxyType == null)
						return;
					if (Settings.Get(proxyType) != null)
						return;
					/// change and use the provider instead of the proxy, to avoir creation of proxy just to create the last used
					IHardwareProxy proxy = Reflector.CreateInstance<IHardwareProxy>(proxyType);
					string hardwareId = set.Read(Settings.LAST_USED, Settings.HARDWAREID<T>(), string.Empty);
					proxy = proxy.LoadFromHardwareId(hardwareId);
					Settings.RegisterInstance(proxy);
				}
				catch
				{
				}
			}
		}
		void SaveHardware()
		{
			SaveHardware<ILaserProxy>();
			SaveHardware<ITurnTableProxy>();
			SaveHardware<ICameraProxy>();
		}
		void SaveHardware<T>()
		{
			Settings set = Settings.Get<Settings>();
			if (set != null )
			{

				IHardwareProxy proxy = Settings.Get<T>() as IHardwareProxy;
				if (proxy != null)
				{
					set.Write(Settings.LAST_USED, Settings.PROXY<T>(), proxy.GetType().AssemblyQualifiedName);
					set.Write(Settings.LAST_USED, Settings.HARDWAREID<T>(), proxy.HardwareId);
				}
				else
				{
					set.Write(Settings.LAST_USED, Settings.PROXY<T>(), string.Empty);
					set.Write(Settings.LAST_USED, Settings.HARDWAREID<T>(), string.Empty);
				}
			}
		}
		protected void NotifyChange()
		{
			SaveHardware();
			ViewControler controler = Settings.Get<ViewControler>();
			if (controler != null)
				controler.FireLockChange();
		}
		public eHardware Hardware { get; private set; }
		void AlignInterface()
		{
			this.Hardware = eHardwareExt.GetFromSettings();
			this.CameraButton.On = this.Hardware.IsSet(eHardware.Camera);
			this.TableButton.On = this.Hardware.IsSet(eHardware.Table);
			this.LaserButton.On = this.Hardware.IsSet(eHardware.Laser);
		}

		IHardwareProxy GetAssociatedProxy(StatusImageButton button)
		{
			if (button == this.TableButton) return Settings.Get<ITurnTableProxy>();
			if (button == this.LaserButton) return Settings.Get<ILaserProxy>();
			if (button == this.CameraButton) return Settings.Get<ICameraProxy>();
			return null;
		}
		private void OnIdle(object sender, EventArgs e)
		{
			try
			{
				bool ignore = false;
				if (!ignore && Visible)
				{
					AlignInterface();
				}
			}
			catch
			{
			}
		}

		private void Button_Click(object sender, EventArgs e)
		{
			NotifyChange();
			if (sender is StatusImageButton)
			{
				bool on = ((StatusImageButton)sender).On;
				Type interfaceType = GetAssociatedType(sender);
				if (!on && interfaceType != typeof(void))
				{
					List<IHardwareProxyProvider> providers = GetAvailableProvider(interfaceType);
					if (providers.Count == 1)
						LaunchProvider(providers[0]);
					else
					{
						ContextMenu cm = new ContextMenu();
						foreach (IHardwareProxyProvider prov in providers)
						{
							MenuItem mi = new MenuItem(prov.Name);
							mi.Tag = prov;
							mi.Click += MenuItem_Click;
							cm.MenuItems.Add(mi);
						}
						StatusImageButton ctl = ((StatusImageButton)sender);
						cm.Show(ctl, new Point(ctl.Width, ctl.Height));
					}
				}
				else
					ShowHideHardwareView((StatusImageButton)sender);
			}
		}
		private void MenuItem_Click(object sender, EventArgs e)
		{
			if (sender is MenuItem)
			{
				var mi = (MenuItem)sender;
				IHardwareProxyProvider provider = ((IHardwareProxyProvider)mi.Tag);
				LaunchProvider(provider);
				AlignInterface();
			}
		}
		public void LaunchProvider(IHardwareProxyProvider provider)
		{
			/// else find the type selectorDialog and register 
			object proxy = provider.Select(this);
			if (proxy != null)
			{
				Settings.RegisterInstance(proxy, true);
				NotifyChange();
			}
			AlignInterface();
		}
		private void UnregisterInstance(IHardwareProxyProvider provider)
		{
			UnregisterInstance(provider.GenerateType);
		}
		private void UnregisterInstance(Type type)
		{
			object cont = Settings.Get(type);
			if (cont != null)
			{
				Settings.UnRegisterInstance(cont, true);
				NotifyChange();
			}
			ClearSettingsPanel();
		}
		private void ShowHideHardwareView(StatusImageButton button)
		{
			IHardwareProxy proxy = GetAssociatedProxy(button);
			if (proxy != null)
			{
				Control viewer = proxy.GetViewer();
				if (viewer == null || (SettingsPanel.Controls.Count > 0 && SettingsPanel.Controls[0].GetType() == viewer.GetType()))
				{
					ClearSettingsPanel();
					if (viewer is IDisposable)
						((IDisposable)viewer).Dispose();
				}
				else
					ShowSettingsPanel(viewer);
			}
		}
		Type GetAssociatedType(object sender)
		{
			if (sender == this.CameraButton)
				return typeof(ICameraProxy);
			if (sender == this.LaserButton)
				return typeof(ILaserProxy);
			else if (sender == this.TableButton)
				return typeof(ITurnTableProxy);
			return typeof(void);
		}
		List<IHardwareProxyProvider> GetAvailableProvider(Type baseType)
		{
			return Reflector.GetProviders(baseType);
		}

		private void HideSettings_Click(object sender, EventArgs e)
		{
			ClearSettingsPanel();
		}
		private void ClearSettingsPanel()
		{
			IDisposable disp = SettingsPanel.Controls.Count > 0 ? SettingsPanel.Controls[0] as IDisposable : null;
			SettingsPanel.Controls.Clear();
			if (disp != null)
				disp.Dispose();
			SettingsPanel.Height = 0;
			this.HideSettings.Visible = false;
			NotifyChange();
		}

		Control _settingsPanel = null;
		public Control SettingsPanel
		{
			get
			{
				if (_settingsPanel == null)
					return this.ProxySettingPanel;
				else
					return _settingsPanel;
			}
			set { _settingsPanel = value; }

		}
		private void ShowSettingsPanel(Control viewer)
		{
			ClearSettingsPanel();
			if (viewer != null)
			{
				viewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
							| System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
				viewer.Location = new System.Drawing.Point(0, 0);
				viewer.Dock = DockStyle.Fill;
				SettingsPanel.Height = viewer.Height;
				SettingsPanel.Controls.Add(viewer);
				this.HideSettings.Visible = true;
			}
		}

		private void HardwareStatusControl_Load(object sender, EventArgs e)
		{
			ToolTip.SetToolTip(this.TableButton, "Turn Table");
			ToolTip.SetToolTip(this.CameraButton, "Camera");
			ToolTip.SetToolTip(this.LaserButton, "Laser(s)");
			ToolTip.SetToolTip(this.HideSettings, "Hide Settings");

		}

		private void TableButton_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				if (sender is StatusImageButton)
				{
					StatusImageButton button = (StatusImageButton)sender;
					if (button.On)
					{
							ContextMenu cm = new ContextMenu();
							MenuItem mi = new MenuItem("Remove");
							mi.Tag = button;
							mi.Click += RemoveMenu_Click;
							cm.MenuItems.Add(mi);
							cm.Show(button, new Point(e.X, e.Y));
					}
				}
			}
		}

		private void RemoveMenu_Click(object sender, EventArgs e)
		{
			if (sender is MenuItem)
			{
				var mi = (MenuItem)sender;
				Type interfaceType = GetAssociatedType(mi.Tag);
				if (interfaceType != typeof(void))
					UnregisterInstance(interfaceType);
				AlignInterface();
			}
		}

		private void Button_DoubleClick(object sender, MouseEventArgs e)
		{

		}



	}
	[Flags]
	public enum eHardware
	{
		None = 0,
		Camera = 1,
		Table = 2,
		Laser = 4,
		All = Camera | Table | Laser
	};
	public static class eHardwareExt
	{
		public static bool IsSet(this eHardware val, eHardware set)
		{
			return (val & set) == set;
		}
		public static eHardware GetModified(eHardware val, eHardware flag, bool set)
		{
			eHardware ret = val;
			if (set)
				ret |= flag;
			else
				ret &= ~flag;
			return ret;
		}
		public static eHardware GetFromInstance(ICameraProxy cam, ITurnTableProxy table, ILaserProxy laser)
		{
			eHardware ret = eHardware.None;
			ret = GetModified(ret, eHardware.Camera, cam != null);
			ret = GetModified(ret, eHardware.Table, table != null);
			ret = GetModified(ret, eHardware.Laser, laser != null);
			return ret;
		}
		public static eHardware GetFromSettings()
		{
			eHardware ret = eHardware.None;
			ret = GetFromInstance(Settings.Get<ICameraProxy>(), Settings.Get<ITurnTableProxy>(), Settings.Get<ILaserProxy>());
			return ret;
		}
	};
}
