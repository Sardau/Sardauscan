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
using DirectShowLib;

namespace Sardauscan.Hardware
{
	  /// <summary>
	  /// Class for Direct show Camera information
	  /// </summary>
    public class DSCameraInfo
    {
			/// <summary>
			/// Ctor
			/// </summary>
			/// <param name="index"></param>
			/// <param name="dev"></param>
        public DSCameraInfo(int index, DsDevice dev)
        {
            Index = index;
            Device = dev;
        }
        public readonly DsDevice Device;
        public int Index { get; private set; }
        public override string ToString()
        {
            return Device.Name;
        }
        public string UniqueId
        {
            get { return Device.DevicePath; }
        }

        public static List<DSCameraInfo> GetAvailableCamera()
        {
            List<DSCameraInfo> ret = new List<DSCameraInfo>();
            DsDevice[] capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            for (int i = 0; i < capDevices.Length; i++)
            {
                DSCameraInfo cam = new DSCameraInfo(i, capDevices[i]);
                ret.Add(cam);
            }
            return ret;
        }
        public List<DSCameraProxy.Resolution> GetAvailableResolution()
        {
            return DSCameraProxy.GetAllAvailableResolution(this.Device);
        }
    }
}
