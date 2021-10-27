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

namespace Sardauscan.Core.Interface
{

	/// <summary>
	/// Basic Interface for a Laser Proxy
	/// </summary>
	public interface ILaserProxy : IDisposable, IHardwareProxy
	{

		/// <summary>
		/// Turn a laser on or off
		/// </summary>
		/// <param name="index"></param>
		/// <param name="on"></param>
		void Turn(int index, bool on);

		/// <summary>
		/// Ask if a laser is on or off
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		bool On(int index);
		/// <summary>
		/// Get the count of laser
		/// </summary>
		int Count { get; }

	}

	/// <summary>
	/// Extention of ILaserProxy
	/// </summary>
	public static class ILaserProxyExtention
	{
		/// <summary>
		/// Turn all the lasers  on or off together
		/// </summary>
		/// <param name="ctrl"></param>
		/// <param name="on"></param>
		public static void TurnAll(this ILaserProxy ctrl, bool on)
		{
			for (int i = 0; i < ctrl.Count; i++)
				ctrl.Turn(i, on);
		}
	}
}
