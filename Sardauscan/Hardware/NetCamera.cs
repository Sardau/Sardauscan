#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#region Based on Camera_NET 

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
#endregion


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
	using Camera_NET;
	#endregion

namespace Sardauscan.Hardware
{

	/// <summary>
	/// The Camera class is an main class that is a wrapper for video device.
	/// </summary>
	/// 
	/// <author> free5lot (free5lot@yandex.ru) </author>
	/// <version> 2013.12.16 </version>
	public class NetCamera : IDisposable
	{
		// ====================================================================

		#region Private members


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

		// Header's size of DIB image returned by IVMRWindowlessControl9::GetCurrentImage method (BITMAPINFOHEADER)
		private static readonly int DIB_Image_HeaderSize = Marshal.SizeOf(typeof(BitmapInfoHeader)); // == 40;



		#endregion

		// ====================================================================

		#region Internal stuff

		/// <summary>
		/// Private field. DirectXInterfaces instance.
		/// </summary>
		internal DirectXInterfaces DX = new DirectXInterfaces();


		/// <summary>
		/// Private field. Was the graph built or not.
		/// </summary>
		internal bool _bGraphIsBuilt = false;


		/// <summary>
		/// Private field. SampleGrabber helper (wrapper)
		/// </summary>
		internal SampleGrabberHelper _pSampleGrabberHelper = null;


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

			if (filter_or_pin is IBaseFilter)
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
				if (filter_or_pin is IPin)
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
		public NetCamera()
		{
		}

		/// <summary>
		/// Initializes camera and connects it to HostingControl and Moniker.
		/// </summary>
		/// <param name="hControl">Control that is used for hosting camera's output.</param>
		/// <param name="moniker">Moniker (device identification) of camera.</param>
		/// <seealso cref="Moniker"/>
		public void Initialize(IMoniker moniker)
		{

			if (moniker == null)
				throw new Exception(@"Camera's moniker should be set.");

			_Moniker = moniker;
		}

		/// <summary>
		/// Destructor (disposer) for <see cref="Camera"/> class.
		/// </summary>
		~NetCamera()
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

			if (_pSampleGrabberHelper != null)
			{
				_pSampleGrabberHelper.Dispose();
				_pSampleGrabberHelper = null;
			}

			DX.CloseInterfaces();


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

				// -------------------------------------------------------
				GraphBuilding_AddFilters();

				GraphBuilding_ConnectPins();

				PostActions_SampleGrabber();


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

		#region Spanshot (screenshots) frame


		/// <summary>
		/// Make snapshot of source image. Much faster than SnapshotOutputImage.
		/// </summary>
		/// <returns>Snapshot as a Bitmap</returns>
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
			sub_type_ok = (
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

					if (media_type_most_appropriate != null)
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
			IPin pinSampleGrabberInput = null;


			int hr = 0;

			try
			{
				// Collect pins
				pinSourceCapture = DsFindPin.ByDirection(DX.CaptureFilter, PinDirection.Output, 0);
				pinSampleGrabberInput = DsFindPin.ByDirection(DX.SampleGrabberFilter, PinDirection.Input, 0);
				hr = DX.FilterGraph.Connect(pinSourceCapture, pinSampleGrabberInput);
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

				SafeReleaseComObject(pinSampleGrabberInput);
				pinSampleGrabberInput = null;
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

		/*
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
			*/
		/*
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
		 * */


		#endregion

		#region Private Helpers

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
		#endregion

		#endregion // Private

		// ====================================================================
	}
}
