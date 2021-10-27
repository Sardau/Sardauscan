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

namespace Sardauscan.Gui
{
	/// <summary>
	/// Extention class for UserControls
	/// </summary>
	public static class UserControlExt
	{
		private static string m_CurrentProcess = string.Empty;
		/// <summary>
		/// Are we in Design mode, correct even for sub controls !
		/// </summary>
		/// <param name="ctrl"></param>
		/// <returns></returns>
		public static bool IsDesignMode(this Control ctrl)
		{
			if (String.IsNullOrEmpty(m_CurrentProcess))
				m_CurrentProcess = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
			return m_CurrentProcess == "devenv";
		}
	}
}
