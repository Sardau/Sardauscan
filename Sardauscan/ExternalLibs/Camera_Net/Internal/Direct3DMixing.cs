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

#if USE_D3D
namespace Camera_NET
{
#region Using directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.DirectX.Direct3D;
    using System.Windows.Forms;
    using System.Drawing;

    #endregion

    /// <summary>
    /// Direct3DMixing class for Direct3D mixing image overlay.
    /// </summary>
    /// 
    /// <author> free5lot (free5lot@yandex.ru) </author>
    /// <version> 2013.10.17 </version>
    internal sealed class Direct3DMixing : IDisposable
    {
        // ====================================================================

        /// <summary>
        /// Constructor for <see cref="Direct3DMixing"/> class.
        /// </summary>
        public Direct3DMixing()
        {
        }

        /// <summary>
        /// Initializes managed Direct3D device if it's not already done.
        /// </summary>
        /// <param name="hostingControl">Control used for managed Direct3D device.</param>
        public void InitializeIfNeeded(Control hostingControl)
        {
            if (m_bDirect3DInitialized)
                return; // Already done everything

            Device.IsUsingEventHandlers = false;

            // Basic Presentation Parameters...
            presentParams = new PresentParameters();
            presentParams.Windowed = true;
            presentParams.SwapEffect = SwapEffect.Discard;

            // Assume a hardware Direct3D device is available
            // Add MultiThreaded to be safe. Each DirectShow filter runs in a separate thread...
            device = new Device(
                0,
                DeviceType.Hardware,
                hostingControl,
                CreateFlags.SoftwareVertexProcessing | CreateFlags.MultiThreaded,
                presentParams
                );

            m_bDirect3DInitialized = true;
        }

        /// <summary>
        /// Disposes Managed Direct3D objects.
        /// </summary>
        public void Dispose()
        {
            // Dispose Managed Direct3D objects
            if (surface != null)
            {
                surface.Dispose();
            }
            if (device != null)
            {
                device.Dispose();
            }
        }

        /// <summary>
        /// Stores an image (bitmap) to Direct3D surface.
        /// </summary>
        /// <param name="alphaBitmap">Bitmap to store to DX surface.</param>
        public void StoreBitmapToSurface(Bitmap alphaBitmap)
        {
            // Create a surface from our alpha bitmap
            surface = new Surface(device, alphaBitmap, Pool.SystemMemory);
            // Get the unmanaged pointer
            unmanagedSurface = surface.GetObjectByValue(DxMagicNumber);
        }

        /// <summary>
        /// Managed Direct3D device.
        /// </summary>   
        public Device device = null;

        /// <summary>
        /// Basic presentation parameters.
        /// </summary>  
        public PresentParameters presentParams;

        /// <summary>
        /// A Direct3D suface filled with alphaBitmap
        /// </summary> 
        public Surface surface = null;

        /// <summary>
        /// A pointer on the unmanaged surface
        /// </summary> 
        public IntPtr unmanagedSurface;

        /// <summary>
        /// Managed Direct3D magic number to retrieve unmanaged Direct3D interfaces.
        /// </summary>        
        private const int DxMagicNumber = -759872593;

        /// <summary>
        /// Was Direct3D initialized or not
        /// </summary> 
        private bool m_bDirect3DInitialized = false;


    }
}
#endif