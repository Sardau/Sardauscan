/*
 ****************************************************************************
 *  Copyright (c) 2015 Fabio Ferretti <Fabio@ferretti.info>                 *
 *	This file is part of Sardauscan.                                        *
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
 *   You should have received a copy of the GNU General Public License      *
 *   along with Sardauscan .  If not, see <http://www.gnu.org/licenses/>.   *
 ****************************************************************************
*/


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
	/// </summary>
	public class FakeCameraProxy : AbstractProxyProvider<ICameraProxy>, ICameraProxy
	{
		Image bmp;

		public FakeCameraProxy()
		{
			bmp = new Bitmap(100, 100);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);
				StringFormat sf = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip);
				sf.Alignment = StringAlignment.Center;
				sf.LineAlignment = StringAlignment.Center;
				g.DrawString("Fake Camera", new Font("Arial", 12), Brushes.Red, new Rectangle(0, 0, bmp.Width, bmp.Height), sf);
			}
		}
		public Bitmap AcquireImage() { return (Bitmap)bmp; }

		public int ImageHeight { get { return bmp.Height; } }

		public int ImageWidth { get { return bmp.Width; } }

		public float SensorWidth { get { return 3.629f; } }
		public float SensorHeight { get { return 2.722f; } }
		public float FocalLength { get { return 3.6f; } }

		public void Dispose() { }

		public String HardwareId { get { return "Unique ID to reload th same"; } }

		public IHardwareProxy LoadFromHardwareId(string hardwareId)
		{
			// load a proxy using the hardwareid, for automatic reload of the last used
			return new FakeCameraProxy();
		}
		public System.Windows.Forms.Control GetViewer() { return null; }

		public override string Name
		{
			get { return "*FAKE CAMERA PROXY*"; }
		}

		public override object Select(System.Windows.Forms.IWin32Window owner)
		{
			return new FakeCameraProxy();
		}
	}
}
