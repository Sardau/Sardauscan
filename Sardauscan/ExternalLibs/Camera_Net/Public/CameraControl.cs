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
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using System.Windows.Forms;
    using System.Runtime.InteropServices.ComTypes;
    using System.ComponentModel;

    // Use DirectShowLib (LGPL v2.1)
    using DirectShowLib;

    #endregion

    /// <summary>
    /// The user control of <see cref="Camera"/> for video output in Windows Forms.
    /// </summary>
    /// <remarks>This class is inherited from <see cref="UserControl"/> class.</remarks>
    /// 
    /// <author> free5lot (free5lot@yandex.ru) </author>
    /// <version> 2013.10.15 </version>
    public partial class CameraControl : UserControl
    {       
        // ====================================================================

        #region Public Main

        /// <summary>
        /// Default constructor for <see cref="CameraControl"/> class.
        /// </summary>
        public CameraControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes camera, builds and runs graph for control.
        /// </summary>
        /// <param name="moniker">Moniker (device identification) of camera.</param>
        /// <param name="resolution">Resolution of camera's output.</param>
        public void SetCamera(IMoniker moniker, Resolution resolution)
        {
            // Close current if it was opened
            CloseCamera();

            if (moniker == null)
                return;

            // Create camera object
            _Camera = new Camera();

            if (!string.IsNullOrEmpty(_DirectShowLogFilepath))
                _Camera.DirectShowLogFilepath = _DirectShowLogFilepath;

            // select resolution
            //ResolutionList resolutions = Camera.GetResolutionList(moniker);

            if (resolution != null)
            {
                _Camera.Resolution = resolution;
            }

            // Initialize
            _Camera.Initialize(this, moniker);

            // Build and Run graph
            _Camera.BuildGraph();
            _Camera.RunGraph();


            _Camera.OutputVideoSizeChanged += Camera_OutputVideoSizeChanged;
        }

        /// <summary>
        /// Close and dispose all camera and DirectX stuff.
        /// </summary>
        public void CloseCamera()
        {
            if (_Camera != null)
            {
                _Camera.StopGraph();
                _Camera.CloseAll();
                _Camera.Dispose();
                _Camera = null;
            }
        }

        #endregion

        // ====================================================================

        #region Public member variables

        /// <summary>
        /// Gets  a value that determines whether or not a Camera object was created.
        /// </summary>
        /// <seealso cref="Camera"/>
        [Browsable(false)] // hide from property browser
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // do not serialize to code ever
        public bool CameraCreated
        {
            get { return (_Camera != null); }
        }

        /// <summary>
        /// Gets a Camera object.
        /// </summary>
        /// <seealso cref="Camera"/>
        [Browsable(false)] // hide from property browser
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // do not serialize to code ever
        public Camera Camera
        {
            get { return _Camera; }
        }

        /// <summary>
        /// Gets a camera moniker (device identification).
        /// </summary> 
        [Browsable(false)] // hide from property browser
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // do not serialize to code ever
        public IMoniker Moniker
        {
            get
            {
                _ThrowIfCameraWasNotCreated();

                return _Camera.Moniker;
            }
        }

        /// <summary>
        /// Gets or sets a resolution of camera's output.
        /// </summary>
        /// <seealso cref="ResolutionListRGB"/>
        [Browsable(false)] // hide from property browser
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // do not serialize to code ever
        public Resolution Resolution
        {
            get
            {
                _ThrowIfCameraWasNotCreated();

                return _Camera.Resolution;
            }
            set
            {
                _ThrowIfCameraWasNotCreated();

                _Camera.Resolution = value;
            }
        }

        /// <summary>
        /// Gets a list of available resolutions (in RGB format).
        /// </summary>        
        [Browsable(false)] // hide from property browser
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // do not serialize to code ever
        public ResolutionList ResolutionListRGB
        {
            get
            {
                _ThrowIfCameraWasNotCreated();

                return _Camera.ResolutionListRGB;
            }
        }

        /// <summary>
        /// Gets or sets a value that determines whether or not the image mixer is enabled for camera output.
        /// </summary>
        /// <value>Set to true to enable image mixer for camera output, or false to disable.</value>
        [Browsable(false)] // hide from property browser
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // do not serialize to code ever
        public bool MixerEnabled
        {
            get
            {
                _ThrowIfCameraWasNotCreated();

                return _Camera.MixerEnabled;
            }
            set
            {
                _ThrowIfCameraWasNotCreated();

                _Camera.MixerEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a GDI Alpha value.
        /// </summary>
        [Browsable(false)] // hide from property browser
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // do not serialize to code ever
        public float GDIAlphaValue
        {
            get
            {
                _ThrowIfCameraWasNotCreated();

                return _Camera.GDIAlphaValue;
            }
            set
            {
                _ThrowIfCameraWasNotCreated();

                _Camera.GDIAlphaValue = value;
            }
        }

        /// <summary>
        /// Gets a size of video output.
        /// </summary> 
        /// <seealso cref="OutputVideoSizeChanged"/>
        [Browsable(false)] // hide from property browser
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // do not serialize to code ever
        public Size OutputVideoSize
        {
            get
            {
                _ThrowIfCameraWasNotCreated();

                return _Camera.OutputVideoSize;
            }
        }

        /// <summary>
        /// Gets or sets a RGB overlay bitmap used for GDI operations.
        /// </summary>         
        /// <seealso cref="GDIColorKey"/>
        [Browsable(false)] // hide from property browser
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // do not serialize to code ever
        public Bitmap OverlayBitmap
        {
            get
            {
                _ThrowIfCameraWasNotCreated();

                return _Camera.OverlayBitmap;
            }
            set
            {
                _ThrowIfCameraWasNotCreated();

                _Camera.OverlayBitmap = value;
            }
        }

        /// <summary>
        /// Gets or sets a color used as ColorKey for GDI operations.
        /// </summary> 
        /// <seealso cref="OverlayBitmap"/>
        [Browsable(false)] // hide from property browser
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // do not serialize to code ever
        public Color GDIColorKey
        {
            get
            {
                _ThrowIfCameraWasNotCreated();

                return _Camera.GDIColorKey;
            }
            set
            {
                _ThrowIfCameraWasNotCreated();

                _Camera.GDIColorKey = value;
            }
        }

        /// <summary>
        /// Gets a value that determines whether or not the crossbar is available for selected camera.
        /// </summary> 
        /// <seealso cref="VideoInput"/>
        [Browsable(false)] // hide from property browser
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // do not serialize to code ever
        public bool CrossbarAvailable
        {
            get
            {
                _ThrowIfCameraWasNotCreated();

                return _Camera.CrossbarAvailable;
            }
        }

        /// <summary>
        /// Gets or sets a video input of camera (via crossbar).
        /// </summary> 
        /// <seealso cref="CrossbarAvailable"/>
        [Browsable(false)] // hide from property browser
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // do not serialize to code ever
        public VideoInput VideoInput
        {
            get
            {
                _ThrowIfCameraWasNotCreated();

                return _Camera.VideoInput;
            }
            set
            {
                _ThrowIfCameraWasNotCreated();

                _Camera.VideoInput = value;
            }
        }

        /// <summary>
        /// Log file path for directshow (used in BuildGraph).
        /// </summary> 
        /// <seealso cref="BuildGraph"/>
        [Description("Log file path for DirectShow (used in BuildGraph)")]
        public string DirectShowLogFilepath
        {
            get
            {
                if (!CameraCreated)
                    return _DirectShowLogFilepath;
                else
                    return _Camera.DirectShowLogFilepath;
            }
            set
            {
                _DirectShowLogFilepath = value;

                if (CameraCreated)
                    _Camera.DirectShowLogFilepath = _DirectShowLogFilepath;
            }
        }


        #if USE_D3D
        /// <summary>
        /// Gets a value that determines whether GDI or Direct3D is used for drawing over mixer image.
        /// </summary> 
        /// <value>Set to true to use GDI for drawing over mixer image, or false to use Direct3D.</value>
        public bool UseGDI
        {
            get
            {
                _ThrowIfCameraWasNotCreated();

                return _Camera.UseGDI;
            }
            set
            {
                _ThrowIfCameraWasNotCreated();

                _Camera.UseGDI = value;
            }
        }
        #endif


        #endregion

        // ====================================================================

        #region Events

        /// <summary>
        /// Subscribe to this event to handle changing of size of video output <see cref="OutputVideoSize"/>.
        /// </summary>
        public event EventHandler OutputVideoSizeChanged;

        #endregion

        // ====================================================================

        #region Property pages (various settings dialogs)

        /// <summary>
        /// Displays property page for device.
        /// </summary>
        /// <param name="moniker">Moniker (device identification) of camera.</param>
        /// <param name="hwndOwner">The window handler for to make it parent of property page.</param>
        /// <seealso cref="Moniker"/>
        public static void DisplayPropertyPage_Device(IMoniker moniker, IntPtr hwndOwner)
        {
            Camera.DisplayPropertyPage_Device(moniker, hwndOwner);
        }

        /// <summary>
        /// Displays property page for crossbar if it's available.
        /// </summary>
        /// <param name="hwndOwner">The window handler for to make it parent of property page.</param>
        /// <seealso cref="CrossbarAvailable"/>
        public void DisplayPropertyPage_Crossbar(IntPtr hwndOwner)
        {
            _ThrowIfCameraWasNotCreated();

            _Camera.DisplayPropertyPage_Crossbar(hwndOwner);
        }

        /// <summary>
        /// Displays property page for capture filter.
        /// </summary>
        /// <param name="hwndOwner">The window handler for to make it parent of property page.</param>
        public void DisplayPropertyPage_CaptureFilter(IntPtr hwndOwner)
        {
            _ThrowIfCameraWasNotCreated();

            _Camera.DisplayPropertyPage_CaptureFilter(hwndOwner);
        }

        /// <summary>
        /// Displays property page for filter's pin output.
        /// </summary>
        /// <param name="hwndOwner">The window handler for to make it parent of property page.</param>
        public void DisplayPropertyPage_SourcePinOutput(IntPtr hwndOwner)
        {
            _ThrowIfCameraWasNotCreated();

            _Camera.DisplayPropertyPage_SourcePinOutput(hwndOwner);
        }

        #endregion

        // ====================================================================

        #region TV Mode

        /// <summary>
        /// Sets TV Mode for device.
        /// </summary>
        /// <param name="mode">TV Mode to set (analog video standard).</param>
        public void SetTVMode(AnalogVideoStandard mode)
        {
            _ThrowIfCameraWasNotCreated();
            
            _Camera.SetTVMode(mode);
        }

        /// <summary>
        /// Gets TV Mode of device.
        /// </summary>
        /// <returns>TV Mode of device (analog video standard)</returns>
        public AnalogVideoStandard GetTVMode()
        {
            _ThrowIfCameraWasNotCreated();

            return _Camera.GetTVMode();
        }

        #endregion

        // ====================================================================

        #region Spanshot (screenshots) frame

        /// <summary>
        /// Make snapshot of output image. Slow, but includes all graph's effects.
        /// </summary>
        /// <returns>Snapshot as a Bitmap</returns>
        /// <seealso cref="SnapshotSourceImage"/>
        public Bitmap SnapshotOutputImage()
        {
            _ThrowIfCameraWasNotCreated();

            return _Camera.SnapshotOutputImage();
        }

        /// <summary>
        /// Make snapshot of source image. Much faster than SnapshotOutputImage.
        /// </summary>
        /// <returns>Snapshot as a Bitmap</returns>
        /// <seealso cref="SnapshotOutputImage"/>
        public Bitmap SnapshotSourceImage()
        {
            _ThrowIfCameraWasNotCreated();

            return _Camera.SnapshotSourceImage();
        }

        #endregion

        // ====================================================================

        #region Coorinate convertions

        // Information: coordinate types:
        // 0) Normalized. [0.0 .. 1.0] for video signal
        // 1) Video.   Related to video stream (e.g. can be VGA (640x480)).
        // 2) Window.  Related to _HostingControl.ClientRectangle
        // 3) Overlay. Related to pixel is the same size as Window-type, but position is related to Video position

        /// <summary>
        /// Converts window coordinates to normalized.
        /// </summary>
        /// <param name="point">Point in window coordinates.</param>
        /// <returns>Normalized coordinates</returns>
        public PointF ConvertWinToNorm(PointF p)
        {
            _ThrowIfCameraWasNotCreated();

            return _Camera.ConvertWinToNorm(p);
        }

        /// <summary>
        /// Sets camera output rect (zooms to selected rect).
        /// </summary>
        /// <param name="zoomRect">Rectangle for zooming in video coordinates.</param>
        public void ZoomToRect(Rectangle ZoomRect)
        {
            _ThrowIfCameraWasNotCreated();

            _Camera.ZoomToRect(ZoomRect);
        }

        #endregion

        // ====================================================================

        #region Public Static functions

        /// <summary>
        /// Returns Moniker (device identification) of camera from device index.
        /// </summary>
        /// <param name="iDeviceIndex">Index (Zero-based) in list of available devices with VideoInputDevice filter category.</param>
        /// <returns>Moniker (device identification) of device</returns>
        public static IMoniker GetDeviceMoniker(int iDeviceNum)
        {
            return Camera.GetDeviceMoniker(iDeviceNum);
        }

        /// <summary>
        /// Returns available resolutions with RGB color system for device moniker.
        /// </summary>
        /// <param name="moniker">Moniker (device identification) of camera.</param>
        /// <returns>List of resolutions with RGB color system of device</returns>
        public static ResolutionList GetResolutionList(IMoniker moniker)
        {
            return Camera.GetResolutionList(moniker);
        }

        #endregion

        // ====================================================================
        
        #region Private stuff

        #region Private members

        /// <summary>
        /// Camera object (user control is a wrapper for it).
        /// </summary>
        private Camera _Camera = null;

        /// <summary>
        /// Log file path for DirectShow (should be saved longer than _Camera lives).
        /// </summary>
        private string _DirectShowLogFilepath = string.Empty;

        /// <summary>
        /// Message for exception when functions are called if camera not being created.
        /// </summary>
        private const string CameraWasNotCreatedMessage = @"Camera is not created.";

        /// <summary>
        /// Checks if camera is created and throws ApplicationException if not.
        /// </summary>
        private void _ThrowIfCameraWasNotCreated()
        {
            if (!CameraCreated)
                throw new Exception(CameraWasNotCreatedMessage);
        }

        #endregion

        /// <summary>
        /// Event handler for OutputVideoSizeChanged event.
        /// </summary>
        private void Camera_OutputVideoSizeChanged(object sender, EventArgs e)
        {
            // Call event handlers (External)
            if (OutputVideoSizeChanged != null)
            {
                OutputVideoSizeChanged(sender, e);
            }
        }

        #endregion

        // ====================================================================

    }
}
