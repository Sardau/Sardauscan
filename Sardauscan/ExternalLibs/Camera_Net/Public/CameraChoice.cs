#region License

/*
Camera_NET - Camera wrapper for directshow for .NET
Copyright (C) 2013
https://github.com/free5lot/Camera_Net

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 3.0 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU LesserGeneral Public 
License along with this library. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

namespace Camera_NET
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    using DirectShowLib;

    #endregion

    /// <summary>
    /// CameraChoice class to select camera from the list of available cameras.
    /// </summary>
    /// 
    /// <author> free5lot (free5lot@yandex.ru) </author>
    /// <version> 2013.10.16 </version>
    public class CameraChoice : IDisposable
    {
        // ====================================================================

        /// <summary>
        /// Constructor for <see cref="CameraChoice"/> class.
        /// </summary>
        public CameraChoice()
        {
        }

        /// <summary>
        /// Updates list of devices (cameras) of CameraChoice.
        /// </summary>
        public void UpdateDeviceList()
        {
            m_pCapDevices = new List<DsDevice>(DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice));
        }

        /// <summary>
        /// Get a list of available devices.
        /// </summary>
        /// <returns>List of available devices</returns>
        /// <seealso cref="UpdateDeviceList"/>
        public List<DsDevice> Devices
        {
            get
            {
                return m_pCapDevices;
            }
        }

        /// <summary>
        /// Disposes device list and devices in it.
        /// </summary>
        public void Dispose()
        {
            foreach (DsDevice cam in m_pCapDevices)
            {
                cam.Dispose();
            }

            m_pCapDevices.Clear();
        }

        /// <summary>
        /// Returns Camera by Name
        /// </summary>
        /// <returns>Camera device</returns>
        public DsDevice GetCameraByName(string name)
        {
            return GetCameraByName(name, 0);
        }

        /// <summary>
        /// Returns Camera by Name and Index (if there can be more than one camera with this name)
        /// </summary>
        /// <param name="camera_name">Name of camera.</param>
        /// <param name="index_in_same_names">Index if there can be more than one camera with this name.</param>
        /// <returns>Camera device</returns>
        public DsDevice GetCameraByName(string camera_name, int index_in_same_names)
        {
            if (string.IsNullOrEmpty(camera_name) || index_in_same_names < 0)
                return null;

            int count_found = 0;

            DsDevice first_with_the_same_name = null;

            foreach (DsDevice cam in m_pCapDevices)
            {
                if (0 == string.Compare(cam.Name, camera_name, true))
                {
                    count_found++;

                    if (first_with_the_same_name == null)
                    {
                        first_with_the_same_name = cam;
                    }
                }
                if (count_found - 1 == index_in_same_names)
                {
                    // we found camera
                    return cam;
                }
            }

            // Didn't found exact. 
            // return the most similar (with the same name if possible)
            // NOTE: maybe this should return null?

            return first_with_the_same_name;
        }

        /// <summary>
        /// Returns camera index in devices list
        /// </summary>
        /// <param name="cam">Camera to get index of.</param>
        /// <returns>Index of camera device</returns>
        public int GetCameraIndexInDevices(DsDevice cam)
        {
            try
            {
                UpdateDeviceList();

                int cam_index = -1;

                if (cam == null)
                {
                    return -1;

                    //// select default camera
                    //if ( m_pCapDevices && m_pCapDevices.Count >= 1 )
                    //{
                    //	cam_index = 0;
                    //}
                }
                else
                {
                    for (int i = 0; i < m_pCapDevices.Count; i++)
                    {
                        if (0 == string.Compare(cam.DevicePath, m_pCapDevices[i].DevicePath))
                        {
                            cam_index = i;
                            break;
                        }
                    }
                }

                return cam_index;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns name of caerma from DsDevice
        /// </summary>
        /// <param name="camera">Camera to get name of.</param>
        /// <param name="camera_name">Name of camera.</param>
        /// <param name="index_in_same_names">Index if there can be more than one camera with this name.</param>
        /// <returns>True if found, False otherwise</returns>
        public bool GetNameByCamera(DsDevice camera, out string camera_name, out int index_in_same_names)
        {
            UpdateDeviceList();

            int count_found_before = 0;

            foreach (DsDevice cam in m_pCapDevices)
            {
                if (0 == string.Compare(cam.DevicePath, camera.DevicePath))
                {
                    // found, we are ready to return result
                    index_in_same_names = count_found_before;
                    camera_name = cam.Name;

                    return true;
                }

                if (0 == string.Compare(cam.Name, camera.Name, true))
                {
                    count_found_before++;
                }
            }
            //Didn't found 

            camera_name = string.Empty;
            index_in_same_names = 0;

            return false;
        }

        // ====================================================================

        #region Private

        /// <summary>
        /// List of installed video devices
        /// </summary>
        protected List<DsDevice> m_pCapDevices = new List<DsDevice>();

        #endregion // Private

        // ====================================================================
    };

}