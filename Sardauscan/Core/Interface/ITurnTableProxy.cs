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
	/// Basic interface for a Turn Table Proxy
	/// </summary>
	public interface ITurnTableProxy : IDisposable, IHardwareProxy
	{
		/// <summary>
		/// Rotate the Table
		/// </summary>
		/// <param name="theta"></param>
		/// <param name="relative">use relative(true) or absolute(false) rotation</param>
		/// <returns></returns>
		int Rotate(double theta, bool relative);
		/// <summary>
		/// Set the current position to 0° (for absolute rotation)
		/// </summary>
		void InitialiseRotation();
		/// <summary>
		/// Get the minimum Rotation angle for the table
		/// </summary>
		/// <returns></returns>
		double MinimumRotation();
		/// <summary>
		/// Enable of disable the motors
		/// </summary>
		bool MotorEnabled { set; }
	}
}
