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
    using System.Text;
    using System.Runtime.InteropServices;

    using DirectShowLib;

    #endregion

    public sealed class DirectXInterfaces
    {
        /// <summary>
        /// Constructor for <see cref="DirectXInterfaces"/> class.
        /// </summary>
        public DirectXInterfaces()
        {
            // Reset interfaces
        }

        /// <summary>
        /// Graph builder interface.
        /// </summary>
        public IFilterGraph2    FilterGraph = null;

        /// <summary>
        /// IVMRMixerBitmap9
        /// </summary>
        public IVMRMixerBitmap9 MixerBitmap = null;

        /// <summary>
        /// VideoMixingRenderer9
        /// </summary>
        public IBaseFilter      VMRenderer = null;

        /// <summary>
        /// Crossbar (on some devices)
        /// </summary>
        public IAMCrossbar      Crossbar = null;

        /// <summary>
        /// IVMRWindowlessControl9
        /// </summary>
        public IVMRWindowlessControl9 WindowlessCtrl = null;

        /// <summary>
        /// IMediaControl
        public IMediaControl    MediaControl = null;

        /// <summary>
        /// Used to grab current snapshots
        /// </summary>
        public ISampleGrabber   SampleGrabber = null;

        /// <summary>
        /// VideoMixingRenderer9
        /// </summary>
        public IBaseFilter      SampleGrabberFilter = null;

        /// <summary>
        /// Tee splitter
        /// </summary>
        public IBaseFilter      SmartTee = null;

        /// <summary>
        /// Capture filter
        /// </summary>
        public IBaseFilter      CaptureFilter = null;

        /// <summary>
        /// Closes and releases all used interfaces.
        /// </summary>
        public void CloseInterfaces()
        {
            if (VMRenderer != null)
            {
                Marshal.ReleaseComObject(VMRenderer);
                VMRenderer = null;
                WindowlessCtrl = null;
                MixerBitmap = null;
            }

            if (FilterGraph != null)
            {
                Marshal.ReleaseComObject(FilterGraph);
                FilterGraph = null;
                MediaControl = null;
            }

            if (SmartTee != null)
            {
                Marshal.ReleaseComObject(SmartTee);
                SmartTee = null;
            }

            if (SampleGrabber != null)
            {
                Marshal.ReleaseComObject(SampleGrabber);
                SampleGrabber = null;
                SampleGrabberFilter = null;
            }

            if (CaptureFilter != null)
            {
                Marshal.ReleaseComObject(CaptureFilter);
                CaptureFilter = null;
            }

            if (Crossbar != null)
            {
                Marshal.ReleaseComObject(Crossbar);
                Crossbar = null;
            }
        }


    }



}
