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

    // Microsoft.Win32 is used for SystemEvents namespace
    using Microsoft.Win32;

    // Use DirectShowLib (LGPL v2.1)
    using DirectShowLib;

    #if USE_D3D
    using Microsoft.DirectX.Direct3D;
    #endif

    #endregion

    /// <summary>
    /// The Camera class is an main class that is a wrapper for video device.
    /// </summary>
    /// 
    /// <author> free5lot (free5lot@yandex.ru) </author>
    /// <version> 2013.12.16 </version>
    public class Camera : IDisposable
    {
        // ====================================================================

        #region Private members

        /// <summary>
        /// Private field. Use the public property <see cref="HostingControl"/> for access to this value.
        /// </summary>
        private Control _HostingControl = null;

        /// <summary>
        /// Private field. Use the public property <see cref="Moniker"/> for access to this value.
        /// </summary>
        private IMoniker _Moniker = null;

        /// <summary>
        /// Private field. Use the public property <see cref="Resolution"/> for access to this value.
        /// </summary>
        private Resolution _Resolution = null;

        /// <summary>
        /// Private field. Use the public property <see cref="ResolutionList"/> for access to this value.
        /// </summary>
        private ResolutionList _ResolutionList = new ResolutionList();

        /// <summary>
        /// Private field. Use the public property <see cref="MixerEnabled"/> for access to this value.
        /// </summary>
        private bool _bMixerEnabled = false;

        /// <summary>
        /// Private field. Use the public property <see cref="GDIAlphaValue"/> for access to this value.
        /// </summary>
        private float _GDIAlphaValue = 1.00f;

        /// <summary>
        /// Private field. Use the public property <see cref="OutputVideoSize"/> for access to this value.
        /// </summary>
        private Size _OutputVideoSize;

        /// <summary>
        /// Private field. Use the public property <see cref="OverlayBitmap"/> for access to this value.
        /// </summary>
        private Bitmap _OverlayBitmap = null;

        /// <summary>
        /// Private field. Use the public property <see cref="GDIColorKey"/> for access to this value.
        /// </summary>
        private Color _GDIColorKey = Color.Violet;

        /// <summary>
        /// Private field. Use the public property <see cref="VideoInput"/> for access to this value.
        /// </summary>
        private VideoInput _VideoInput = VideoInput.Default;

        /// <summary>
        /// Private field. Use the public property <see cref="DirectShowLogFilepath"/> for access to this value.
        /// </summary>
        private string _DirectShowLogFilepath = string.Empty;

        /// <summary>
        /// Private field for DirectShow log file handle.
        /// </summary>
        private IntPtr _DirectShowLogHandle = IntPtr.Zero;

        // Header's size of DIB image returned by IVMRWindowlessControl9::GetCurrentImage method (BITMAPINFOHEADER)
        private static readonly int DIB_Image_HeaderSize = Marshal.SizeOf(typeof(BitmapInfoHeader)); // == 40;


        #if USE_D3D
        /// <summary>
        /// Private field. Use the public property <see cref="UseGDI"/> for access to this value.
        /// </summary>
        private bool _UseGDI = true;
        #endif

        #endregion

        // ====================================================================

        #region Internal stuff

        /// <summary>
        /// Private field. DirectXInterfaces instance.
        /// </summary>
        internal DirectXInterfaces DX = new DirectXInterfaces();

        #if USE_D3D
        /// <summary>
        /// Private field. Direct3D mixing helper. It's created only when needed
        /// </summary>
        internal Direct3DMixing _pDirect3DMixing = null;
        #endif


        /// <summary>
        /// Private field. Was the graph built or not.
        /// </summary>
        internal bool _bGraphIsBuilt = false;

        /// <summary>
        /// Private field. Were handlers added or not. Needed to remove delegates
        /// </summary>
        internal bool _bHandlersAdded = false;

        /// <summary>
        /// Private field. Was mixer image used or not.
        /// </summary>
        internal bool _bMixerImageWasUsed = false;

        /// <summary>
        /// Private field. SampleGrabber helper (wrapper)
        /// </summary>
        internal SampleGrabberHelper _pSampleGrabberHelper = null;

        #if DEBUG
        /// <summary>
        /// Private field. DsROTEntry allows to "Connect to remote graph" from GraphEdit
        /// </summary>
        DsROTEntry _rot = null;
        #endif


        /// <summary>
        /// Internal. Displays a property page for a filter
        /// </summary>
        /// <param name="filter">The filter for which to display a property page.</param>
        /// <param name="hwndOwner">The window handler for to make it parent of property page.</param>
        internal static void DisplayPropertyPageFilter(IBaseFilter filter, IntPtr hwndOwner)
        {
            _DisplayPropertyPage(filter, hwndOwner);
        }
        internal static void DisplayPropertyPagePin(IPin pin, IntPtr hwndOwner)
        {
            _DisplayPropertyPage(pin, hwndOwner);
        }

        internal static void _DisplayPropertyPage(object filter_or_pin, IntPtr hwndOwner)
        {
            if (filter_or_pin == null)
                return;

            //Get the ISpecifyPropertyPages for the filter
            ISpecifyPropertyPages pProp = filter_or_pin as ISpecifyPropertyPages;
            int hr = 0;

            if (pProp == null)
            {
                //If the filter doesn't implement ISpecifyPropertyPages, try displaying IAMVfwCompressDialogs instead!
                IAMVfwCompressDialogs compressDialog = filter_or_pin as IAMVfwCompressDialogs;
                if (compressDialog != null)
                {

                    hr = compressDialog.ShowDialog(VfwCompressDialogs.Config, IntPtr.Zero);
                    DsError.ThrowExceptionForHR(hr);
                }
                return;
            }

            string caption = string.Empty;

            if ( filter_or_pin is IBaseFilter )
            {
                //Get the name of the filter from the FilterInfo struct
                IBaseFilter as_filter = filter_or_pin as IBaseFilter;
                FilterInfo filterInfo;
                hr = as_filter.QueryFilterInfo(out filterInfo);
                DsError.ThrowExceptionForHR(hr);

                caption = filterInfo.achName;

                if (filterInfo.pGraph != null)
                {
                    Marshal.ReleaseComObject(filterInfo.pGraph);
                }
            }
            else
            if ( filter_or_pin is IPin )
            {
                //Get the name of the filter from the FilterInfo struct
                IPin as_pin = filter_or_pin as IPin;
                PinInfo pinInfo;
                hr = as_pin.QueryPinInfo(out pinInfo);
                DsError.ThrowExceptionForHR(hr);

                caption = pinInfo.name;
            }


            // Get the propertypages from the property bag
            DsCAUUID caGUID;
            hr = pProp.GetPages(out caGUID);
            DsError.ThrowExceptionForHR(hr);

            // Create and display the OlePropertyFrame
            object oDevice = (object)filter_or_pin;
            hr = NativeMethodes.OleCreatePropertyFrame(hwndOwner, 0, 0, caption, 1, ref oDevice, caGUID.cElems, caGUID.pElems, 0, 0, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            // Release COM objects
            Marshal.FreeCoTaskMem(caGUID.pElems);
            Marshal.ReleaseComObject(pProp);
        }

        #endregion

        // ====================================================================

        #region Public properties
        
        /// <summary>
        /// Gets a control that is used for hosting camera's output.
        /// </summary>
        public Control HostingControl
        {
            get { return _HostingControl; }
        }

        /// <summary>
        /// Gets a camera moniker (device identification).
        /// </summary> 
        public IMoniker Moniker
        {
            get { return _Moniker; }
        }

        /// <summary>
        /// Gets or sets a resolution of camera's output.
        /// </summary>
        /// <seealso cref="ResolutionListRGB"/>
        public Resolution Resolution
        {
            get { return _Resolution; }
            set
            {
                // Change of resolution is not allowed after graph's built
                if (_bGraphIsBuilt)
                    throw new Exception(@"Change of resolution is not allowed after graph's built.");

                _Resolution = value;
            }
        }

        /// <summary>
        /// Gets a list of available resolutions (in RGB format).
        /// </summary>        
        public ResolutionList ResolutionListRGB
        {
            get { return _ResolutionList; }
        }

        /// <summary>
        /// Gets or sets a value that determines whether or not the image mixer is enabled for camera output.
        /// </summary>
        /// <value>Set to true to enable image mixer for camera output, or false to disable.</value>
        public bool MixerEnabled
        {
            get { return _bMixerEnabled; }
            set
            {
                _bMixerEnabled = value;
                UpdateMixerStuff();
            }
        }

        /// <summary>
        /// Gets or sets a GDI Alpha value.
        /// </summary>
        public float GDIAlphaValue
        {
            get { return _GDIAlphaValue; }
            set { _GDIAlphaValue = value; }
        }

        /// <summary>
        /// Gets a size of video output.
        /// </summary> 
        /// <seealso cref="OutputVideoSizeChanged"/>
        public Size OutputVideoSize
        {
            get { return _OutputVideoSize; }
        }

        /// <summary>
        /// Gets or sets a RGB overlay bitmap used for GDI operations.
        /// </summary>         
        /// <seealso cref="GDIColorKey"/>
        public Bitmap OverlayBitmap
        {
            get { return _OverlayBitmap; }
            set
            {
                _OverlayBitmap = value;
                if (_bMixerEnabled)
                {
                    UpdateMixerStuff();
                }
            }
        }

        /// <summary>
        /// Gets or sets a color used as ColorKey for GDI operations.
        /// </summary> 
        /// <seealso cref="OverlayBitmap"/>
        public Color GDIColorKey
        {
            get { return _GDIColorKey; }
            set
            {
                _GDIColorKey = value;
                // No update is needed probably (UpdateMixerStuff)
            }
        }

        /// <summary>
        /// Gets a value that determines whether or not the crossbar is available for selected camera.
        /// </summary> 
        /// <seealso cref="VideoInput"/>
        public bool CrossbarAvailable
        {
            get { return (DX.Crossbar != null); }
        }

        /// <summary>
        /// Gets or sets a video input of camera (via crossbar).
        /// </summary> 
        /// <seealso cref="CrossbarAvailable"/>
        public VideoInput VideoInput
        {
            get
            {
                return _VideoInput;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("VideoInput", "VideoInput should not be set to null, use Default instead.");

                _VideoInput = value;

                // If we need to change
                if (_bGraphIsBuilt)
                {
                    SetCrossbarInput(DX.Crossbar, _VideoInput);

                    _VideoInput = GetCrossbarInput(DX.Crossbar);
                }
            }
        }

        /// <summary>
        /// Log file path for directshow (used in BuildGraph)
        /// </summary> 
        /// <seealso cref="BuildGraph"/>
        public string DirectShowLogFilepath
        {
            get
            {
                return _DirectShowLogFilepath;
            }
            set
            {
                _DirectShowLogFilepath = value;

                ApplyDirectShowLogFile();
            }
        }




        #if USE_D3D
        /// <summary>
        /// Gets a value that determines whether GDI or Direct3D is used for drawing over mixer image.
        /// </summary> 
        /// <value>Set to true to use GDI for drawing over mixer image, or false to use Direct3D.</value>
        public bool UseGDI
        {
            get { return _UseGDI; }
            set
            {
                _UseGDI = value;

                if (_bMixerEnabled)
                {
                    UpdateMixerStuff();
                }
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

        #region Public Static functions

        /// <summary>
        /// Returns Moniker (device identification) of camera from device index.
        /// </summary>
        /// <param name="iDeviceIndex">Index (Zero-based) in list of available devices with VideoInputDevice filter category.</param>
        /// <returns>Moniker (device identification) of device</returns>
        public static IMoniker GetDeviceMoniker(int iDeviceIndex)
        {
            DsDevice[] capDevices;

            // Get the collection of video devices
            capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            if (iDeviceIndex >= capDevices.Length)
            {
                throw new Exception(@"No video capture devices found at that index.");
            }

            return capDevices[iDeviceIndex].Mon;
        }

        /// <summary>
        /// Returns available resolutions with RGB color system for device moniker
        /// </summary>
        /// <param name="moniker">Moniker (device identification) of camera.</param>
        /// <returns>List of resolutions with RGB color system of device</returns>
        public static ResolutionList GetResolutionList(IMoniker moniker)
        {
            int hr;

            ResolutionList ResolutionsAvailable = null; //new ResolutionList();

            // Get the graphbuilder object
            IFilterGraph2 filterGraph = new FilterGraph() as IFilterGraph2;
            IBaseFilter capFilter = null;

            try
            {
                // add the video input device
                hr = filterGraph.AddSourceFilterForMoniker(moniker, null, "Source Filter", out capFilter);
                DsError.ThrowExceptionForHR(hr);

                ResolutionsAvailable = GetResolutionsAvailable(capFilter);
            }
            finally
            {
                SafeReleaseComObject(filterGraph);
                filterGraph = null;

                SafeReleaseComObject(capFilter);
                capFilter = null;
            }

            return ResolutionsAvailable;
        }

        #endregion

        // ====================================================================

        #region Public member functions

        #region Create, Initialize and Dispose/Close

        /// <summary>
        /// Default constructor for <see cref="Camera"/> class.
        /// </summary>
        public Camera()
        {
        }

        /// <summary>
        /// Initializes camera and connects it to HostingControl and Moniker.
        /// </summary>
        /// <param name="hControl">Control that is used for hosting camera's output.</param>
        /// <param name="moniker">Moniker (device identification) of camera.</param>
        /// <seealso cref="HostingControl"/>
        /// <seealso cref="Moniker"/>
        public void Initialize(Control hControl, IMoniker moniker)
        {
            if ( hControl == null )
                throw new Exception(@"Hosting control should be set.");

            if ( moniker == null )
                throw new Exception(@"Camera's moniker should be set.");

            _Moniker = moniker;

            _HostingControl = hControl;

            #if USE_D3D
            if (!_UseGDI)
            {
                // Init Managed Direct3D
                InitializeDirect3DIfNeeded();
            }
            #endif
        }

        /// <summary>
        /// Destructor (disposer) for <see cref="Camera"/> class.
        /// </summary>
        ~Camera()
        {
            Dispose();
        }

        /// <summary>
        /// Dispose of <see cref="IDisposable"/> for <see cref="Camera"/> class.
        /// </summary>
        public void Dispose()
        {
            CloseAll();
        }

        /// <summary>
        /// Close and dispose all camera and DirectX stuff.
        /// </summary>
        public void CloseAll()
        {
            _bGraphIsBuilt = false;

            // close log file if needed
            try
            {
                CloseDirectShowLogFile();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            // stop rendering
            if (DX.MediaControl != null)
            {
                try
                {
                    DX.MediaControl.StopWhenReady();
                    DX.MediaControl.Stop();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            if (_bHandlersAdded)
            {
                RemoveHandlers();
            }

            //FilterGraphTools.RemoveAllFilters(this.graphBuilder);

#if DEBUG
            if (_rot != null)
            {
                _rot.Dispose();
            }
#endif

            // Dispose Managed Direct3D objects
            if (_pSampleGrabberHelper != null)
            {
                _pSampleGrabberHelper.Dispose();
                _pSampleGrabberHelper = null;
            }

            DX.CloseInterfaces();

            _bMixerImageWasUsed = false;
            _bMixerEnabled = false;

#if USE_D3D
            _UseGDI = true;

            // Dispose Managed Direct3D objects
            if (_pDirect3DMixing != null)
            {
                _pDirect3DMixing.Dispose();
                _pDirect3DMixing = null;
            }
#endif
        }

        #endregion

        #region Graph: Build, Run, Stop



        /// <summary>
        /// Builds DirectShow graph for rendering.
        /// </summary>
        public void BuildGraph()
        {
            _bGraphIsBuilt = false;

            try
            {
                // -------------------------------------------------------
                DX.FilterGraph = (IFilterGraph2)new FilterGraph();
                DX.MediaControl = (IMediaControl)DX.FilterGraph;

                // Log file if needed
                ApplyDirectShowLogFile();

#if DEBUG
                // Allows you to view the graph with GraphEdit File/Connect
                _rot = new DsROTEntry(DX.FilterGraph);
#endif

                // -------------------------------------------------------
                GraphBuilding_AddFilters();

                GraphBuilding_ConnectPins();

                // -------------------------------------------------------
                PostActions_SampleGrabber();
                PostActions_Renderer();
                // -------------------------------------------------------

                UpdateOutputVideoSize();

                SetMixerSettings();
                

                // -------------------------------------------------------
                _bGraphIsBuilt = true;
                // -------------------------------------------------------

            }
            catch
            {
                CloseAll();
                throw;
            }

#if DEBUG
            // Double check to make sure we aren't releasing something
            // important.
            GC.Collect();
            GC.WaitForPendingFinalizers();
#endif
        }

        /// <summary>
        /// Runs DirectShow graph for rendering.
        /// </summary>
        public void RunGraph()
        {
            //var graph_guilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            //int hr = graph_guilder.RenderStream(PinCategory.Preview, MediaType.Video, DX.CaptureFilter, null, DX.VMRenderer);
            //DsError.ThrowExceptionForHR(hr);

            if (DX.MediaControl != null)
            {
                int hr = DX.MediaControl.Run();
                DsError.ThrowExceptionForHR(hr);
            }

        }

        /// <summary>
        /// Stops DirectShow graph for rendering.
        /// </summary>
        public void StopGraph()
        {
            if (DX.MediaControl != null)
            {
                int hr = DX.MediaControl.Stop();
                DsError.ThrowExceptionForHR(hr);
            }
        }
        #endregion

        #region Property pages (various settings dialogs)

        /// <summary>
        /// Displays property page for device.
        /// </summary>
        /// <param name="moniker">Moniker (device identification) of camera.</param>
        /// <param name="hwndOwner">The window handler for to make it parent of property page.</param>
        /// <seealso cref="Moniker"/>
        public static void DisplayPropertyPage_Device(IMoniker moniker, IntPtr hwndOwner)
        {
            if (moniker == null)
                return;

            object source = null;
            Guid iid = typeof(IBaseFilter).GUID;
            moniker.BindToObject(null, null, ref iid, out source);
            IBaseFilter theDevice = (IBaseFilter)source;

            DisplayPropertyPageFilter(theDevice, hwndOwner);

            //Release COM objects
            SafeReleaseComObject(theDevice);
            theDevice = null;
        }

        /// <summary>
        /// Displays property page for crossbar if it's available.
        /// </summary>
        /// <param name="hwndOwner">The window handler for to make it parent of property page.</param>
        /// <seealso cref="CrossbarAvailable"/>
        public void DisplayPropertyPage_Crossbar(IntPtr hwndOwner)
        {
            if (DX.Crossbar == null)
                return;

            DisplayPropertyPageFilter((IBaseFilter)DX.Crossbar, hwndOwner);

            // update VideoInput - it can be changed
            _VideoInput = GetCrossbarInput(DX.Crossbar);
        }

        /// <summary>
        /// Displays property page for capture filter.
        /// </summary>
        /// <param name="hwndOwner">The window handler for to make it parent of property page.</param>
        public void DisplayPropertyPage_CaptureFilter(IntPtr hwndOwner)
        {
            DisplayPropertyPageFilter(DX.CaptureFilter, hwndOwner);
        }

        /// <summary>
        /// Displays property page for filter's pin output.
        /// </summary>
        /// <param name="hwndOwner">The window handler for to make it parent of property page.</param>
        public void DisplayPropertyPage_SourcePinOutput(IntPtr hwndOwner)
        {
            IPin pinSourceCapture = null;

            try
            {
                pinSourceCapture = DsFindPin.ByDirection(DX.CaptureFilter, PinDirection.Output, 0);
                DisplayPropertyPagePin(pinSourceCapture, hwndOwner);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                SafeReleaseComObject(pinSourceCapture);
                pinSourceCapture = null;
            }
        }


        #endregion

        #region TV Mode

        /// <summary>
        /// Sets TV Mode for device.
        /// </summary>
        /// <param name="mode">TV Mode to set (analog video standard).</param>
        public void SetTVMode(AnalogVideoStandard mode)
        {
            if (DX.CaptureFilter == null)
                return;

            IAMAnalogVideoDecoder pDecoder = DX.CaptureFilter as IAMAnalogVideoDecoder;

            if (pDecoder == null)
                return;

            int hr = pDecoder.put_TVFormat(mode);
            DsError.ThrowExceptionForHR(hr);

            //Marshal.ReleaseComObject(pDecoder);
        }

        /// <summary>
        /// Gets TV Mode of device.
        /// </summary>
        /// <returns>TV Mode of device (analog video standard)</returns>
        public AnalogVideoStandard GetTVMode()
        {
            if (DX.CaptureFilter == null)
                return AnalogVideoStandard.None;

            IAMAnalogVideoDecoder pDecoder = DX.CaptureFilter as IAMAnalogVideoDecoder;

            if (pDecoder == null)
                return AnalogVideoStandard.None;

            AnalogVideoStandard mode = AnalogVideoStandard.None;
            int hr = pDecoder.get_TVFormat(out mode);
            DsError.ThrowExceptionForHR(hr);

            //Marshal.ReleaseComObject(pDecoder);

            return mode;
        }

        #endregion

        #region Spanshot (screenshots) frame

        /// <summary>
        /// Make snapshot of output image. Slow, but includes all graph's effects.
        /// </summary>
        /// <returns>Snapshot as a Bitmap</returns>
        /// <seealso cref="SnapshotSourceImage"/>
        public Bitmap SnapshotOutputImage()
        {
            if (DX.WindowlessCtrl == null)
                throw new Exception("WindowlessCtrl is not initialized.");

            IntPtr currentImage = IntPtr.Zero;
            Bitmap bitmap = null;
            Bitmap bitmap_clone = null;

            try
            {
                int hr = DX.WindowlessCtrl.GetCurrentImage(out currentImage);
                DsError.ThrowExceptionForHR(hr);

                if (currentImage != IntPtr.Zero)
                {
                    BitmapInfoHeader structure = new BitmapInfoHeader();
                    Marshal.PtrToStructure(currentImage, structure);

                    PixelFormat pixelFormat = PixelFormat.Format24bppRgb;
                    switch (structure.BitCount)
                    {
                        case 24:
                            pixelFormat = PixelFormat.Format24bppRgb;
                            break;
                        case 32:
                            pixelFormat = PixelFormat.Format32bppRgb;
                            break;
                        case 48:
                            pixelFormat = PixelFormat.Format48bppRgb;
                            break;
                        default:
                            throw new Exception("Unsupported BitCount.");
                    }

                    bitmap = new Bitmap(structure.Width, structure.Height, (structure.BitCount / 8) * structure.Width, pixelFormat, new IntPtr(currentImage.ToInt64() + DIB_Image_HeaderSize));

                    bitmap_clone = bitmap.Clone(new Rectangle(0, 0, structure.Width, structure.Height), PixelFormat.Format24bppRgb);

                    bitmap_clone.RotateFlip(RotateFlipType.RotateNoneFlipY);
                }
            }
            catch
            {
                if (bitmap != null)
                {
                    bitmap.Dispose();
                    bitmap = null;
                }

                throw;
            }
            finally
            {
                Marshal.FreeCoTaskMem(currentImage);
            }

            return bitmap_clone;
        }

        /// <summary>
        /// Make snapshot of source image. Much faster than SnapshotOutputImage.
        /// </summary>
        /// <returns>Snapshot as a Bitmap</returns>
        /// <seealso cref="SnapshotOutputImage"/>
        public Bitmap SnapshotSourceImage()
        {
            if (_pSampleGrabberHelper == null)
                throw new Exception("SampleGrabberHelper is not initialized.");

            return _pSampleGrabberHelper.SnapshotNextFrame();
        }

        #endregion

        #endregion

        // ====================================================================

        #region Private members

        #region Graph building stuff
        
        /// <summary>
        /// Adds filters to DirectShow graph.
        /// </summary>
        private void GraphBuilding_AddFilters()
        {
            AddFilter_Source();
            SetSourceParams();

            AddFilter_Renderer();

            AddFilter_Crossbar();

            AddFilter_TeeSplitter();

            AddFilter_SampleGrabber();
        }

        /// <summary>
        /// Sets the Framerate, and video size.
        /// </summary>
        private void SetSourceParams()
        {
            // Pins used in graph
            IPin pinSourceCapture = null;

            try
            {
                // Collect pins
                //pinSourceCapture = DsFindPin.ByCategory(DX.CaptureFilter, PinCategory.Capture, 0);
                pinSourceCapture = DsFindPin.ByDirection(DX.CaptureFilter, PinDirection.Output, 0);

                SetSourceParams(pinSourceCapture, _Resolution);
            }
            catch
            {
                throw;
            }
            finally
            {
                SafeReleaseComObject(pinSourceCapture);
                pinSourceCapture = null;
            }
        }

        /// <summary>
        /// Checks if AMMediaType's resolution is appropriate for desired resolution.
        /// </summary>
        /// <param name="media_type">Media type to analyze.</param>
        /// <param name="resolution_desired">Desired resolution. Can be null or have 0 for height or width if it's not important.</param>
        private static bool IsResolutionAppropiate(AMMediaType media_type, Resolution resolution_desired)
        {
            // if we were asked to choose resolution
            if (resolution_desired == null)
                return true;

            VideoInfoHeader videoInfoHeader = new VideoInfoHeader();
            Marshal.PtrToStructure(media_type.formatPtr, videoInfoHeader);

            if (resolution_desired.Width > 0 &&
                videoInfoHeader.BmiHeader.Width != resolution_desired.Width)
            {
                return false;
            }
            if (resolution_desired.Height > 0 &&
                videoInfoHeader.BmiHeader.Height != resolution_desired.Height)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get resoltuin from if AMMediaType's resolution is appropriate for resolution_desired
        /// </summary>
        /// <param name="media_type">Media type to analyze.</param>
        /// <param name="resolution_desired">Desired resolution. Can be null or have 0 for height or width if it's not important.</param>
        private static Resolution GetResolutionForMediaType(AMMediaType media_type)
        {
            VideoInfoHeader videoInfoHeader = new VideoInfoHeader();
            Marshal.PtrToStructure(media_type.formatPtr, videoInfoHeader);

            return new Resolution(videoInfoHeader.BmiHeader.Width, videoInfoHeader.BmiHeader.Height);
        }


        /// <summary>
        /// Get bit count for mediatype
        /// </summary>
        /// <param name="media_type">Media type to analyze.</param>
        private static short GetBitCountForMediaType(AMMediaType media_type)
        {

            VideoInfoHeader videoInfoHeader = new VideoInfoHeader();
            Marshal.PtrToStructure(media_type.formatPtr, videoInfoHeader);

            return videoInfoHeader.BmiHeader.BitCount;
        }

        /// <summary>
        /// Check if bit count is appropriate for us
        /// </summary>
        /// <param name="media_type">Media type to analyze.</param>
        private static bool IsBitCountAppropriate(short bit_count)
        {
            if (bit_count == 16 ||
                bit_count == 24 ||
                bit_count == 32)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Analyze AMMediaType during enumeration and decide if it's good choice for us.
        /// </summary>
        /// <param name="media_type">Media type to analyze.</param>
        /// <param name="resolution_desired">Desired resolution.</param>
        private static void AnalyzeMediaType(AMMediaType media_type, Resolution resolution_desired, out bool bit_count_ok, out bool sub_type_ok, out bool resolution_ok)
        {
            // ---------------------------------------------------
            short bit_count = GetBitCountForMediaType(media_type);

            bit_count_ok = IsBitCountAppropriate(bit_count);

            // ---------------------------------------------------

            // We want (A)RGB32, RGB24 or RGB16 and YUY2.
            // These have priority
            // Change this if you're not agree.
            sub_type_ok =  (
                media_type.subType == MediaSubType.RGB32 ||
                media_type.subType == MediaSubType.ARGB32 ||
                media_type.subType == MediaSubType.RGB24 ||
                media_type.subType == MediaSubType.RGB16_D3D_DX9_RT ||
                media_type.subType == MediaSubType.RGB16_D3D_DX7_RT ||
                media_type.subType == MediaSubType.YUY2);

            // ---------------------------------------------------

            // flag to show if media_type's resolution is appropriate for us
            resolution_ok = IsResolutionAppropiate(media_type, resolution_desired);
            // ---------------------------------------------------
        }

        /// <summary>
        /// Sets parameters for source capture pin.
        /// </summary>
        /// <param name="pinSourceCapture">Pin of source capture.</param>
        /// <param name="resolution">Resolution to set if possible.</param>
        private static void SetSourceParams(IPin pinSourceCapture, Resolution resolution_desired)
        {
            int hr = 0;

            AMMediaType media_type_most_appropriate = null;
            AMMediaType media_type = null;

            //NOTE: pSCC is not used. All we need is media_type
            IntPtr pSCC = IntPtr.Zero;


            bool appropriate_media_type_found = false;

            try
            {
                IAMStreamConfig videoStreamConfig = pinSourceCapture as IAMStreamConfig;

                // -------------------------------------------------------------------------
                // We want the interface to expose all media types it supports and not only the last one set
                hr = videoStreamConfig.SetFormat(null);
                DsError.ThrowExceptionForHR(hr);

                int piCount = 0;
                int piSize = 0;

                hr = videoStreamConfig.GetNumberOfCapabilities(out piCount, out piSize);
                DsError.ThrowExceptionForHR(hr);

                for (int i = 0; i < piCount; i++)
                {
                    // ---------------------------------------------------
                    pSCC = Marshal.AllocCoTaskMem(piSize);
                    videoStreamConfig.GetStreamCaps(i, out media_type, pSCC);
                    FreeSCCMemory(ref pSCC);

                    // NOTE: we could use VideoStreamConfigCaps.InputSize or something like that to get resolution, but it's deprecated
                    //VideoStreamConfigCaps videoStreamConfigCaps = (VideoStreamConfigCaps)Marshal.PtrToStructure(pSCC, typeof(VideoStreamConfigCaps));
                    // ---------------------------------------------------

                    bool bit_count_ok = false;
                    bool sub_type_ok = false;
                    bool resolution_ok = false;

                    AnalyzeMediaType(media_type, resolution_desired, out bit_count_ok, out sub_type_ok, out resolution_ok);

                    if (bit_count_ok && resolution_ok)
                    {
                        if (sub_type_ok)
                        {
                            hr = videoStreamConfig.SetFormat(media_type);
                            DsError.ThrowExceptionForHR(hr);

                            appropriate_media_type_found = true;
                            break; // stop search, we've found appropriate media type
                        }
                        else
                        {
                            // save as appropriate if no other found
                            if (media_type_most_appropriate == null)
                            {
                                media_type_most_appropriate = media_type;
                                media_type = null; // we don't want for free it, now it's media_type_most_appropriate's problem
                            }
                        }
                    }
                    
                    FreeMediaType(ref media_type);
                }

                if (!appropriate_media_type_found)
                {
                    // Found nothing exactly as we were asked 

                    if ( media_type_most_appropriate != null)
                    {
                        // set appropriate RGB format with different resolution
                        hr = videoStreamConfig.SetFormat(media_type_most_appropriate);
                        DsError.ThrowExceptionForHR(hr);
                    }
                    else
                    {
                        // throw. We didn't find exactly what we were asked to
                        throw new Exception("Camera doesn't support media type with requested resolution and bits per pixel.");
                        //DsError.ThrowExceptionForHR(DsResults.E_InvalidMediaType);
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                // clean up
                FreeMediaType(ref media_type);
                FreeMediaType(ref media_type_most_appropriate);
                FreeSCCMemory(ref pSCC);

            }
        }

        /// <summary>
        /// Connects pins of graph
        /// </summary>
        private void GraphBuilding_ConnectPins()
        {
            // Pins used in graph
            IPin pinSourceCapture = null;

            IPin pinTeeInput = null;
            IPin pinTeePreview = null;
            IPin pinTeeCapture = null;

            IPin pinSampleGrabberInput = null;

            IPin pinRendererInput = null;

            int hr = 0;

            try
            {
                // Collect pins
                //pinSourceCapture = DsFindPin.ByCategory(DX.CaptureFilter, PinCategory.Capture, 0);
                pinSourceCapture = DsFindPin.ByDirection(DX.CaptureFilter, PinDirection.Output, 0);

                pinTeeInput = DsFindPin.ByDirection(DX.SmartTee, PinDirection.Input, 0);
                pinTeePreview = DsFindPin.ByName(DX.SmartTee, "Preview");
                pinTeeCapture = DsFindPin.ByName(DX.SmartTee, "Capture");

                pinSampleGrabberInput = DsFindPin.ByDirection(DX.SampleGrabberFilter, PinDirection.Input, 0);
                pinRendererInput = DsFindPin.ByDirection(DX.VMRenderer, PinDirection.Input, 0);

                // Connect source to tee splitter
                hr = DX.FilterGraph.Connect(pinSourceCapture, pinTeeInput);
                DsError.ThrowExceptionForHR(hr);

                // Connect samplegrabber on preview-pin of tee splitter
                hr = DX.FilterGraph.Connect(pinTeePreview, pinSampleGrabberInput);
                DsError.ThrowExceptionForHR(hr);

                // Connect the capture-pin of tee splitter to the renderer
                hr = DX.FilterGraph.Connect(pinTeeCapture, pinRendererInput);
                DsError.ThrowExceptionForHR(hr);
            }
            catch
            {
                throw;
            }
            finally
            {
                SafeReleaseComObject(pinSourceCapture);
                pinSourceCapture = null;

                SafeReleaseComObject(pinTeeInput);
                pinTeeInput = null;

                SafeReleaseComObject(pinTeePreview);
                pinTeePreview = null;

                SafeReleaseComObject(pinTeeCapture);
                pinTeeCapture = null;

                SafeReleaseComObject(pinSampleGrabberInput);
                pinSampleGrabberInput = null;

                SafeReleaseComObject(pinRendererInput);
                pinRendererInput = null;
            }
        }
        
        #endregion

        #region Filters

        /// <summary>
        /// Adds video source filter to the filter graph.
        /// </summary>
        private void AddFilter_Source()
        {
            int hr = 0;

            DX.CaptureFilter = null;
            hr = DX.FilterGraph.AddSourceFilterForMoniker(_Moniker, null, "Source Filter", out DX.CaptureFilter);
            DsError.ThrowExceptionForHR(hr);

            _ResolutionList = GetResolutionsAvailable(DX.CaptureFilter);
        }

        /// <summary>
        /// Adds crossbar filter to the filter graph.
        /// </summary>
        private void AddFilter_Crossbar()
        {
            // NOTE: It's hard to add suitable crossbar manually
            // It's easy to add it by using ICaptureGraphBuilder2
            // This way seems to be ugly (and it totally is)
            // but it's Microsoft's recommended approach 
            // See http://msdn.microsoft.com/en-us/library/windows/desktop/dd390973%28v=vs.85%29.aspx
            // --------------------------------------------------

            int hr = 0;

            DX.Crossbar = null;

            ICaptureGraphBuilder2 graphBuilder = null;

            try
            {
                graphBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();

                // set filter graph to the capture graph builder
                hr = graphBuilder.SetFiltergraph((IGraphBuilder)DX.FilterGraph);
                DsError.ThrowExceptionForHR(hr);

                // get crossbar object to to allows configuring pins of capture card
                object crossbarObject = null;

                graphBuilder.FindInterface(FindDirection.UpstreamOnly, Guid.Empty, DX.CaptureFilter, typeof(IAMCrossbar).GUID, out crossbarObject);
                if (crossbarObject != null)
                {
                    DX.Crossbar = (IAMCrossbar)crossbarObject;
                }

            }
            finally
            {
                SafeReleaseComObject(graphBuilder);
                graphBuilder = null;
            }

            // check is crossbar is needed
            if (DX.Crossbar != null)
            {
                hr = DX.FilterGraph.AddFilter((IBaseFilter)DX.Crossbar, "Crossbar");
                DsError.ThrowExceptionForHR(hr);

                // Route Crossbar's inputs
                SetCrossbarInput(DX.Crossbar, _VideoInput);

                _VideoInput = GetCrossbarInput(DX.Crossbar);
            }
        }

        /// <summary>
        /// Adds VMR9 (renderer) filter to the filter graph.
        /// </summary>
        private void AddFilter_Renderer()
        {
            int hr = 0;

            DX.VMRenderer = (IBaseFilter) new VideoMixingRenderer9();

            ConfigureVMRInWindowlessMode();


            hr = DX.FilterGraph.AddFilter(DX.VMRenderer, "Video Mixing Renderer 9");
            DsError.ThrowExceptionForHR(hr);
        }

        /// <summary>
        /// Does stuff for renderer (set video size, resolution, etc.) after graph building before rendering.
        /// </summary>
        private void PostActions_Renderer()
        {
            int hr = 0;

            // Save resolution
            int video_width, video_height, arW, arH;
            hr = DX.WindowlessCtrl.GetNativeVideoSize(out video_width, out video_height, out arW, out arH);
            DsError.ThrowExceptionForHR(hr);

            _Resolution = new Resolution(video_width, video_height);


            // For VMR9
            DX.MixerBitmap = (IVMRMixerBitmap9)DX.VMRenderer;

            #region Code for VMR7 to set PointFiltering
            // For VMR7:
            //mixerBitmap = (IVMRMixerBitmap)vmr;

            // For VMR7:
            //// Request point filtering (instead of bilinear filtering)
            //IVMRMixerControl pMixerControl = (IVMRMixerControl) vmr;

            //VMRMixerPrefs dwMixerPrefs = 0;
            //hr = pMixerControl.GetMixingPrefs(out dwMixerPrefs);
            //DsError.ThrowExceptionForHR(hr);

            //dwMixerPrefs = (dwMixerPrefs | VMRMixerPrefs.PointFiltering) & (~VMRMixerPrefs.BiLinearFiltering);

            //hr = pMixerControl.SetMixingPrefs(dwMixerPrefs);
            //DsError.ThrowExceptionForHR(hr);
            #endregion
        }

        /// <summary>
        /// Configures VMR9 to run in hosting control and etc.
        /// </summary>
        private void ConfigureVMRInWindowlessMode()
        {
            int hr = 0;

            IVMRFilterConfig9 filterConfig = (IVMRFilterConfig9)DX.VMRenderer;

            // Not really needed for vmr but don't forget calling it with VMR7
            hr = filterConfig.SetNumberOfStreams(1);
            DsError.ThrowExceptionForHR(hr);

            // Change vmr mode to Windowless
            hr = filterConfig.SetRenderingMode(VMR9Mode.Windowless);
            DsError.ThrowExceptionForHR(hr);

            DX.WindowlessCtrl = (IVMRWindowlessControl9)DX.VMRenderer;

            // Set "Parent" window
            if (_HostingControl != null)
            {
                hr = DX.WindowlessCtrl.SetVideoClippingWindow(_HostingControl.Handle);
                DsError.ThrowExceptionForHR(hr);
            }

            // Set Aspect-Ratio
            hr = DX.WindowlessCtrl.SetAspectRatioMode(VMR9AspectRatioMode.LetterBox);
            DsError.ThrowExceptionForHR(hr);

            // Add delegates for Windowless operations
            AddHandlers();

            // Call the resize handler to configure the output size
            HostingControl_ResizeMove(null, null);
        }
        
        /// <summary>
        /// Adds tee splitter filter to split for grabber and for capture.
        /// </summary>
        private void AddFilter_TeeSplitter()
        {
            int hr = 0;

            // Add a splitter
            DX.SmartTee = (IBaseFilter)new SmartTee();

            hr = DX.FilterGraph.AddFilter(DX.SmartTee, "SmartTee");
            DsError.ThrowExceptionForHR(hr);
        }
        
        /// <summary>
        /// Adds SampleGrabber for screenshot making.
        /// </summary>
        private void AddFilter_SampleGrabber()
        {
            int hr = 0;

            // Get the SampleGrabber interface
            DX.SampleGrabber = new SampleGrabber() as ISampleGrabber;
            
            // Configure the sample grabber
            DX.SampleGrabberFilter = DX.SampleGrabber as IBaseFilter;
            _pSampleGrabberHelper = new SampleGrabberHelper(DX.SampleGrabber, false);

            _pSampleGrabberHelper.ConfigureMode();

            // Add the sample grabber to the graph
            hr = DX.FilterGraph.AddFilter(DX.SampleGrabberFilter, "Sample Grabber");
            DsError.ThrowExceptionForHR(hr);
        }

        /// <summary>
        /// Does stuff for SampleGrabber after graph building before rendering.
        /// </summary>
        private void PostActions_SampleGrabber()
        {
            _pSampleGrabberHelper.SaveMode();
        }

        #endregion

        #region Crossbar helpers
        
        /// <summary>
        /// Gets type of input connected to video output of the crossbar.
        /// </summary>
        /// <param name="crossbar">The crossbar of device.</param>
        /// <returns>Video input of device</returns>
        /// <seealso cref="CrossbarAvailable"/>
        private static VideoInput GetCrossbarInput(IAMCrossbar crossbar)
        {
            VideoInput videoInput = VideoInput.Default;

            int inPinsCount, outPinsCount;

            // gen number of pins in the crossbar
            if (crossbar.get_PinCounts(out outPinsCount, out inPinsCount) == 0)
            {
                int videoOutputPinIndex = -1;
                int pinIndexRelated;
                PhysicalConnectorType type;

                // find index of the video output pin
                for (int i = 0; i < outPinsCount; i++)
                {
                    if (crossbar.get_CrossbarPinInfo(false, i, out pinIndexRelated, out type) != 0)
                        continue;

                    if (type == PhysicalConnectorType.Video_VideoDecoder)
                    {
                        videoOutputPinIndex = i;
                        break;
                    }
                }

                if (videoOutputPinIndex != -1)
                {
                    int videoInputPinIndex;

                    // get index of the input pin connected to the output
                    if (crossbar.get_IsRoutedTo(videoOutputPinIndex, out videoInputPinIndex) == 0)
                    {
                        PhysicalConnectorType inputType;

                        crossbar.get_CrossbarPinInfo(true, videoInputPinIndex, out pinIndexRelated, out inputType);

                        videoInput = new VideoInput(videoInputPinIndex, inputType);
                    }
                }
            }

            return videoInput;
        }

        /// <summary>
        /// Sets type of input connected to video output of the crossbar.
        /// </summary>
        /// <param name="crossbar">The crossbar of device.</param>
        /// <param name="videoInput">Video input of device.</param>
        /// <seealso cref="CrossbarAvailable"/>
        private static void SetCrossbarInput(IAMCrossbar crossbar, VideoInput videoInput)
        {
            if (videoInput.Type != VideoInput.PhysicalConnectorType_Default &&
                videoInput.Index != -1)
            {
                int inPinsCount, outPinsCount;

                // gen number of pins in the crossbar
                if (crossbar.get_PinCounts(out outPinsCount, out inPinsCount) == 0)
                {
                    int videoOutputPinIndex = -1;
                    int videoInputPinIndex = -1;
                    int pinIndexRelated;
                    PhysicalConnectorType type;

                    // find index of the video output pin
                    for (int i = 0; i < outPinsCount; i++)
                    {
                        if (crossbar.get_CrossbarPinInfo(false, i, out pinIndexRelated, out type) != 0)
                            continue;

                        if (type == PhysicalConnectorType.Video_VideoDecoder)
                        {
                            videoOutputPinIndex = i;
                            break;
                        }
                    }

                    // find index of the required input pin
                    for (int i = 0; i < inPinsCount; i++)
                    {
                        if (crossbar.get_CrossbarPinInfo(true, i, out pinIndexRelated, out type) != 0)
                            continue;

                        if ((type == videoInput.Type) && (i == videoInput.Index))
                        {
                            videoInputPinIndex = i;
                            break;
                        }
                    }

                    // try connecting pins
                    if ((videoInputPinIndex != -1) && (videoOutputPinIndex != -1))
                    {
                        if (crossbar.CanRoute(videoOutputPinIndex, videoInputPinIndex) == 0)
                        {
                            int hr = crossbar.Route(videoOutputPinIndex, videoInputPinIndex);
                            DsError.ThrowExceptionForHR(hr);
                        }
                        else
                        {
                            throw new Exception("Can't route from selected VideoInput to VideoDecoder.");
                        }
                    }
                    else
                    {
                        throw new Exception("Can't find routing pins.");
                    }

                }
            }
        }

        #endregion

        #region Internal event handlers for HostingControl and system

        /// <summary>
        /// Adds event handlers for hosting control.
        /// </summary>
        /// <seealso cref="HostingControl"/>
        private void AddHandlers()
        {
            if (_HostingControl == null)
                throw new Exception("Can't add handlers. Hosting control is not set.");

            // Add handlers for VMR purpose
            _HostingControl.Paint += new PaintEventHandler(HostingControl_Paint); // for WM_PAINT
            _HostingControl.Resize += new EventHandler(HostingControl_ResizeMove); // for WM_SIZE
            _HostingControl.Move += new EventHandler(HostingControl_ResizeMove); // for WM_MOVE
            SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingsChanged); // for WM_DISPLAYCHANGE
            _bHandlersAdded = true;
        }

        /// <summary>
        /// Removes event handlers for hosting control.
        /// </summary>
        /// <seealso cref="HostingControl"/>
        private void RemoveHandlers()
        {
            if (_HostingControl == null)
                throw new Exception("Can't remove handlers. Hosting control is not set.");

            // remove handlers when they are no more needed
            _bHandlersAdded = false;
            _HostingControl.Paint -= new PaintEventHandler(HostingControl_Paint);
            _HostingControl.Resize -= new EventHandler(HostingControl_ResizeMove);
            _HostingControl.Move -= new EventHandler(HostingControl_ResizeMove);
            SystemEvents.DisplaySettingsChanged -= new EventHandler(SystemEvents_DisplaySettingsChanged);
        }

        /// <summary>
        /// Handler of Paint event of HostingControl.
        /// </summary>
        /// <seealso cref="HostingControl"/>
        private void HostingControl_Paint(object sender, PaintEventArgs e)
        {
            if (!_bGraphIsBuilt)
                return; // Do nothing before graph was built

            if (DX.WindowlessCtrl != null && _HostingControl != null)
            {
                IntPtr hdc = e.Graphics.GetHdc();
                try
                {
                    int hr = DX.WindowlessCtrl.RepaintVideo(_HostingControl.Handle, hdc);
                }
                catch (System.Runtime.InteropServices.COMException ex)
                {
                    // Catch com-expection VFW_E_BUFFER_NOTSET (0x8004020c) in RepaintVideo() and ignore it
                    // it can be in the moment of moving window out of first monitor to second one.
                    // NOTE: This could be probably fixed with checking if graph is running or not
                    if (ex.ErrorCode == DsResults.E_BufferNotSet)
                    {
                        ; // ignore exception
                    }
                    else
                    {
                        throw; // re-throw exception up
                    }
                
                }
                finally
                {
                    e.Graphics.ReleaseHdc(hdc);
                }
            }
        }

        /// <summary>
        /// Handler of Resize and Move events of HostingControl.
        /// </summary>
        /// <seealso cref="HostingControl"/>
        private void HostingControl_ResizeMove(object sender, EventArgs e)
        {
            if (DX.WindowlessCtrl == null)
                return;
            if (_HostingControl == null)
                return;

            int hr = DX.WindowlessCtrl.SetVideoPosition(null, DsRect.FromRectangle(_HostingControl.ClientRectangle));

            if (!_bGraphIsBuilt)
                return; // Do nothing before graph was built

            UpdateOutputVideoSize();

            // Call event handlers (External)
            if ( OutputVideoSizeChanged != null )
            {
                OutputVideoSizeChanged(sender, e);
            }
        
            

            //// Get the bitmap with alpha transparency
            //alphaBitmap = BitmapGenerator.GenerateAlphaBitmap(p.Width, p.Height);

            //// Create a surface from our alpha bitmap
            //surface = new Surface(device, alphaBitmap, Pool.SystemMemory);
            //// Get the unmanaged pointer
            //unmanagedSurface = surface.GetObjectByValue(DxMagicNumber);
        }

        /// <summary>
        /// Handler of SystemEvents.DisplaySettingsChanged.
        /// </summary>
        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            if (!_bGraphIsBuilt)
                return; // Do nothing before graph was built

            if (DX.WindowlessCtrl != null)
            {
                int hr = DX.WindowlessCtrl.DisplayModeChanged();
            }
        }

        #endregion

        #region Resolution lists

        /// <summary>
        /// Gets available resolutions (which are appropriate for us) for capture filter.
        /// </summary>
        /// <param name="captureFilter">Capture filter for asking for resolution list.</param>
        private static ResolutionList GetResolutionsAvailable(IBaseFilter captureFilter)
        {
            ResolutionList resolution_list = null;

            IPin pRaw = null;
            try
            {
                pRaw = DsFindPin.ByDirection(captureFilter, PinDirection.Output, 0);
                //pRaw = DsFindPin.ByCategory(captureFilter, PinCategory.Capture, 0);
                //pRaw = DsFindPin.ByCategory(filter, PinCategory.Preview, 0);

                resolution_list = GetResolutionsAvailable(pRaw);
            }
            catch
            {
                throw;
                //resolution_list = new ResolutionList();
                //resolution_list.Add(new Resolution(640, 480));
            }
            finally
            {
                SafeReleaseComObject(pRaw);
                pRaw = null;
            }

            return resolution_list;
        }

        /// <summary>
        /// Free media type if needed.
        /// </summary>
        /// <param name="media_type">Media type to free.</param>
        private static void FreeMediaType(ref AMMediaType media_type)
        {
            if (media_type == null)
                return;

            DsUtils.FreeAMMediaType(media_type);
            media_type = null;
        }

        /// <summary>
        /// Free SCC (it's not used but required for GetStreamCaps()).
        /// </summary>
        /// <param name="pSCC">SCC to free.</param>
        private static void FreeSCCMemory(ref IntPtr pSCC)
        {
            if (pSCC == IntPtr.Zero)
                return;

            Marshal.FreeCoTaskMem(pSCC);
            pSCC = IntPtr.Zero;
        }


        /// <summary>
        /// Gets available resolutions (which are appropriate for us) for capture pin (PinCategory.Capture).
        /// </summary>
        /// <param name="captureFilter">Capture pin (PinCategory.Capture) for asking for resolution list.</param>
        private static ResolutionList GetResolutionsAvailable(IPin pinOutput)
        {
            int hr = 0;

            ResolutionList ResolutionsAvailable = new ResolutionList();

            //ResolutionsAvailable.Clear();

            // Media type (shoudl be cleaned)
            AMMediaType media_type = null;

            //NOTE: pSCC is not used. All we need is media_type
            IntPtr pSCC = IntPtr.Zero;

            try
            {
                IAMStreamConfig videoStreamConfig = pinOutput as IAMStreamConfig;

                // -------------------------------------------------------------------------
                // We want the interface to expose all media types it supports and not only the last one set
                hr = videoStreamConfig.SetFormat(null);
                DsError.ThrowExceptionForHR(hr);

                int piCount = 0;
                int piSize = 0;

                hr = videoStreamConfig.GetNumberOfCapabilities(out piCount, out piSize);
                DsError.ThrowExceptionForHR(hr);

                for (int i = 0; i < piCount; i++)
                {
                    // ---------------------------------------------------
                    pSCC = Marshal.AllocCoTaskMem(piSize);
                    videoStreamConfig.GetStreamCaps(i, out media_type, pSCC);

                    // NOTE: we could use VideoStreamConfigCaps.InputSize or something like that to get resolution, but it's deprecated
                    //VideoStreamConfigCaps videoStreamConfigCaps = (VideoStreamConfigCaps)Marshal.PtrToStructure(pSCC, typeof(VideoStreamConfigCaps));
                    // ---------------------------------------------------

                    if (IsBitCountAppropriate(GetBitCountForMediaType(media_type)))
                    {
                        ResolutionsAvailable.AddIfNew(GetResolutionForMediaType(media_type));
                    }

                    FreeSCCMemory(ref pSCC);
                    FreeMediaType(ref media_type);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                // clean up
                FreeSCCMemory(ref pSCC);
                FreeMediaType(ref media_type);
            }

            return ResolutionsAvailable;
        }

        #endregion
        
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
        public PointF ConvertWinToNorm(PointF point)
        {
            // window_coordinate
            NormalizedRect video_rect = GetVideoRect();

            return new PointF(
                    (point.X - video_rect.left) / (video_rect.right - video_rect.left),
                    (point.Y - video_rect.top) / (video_rect.bottom - video_rect.top)
                );
        }

        /// <summary>
        /// Gets Video Rect in pixels (float values of pixels).
        /// </summary>
        /// <returns>return Video rect in normalized coordinates</returns>
        private NormalizedRect GetVideoRect()
        {
            int window_width = _HostingControl.ClientRectangle.Width;
            int window_height = _HostingControl.ClientRectangle.Height;

            return new NormalizedRect(
                    (window_width  - _OutputVideoSize.Width) / 2.0f,
                    (window_height - _OutputVideoSize.Height) / 2.0f,
                    window_width  - (window_width - _OutputVideoSize.Width) / 2.0f,
                    window_height - (window_height - _OutputVideoSize.Height) / 2.0f
                );
        }

        /// <summary>
        /// Sets camera output rect (zooms to selected rect).
        /// </summary>
        /// <param name="zoomRect">Rectangle for zooming in video coordinates.</param>
        public void ZoomToRect(Rectangle zoomRect)
        {
            if (zoomRect.Height == 0 || zoomRect.Width == 0)
                throw new Exception(@"ZoomRect has zero size.");

            IVMRMixerControl9 pMix = (IVMRMixerControl9)DX.VMRenderer;

            if (pMix == null)
                throw new Exception(@"The Mixer control is not created.");

            float x_scale = (float)_Resolution.Width / zoomRect.Width;
            float y_scale = (float)_Resolution.Height / zoomRect.Height;

            NormalizedRect rect = new NormalizedRect
                (
                -(float)zoomRect.Left * x_scale,
                -(float)zoomRect.Top * y_scale,
                -(float)zoomRect.Right * x_scale + _Resolution.Width * (x_scale + 1),
                -(float)zoomRect.Bottom * y_scale + _Resolution.Height * (y_scale + 1)
                );


            rect.left /= _Resolution.Width;
            rect.right /= _Resolution.Width;
            rect.top /= _Resolution.Height;
            rect.bottom /= _Resolution.Height;

            //NormalizedRect rect = new NormalizedRect(-1, -1, 2, 2);

            //NormalizedRect rect = new NormalizedRect(
            //    (float)zoomRect.Left / _Resolution.Width,
            //    (float)zoomRect.Top / _Resolution.Height,
            //    (float)zoomRect.Right / _Resolution.Width,
            //    (float)zoomRect.Bottom / _Resolution.Height);

            pMix.SetOutputRect(0, ref rect);
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Open log file for DirectShow.
        /// </summary>
        private void ApplyDirectShowLogFile()
        {
            if (DX.FilterGraph == null)
                return; // can't be set now. Will be set on BuildGraph()

            CloseDirectShowLogFile();

            if (string.IsNullOrEmpty(_DirectShowLogFilepath))
            {
                return;
            }

            _DirectShowLogHandle = NativeMethodes.CreateFile(_DirectShowLogFilepath,
                System.IO.FileAccess.Write,
                System.IO.FileShare.Read,
                IntPtr.Zero,
                System.IO.FileMode.OpenOrCreate,
                System.IO.FileAttributes.Normal,
                IntPtr.Zero);


            if (_DirectShowLogHandle.ToInt32() == -1 || _DirectShowLogHandle == IntPtr.Zero)
            {
                _DirectShowLogHandle = IntPtr.Zero;

                throw new Exception("Can't open log file for writing: " + _DirectShowLogFilepath);
            }

            // Append to file - move to end (WinApi's CreateFile doesn't support append FileMode)
            NativeMethodes.SetFilePointerEx(_DirectShowLogHandle, 0, IntPtr.Zero, NativeMethodes.FILE_END);


            DX.FilterGraph.SetLogFile(_DirectShowLogHandle);
        }

        /// <summary>
        /// Close log file for DirectShow.
        /// </summary>
        private void CloseDirectShowLogFile()
        {
            try
            {
                if (DX.FilterGraph != null)
                {
                    DX.FilterGraph.SetLogFile(IntPtr.Zero);
                }

                NativeMethodes.CloseHandle(_DirectShowLogHandle);
            }
            catch
            {
                throw;
            }
            finally
            {
                _DirectShowLogHandle = IntPtr.Zero;
            }

        }

        /// <summary>
        /// Releases COM object
        /// </summary>
        /// <param name="obj">COM object to release.</param>
        private static void SafeReleaseComObject(object obj)
        {
            if (obj != null)
            {
                Marshal.ReleaseComObject(obj);
            }
        }

        /// <summary>
        /// Updates output of video size from HostingControl
        /// </summary>
        private void UpdateOutputVideoSize()
        {
            int w = _HostingControl.ClientRectangle.Width;
            int h = _HostingControl.ClientRectangle.Height;

            int video_width  = _Resolution.Width;
            int video_height = _Resolution.Height;

            // Check size of video data, to save original ratio
            int window_width = _HostingControl.ClientRectangle.Width;
            int window_height = _HostingControl.ClientRectangle.Height;

            if (window_width == 0 || window_height == 0)
            {
                throw new Exception(@"Incorrect window size (zero).");
            }
            if (video_width == 0 || video_height == 0)
            {
                throw new Exception(@"Incorrect video size (zero).");
            }

            Size result;

            double ratio_video = (double)video_width / video_height;
            double ratio_window = (double)window_width / window_height;

            if (ratio_video <= ratio_window)
            {
                // calculate width of video
                int real_video_width = Convert.ToInt32(Math.Round(window_height * ratio_video));
                real_video_width = Math.Min(window_width, real_video_width); // Check it's not bigger than window's size

                // Put video frame in center of window
                result = new Size(real_video_width, window_height);
            }
            else
            {
                // calculate width of video
                int real_video_height = Convert.ToInt32(Math.Round(window_width / ratio_video));
                real_video_height = Math.Min(window_height, real_video_height); // Check it's not bigger than window's size

                // Put video frame in center of window
                result = new Size(window_width, real_video_height);
            }

            _OutputVideoSize = result;
        }

        #endregion

        #region Bitmap Mixer (image overlay)

        // Update mixer stuff if needed
        private void UpdateMixerStuff()
        {
#if USE_D3D
            //Direct3D should be initialized if needed
            if (!_UseGDI)
            {
                InitializeDirect3DIfNeeded();
            }
#endif

            // Update mixer stuff
            SetMixerSettings();
        }

        private void SetMixerSettings()
        {
            int hr = 0;
            VMR9AlphaBitmap alphaBmp;

            if (!_bMixerEnabled) // Did the user disable the bitmap ?
            {
                if (!_bMixerImageWasUsed)
                    return; // Mixer was not used, can't disable it

                // Get current Alpha Bitmap Parameters
                hr = DX.MixerBitmap.GetAlphaBitmapParameters(out alphaBmp);
                DsError.ThrowExceptionForHR(hr);

                // Disable them
                //alphaBmp.dwFlags = VMRBitmap.Disable;
                alphaBmp.dwFlags = VMR9AlphaBitmapFlags.Disable;

                // Update the Alpha Bitmap Parameters
                hr = DX.MixerBitmap.UpdateAlphaBitmapParameters(ref alphaBmp);
                DsError.ThrowExceptionForHR(hr);

                return;
            }

            if (_OverlayBitmap == null)
            {
                // ignore it. Bitmap can be null
                //throw new Exception(@"Overlay bitmap should be already initialized.");
                return;
            }

#if USE_D3D
            if (_UseGDI)
            {
#endif
            // Old school GDI stuff...
            Graphics g = Graphics.FromImage(_OverlayBitmap);

            IntPtr hdc = g.GetHdc();
            IntPtr memDC = NativeMethodes.CreateCompatibleDC(hdc);
            IntPtr hBitmap = _OverlayBitmap.GetHbitmap();
            NativeMethodes.SelectObject(memDC, hBitmap);

            // Set Alpha Bitmap Parameters for using a GDI DC
            alphaBmp = new VMR9AlphaBitmap();
            alphaBmp.dwFlags = VMR9AlphaBitmapFlags.hDC | VMR9AlphaBitmapFlags.SrcColorKey | VMR9AlphaBitmapFlags.FilterMode;
            alphaBmp.hdc = memDC;
            alphaBmp.rSrc = new DsRect(0, 0, _OverlayBitmap.Size.Width, _OverlayBitmap.Size.Height);
            alphaBmp.rDest = new NormalizedRect(0.0f, 0.0f, 1.0f, 1.0f);
            alphaBmp.clrSrcKey = ColorTranslator.ToWin32(_GDIColorKey);
            alphaBmp.dwFilterMode = VMRMixerPrefs.PointFiltering;
            alphaBmp.fAlpha = _GDIAlphaValue;

            // Set Alpha Bitmap Parameters
            hr = DX.MixerBitmap.SetAlphaBitmap(ref alphaBmp);
            DsError.ThrowExceptionForHR(hr);

            // Release GDI handles
            NativeMethodes.DeleteObject(hBitmap);
            NativeMethodes.DeleteDC(memDC);
            g.ReleaseHdc(hdc);
            g.Dispose();
#if USE_D3D
            }
            else // Using a Direct3D surface
            {
                _pDirect3DMixing.StoreBitmapToSurface(_OverlayBitmap);

                // Set Alpha Bitmap Parameters for using a Direct3D surface
                alphaBmp = new VMR9AlphaBitmap();
                alphaBmp.dwFlags = VMR9AlphaBitmapFlags.EntireDDS;
                alphaBmp.pDDS = _pDirect3DMixing.unmanagedSurface;
                //alphaBmp.rDest = CalculateNormalizedVideoFrameSize();
                alphaBmp.rDest = new NormalizedRect(0.0f, 0.0f, 1.0f, 1.0f);
                alphaBmp.fAlpha = 1.0f;
                // Note : Alpha values from the bitmap are cumulative with the fAlpha parameter.
                // Example : texel alpha = 128 (50%) & fAlpha = 0.5f (50%) = effective alpha : 64 (25%)

                // Set Alpha Bitmap Parameters
                hr = DX.MixerBitmap.SetAlphaBitmap(ref alphaBmp);
                DsError.ThrowExceptionForHR(hr);
            }
#endif
            _bMixerImageWasUsed = true;

        }

        #endregion

        #region For Direct3D mixer mode only

#if USE_D3D
        private void InitializeDirect3DIfNeeded()
        {
            if (_pDirect3DMixing == null)
            {
                _pDirect3DMixing = new Direct3DMixing();
            }

            _pDirect3DMixing.InitializeIfNeeded(_HostingControl);

        }
#endif
        #endregion

        #endregion // Private

        // ====================================================================
    }
}
