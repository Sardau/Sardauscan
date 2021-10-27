#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sardauscan.Gui.Controls.ApplicationView
{
	/// <summary>
	/// View controler
	/// </summary>
	public class ViewControler
	{

		private Panel m_BigContainer;
		private Panel m_SmallContainer;
		private Dictionary<ViewType, View> m_RegisteredViews;
		MainForm m_MainForm;
		private ViewType? m_CurrentView;
		private bool _Lock = false;
		public bool Lock 
		{
			get { return _Lock; }
			set
			{
				bool fire = _Lock != value;
				_Lock = value;
				if (fire)
					FireLockChange();
			}
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="mainForm"></param>
		/// <param name="container"></param>
		public ViewControler(MainForm mainForm, Panel bigContainer,Panel smallContainer)
		{
			m_BigContainer = bigContainer;
			m_SmallContainer = smallContainer;
			m_RegisteredViews = new Dictionary<ViewType, View>();
			m_MainForm = mainForm;
			m_CurrentView = null;
			Lock = false;
		}

		public bool IsCurrent(ViewType type)
		{
			return type == m_CurrentView;
		}
		public bool IsEnabled(ViewType type)
		{
			if (RegisterdViews.Contains(type))
				return m_RegisteredViews[type].Enable;
			return false;
		}
		public List<ViewType> RegisterdViews
		{
			get { return new List<ViewType>(m_RegisteredViews.Keys); }
		}
		/// <summary>
		/// register a view
		/// </summary>
		/// <param name="view"></param>
		/// <param name="ctl"></param>
		public void RegisterView(View view)
		{
			m_RegisteredViews[view.Type] = view;
			FireViewListChange();
		}

		public void RegisterView(ViewType type, Control bigControl,Control smallControl)
		{
			View view = new View(type, bigControl, smallControl);
			if (view.BigControl != null)
				view.BigControl.Visible = false;
			if (view.SmallControl != null)
				view.SmallControl.Visible = false;
			RegisterView(view);
		}


		/// <summary>
		/// change view
		/// </summary>
		/// <param name="view"></param>
		/// <param name="parmeters"></param>
		public void ChangeView(ViewType viewtype)
		{
			m_CurrentView = viewtype;
			if (m_RegisteredViews.ContainsKey(viewtype))
			{
				View view = m_RegisteredViews[viewtype];

				foreach (View v in m_RegisteredViews.Values)
				{
					bool vis = v.Type == view.Type;
					if (v.BigControl != null)
						v.BigControl.Visible = vis;
					if (v.SmallControl!= null)
						v.SmallControl.Visible = vis;
				}

				m_BigContainer.Controls.Clear();
				m_SmallContainer.Controls.Clear();
				if(view.BigControl!=null)
				{
					m_BigContainer.Controls.Add(view.BigControl);
					view.BigControl.Dock = DockStyle.Fill;
					view.BigControl.Visible = true;
				}
				if(view.SmallControl!=null)
				{
					m_SmallContainer.Controls.Add(view.SmallControl);
					view.SmallControl.Dock = DockStyle.Fill;
					view.SmallControl.Visible = true;
				}
			}
			else
				throw new Exception("View " + viewtype.ToString() + " is not registered");
			FireViewChanged();
		}


		/// <summary>
		/// Lock change delegate
		/// </summary>
		public delegate void LockChangeChange();
		/// <summary>
		/// On Lock change 
		/// </summary>
		public event ViewChanged OnLockChange;
		/// <summary>
		/// Fire lock changed event
		/// </summary>
		public void FireLockChange()
		{
			if (OnLockChange != null)
				OnLockChange();
		}


		/// <summary>
		/// View list change delegate
		/// </summary>
		public delegate void ViewListChange();
		/// <summary>
		/// On view list change
		/// </summary>
		public event ViewChanged OnViewListChange;
		/// <summary>
		/// Fire view list changed event
		/// </summary>
		public void FireViewListChange()
		{
			if (OnViewChanged != null)
				OnViewListChange();
		}


		/// <summary>
		/// View changed delegate
		/// </summary>
		public delegate void ViewChanged();

		/// <summary>
		/// On view changed
		/// </summary>
		public event ViewChanged OnViewChanged;

		/// <summary>
		/// Fire view changed event
		/// </summary>
		public void FireViewChanged()
		{
			if (OnViewChanged != null)
				OnViewChanged();
		}
	}
}
