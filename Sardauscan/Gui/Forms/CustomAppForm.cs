#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#region BASED ON Custom Border Forms - Copyright (C) 2005 Szymon Kobalczyk

// Custom Border Forms
// Copyright (C) 2005 Szymon Kobalczyk
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.

// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//
// Szymon Kobalczyk (http://www.geekswithblogs.com/kobush)

#endregion
#endregion
#region Custom Border Forms - Copyright (C) 2005 Szymon Kobalczyk

// Custom Border Forms
// Copyright (C) 2005 Szymon Kobalczyk
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.

// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//
// Szymon Kobalczyk (http://www.geekswithblogs.com/kobush)

#endregion

#region Using directives

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

using System.Reflection; // used for logging 
using Sardauscan.Core;
using Sardauscan.Properties;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Sardauscan.Gui.Forms.CustomForm;
using Sardauscan.Gui.Controls; // used for logging 

#endregion

namespace Sardauscan.Gui.Forms
{
	/// <summary>
	/// Extended form class that suports drawing in non-client area.
	/// </summary>
	public class CustomAppForm : Form
	{
		#region Constructor

		/// <summary>
		/// Default ctor
		/// </summary>
		public CustomAppForm()
		{
			this.Layout += new LayoutEventHandler(NonClientBaseForm_Layout);
			ClientPaddingInfo = new System.Windows.Forms.Padding(Skin.BorderWidth, 2 * Skin.BorderWidth + Skin.IconSize, Skin.BorderWidth, Skin.BorderWidth);
			InitSystemButtons();
			UpdateButtonSkin();
		}
		private void InitSystemButtons()
		{
			_closeButton = new CustomCaptionButton();
			_closeButton.Key = CaptionButtonKey.Close;
			_closeButton.Visible = true;
			_closeButton.HitTestCode = (int)NativeMethods.NCHITTEST.HTCLOSE;
			_closeButton.SystemCommand = (int)NativeMethods.SystemCommands.SC_CLOSE;
			_captionButtons.Add(_closeButton);

			_restoreButton = new CustomCaptionButton();
			_restoreButton.Key = CaptionButtonKey.Restore;
			_restoreButton.Enabled = this.MaximizeBox;
			_restoreButton.HitTestCode = (int)NativeMethods.NCHITTEST.HTMAXBUTTON;
			_restoreButton.SystemCommand = (int)NativeMethods.SystemCommands.SC_RESTORE;
			_captionButtons.Add(_restoreButton);

			_maximizeButton = new CustomCaptionButton();
			_maximizeButton.Key = CaptionButtonKey.Maximize;
			_maximizeButton.Enabled = this.MaximizeBox;
			_maximizeButton.HitTestCode = (int)NativeMethods.NCHITTEST.HTMAXBUTTON;
			_maximizeButton.SystemCommand = (int)NativeMethods.SystemCommands.SC_MAXIMIZE;
			_captionButtons.Add(_maximizeButton);

			_minimizeButton = new CustomCaptionButton();
			_minimizeButton.Key = CaptionButtonKey.Minimize;
			_minimizeButton.Enabled = this.MinimizeBox;
			_minimizeButton.HitTestCode = (int)NativeMethods.NCHITTEST.HTMINBUTTON;
			_minimizeButton.SystemCommand = (int)NativeMethods.SystemCommands.SC_MINIMIZE;
			_captionButtons.Add(_minimizeButton);

			_helpButton = new CustomCaptionButton();
			_helpButton.Key = CaptionButtonKey.Help;
			_helpButton.Visible = this.HelpButton;
			_helpButton.HitTestCode = (int)NativeMethods.NCHITTEST.HTHELP;
			_helpButton.SystemCommand = (int)NativeMethods.SystemCommands.SC_CONTEXTHELP;
			_captionButtons.Add(_helpButton);

			_settingsButton = new CustomCaptionButton();
			_settingsButton.Key = CaptionButtonKey.Settings;
			_settingsButton.Visible = true;
			_settingsButton.HitTestCode = (int)NativeMethods.NCHITTEST.HTOBJECT;
			_settingsButton.SystemCommand = (int)-1;
			_captionButtons.Add(_settingsButton);


			_aboutButton = new CustomCaptionButton();
			_aboutButton.Key = CaptionButtonKey.About;
			_aboutButton.Visible = true;
			_aboutButton.HitTestCode = (int)NativeMethods.NCHITTEST.HTHELP;
			_aboutButton.SystemCommand = (int)-1;
			_captionButtons.Add(_aboutButton);

			OnUpdateWindowState();
		}

		#endregion

		#region CreateParams

		protected override System.Windows.Forms.CreateParams CreateParams
		{
			get
			{
				Int32 CS_VREDRAW = 0x1;
				Int32 CS_HREDRAW = 0x2;
				Int32 CS_OWNDC = 0x20;
				CreateParams cp = base.CreateParams;
				cp.ClassStyle = cp.ClassStyle | CS_VREDRAW | CS_HREDRAW | CS_OWNDC;
				return cp;
			}
		}

		#endregion

		#region Variables

		private bool _nonClientAreaDoubleBuffering;
		private bool _enableNonClientAreaPaint = true;

		// Shortcuts to the standards buttons
		private CustomCaptionButton _closeButton;
		private CustomCaptionButton _restoreButton;
		private CustomCaptionButton _maximizeButton;
		private CustomCaptionButton _minimizeButton;
		private CustomCaptionButton _helpButton;
		private CustomCaptionButton _settingsButton;
		private CustomCaptionButton _aboutButton;

		private List<CustomCaptionButton> _captionButtons = new List<CustomCaptionButton>();


		NativeMethods.TRACKMOUSEEVENT _trackMouseEvent;
		bool _trakingMouse = false;

		Icon _smallIcon;


		private Padding ClientPaddingInfo;
		private Padding TitlePadding = new Padding();


		private SkinInfo Skin = new SkinInfo();

		private Color _ActiveTitleBackgroundColor = SkinInfo.ActiveTitleBackColor;
		private Color _InactiveTitleBackgroundColor = SkinInfo.InactiveTitleBackColor;
		private Color _ActiveTitleTextColor = SkinInfo.ActiveTitleTextColor;
		private Color _InactiveTitleTextColor = SkinInfo.InactiveTitleTextColor;
		private Font _TitleFont = System.Drawing.SystemFonts.CaptionFont;
		#endregion

		#region Properties
		[Browsable(false)]
		public bool IsActive
		{
			get { return this.IsDesignMode() || Form.ActiveForm != null; }
		}

		[Browsable(false)]
		public Color TitleBackgroundColor
		{
			get { return IsActive ? ActiveTitleBackgroundColor : InactiveTitleBackgroundColor; }
		}
		public Color ActiveTitleBackgroundColor
		{
			get { return _ActiveTitleBackgroundColor; }
			set { _ActiveTitleBackgroundColor = value; Invalidate(); }
		}
		public Color InactiveTitleBackgroundColor
		{
			get { return _InactiveTitleBackgroundColor; }
			set { _InactiveTitleBackgroundColor = value; Invalidate(); }
		}
		[Browsable(false)]
		public Color TitleTextColor
		{
			get { return IsActive ? ActiveTitleTextColor : InactiveTitleTextColor; }
		}
		public Color ActiveTitleTextColor
		{
			get { return _ActiveTitleTextColor; }
			set { _ActiveTitleTextColor = value; Invalidate(); }
		}
		public Color InactiveTitleTextColor
		{
			get { return _InactiveTitleTextColor; }
			set { _InactiveTitleTextColor = value; Invalidate(); }
		}
		public Font TitleFont
		{
			get { return _TitleFont; }
			set { _TitleFont = value; Invalidate(); }
		}


		[DefaultValue(false)]
		public virtual bool NonClientAreaDoubleBuffering
		{
			get { return _nonClientAreaDoubleBuffering; }
			set
			{
				_nonClientAreaDoubleBuffering = value;
				// No need to invalidate anything,
				// next painting will use double-buffering.
			}
		}

		[DefaultValue(true)]
		public bool EnableNonClientAreaPaint
		{
			get { return _enableNonClientAreaPaint; }
			set
			{
				_enableNonClientAreaPaint = value;
				InvalidateWindow();
			}
		}

		public List<CustomCaptionButton> CaptionButtons
		{
			get { return _captionButtons; }
		}

		[DefaultValue(false)]
		[Browsable(true)]
		public bool SettingsBox
		{
			get { return _settingsButton.Visible; }
			set { _settingsButton.Visible = value; }
		}
		[DefaultValue(false)]
		[Browsable(true)]
		public bool AboutBox
		{
			get { return _aboutButton.Visible; }
			set { _aboutButton.Visible = value; }
		}
		
		public new bool MaximizeBox
		{
			get { return base.MaximizeBox; }
			set
			{
				this._maximizeButton.Enabled = value;
				this._minimizeButton.Visible = this._maximizeButton.Visible
						= this._maximizeButton.Enabled | this._minimizeButton.Enabled;

				base.MaximizeBox = value;
			}
		}

		public new bool MinimizeBox
		{
			get { return base.MinimizeBox; }
			set
			{
				this._minimizeButton.Enabled = value;
				this._minimizeButton.Visible = this._maximizeButton.Visible
						= this._maximizeButton.Enabled | this._minimizeButton.Enabled;

				base.MinimizeBox = value;
			}
		}

		public new bool ControlBox
		{
			get { return base.ControlBox; }
			set
			{
				this._closeButton.Enabled = value;
				base.ControlBox = value;

			}
		}

		public new bool HelpButton
		{
			get { return base.HelpButton; }
			set
			{
				this._helpButton.Visible = value;
				base.HelpButton = value;
			}
		}

		public new Icon Icon
		{
			get { return _smallIcon; }
			set
			{
				_smallIcon = value;
				base.Icon = value;
			}
		}

		#endregion

		#region On...

		protected override void OnHandleCreated(EventArgs e)
		{
			// Disable theming on current window so we don't get 
			// any funny artifacts (round corners, etc.)
			NativeMethods.SetWindowTheme(this.Handle, "", "");

#if !DEBUG
			// When application window stops responding to messages
			// system will finally loose patience and repaint it with default theme.
			// This prevents such behavior for entire application.
			// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/lowlevelclientsupport/misc/rtldisableprocesswindowsghosting.asp
			NativeMethods.DisableProcessWindowsGhosting();
#endif
			base.OnHandleCreated(e);
		}

		protected virtual void OnUpdateWindowState()
		{
			this._minimizeButton.Visible = this._maximizeButton.Enabled | this._minimizeButton.Enabled;
			this._maximizeButton.Visible = this._minimizeButton.Visible && this.WindowState != FormWindowState.Maximized;
			this._restoreButton.Visible = this._minimizeButton.Visible && this.WindowState == FormWindowState.Maximized;
		}

		protected virtual void OnSystemCommand(int sc)
		{ }

		/// <summary>Adjust the supplied Rectangle to the desired position of the client rectangle.</summary>
		protected virtual void OnNonClientAreaCalcSize(ref Rectangle bounds, bool update)
		{

			if (update)
				UpdateCaptionButtonBounds(bounds);

			Padding clientPadding = ClientPaddingInfo;
			bounds = new Rectangle(bounds.Left + clientPadding.Left, bounds.Top + clientPadding.Top,
					bounds.Width - clientPadding.Horizontal, bounds.Height - clientPadding.Vertical);
		}

		/// <summary>
		/// Returns a value from a NCHITTEST enumeration specifing the window element on given point.
		/// </summary>
		protected virtual int OnNonClientAreaHitTest(Point p)
		{
			foreach (CustomCaptionButton button in this.CaptionButtons)
			{
				if (button.Visible && button.Bounds.Contains(p) && (button.HitTestCode > 0 || button.HitTestCode < -1))
					return button.HitTestCode;
			}

			if (FormBorderStyle != FormBorderStyle.FixedToolWindow &&
					FormBorderStyle != FormBorderStyle.SizableToolWindow)
			{
				if (GetIconRectangle().Contains(p))
					return (int)NativeMethods.NCHITTEST.HTSYSMENU;
			}

			if (this.FormBorderStyle == FormBorderStyle.Sizable
					|| this.FormBorderStyle == FormBorderStyle.SizableToolWindow)
			{
				#region Handle sizable window borders
				if (p.X <= Skin.BorderWidth) // left border
				{
					if (p.Y <= Skin.BorderWidth)
						return (int)NativeMethods.NCHITTEST.HTTOPLEFT;
					else if (p.Y >= this.Height - Skin.BorderWidth)
						return (int)NativeMethods.NCHITTEST.HTBOTTOMLEFT;
					else
						return (int)NativeMethods.NCHITTEST.HTLEFT;
				}
				else if (p.X >= this.Width - Skin.BorderWidth) // right border
				{
					if (p.Y <= Skin.BorderWidth)
						return (int)NativeMethods.NCHITTEST.HTTOPRIGHT;
					else if (p.Y >= this.Height - Skin.BorderWidth)
						return (int)NativeMethods.NCHITTEST.HTBOTTOMRIGHT;
					else
						return (int)NativeMethods.NCHITTEST.HTRIGHT;
				}
				else if (p.Y <= Skin.BorderWidth) // top border
				{
					if (p.X <= Skin.BorderWidth)
						return (int)NativeMethods.NCHITTEST.HTTOPLEFT;
					if (p.X >= this.Width - Skin.BorderWidth)
						return (int)NativeMethods.NCHITTEST.HTTOPRIGHT;
					else
						return (int)NativeMethods.NCHITTEST.HTTOP;
				}
				else if (p.Y >= this.Height - Skin.BorderWidth) // bottom border
				{
					if (p.X <= Skin.BorderWidth)
						return (int)NativeMethods.NCHITTEST.HTBOTTOMLEFT;
					if (p.X >= this.Width - Skin.BorderWidth)
						return (int)NativeMethods.NCHITTEST.HTBOTTOMRIGHT;
					else
						return (int)NativeMethods.NCHITTEST.HTBOTTOM;
				}
				#endregion
			}

			// title bar
			if (p.Y <= ClientPaddingInfo.Top)
				return (int)NativeMethods.NCHITTEST.HTCAPTION;

			// rest of non client area
			if (p.X <= this.ClientPaddingInfo.Left || p.X >= this.ClientPaddingInfo.Right
					|| p.Y >= this.ClientPaddingInfo.Bottom)
				return (int)NativeMethods.NCHITTEST.HTBORDER;

			return (int)NativeMethods.NCHITTEST.HTCLIENT;
		}

		/// <summary>
		/// Delivers new mouse position when it is moved over the non client area of the window. 
		/// </summary>
		protected virtual void OnNonClientMouseMove(MouseEventArgs mouseEventArgs)
		{
			foreach (CustomCaptionButton button in this.CaptionButtons)
			{
				if (button.Visible && button.Bounds.Contains(mouseEventArgs.X, mouseEventArgs.Y) && button.HitTestCode > 0)
				{
					if (button.State != CaptionButtonState.Over)
					{
						button.State = CaptionButtonState.Over;
						DrawButton(button);
						HookMouseEvent();
					}
				}
				else
				{
					if (button.State != CaptionButtonState.Normal)
					{
						button.State = CaptionButtonState.Normal;
						DrawButton(button);
						UnhookMouseEvent();
					}
				}
			}
		}

		/// <summary>
		/// Called when mouse cursor leaves the non client window area.
		/// </summary>
		/// <param name="args"></param>
		protected virtual void OnNonClientMouseLeave(EventArgs args)
		{
			if (!_trakingMouse)
				return;

			foreach (CustomCaptionButton button in this.CaptionButtons)
			{
				if (button.State != CaptionButtonState.Normal)
				{
					button.State = CaptionButtonState.Normal;
					DrawButton(button);
					UnhookMouseEvent();
				}
			}
		}

		/// <summary>
		/// Called each time one of the mouse buttons was pressed over the non-client area.
		/// </summary>
		/// <param name="args">NonClientMouseEventArgs contain mouse position, button pressed,
		/// and hit-test code for current position. </param>
		protected virtual void OnNonClientMouseDown(NonClientMouseEventArgs args)
		{
			if (args.Button != MouseButtons.Left)
				return;

			// custom button
			foreach (CustomCaptionButton button in this.CaptionButtons)
				if (args.HitTest > short.MaxValue && args.HitTest == button.HitTestCode && button.Visible && button.Enabled)
				{
					//                    ((CustomCaptionButton)button).OnClick();
					args.Handled = true;
					return;
				}

			// find appropriate button
			foreach (CustomCaptionButton button in this.CaptionButtons)
			{
				// [1530]: Don't execute any action when button is disabled or not visible.
				if (args.HitTest == button.HitTestCode && button.Visible && button.Enabled)
				{
					Log(MethodInfo.GetCurrentMethod(), "MouseDown: button = {0}", button);

					if (DepressButton(button))
					{
						if (button.SystemCommand >= 0)
						{
							int sc = button.SystemCommand;

							if (button == _maximizeButton)
								sc = (WindowState == FormWindowState.Maximized) ?
										(int)NativeMethods.SystemCommands.SC_RESTORE : (int)NativeMethods.SystemCommands.SC_MAXIMIZE;

							NativeMethods.SendMessage(this.Handle,
									(int)NativeMethods.WindowMessages.WM_SYSCOMMAND,
									(IntPtr)sc, IntPtr.Zero);
						}
						else if (button == _settingsButton)
						{
							if (SettingsClick != null)
								SettingsClick(button, new EventArgs());
						}
						else if (button == _aboutButton)
						{
							if (AboutClick != null)
								AboutClick(button, new EventArgs());
						}
						args.Handled = true;
					}
					return;
				}
			}
		}

		/// <summary>
		/// Paints the client rect - e.ClipingRect has the correct window size, since this.Width, this.Height
		/// aren't always correct when calling this methode (because window is actually resizing)
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnNonClientAreaPaint(NonClientPaintEventArgs e)
		{
			// assign clip region to exclude client area
			Region clipRegion = new Region(e.Bounds);
			Rectangle rect = new Rectangle(ClientRectangle.Location, ClientRectangle.Size);
			rect.Offset(ClientPaddingInfo.Left, ClientPaddingInfo.Top);
			clipRegion.Exclude(rect);
			clipRegion.Exclude(CustomDrawUtil.ExcludePadding(e.Bounds, ClientPaddingInfo));
			e.Graphics.Clip = clipRegion;

			Color bgColor = this.TitleBackgroundColor;
			Color textColor = this.TitleTextColor;
			Font textFont = this.TitleFont;

			e.Graphics.FillRectangle(new SolidBrush(bgColor), 0, 0, e.Bounds.Width, e.Bounds.Height);

			// paint borders
			//ActiveFormSkin.NormalState.DrawImage(e.Graphics, e.Bounds);
			//ControlPaint.DrawBorder(e.Graphics, e.Bounds, ForeColor, ButtonBorderStyle.Solid);
			ControlPaint.DrawBorder3D(e.Graphics, e.Bounds, Border3DStyle.Flat);
			//rect = new Rectangle(e.Bounds.Location, new Size(e.Bounds.Width, ClientPaddingInfo.Top));
			int inflate = Skin.BorderWidth - 5;
			rect.Inflate(inflate, inflate);
			ControlPaint.DrawBorder3D(e.Graphics, rect, Border3DStyle.Flat);

			int textOffset = 0;

			// paint icon
			if (ShowIcon &&
					FormBorderStyle != FormBorderStyle.FixedToolWindow &&
					FormBorderStyle != FormBorderStyle.SizableToolWindow)
			{
				Rectangle iconRect = GetIconRectangle();
				textOffset += iconRect.Right;

				if (_smallIcon != null)
					e.Graphics.DrawIcon(_smallIcon, iconRect);
			}


			// paint caption
			string text = this.Text;
			if (!String.IsNullOrEmpty(text))
			{
				// disable text wrapping and request elipsis characters on overflow
				using (StringFormat sf = new StringFormat())
				{
					sf.Trimming = StringTrimming.EllipsisCharacter;
					sf.FormatFlags = StringFormatFlags.NoWrap;
					sf.LineAlignment = StringAlignment.Center;

					// find position of the first button from left
					int firstButton = e.Bounds.Width;
					foreach (CustomCaptionButton button in this.CaptionButtons)
						if (button.Visible)
							firstButton = Math.Min(firstButton, button.Bounds.X);

					Padding padding = TitlePadding;
					Rectangle textRect = new Rectangle(textOffset + padding.Left,
							padding.Top, firstButton - textOffset - padding.Horizontal,
							ClientPaddingInfo.Top - padding.Vertical);

					if (Font != null)
						textFont = Font;

					// draw text
					using (Brush b = new SolidBrush(textColor))
					{
						e.Graphics.DrawString(text, textFont, b, textRect, sf);
					}
				}
			}

			// Translate for the frame border
			//            if (this.WindowState == FormWindowState.Maximized)
			//                e.Graphics.TranslateTransform(0, SystemInformation.FrameBorderSize.Height);

			// [2415] Because mouse actions over a button might cause it to repaint we need 
			// to buffer the background under the button in order to repaint it correctly.
			// This is important when partially transparent images are used for some button states.
			//            foreach (CustomCaptionButton button in this.CaptionButtons)
			//							button.BackColor= this.BackColor;

			// Paint all visible buttons (in this case the background doesn't need to be repainted)
			foreach (CustomCaptionButton button in this.CaptionButtons)
				button.DrawButton(e.Graphics, false, this.TitleBackgroundColor);
		}

		#endregion

		#region Window Events

		void NonClientBaseForm_Layout(object sender, LayoutEventArgs e)
		{
			Rectangle bounds = new Rectangle(Left, Top, Width, Height);
			OnNonClientAreaCalcSize(ref bounds, true);
			OnUpdateWindowState();
			InvalidateWindow();
		}

		#endregion

		#region WndProc

		protected override void WndProc(ref Message m)
		{
			Log(MethodInfo.GetCurrentMethod(), "Message = {0}", (NativeMethods.WindowMessages)m.Msg);

			if (!this.EnableNonClientAreaPaint)
			{
				base.WndProc(ref m);
				return;
			}

			switch (m.Msg)
			{
				case (int)NativeMethods.WindowMessages.WM_NCCALCSIZE:
					{
						// Provides new coordinates for the window client area.
						WmNCCalcSize(ref m);
						break;
					}
				case (int)NativeMethods.WindowMessages.WM_NCHITTEST:
					{
						// Tell the system what element is at the current mouse location. 
						WmNCHitTest(ref m);
						break;
					}
				case (int)NativeMethods.WindowMessages.WM_NCPAINT:
					{
						// Here should all our painting occur, but...
						WmNCPaint(ref m);
						break;
					}
				case (int)NativeMethods.WindowMessages.WM_NCACTIVATE:
					{
						// ... WM_NCACTIVATE does some painting directly 
						// without bothering with WM_NCPAINT ...
						WmNCActivate(ref m);
						break;
					}
				case (int)NativeMethods.WindowMessages.WM_SETTEXT:
					{
						// ... and some painting is required in here as well
						WmSetText(ref m);
						break;
					}
				case (int)NativeMethods.WindowMessages.WM_NCMOUSEMOVE:
					{
						WmNCMouseMove(ref m);
						break;
					}
				case (int)NativeMethods.WindowMessages.WM_NCMOUSELEAVE:
					{
						WmNCMouseLeave(ref m);
						break;
					}
				case (int)NativeMethods.WindowMessages.WM_NCLBUTTONDOWN:
					{
						WmNCLButtonDown(ref m);
						break;
					}
				case 174: // ignore magic message number
					{
						Log(MethodInfo.GetCurrentMethod(), "### Magic message ### WParam = {0}", m.WParam.ToInt32());
						break;
					}
				case (int)NativeMethods.WindowMessages.WM_SYSCOMMAND:
					{
						WmSysCommand(ref m);
						break;
					}
				/*
		case (int)NativeMethods.WindowMessages.WM_WINDOWPOSCHANGED:
				{
						WmWindowPosChanged(ref m);
						break;
				}*/
				case (int)NativeMethods.WindowMessages.WM_ERASEBKGND:
					{
						WmEraseBkgnd(ref m);
						break;
					}
				default:
					{
						base.WndProc(ref m);
						break;
					}
			}
		}

		#endregion

		#region Wm ... (Windows message...)

		private void WmEraseBkgnd(ref Message m)
		{
			base.WndProc(ref m);

			Log(MethodInfo.GetCurrentMethod(), "{0}", WindowState);
			OnUpdateWindowState();
		}

		private void WmWindowPosChanged(ref Message m)
		{
			base.WndProc(ref m);

			Log(MethodInfo.GetCurrentMethod(), "{0}", WindowState);
			OnUpdateWindowState();
		}

		private void WmSysCommand(ref Message m)
		{
			Log(MethodInfo.GetCurrentMethod(), "{0}", (NativeMethods.SystemCommands)m.WParam.ToInt32());

			this.OnSystemCommand(m.WParam.ToInt32());

			DefWndProc(ref m);
		}

		private void WmNCCalcSize(ref Message m)
		{
			// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/windows/windowreference/windowmessages/wm_nccalcsize.asp
			// http://groups.google.pl/groups?selm=OnRNaGfDEHA.1600%40tk2msftngp13.phx.gbl

			if (m.WParam == NativeMethods.FALSE)
			{
				NativeMethods.RECT ncRect = (NativeMethods.RECT)m.GetLParam(typeof(NativeMethods.RECT));
				Rectangle proposed = ncRect.Rect;

				Log(MethodInfo.GetCurrentMethod(), string.Format("### Client Rect [0] = ({0},{1}) x ({2},{3})",
						proposed.Left, proposed.Top, proposed.Width, proposed.Height));

				OnNonClientAreaCalcSize(ref proposed, true);
				ncRect = NativeMethods.RECT.FromRectangle(proposed);

				Marshal.StructureToPtr(ncRect, m.LParam, false);
			}
			else if (m.WParam == NativeMethods.TRUE)
			{
				NativeMethods.NCCALCSIZE_PARAMS ncParams = (NativeMethods.NCCALCSIZE_PARAMS)m.GetLParam(typeof(NativeMethods.NCCALCSIZE_PARAMS));
				Rectangle proposed = ncParams.rectProposed.Rect;

				Log(MethodInfo.GetCurrentMethod(), string.Format("### Client Rect [1] = ({0},{1}) x ({2},{3})",
						proposed.Left, proposed.Top, proposed.Width, proposed.Height));

				OnNonClientAreaCalcSize(ref proposed, true);
				ncParams.rectProposed = NativeMethods.RECT.FromRectangle(proposed);

				Marshal.StructureToPtr(ncParams, m.LParam, false);
			}
			m.Result = IntPtr.Zero;
		}

		#endregion

		#region WmNC ... (Windows message... Non Client)

		private void WmNCHitTest(ref Message m)
		{
			// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/userinput/mouseinput/mouseinputreference/mouseinputmessages/wm_nchittest.asp

			Point screenPoint = new Point(m.LParam.ToInt32());
			Log(MethodInfo.GetCurrentMethod(), string.Format("### Screen Point ({0},{1})", screenPoint.X, screenPoint.Y));

			// convert to local coordinates
			Point clientPoint = PointToWindow(screenPoint);
			Log(MethodInfo.GetCurrentMethod(), string.Format("### Client Point ({0},{1})", clientPoint.X, clientPoint.Y));
			m.Result = new System.IntPtr(OnNonClientAreaHitTest(clientPoint));
		}

		private void WmNCMouseMove(ref Message msg)
		{
			// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/userinput/mouseinput/mouseinputreference/mouseinputmessages/wm_nchittest.asp
			Point clientPoint = this.PointToWindow(new Point(msg.LParam.ToInt32()));
			OnNonClientMouseMove(new MouseEventArgs(MouseButtons.None, 0,
					clientPoint.X, clientPoint.Y, 0));
			msg.Result = IntPtr.Zero;
		}

		private void WmNCMouseLeave(ref Message m)
		{
			// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/userinput/mouseinput/mouseinputreference/mouseinputmessages/wm_ncmouseleave.asp
			OnNonClientMouseLeave(EventArgs.Empty);
		}

		private void WmNCLButtonDown(ref Message msg)
		{
			Point pt = this.PointToWindow(new Point(msg.LParam.ToInt32()));
			NonClientMouseEventArgs args = new NonClientMouseEventArgs(
					MouseButtons.Left, 1, pt.X, pt.Y, 0, msg.WParam.ToInt32());
			OnNonClientMouseDown(args);
			if (!args.Handled)
			{
				DefWndProc(ref msg);
			}
			msg.Result = NativeMethods.TRUE;
		}

		private void WmNCPaint(ref Message msg)
		{
			// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/gdi/pantdraw_8gdw.asp
			// example in q. 2.9 on http://www.syncfusion.com/FAQ/WindowsForms/FAQ_c41c.aspx#q1026q

			// The WParam contains handle to clipRegion or 1 if entire window should be repainted
			PaintNonClientArea(msg.HWnd, (IntPtr)msg.WParam);

			// we handled everything
			msg.Result = NativeMethods.TRUE;
		}

		private void WmSetText(ref Message msg)
		{
			// allow the system to receive the new window title
			DefWndProc(ref msg);

			// repaint title bar
			PaintNonClientArea(msg.HWnd, (IntPtr)1);
		}

		private void WmNCActivate(ref Message msg)
		{
			// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/windows/windowreference/windowmessages/wm_ncactivate.asp

			bool active = (msg.WParam == NativeMethods.TRUE);
			Log(MethodInfo.GetCurrentMethod(), "### Draw active title bar = {0}", active);

			if (WindowState == FormWindowState.Minimized)
				DefWndProc(ref msg);
			else
			{
				// repaint title bar
				PaintNonClientArea(msg.HWnd, (IntPtr)1);

				// allow to deactivate window
				msg.Result = NativeMethods.TRUE;
			}
		}

		#endregion

		#region Size , Positioning ...

		public Point PointToWindow(Point screenPoint)
		{
			return new Point(screenPoint.X - Location.X, screenPoint.Y - Location.Y);
		}

		protected override void SetClientSizeCore(int x, int y)
		{
			if (!EnableNonClientAreaPaint)
				base.SetClientSizeCore(x, y);

			this.Size = SizeFromClientSize(x, y, true);

			//  this.clientWidth = x;
			//  this.clientHeight = y;
			//  this.OnClientSizeChanged(EventArgs.Empty);
			// do this instead of above.
			this.UpdateBounds(Location.X, Location.Y, Size.Width, Size.Height, x, y);
		}

		private Size SizeFromClientSize(int x, int y, bool updatebuttons)
		{
			Rectangle bounds = new Rectangle(0, 0, 1000, 1000);
			OnNonClientAreaCalcSize(ref bounds, updatebuttons);

			return new Size(x + (1000 - bounds.Width), y + (1000 - bounds.Height));
		}

		protected override Size SizeFromClientSize(Size clientSize)
		{
			if (!EnableNonClientAreaPaint)
				return base.SizeFromClientSize(clientSize);

			return SizeFromClientSize(clientSize.Width, clientSize.Height, false);
		}

		#endregion

		#region PaintNonClientArea

		private void PaintNonClientArea(IntPtr hWnd, IntPtr hRgn)
		{
			NativeMethods.RECT windowRect = new NativeMethods.RECT();
			if (NativeMethods.GetWindowRect(hWnd, ref windowRect) == 0)
				return;

			Rectangle bounds = new Rectangle(0, 0,
					windowRect.right - windowRect.left,
					windowRect.bottom - windowRect.top);

			if (bounds.Width == 0 || bounds.Height == 0)
				return;

			// The update region is clipped to the window frame. When wParam is 1, the entire window frame needs to be updated. 
			Region clipRegion = null;
			if (hRgn != (IntPtr)1)
			{
				clipRegion = System.Drawing.Region.FromHrgn(hRgn);
				hRgn = (IntPtr)0;
			}

			// MSDN states that only WINDOW and INTERSECTRGN are needed,
			// but other sources confirm that CACHE is required on Win9x
			// and you need CLIPSIBLINGS to prevent painting on overlapping windows.
			IntPtr hDC = NativeMethods.GetDCEx(hWnd, hRgn,
					(int)(NativeMethods.DCX.DCX_WINDOW | NativeMethods.DCX.DCX_INTERSECTRGN
							| NativeMethods.DCX.DCX_CACHE | NativeMethods.DCX.DCX_CLIPSIBLINGS));
			if (hDC == IntPtr.Zero)
				hDC = NativeMethods.GetWindowDC(hWnd);

			if (hDC == IntPtr.Zero)
				return;


			try
			{

				if (!this.NonClientAreaDoubleBuffering)
				{
					using (Graphics g = Graphics.FromHdc(hDC))
					{
						//cliping rect is not cliping rect but actual rectangle
						OnNonClientAreaPaint(new NonClientPaintEventArgs(g, bounds, clipRegion));
					}

					//NOTE: The Graphics object would realease the HDC on Dispose.
					// So there is no need to call NativeMethods.ReleaseDC(msg.HWnd, hDC);
					//http://groups.google.pl/groups?hl=pl&lr=&c2coff=1&client=firefox-a&rls=org.mozilla:en-US:official_s&threadm=%23DDSaH7BFHA.3644%40TK2MSFTNGP15.phx.gbl&rnum=15&prev=/groups%3Fq%3DWM_NCPaint%2B%2BGetDCEx%26start%3D10%26hl%3Dpl%26lr%3D%26c2coff%3D1%26client%3Dfirefox-a%26rls%3Dorg.mozilla:en-US:official_s%26selm%3D%2523DDSaH7BFHA.3644%2540TK2MSFTNGP15.phx.gbl%26rnum%3D15
					//http://groups.google.pl/groups?hl=pl&lr=&c2coff=1&client=firefox-a&rls=org.mozilla:en-US:official_s&threadm=cmo00r%24j9v%241%40mamut1.aster.pl&rnum=1&prev=/groups%3Fq%3DDCX_PARENTCLIP%26hl%3Dpl%26lr%3D%26c2coff%3D1%26client%3Dfirefox-a%26rls%3Dorg.mozilla:en-US:official_s%26selm%3Dcmo00r%2524j9v%25241%2540mamut1.aster.pl%26rnum%3D1
				}
				else
				{
					//http://www.codeproject.com/csharp/flicker_free.asp
					//http://www.pinvoke.net/default.aspx/gdi32/BitBlt.html

					IntPtr CompatiblehDC = NativeMethods.CreateCompatibleDC(hDC);
					IntPtr CompatibleBitmap = NativeMethods.CreateCompatibleBitmap(hDC, bounds.Width, bounds.Height);

					try
					{
						NativeMethods.SelectObject(CompatiblehDC, CompatibleBitmap);

						// copy current screen to bitmap
						NativeMethods.BitBlt(CompatiblehDC, 0, 0, bounds.Width, bounds.Height, hDC, 0, 0, NativeMethods.TernaryRasterOperations.SRCCOPY);

						using (Graphics g = Graphics.FromHdc(CompatiblehDC))
						{
							OnNonClientAreaPaint(new NonClientPaintEventArgs(g, bounds, clipRegion));
						}

						// copy current from bitmap to screen
						NativeMethods.BitBlt(hDC, 0, 0, bounds.Width, bounds.Height, CompatiblehDC, 0, 0, NativeMethods.TernaryRasterOperations.SRCCOPY);
					}
					finally
					{
						NativeMethods.DeleteObject(CompatibleBitmap);
						NativeMethods.DeleteDC(CompatiblehDC);

					}

#if !NET1X
					// .NET 2.0 has this new class BufferedGraphics. But it paints the clien area in all black.
					// I dont know how to use it properly.

					//using (BufferedGraphics bg = BufferedGraphicsManager.Current.Allocate(hDC, bounds))
					//{
					//    Graphics g = bg.Graphics;
					//    //Rectangle clientRect = this.ClientRectangle;
					//    //clientRect.Offset(this.PointToScreen(Point.Empty));
					//    //g2.SetClip(clientRect, CombineMode.Exclude);
					//    OnNonClientAreaPaint(new NonClientPaintEventArgs(g, bounds, clipRegion));
					//    bg.Render();
					//}
#endif
				}
			}
			finally
			{
				NativeMethods.ReleaseDC(this.Handle, hDC);
			}
		}

		#endregion

		#region InvalidateWindow

		/// <summary>
		/// This method should invalidate entire window including the non-client area.
		/// </summary>
		protected void InvalidateWindow()
		{
			if (!IsDisposed && IsHandleCreated)
			{
				NativeMethods.SetWindowPos(this.Handle, IntPtr.Zero, 0, 0, 0, 0,
						(int)(NativeMethods.SetWindowPosOptions.SWP_NOACTIVATE | NativeMethods.SetWindowPosOptions.SWP_NOMOVE | NativeMethods.SetWindowPosOptions.SWP_NOSIZE |
						NativeMethods.SetWindowPosOptions.SWP_NOZORDER | NativeMethods.SetWindowPosOptions.SWP_NOOWNERZORDER | NativeMethods.SetWindowPosOptions.SWP_FRAMECHANGED));

				NativeMethods.RedrawWindow(this.Handle, IntPtr.Zero, IntPtr.Zero,
						(int)(NativeMethods.RedrawWindowOptions.RDW_FRAME | NativeMethods.RedrawWindowOptions.RDW_UPDATENOW | NativeMethods.RedrawWindowOptions.RDW_INVALIDATE));
			}
		}

		#endregion

		#region Log
		//[Conditional("DEBUGFORM")]
		protected internal static void Log(System.Reflection.MemberInfo callingMethod, string message, params object[] args)
		{
			/*
				if (args != null)
						message = String.Format(message, args);
				Debug.WriteLine(String.Format("{0}.{1} - {2}", callingMethod.DeclaringType.Name, callingMethod.Name, message));
			 */
		}
		#endregion //Log

		#region Mouse events hook...

		private void HookMouseEvent()
		{
			if (!_trakingMouse)
			{
				_trakingMouse = true;
				if (this._trackMouseEvent == null)
				{
					this._trackMouseEvent = new NativeMethods.TRACKMOUSEEVENT();
					this._trackMouseEvent.dwFlags =
							(int)(NativeMethods.TrackMouseEventFalgs.TME_HOVER |
										NativeMethods.TrackMouseEventFalgs.TME_LEAVE |
										NativeMethods.TrackMouseEventFalgs.TME_NONCLIENT);

					this._trackMouseEvent.hwndTrack = this.Handle;
				}

				if (NativeMethods.TrackMouseEvent(this._trackMouseEvent) == false)
					// use getlasterror to see whats wrong
					Log(MethodInfo.GetCurrentMethod(), "Failed enabling TrackMouseEvent: error {0}",
							NativeMethods.GetLastError());
			}
		}

		private void UnhookMouseEvent()
		{
			_trakingMouse = false;
		}

		#endregion

		#region Update...


		private void UpdateButtonSkin()
		{
			foreach (CustomCaptionButton button in _captionButtons)
			{
				Image img = SkinInfo.GetImage(button.Key);
				if (button.Key == CaptionButtonKey.Maximize && this.WindowState == FormWindowState.Maximized)
					img = SkinInfo.GetImage(CaptionButtonKey.Restore);
				button.Skin = new CustomCaptionButton.CaptionButtonSkin(img);
			}
		}

		private void UpdateCaptionButtonBounds(Rectangle windowRect)
		{
			// start from top-right corner
			int x = windowRect.Width - Skin.BorderWidth;
			int y = Skin.BorderWidth;

			foreach (CustomCaptionButton button in this.CaptionButtons)
			{
				if (button.Visible && button.Skin != null)
				{
					int size = Skin.IconSize;
					x -= (size);
					button.Bounds = new Rectangle(x, y, size, size);
				}
			}

			// Should I move this where this actually changes WM_GETMINMAXINFO ??
			//maximizeButton.Appearance = (this.WindowState == FormWindowState.Maximized) ?
			//    _borderAppearance.RestoreButton : _borderAppearance.MaximizeButton;
		}



		#endregion

		#region Misc...

		/// <summary>
		/// This method handles depressing the titlebar button. It captures the mouse and creates a message loop
		/// filtring only the mouse buttons until a WM_MOUSEMOVE or WM_LBUTTONUP message is received.
		/// </summary>
		/// <param name="currentButton">The button that was pressed</param>
		/// <returns>true if WM_LBUTTONUP occured over this button; false when mouse was moved away from this button.</returns>
		private bool DepressButton(CustomCaptionButton currentButton)
		{
			try
			{
				// callect all mouse events (should do the same as SetCapture())
				this.Capture = true;

				// draw button in pressed mode
				currentButton.State = CaptionButtonState.Pressed;
				DrawButton(currentButton);

				// loop until button is released
				bool result = false;
				bool done = false;
				while (!done)
				{
					// NOTE: This struct needs to be here. We had strange error (starting from Beta 2).
					// when this was defined at begining of this method. also check if types are correct (no overlap). 
					Message m = new Message();

					if (NativeMethods.PeekMessage(ref m, IntPtr.Zero,
							(int)NativeMethods.WindowMessages.WM_MOUSEFIRST, (int)NativeMethods.WindowMessages.WM_MOUSELAST,
							(int)NativeMethods.PeekMessageOptions.PM_REMOVE))
					{
						Log(MethodInfo.GetCurrentMethod(), "Message = {0}, Button = {1}", (NativeMethods.WindowMessages)m.Msg, currentButton);
						switch (m.Msg)
						{
							case (int)NativeMethods.WindowMessages.WM_LBUTTONUP:
								{
									if (currentButton.State == CaptionButtonState.Pressed)
									{
										currentButton.State = CaptionButtonState.Normal;
										DrawButton(currentButton);
									}
									Point pt = PointToWindow(PointToScreen(new Point(m.LParam.ToInt32())));
									Log(MethodInfo.GetCurrentMethod(), "### Point = ({0}, {1})", pt.X, pt.Y);

									result = currentButton.Bounds.Contains(pt);
									done = true;
								}
								break;
							case (int)NativeMethods.WindowMessages.WM_MOUSEMOVE:
								{
									Point clientPoint = PointToWindow(PointToScreen(new Point(m.LParam.ToInt32())));
									if (currentButton.Bounds.Contains(clientPoint))
									{
										if (currentButton.State == CaptionButtonState.Normal)
										{
											currentButton.State = CaptionButtonState.Pressed;
											DrawButton(currentButton);
										}
									}
									else
									{
										if (currentButton.State == CaptionButtonState.Pressed)
										{
											currentButton.State = CaptionButtonState.Normal;
											DrawButton(currentButton);
										}
									}

									// [1531]: These variables need to be reset here although thay aren't changed 
									// before reaching this point
									result = false;
									done = false;
								}
								break;
						}
					}
				}
				return result;
			}
			finally
			{
				this.Capture = false;
			}
		}

		private void DrawButton(CustomCaptionButton button)
		{
			if (IsHandleCreated)
			{
				// MSDN states that only WINDOW and INTERSECTRGN are needed,
				// but other sources confirm that CACHE is required on Win9x
				// and you need CLIPSIBLINGS to prevent painting on overlapping windows.
				IntPtr hDC = NativeMethods.GetDCEx(this.Handle, (IntPtr)1,
						(int)(NativeMethods.DCX.DCX_WINDOW | NativeMethods.DCX.DCX_INTERSECTRGN
								| NativeMethods.DCX.DCX_CACHE | NativeMethods.DCX.DCX_CLIPSIBLINGS));
				if (hDC == IntPtr.Zero)
					hDC = NativeMethods.GetWindowDC(this.Handle);

				if (hDC != IntPtr.Zero)
				{
					using (Graphics g = Graphics.FromHdc(hDC))
					{
						button.DrawButton(g, true, this.TitleBackgroundColor);
					}
				}

				NativeMethods.ReleaseDC(this.Handle, hDC);
			}
		}

		private Rectangle GetIconRectangle()
		{
			return new Rectangle(Skin.BorderWidth, Skin.BorderWidth, Skin.IconSize, Skin.IconSize);
		}

		#endregion

		#region FormSkin events...

		public event EventHandler SettingsClick;
		public event EventHandler AboutClick;

		#endregion

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// CustomAppForm
			// 
			this.ClientSize = new System.Drawing.Size(284, 265);
			this.Name = "CustomAppForm";
			this.ResumeLayout(false);

		}

	}
}
