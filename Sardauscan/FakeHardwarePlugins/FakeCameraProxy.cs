#region COPYRIGHT
/****************************************************************************
 *  Copyright (c) 2015 Fabio Ferretti <https://plus.google.com/+FabioFerretti3D>                 *
 *  This file is part of Sardauscan.                                        *
 *                                                                          *
 *  Sardauscan is free software: you can redistribute it and/or modify      *
 *  it under the terms of the GNU General Public License as published by    *
 *  the Free Software Foundation, either version 3 of the License, or       *
 *  (at your option) any later version.                                     *
 *                                                                          *
 *  Sardauscan is distributed in the hope that it will be useful,           *
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of          *
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the           *
 *  GNU General Public License for more details.                            *
 *                                                                          *
 *  You are not allowed to Sell in any form this code                       * 
 *  or any compiled version. This code is free and for free purpose only    *
 *                                                                          *
 *  You should have received a copy of the GNU General Public License       *
 *  along with Sardaukar.  If not, see <http://www.gnu.org/licenses/>       *
 ****************************************************************************
*/
#endregion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Sardauscan.Core.Interface;
using Sardauscan.Hardware;


namespace FakeHardwarePlugins
{

	/// <summary>
	/// Fake camera for educational purpose ( and debug too ;) )
	/// in this cas the plugin implement the provider and the hardwareproxy
	/// it is interesting when you don't have any configuration to make 
	/// ( if you can only select one hardware, whitout any parameter)
	/// </summary>
	/// <remarks>
	/// In this cas the object derive from AbstractProxyProvider&lt;CameraProxy&gt;
	/// a abstract class that implement some function for a ICameraProxy provider
	/// </remarks>
	public class FakeCameraProxy : AbstractProxyProvider<ICameraProxy>, ICameraProxy
	{
		Image bmp;
		/// <summary>
		/// Cto must be whitout parameter !!
		/// </summary>
		public FakeCameraProxy()
		{
			bmp = new Bitmap(100, 100);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);
				StringFormat sf = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip);
				sf.Alignment = StringAlignment.Center;
				sf.LineAlignment = StringAlignment.Center;
				g.DrawString("Fake\nCamera", new Font("Arial", 12), Brushes.Red, new Rectangle(0, 0, bmp.Width, bmp.Height), sf);
			}
		}
		#region ICameraProxy implementation
		/// <summary>
		/// Return the current image of the camera
		/// </summary>
		/// <returns></returns>
		public Bitmap AcquireImage() { return (Bitmap)bmp; }
		/// <summary>
		/// Get the camera image Height resolution (height of the current image)
		/// </summary>
		public int ImageHeight { get { return bmp.Height; } }

		/// <summary>
		/// Get the camera image Width resolution (Width of the current image)
		/// </summary>
		public int ImageWidth { get { return bmp.Width; } }

		/// <summary>
		/// Get the sensor width
		/// </summary>
		public float SensorWidth { get { return 3.629f; } }
		/// <summary>
		/// Get the sensor height
		/// </summary>
		public float SensorHeight { get { return 2.722f; } }
		/// <summary>
		/// Get the focal lenght
		/// </summary>
		public float FocalLength { get { return 3.6f; } }
		#endregion
		/// <summary>
		/// dispose your Camera proxy
		/// </summary>
		public void Dispose() { }


		/// <summary>
		/// A unique id to identify a specific instance of IHardwareProxy (mainly used for reload a IHardwareproxy, so store all the properties)
		/// </summary>
		public String HardwareId { get { return "Unique ID to reload th same"; } }

		/// <summary>
		///  Load a IHardwareProxy with a specific HardwareId 
		/// </summary>
		/// <param name="hardwareId"></param>
		/// <returns> the loaded IHardwareProxy or null if you can't reload it</returns>
		public IHardwareProxy LoadFromHardwareId(string hardwareId)
		{
			// load a proxy using the hardwareid, for automatic reload of the last used
			return new FakeCameraProxy();
		}

		/// <summary>
		/// Get the associated Viewer of these IHardwareProxy
		/// the viewer allow the user to interact with or tweak the hardware.
		/// you can return null if there is no setting or viewer ( it will be a shame for camera, but hey you do what you want)
		/// </summary>
		/// <returns></returns>
		public System.Windows.Forms.Control GetViewer() { return null; }

		/// <summary>
		/// Display name of the Provider, for the user to know what he select;)
		/// </summary>
		public override string Name
		{
			get { return "*FAKE CAMERA PROXY*"; }
		}

		/// <summary>
		/// This function is call when the user request a instance of ther IHardwareProxy
		/// You can call winforms to as information ( Com port, configuration etc)
		/// </summary>
		/// <param name="owner">owner window</param>
		/// <returns>a IHardwareProxy if one is selected, Null in case of cancle or not disponible</returns>
		public override object Select(System.Windows.Forms.IWin32Window owner)
		{

			// do whatever interface stuff to select/configure your proxy
			// return the selected one
			return new FakeCameraProxy();
		}
	}
}
