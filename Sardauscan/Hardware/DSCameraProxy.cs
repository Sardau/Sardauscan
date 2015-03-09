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
/****************************************************************************
While the underlying libraries are covered by LGPL, this sample is released 
as public domain.  It is distributed in the hope that it will be useful, but 
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE.  
*****************************************************************************/

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;

using DirectShowLib;
using System.Collections.Generic;
using Sardauscan.Core.Interface;
using Sardauscan.Core;
using Sardauscan.Hardware.Gui;


namespace Sardauscan.Hardware
{
    public class DSCameraProxy : ISampleGrabberCB, IDisposable, ICameraProxy
    {
        public class ControlPropertyInfo
        {
            public int Min;
            public int Value;
            public CameraControlFlags  ValueFlags;
            public int Max;
            public CameraControlFlags Flags;
            public int Delta;
            public int Default;
            public bool IsInRange(int val)
            {
                return val >= Min && val <= Max;
            }
        }
        public class Resolution : IComparable, IEquatable<Resolution>
        {
            public Resolution(int width, int height, short bitcount)
            {
                Width = width;
                Height = height;
                BitCount = bitcount;
            }

            public int Width { get; private set; }
            public int Height { get; private set; }
            public short BitCount { get; private set; }

            public override string ToString()
            {
                return string.Format("{0}x{1} ({2}bits)",Width,Height,BitCount);
            }

            public int CompareTo(object obj)
            {
                int c = Width.CompareTo(((Resolution)obj).Width);
                if (c != 0)
                    return c;
                c = Height.CompareTo(((Resolution)obj).Height);
                if (c != 0)
                    return c;
                c = BitCount.CompareTo(((Resolution)obj).BitCount);
                return c;
            }

            public bool Equals(Resolution other)
            {
                return ToString() == other.ToString();
            }
        }
        #region Member variables

        /// <summary> graph builder interface. </summary>
        private IFilterGraph2 m_FilterGraph = null;

        public IAMVideoControl VidControl { get { return m_VidControl; } }
        // Used to snap picture on Still pin
        private IAMVideoControl m_VidControl = null;
        private IPin m_pinStill = null;

        /// <summary> so we can wait for the async job to finish </summary>
        private ManualResetEvent m_PictureReady = null;

        private bool m_WantOne = false;

        /// <summary> Dimensions of the image, calculated once in constructor for perf. </summary>
        private int m_videoWidth;
        private int m_videoHeight;
        private int m_stride;

        /// <summary> buffer for bitmap data.  Always release by caller</summary>
        private IntPtr m_ipBuffer = IntPtr.Zero;
       #endregion

        #region APIs
        [DllImport("Kernel32.dll", EntryPoint="RtlMoveMemory")]
        private static extern void CopyMemory(IntPtr Destination, IntPtr Source, [MarshalAs(UnmanagedType.U4)] int Length);
        #endregion

				public DSCameraProxy()
				{
					// for load from HardwareID (it use a instance to call LoadFromHardwareId
				}

				DSCameraInfo _Caminfo;
				Resolution _Resolution;
        // Zero based device index and device params and output window
				public DSCameraProxy(DSCameraInfo caminfo, Resolution resolution, Control hControl)
        {
					_Caminfo=caminfo;
					_Resolution = resolution;
					int iDeviceNum = caminfo.Index;
					int iWidth = resolution.Width;
					int iHeight = resolution.Height;
					short iBPP = resolution.BitCount;
					
            DsDevice [] capDevices;

            // Get the collection of video devices
            capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            if (iDeviceNum + 1 > capDevices.Length)
            {
                throw new Exception("No video capture devices found at that index!");
            }

            try
            {
                // Set up the capture graph
                SetupGraph( capDevices[iDeviceNum], iWidth, iHeight, iBPP, hControl);

                // tell the callback to ignore new images
                m_PictureReady = new ManualResetEvent(false);


                int exposure = Settings.Get<Settings>().Read(Settings.CAMERA, CameraControlProperty.Exposure.ToString(), int.MaxValue);
                this.SetControlProperty(CameraControlProperty.Exposure, exposure);
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        /// <summary> release everything. </summary>
				public void Dispose()
        {
            CloseInterfaces();
            if (m_PictureReady != null)
            {
                m_PictureReady.Close();
            }
            if (m_ip != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(m_ip);
                m_ip = IntPtr.Zero;
            }

        }
        // Destructor
			/// <summary>
			/// Destructor
			/// </summary>
        ~DSCameraProxy()
        {
            Dispose();
        }

        /// <summary>
        /// Get the image from the Still pin.  The returned image can turned into a bitmap with
        /// Bitmap b = new Bitmap(cam.Width, cam.Height, cam.Stride, PixelFormat.Format24bppRgb, m_ip);
        /// If the image is upside down, you can fix it with
        /// b.RotateFlip(RotateFlipType.RotateNoneFlipY);
        /// </summary>
        /// <returns>Returned pointer to be freed by caller with Marshal.FreeCoTaskMem</returns>
        public IntPtr Click()
        {
            lock (this)
            {
                int hr;

                // get ready to wait for new image
                m_PictureReady.Reset();
                m_ipBuffer = Marshal.AllocCoTaskMem(Math.Abs(m_stride) * m_videoHeight);

                try
                {
                    m_WantOne = true;

                    // If we are using a still pin, ask for a picture
                    if (m_VidControl != null)
                    {
                        // Tell the camera to send an image
                        hr = m_VidControl.SetMode(m_pinStill, VideoControlFlags.Trigger);
                        DsError.ThrowExceptionForHR(hr);
                    }

                    // Start waiting
                    if (!m_PictureReady.WaitOne(1000, false))
                    {
                        throw new Exception("Timeout waiting to get picture");
                    }
                }
                catch
                {
                    Marshal.FreeCoTaskMem(m_ipBuffer);
                    m_ipBuffer = IntPtr.Zero;
                    throw;
                }

                // Got one
                return m_ipBuffer;
            }
        }

        public int Width
        {
            get
            {
                return m_videoWidth;
            }
        }
        public int Height
        {
            get
            {
                return m_videoHeight;
            }
        }
        public int Stride
        {
            get
            {
                return m_stride;
            }
        }


        public ControlPropertyInfo GetControlPropertyInfo(CameraControlProperty prop, bool getCurrentValue)
        {
            IAMCameraControl icc = VidControl as IAMCameraControl;
            if (icc != null)
            {
                ControlPropertyInfo ret = new ControlPropertyInfo();
                if (0 == icc.GetRange(prop, out ret.Min, out ret.Max, out ret.Delta, out ret.Default, out ret.Flags))
                {
                    if(!getCurrentValue)
                        return ret;
                    if(0==icc.Get(prop,out ret.Value,out ret.ValueFlags))
                        return ret;
                }
            }
            return null;
        }
        public void SetControlPropertyDefault(CameraControlProperty prop)
        {
            SetControlProperty(prop,int.MaxValue);
        }
        public void SetControlProperty(CameraControlProperty prop,int value)
        {
            IAMCameraControl icc = VidControl as IAMCameraControl;
            if (icc != null)
            {
                ControlPropertyInfo info = GetControlPropertyInfo(prop, false);
                if (info != null)
                {
                    if (int.MaxValue == value)
                        icc.Set(prop, 0, CameraControlFlags.Auto);
                    else
                    {
                        if (info.IsInRange(value))
                            icc.Set(prop, value, CameraControlFlags.Manual);
                    }
                }
                Settings.Get<Settings>().Write(Settings.CAMERA, prop.ToString(), value);

            }
        }



        /// <summary> build the capture graph for grabber. </summary>
        private void SetupGraph(DsDevice dev, int iWidth, int iHeight, short iBPP, Control hControl)
        {
            int hr;

            ISampleGrabber sampGrabber = null;
            IBaseFilter capFilter = null;
            IPin pCaptureOut = null;
            IPin pSampleIn = null;
            IPin pRenderIn = null;

            // Get the graphbuilder object
            m_FilterGraph = new FilterGraph() as IFilterGraph2;

            try
            {
                // add the video input device
                hr = m_FilterGraph.AddSourceFilterForMoniker(dev.Mon, null, dev.Name, out capFilter);
                DsError.ThrowExceptionForHR( hr );

                // Find the still pin
                m_pinStill = DsFindPin.ByCategory(capFilter, PinCategory.Still, 0);

                // Didn't find one.  Is there a preview pin?
                if (m_pinStill == null)
                {
                    m_pinStill = DsFindPin.ByCategory(capFilter, PinCategory.Preview, 0);
                }

                // Still haven't found one.  Need to put a splitter in so we have
                // one stream to capture the bitmap from, and one to display.  Ok, we
                // don't *have* to do it that way, but we are going to anyway.
                if (m_pinStill == null)
                {
                    IPin pRaw = null;
                    IPin pSmart = null;

                    // There is no still pin
                    m_VidControl = null;

                    // Add a splitter
                    IBaseFilter iSmartTee = (IBaseFilter)new SmartTee();

                    try
                    {
                        hr = m_FilterGraph.AddFilter(iSmartTee, "SmartTee");
                        DsError.ThrowExceptionForHR( hr );

                        // Find the find the capture pin from the video device and the
                        // input pin for the splitter, and connnect them
                        pRaw = DsFindPin.ByCategory(capFilter, PinCategory.Capture, 0);
                        pSmart = DsFindPin.ByDirection(iSmartTee, PinDirection.Input, 0);

                        hr = m_FilterGraph.Connect(pRaw, pSmart);
                        DsError.ThrowExceptionForHR( hr );

                        // Now set the capture and still pins (from the splitter)
                        m_pinStill = DsFindPin.ByName(iSmartTee, "Preview");
                        pCaptureOut = DsFindPin.ByName(iSmartTee, "Capture");

                        // If any of the default config items are set, perform the config
                        // on the actual video device (rather than the splitter)
                        if (iHeight + iWidth + iBPP > 0)
                        {
                            SetConfigParms(pRaw, iWidth, iHeight, iBPP);
                        }
                    }
                    finally
                    {
                        if (pRaw != null)
                        {
                            Marshal.ReleaseComObject(pRaw);
                        }
                        if (pRaw != pSmart)
                        {
                            Marshal.ReleaseComObject(pSmart);
                        }
                        if (pRaw != iSmartTee)
                        {
                            Marshal.ReleaseComObject(iSmartTee);
                        }
                    }
                }
                else
                {
                    // Get a control pointer (used in Click())
                    m_VidControl = capFilter as IAMVideoControl;

                    pCaptureOut = DsFindPin.ByCategory(capFilter, PinCategory.Capture, 0);

                    // If any of the default config items are set
                    if (iHeight + iWidth + iBPP > 0)
                    {
                        SetConfigParms(m_pinStill, iWidth, iHeight, iBPP);
                    }
                }

                // Get the SampleGrabber interface
                sampGrabber = new SampleGrabber() as ISampleGrabber;

                // Configure the sample grabber
                IBaseFilter baseGrabFlt = sampGrabber as IBaseFilter;
                ConfigureSampleGrabber(sampGrabber);
                pSampleIn = DsFindPin.ByDirection(baseGrabFlt, PinDirection.Input, 0);

                // Get the default video renderer
                IBaseFilter pRenderer = new VideoRendererDefault() as IBaseFilter;
                hr = m_FilterGraph.AddFilter(pRenderer, "Renderer");
                DsError.ThrowExceptionForHR( hr );

                pRenderIn = DsFindPin.ByDirection(pRenderer, PinDirection.Input, 0);

                // Add the sample grabber to the graph
                hr = m_FilterGraph.AddFilter( baseGrabFlt, "Ds.NET Grabber" );
                DsError.ThrowExceptionForHR( hr );

                if (m_VidControl == null)
                {
                    // Connect the Still pin to the sample grabber
                    hr = m_FilterGraph.Connect(m_pinStill, pSampleIn);
                    DsError.ThrowExceptionForHR( hr );

                    // Connect the capture pin to the renderer
                    hr = m_FilterGraph.Connect(pCaptureOut, pRenderIn);
                    DsError.ThrowExceptionForHR( hr );
                }
                else
                {
                    // Connect the capture pin to the renderer
                    hr = m_FilterGraph.Connect(pCaptureOut, pRenderIn);
                    DsError.ThrowExceptionForHR( hr );

                    // Connect the Still pin to the sample grabber
                    hr = m_FilterGraph.Connect(m_pinStill, pSampleIn);
                    DsError.ThrowExceptionForHR( hr );
                }

                // Learn the video properties
                SaveSizeInfo(sampGrabber);
                ConfigVideoWindow(hControl);

                // Start the graph
                IMediaControl mediaCtrl = m_FilterGraph as IMediaControl;
                hr = mediaCtrl.Run();
                DsError.ThrowExceptionForHR( hr );
            }
            finally
            {
                if (sampGrabber != null)
                {
                    Marshal.ReleaseComObject(sampGrabber);
                    sampGrabber = null;
                }
                if (pCaptureOut != null)
                {
                    Marshal.ReleaseComObject(pCaptureOut);
                    pCaptureOut = null;
                }
                if (pRenderIn != null)
                {
                    Marshal.ReleaseComObject(pRenderIn);
                    pRenderIn = null;
                }
                if (pSampleIn != null)
                {
                    Marshal.ReleaseComObject(pSampleIn);
                    pSampleIn = null;
                }
            }
        }

        private void SaveSizeInfo(ISampleGrabber sampGrabber)
        {
            int hr;

            // Get the media type from the SampleGrabber
            AMMediaType media = new AMMediaType();

            hr = sampGrabber.GetConnectedMediaType( media );
            DsError.ThrowExceptionForHR( hr );

            if( (media.formatType != FormatType.VideoInfo) || (media.formatPtr == IntPtr.Zero) )
            {
                throw new NotSupportedException( "Unknown Grabber Media Format" );
            }

            // Grab the size info
            VideoInfoHeader videoInfoHeader = (VideoInfoHeader) Marshal.PtrToStructure( media.formatPtr, typeof(VideoInfoHeader) );
            m_videoWidth = videoInfoHeader.BmiHeader.Width;
            m_videoHeight = videoInfoHeader.BmiHeader.Height;
            m_stride = m_videoWidth * (videoInfoHeader.BmiHeader.BitCount / 8);

            DsUtils.FreeAMMediaType(media);
            media = null;
        }

        // Set the video window within the control specified by hControl
        private void ConfigVideoWindow(Control hControl)
        {
            int hr;

            IVideoWindow ivw = m_FilterGraph as IVideoWindow;

            // Set the parent
						hr = ivw.put_Owner(hControl==null?(IntPtr)0:hControl.Handle);
            DsError.ThrowExceptionForHR( hr );
            // Turn off captions, etc
            hr = ivw.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipChildren | WindowStyle.ClipSiblings);
            DsError.ThrowExceptionForHR( hr );


					// Yes, make it visible
						if (hControl == null)
						{
							hr = ivw.put_Visible(OABool.False);
							ivw.put_AutoShow(OABool.False);
						}
						else
							hr = ivw.put_Visible(OABool.True);
            DsError.ThrowExceptionForHR( hr );

						if (hControl != null)
						{
							// Move to upper left corner
							Rectangle rc = hControl.ClientRectangle;
							hr = ivw.SetWindowPosition(0, 0, rc.Right, rc.Bottom);
							DsError.ThrowExceptionForHR(hr);
						}
						else
						{
							// move outside the desktop
							Screen[] screens = Screen.AllScreens;
							int h = 0;
							for (int i = 0; i < screens.Length; i++)
							{
								Screen screen = screens[i];
								h = Math.Max(h, screen.Bounds.Bottom+10);
							}
							Rectangle rc = new Rectangle(0,h,1,1);
							hr = ivw.SetWindowPosition(0, 0, rc.Right, rc.Bottom);
							DsError.ThrowExceptionForHR(hr);
						}
        }

        private void ConfigureSampleGrabber(ISampleGrabber sampGrabber)
        {
            int hr;
            AMMediaType media = new AMMediaType();

            // Set the media type to Video/RBG24
            media.majorType = MediaType.Video;
            media.subType = MediaSubType.RGB24;
            media.formatType = FormatType.VideoInfo;
            hr = sampGrabber.SetMediaType( media );
            DsError.ThrowExceptionForHR( hr );

            DsUtils.FreeAMMediaType(media);
            media = null;

            // Configure the samplegrabber
            hr = sampGrabber.SetCallback( this, 1 );
            DsError.ThrowExceptionForHR( hr );
        }

        // Set the Framerate, and video size
        private void SetConfigParms(IPin pStill, int iWidth, int iHeight, short iBPP)
        {
            int hr;
            AMMediaType media;
            VideoInfoHeader v;

            IAMStreamConfig videoStreamConfig = pStill as IAMStreamConfig;

            // Get the existing format block
            hr = videoStreamConfig.GetFormat(out media);
            DsError.ThrowExceptionForHR(hr);

            try
            {
                // copy out the videoinfoheader
                v = new VideoInfoHeader();
                Marshal.PtrToStructure( media.formatPtr, v );

                // if overriding the width, set the width
                if (iWidth > 0)
                {
                    v.BmiHeader.Width = iWidth;
                }

                // if overriding the Height, set the Height
                if (iHeight > 0)
                {
                    v.BmiHeader.Height = iHeight;
                }

                // if overriding the bits per pixel
                if (iBPP > 0)
                {
                    v.BmiHeader.BitCount = iBPP;
                }

                // Copy the media structure back
                Marshal.StructureToPtr( v, media.formatPtr, false );

                // Set the new format
                hr = videoStreamConfig.SetFormat( media );
                DsError.ThrowExceptionForHR( hr );
            }
            finally
            {
                DsUtils.FreeAMMediaType(media);
                media = null;
            }
        }

        /// <summary> Shut down capture </summary>
        private void CloseInterfaces()
        {
            int hr;

            try
            {
                if( m_FilterGraph != null )
                {
                    IMediaControl mediaCtrl = m_FilterGraph as IMediaControl;

                    // Stop the graph
                    hr = mediaCtrl.Stop();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            if (m_FilterGraph != null)
            {
                Marshal.ReleaseComObject(m_FilterGraph);
                m_FilterGraph = null;
            }

            if (m_VidControl != null)
            {
                Marshal.ReleaseComObject(m_VidControl);
                m_VidControl = null;
            }

            if (m_pinStill != null)
            {
                Marshal.ReleaseComObject(m_pinStill);
                m_pinStill = null;
            }
        }

        /// <summary> sample callback, NOT USED. </summary>
        int ISampleGrabberCB.SampleCB( double SampleTime, IMediaSample pSample )
        {
            Marshal.ReleaseComObject(pSample);
            return 0;
        }

        /// <summary> buffer callback, COULD BE FROM FOREIGN THREAD. </summary>
        int ISampleGrabberCB.BufferCB( double SampleTime, IntPtr pBuffer, int BufferLen )
        {
            // Note that we depend on only being called once per call to Click.  Otherwise
            // a second call can overwrite the previous image.
            Debug.Assert(BufferLen == Math.Abs(m_stride) * m_videoHeight, "Incorrect buffer length");

            if (m_WantOne)
            {
                m_WantOne = false;
                Debug.Assert(m_ipBuffer != IntPtr.Zero, "Unitialized buffer");

                // Save the buffer
                CopyMemory(m_ipBuffer, pBuffer, BufferLen);

                // Picture is ready.
                m_PictureReady.Set();
            }

            return 0;
        }


        public static List<Resolution> GetAllAvailableResolution(DsDevice vidDev)
        {
            try
            {
                int hr;
                int max = 0;
                int bitCount = 0;

                IBaseFilter sourceFilter = null;

                var m_FilterGraph2 = new FilterGraph() as IFilterGraph2;

                hr = m_FilterGraph2.AddSourceFilterForMoniker(vidDev.Mon, null, vidDev.Name, out sourceFilter);

                var pRaw2 = DsFindPin.ByCategory(sourceFilter, PinCategory.Capture, 0);

                var AvailableResolutions = new List<Resolution>();

                VideoInfoHeader v = new VideoInfoHeader();
                IEnumMediaTypes mediaTypeEnum;
                hr = pRaw2.EnumMediaTypes(out mediaTypeEnum);

                AMMediaType[] mediaTypes = new AMMediaType[1];
                IntPtr fetched = IntPtr.Zero;
                hr = mediaTypeEnum.Next(1, mediaTypes, fetched);

                while (fetched != null && mediaTypes[0] != null)
                {
                    Marshal.PtrToStructure(mediaTypes[0].formatPtr, v);
                    if (v.BmiHeader.Size != 0 && v.BmiHeader.BitCount != 0)
                    {
                        if (v.BmiHeader.BitCount > bitCount)
                        {
                            //AvailableResolutions.Clear();
                            max = 0;
                            bitCount = v.BmiHeader.BitCount;


                        }
                        AvailableResolutions.Add(new Resolution(v.BmiHeader.Width, v.BmiHeader.Height, v.BmiHeader.BitCount));
                        if (v.BmiHeader.Width > max || v.BmiHeader.Height > max)
                            max = (Math.Max(v.BmiHeader.Width, v.BmiHeader.Height));
                    }
                    hr = mediaTypeEnum.Next(1, mediaTypes, fetched);
                }
                return AvailableResolutions;
            }

            catch (Exception ex)
            {
                throw ex;
                //return new List<Resolution>();
            }
        }

        private IntPtr m_ip;
        public Bitmap AcquireImage()
        {
            if (m_ip != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(m_ip);
                m_ip = IntPtr.Zero;
            }
            // capture image
            m_ip = Click();
            Bitmap b = new Bitmap(Width, Height, Stride, PixelFormat.Format24bppRgb, m_ip);

            // If the image is upsidedown
            b.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return  b;
        }

        public int ImageHeight
        {
            get { return this.Height; }
        }

        public int ImageWidth
        {
            get { return this.Width; }
        }

        public float SensorWidth
        {
            get 
            {
							return 0.4111f;
            }
        }

       public float SensorHeight
        {
            get 
            {
							return 0.37f;
            }
        }
        public float FocalLength
        {
            get
            {
							return 0.5f;
            }
        }
				#region Reload
				DSCameraInfo GetCamInfo(string uniqueId)
				{
					List<DSCameraInfo> camInfos = DSCameraInfo.GetAvailableCamera();
					for (int i = 0; i < camInfos.Count; i++)
					{
						if (camInfos[i].UniqueId == uniqueId)
							return camInfos[i];
					}
					return null;
				}
			  Resolution GetResolution(DSCameraInfo camInfo,string resolutionAsString)
				{
					List<Resolution> list = camInfo.GetAvailableResolution();
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].ToString() == resolutionAsString)
							return list[i];
					}
					return null;
				}
				#endregion
				#region IHardwareProxy

				public string HardwareId
				{
					get 
					{ 
						return _Caminfo.UniqueId+"|"+_Resolution.ToString(); 
					}
				}
				public IHardwareProxy LoadFromHardwareId(string hardwareId)
				{
					try
					{
						string[] part = hardwareId.Split("|".ToCharArray());
						DSCameraInfo camInfo = GetCamInfo(part[0]);
						Resolution res = GetResolution(camInfo, part[1]);
						return new DSCameraProxy(camInfo, res, null);
							
					}
					catch { return null; }
				}
			  public System.Windows.Forms.Control GetViewer() 
				{
					DSCameraProxyControl viewer = new DSCameraProxyControl();
					viewer.Proxy = (ICameraProxy)Settings.Get<DSCameraProxy>();
					return viewer;
				}
				#endregion
		}
}
