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
using System.Drawing;
using Sardauscan.Gui.Controls;
using Sardauscan.Core;

namespace Sardauscan.Gui.CalibrationSteps
{
	/// <summary>
	/// Interface for a Calibration Step 
	/// </summary>
	public interface ICalibrationStepInfo 
	{
		/// <summary>
		/// Order id for visual order of the Clibration step button
		/// </summary>
		int OrderId { get; }
		/// <summary>
		/// Label of the Calibration Step button
		/// </summary>
		string Label { get; }
		/// <summary>
		/// Type of the Main Calibration step Control
		/// </summary>
		Type ControlType { get; }
	}
	/// <summary>
	/// Extention class For ICalibrationStepInfo
	/// </summary>
	public static class ICalibrationStepInfoExt 
	{
		/// <summary>
		/// Get the predefined Image for the button
		/// </summary>
		/// <param name="step"></param>
		/// <returns></returns>
		public static Image Image(this ICalibrationStepInfo  step)
		{
			Type stepControlType = step.ControlType;
			if(stepControlType == typeof(Manual))
					return global::Sardauscan.Properties.Resources.Tools;
			if(stepControlType == typeof(CorrectionMatrix))
					return global::Sardauscan.Properties.Resources.Magic;
			if(stepControlType == typeof(Dimention))
					return global::Sardauscan.Properties.Resources.Cube;
			return global::Sardauscan.Properties.Resources.Gears;
		}
		/// <summary>
		/// Create the Calibration Step info Control
		/// </summary>
		/// <param name="step"></param>
		/// <returns></returns>
		public static Control CreateControl(this ICalibrationStepInfo step)
		{
			try
			{
				object ctrl = Reflector.CreateInstance<object>(step.ControlType);
				if (ctrl!=null && ctrl is Control)
					return (Control)ctrl;
			}
			catch
			{
			}
				return new UnderConstruction();		 
		}
		 
	}
}
