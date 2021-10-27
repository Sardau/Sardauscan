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

namespace Sardauscan.Core.Interface
{
	/// <summary>
	/// Basic interface fo a IHardwareProxy provider
	/// </summary>
	public interface IHardwareProxyProvider
	{
		 /// <summary>
		 /// Display name of the Provider, for the user to know what he select;)
		 /// </summary>
			string Name { get; }

		 /// <summary>
		 /// Return the typeof of the IHardwareproxy privided by this provider
		 /// </summary>
			Type GenerateType { get; }

			/// <summary>
			/// This function is call when the user request a instance of ther IHardwareProxy
			/// You can call winforms to as information ( Com port, configuration etc)
			/// </summary>
			/// <param name="owner">owner window</param>
			/// <returns>a IHardwareProxy if one is selected, Null in case of cancel or not disponible</returns>
			object Select(IWin32Window owner);

	}
}
