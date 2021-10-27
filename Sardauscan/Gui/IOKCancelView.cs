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

namespace Sardauscan.Gui
{
	/// <summary>
	/// Interface for a control to be show in a OK-Cancel dialog
	/// </summary>
	public interface IOKCancelView 
	{
		/// <summary>
		/// Enable the OK button
		/// </summary>
		/// <returns></returns>
		bool IsValid();
		/// <summary>
		/// Called when OK is click
		/// </summary>
		void OnOk();
		/// <summary>
		/// Called whan Cancel button is click
		/// </summary>
		void OnCancel();
	}
}
