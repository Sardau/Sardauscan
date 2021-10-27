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
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Windows.Forms;

namespace Sardauscan.Core.Interface
{

	/// <summary>
	/// Basic interface for a Hardware proxy
	/// </summary>
	public interface IHardwareProxy
	{
		/// <summary>
		/// A unique id to identify a specific instance of IHardwareProxy (mainly used for reload a IHardwareproxy, so store all the properties)
		/// </summary>
		String HardwareId { get; }

		/// <summary>
		///  Load a IHardwareProxy with a specific HardwareId 
		/// </summary>
		/// <param name="hardwareId"></param>
		/// <returns> the loaded IHardwareProxy or null if you can't reload it</returns>
		IHardwareProxy LoadFromHardwareId(string hardwareId);

		/// <summary>
		/// Get the associated Viewer Control for these IHardwareProxy
		/// the viewer allow the user to interact with or tweak the hardware.
		/// you can return null if there is no setting or viewer 
		/// </summary>
		/// <returns></returns>
		Control GetViewer();
	}
}
