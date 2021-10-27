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

namespace Sardauscan.Gui
{
	public partial class CalibrationBigView : UserControl
	{
		/// <summary>
		/// default ctor
		/// </summary>
		public CalibrationBigView()
		{
			InitializeComponent();
		}

		Control CurrentView = null;
		public void SetCurrentView(Control view)
		{
			if(view==CurrentView)
				return;
			if (CurrentView != null)
			{
				CurrentView.Visible = false;
				if (CurrentView is IDisposable)
					((IDisposable)CurrentView).Dispose();
			}

			this.StepContainerPanel.Controls.Clear();
			if (view != null)
			{
				this.StepContainerPanel.Controls.Add(view);
				view.Visible = true;
				view.Dock = DockStyle.Fill;
				CurrentView = view;
			}
		}
	
	
	}

}
