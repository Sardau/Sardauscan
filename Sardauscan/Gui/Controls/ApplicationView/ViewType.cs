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
using System.Drawing;

namespace Sardauscan.Gui.Controls.ApplicationView
{
	/// <summary>
	/// Views id enum
	/// </summary>
	public enum ViewType 
	{ 

		/// <summary>
		/// Process view
		/// </summary>
		Process, 
		/// <summary>
		/// Tune view
		/// </summary>
		Tune, 
		/// <summary>
		/// Calibration view
		/// </summary>
		Calibrate 
	}

	public static class ViewTypeExt
	{
		public static Bitmap Bitmap(this ViewType type)
		{
			switch (type)
			{
				case ViewType.Process:
					return global::Sardauscan.Properties.Resources.Lab;
				case ViewType.Calibrate:
					return global::Sardauscan.Properties.Resources.Target;
				case ViewType.Tune:
					return global::Sardauscan.Properties.Resources.Magic;
			}
			return global::Sardauscan.Properties.Resources.Gear;
		}
	}
}
