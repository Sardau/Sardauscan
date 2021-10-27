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
using System.Diagnostics;
using Sardauscan.Core;

namespace Sardauscan.Core.Interface
{


	/// <summary>
	/// Basic interface For a Camera proxy
	/// </summary>
	public interface ICameraProxy : IDisposable, IHardwareProxy
	{
		/// <summary>
		/// Return the current image of the camera
		/// </summary>
		/// <returns></returns>
		Bitmap AcquireImage();

		/// <summary>
		/// Get the camera image Height resolution (height of the current image)
		/// </summary>
		int ImageHeight { get; }

		/// <summary>
		/// Get the camera image Width resolution (Width of the current image)
		/// </summary>
		int ImageWidth { get; }

		/// <summary>
		/// Get the sensor width
		/// </summary>
		double SensorWidth { get; }

		/// <summary>
		/// Get the sensor height
		/// </summary>
		double SensorHeight { get; }

		/// <summary>
		/// Get the focal lenght
		/// </summary>
		double FocalLength { get; }
	}
}

