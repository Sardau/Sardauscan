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
using Sardauscan.Gui.Controls;
using Sardauscan.Core.Interface;
using Sardauscan.Hardware;

namespace FakeHardwarePlugins
{
	public class FakeTurnTableProxy : AbstractProxyProvider<ITurnTableProxy>, ITurnTableProxy
	{

		public int Rotate(double theta, bool relative) { return 0; }

		public void InitialiseRotation() { }

		public float MinimumRotation() { return 1; }

		public bool MotorEnabled { set { } }

		public void Dispose() { }

		public String HardwareId { get { return "Unique ID to reload th same"; } }

		public IHardwareProxy LoadFromHardwareId(string hardwareId)
		{
			// load a proxy using the hardwareid, for automatic reload of the last used
			return new FakeTurnTableProxy();
		}
		 

		public System.Windows.Forms.Control GetViewer()
		{
			TurnTableControl view = new TurnTableControl();
			view.Height = 70;
			view.Proxy = this;
			return view;
		}

		public override string Name
		{
			get { return "*FAKE TURNTABLE PROXY*"; }
		}

		public override object Select(System.Windows.Forms.IWin32Window owner)
		{
			return new FakeTurnTableProxy();
		}
	}
}
