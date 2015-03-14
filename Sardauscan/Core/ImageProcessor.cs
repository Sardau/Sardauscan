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
#region Based on Frelss Copyright Uriah Liggett <freelss@murobo.com >
/*
 ****************************************************************************
 *  Copyright (c) 2014 Uriah Liggett <freelss@murobo.com >                   *
 *	This file is part of FreeLSS.                                           *
 *                                                                          *
 *  FreeLSS is free software: you can redistribute it and/or modify         *
 *  it under the terms of the GNU General Public License as published by    *
 *  the Free Software Foundation, either version 3 of the License, or       *
 *  (at your option) any later version.                                     *
 *                                                                          *
 *  FreeLSS is distributed in the hope that it will be useful,              *
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of          *
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the           *
 *  GNU General Public License for more details.                            *
 *                                                                          *
 *   You should have received a copy of the GNU General Public License      *
 *   along with FreeLSS.  If not, see <http://www.gnu.org/licenses/>.       *
 ****************************************************************************
*/
#endregion
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using Sardauscan.Core.Interface;
using Sardauscan.Core;
using Sardauscan.Core.Geometry;

namespace Sardauscan.Core
{
	public class ImageProcessor : IDisposable
	{
		public enum eLaserDetectionMode
		{
			MaxCenter,
			MassCenter,
			MassHarmonicCenter,
			QuadricCenter

		}
		#region strucs
		/** The starting and ending column for a detected laser line in the image */
		private struct LaserRange
		{
			public int startCol;
			public int endCol;
			public int centerCol;
		};

		#endregion

		public ImageProcessor(double magnitudeThreshold, int minLaserWidth, int maxLaserWidth)
		{
			m_laserMagnitudeThreshold = magnitudeThreshold;
			m_maxLaserWidth = maxLaserWidth;
			m_minLaserWidth = minLaserWidth;
			Settings set = Settings.Get<Settings>();
			eLaserDetectionMode detectionMode = (eLaserDetectionMode)set.Read(Settings.SCANNER, Settings.CENTERDETECTIONMODE, eLaserDetectionMode.MassCenter);

			this.LaserDetectionMode = detectionMode;
		}
		/// <summary>
		/// Dispose object
		/// </summary>
		public void Dispose()
		{
			//m_laserRanges = null;
		}


		/**
	 * Detects the laser in x, y pixel coordinates.
	 * @param debuggingImage - If non-NULL, it will be populated with the processed image that was used to detect the laser locations.
	 *     This can be helpful when debugging laser detection issues.
	 * @param laserLocations - Output variable to store the laser locations.
	 * @param maxNumLocations - The maximum number of locations to store in @p laserLocations.
	 * @param thresholdFactor - Scales the laser threshold by this amount.  default = 1.0
	 * @return Returns the number of locations written to @p laserLocations.
	 */
		public List<PixelLocation> Process(Bitmap before, Bitmap after, Bitmap debuggingImage,
										ref int firstRowLaserCol, ref int numSuspectedBadLaserLocations, ref int numImageProcessingRetries)
		{
			int numBad = 0;
			List<PixelLocation> ret = SubProcess(before, after, debuggingImage, m_laserMagnitudeThreshold, ref firstRowLaserCol, ref numBad);

			numSuspectedBadLaserLocations += numBad;

			return ret;
		}


		private int DetectBestLaserRange(ImageProcessor.LaserRange[] ranges, int numRanges, int prevLaserCol)
		{
			int bestRange = 0;
			int distanceOfBest = Math.Abs(ranges[0].centerCol - prevLaserCol);

			// TODO: instead of just looking at the last laser position, this should probably be a sliding window
			// Select based off of minimum distance to last laser position
			for (int i = 1; i < numRanges; i++)
			{
				int dist = Math.Abs(ranges[i].centerCol - prevLaserCol);
				if (dist < distanceOfBest)
				{
					bestRange = i;
					distanceOfBest = dist;
				}
			}

			return bestRange;
		}
		private double DetectLaserRangeCenter(ImageProcessor.LaserRange range, List<Color> ar, List<Color> br)
		{
			switch (LaserDetectionMode)
			{
				case eLaserDetectionMode.MassCenter:
					return GetMassCenter(range, ar, br);
				case eLaserDetectionMode.MassHarmonicCenter:
					return GetMassHarmonicCenter(range, ar, br);
				case eLaserDetectionMode.QuadricCenter:
					return GetQuadricCenter(range, ar, br);
			}
			return GetMaxCenter(range, ar, br);
		}
		//http://fr.wikipedia.org/wiki/Moyenne_quadratique
		private double GetQuadricCenter(ImageProcessor.LaserRange range, List<Color> ar, List<Color> br)
		{
			int startCol = range.startCol;
			int endCol = range.endCol;

			double count = 0;
			double sum = 0;
			for (int col = startCol; col < endCol; col++)
			{
					count++;
					sum+=col*col;
			}
			return Math.Sqrt(sum / count);

		}

		//http://fr.wikipedia.org/wiki/Moyenne_harmonique_pond%C3%A9r%C3%A9e
		private double GetMassHarmonicCenter(ImageProcessor.LaserRange range, List<Color> ar, List<Color> br)
		{
			int startCol = range.startCol;
			int endCol = range.endCol;

			double numerator = 0;
			double denominator = 0;
			for (int col = startCol; col < endCol; col++)
			{
				int r = br[col].R - ar[col].R;
				int g = br[col].G - ar[col].G;
				int b = br[col].B - ar[col].B;
				double mag = r * r + g * g + b * b;
				if (mag != 0)
				{
					numerator += mag;
					denominator += mag / col;
				}
			}
			return numerator / denominator;

		}
		private double GetMassCenter(ImageProcessor.LaserRange range, List<Color> ar, List<Color> br)
		{
			int startCol = range.startCol;
			double centerCol = startCol;
			int endCol = range.endCol;

			double totalSum = 0.0;
			double weightedSum = 0.0;
			int cCol = 0;
			for (int bCol = startCol; bCol < endCol; bCol++)
			{
				int iCol = bCol;
				int r = br[iCol].R - ar[iCol].R;
				int g = br[iCol].G - ar[iCol].G;
				int b = br[iCol].B - ar[iCol].B;

				double mag = r * r + g * g + b * b;
				totalSum += mag;
				weightedSum += mag * cCol;

				cCol++;
			}

			// Compute the center of mass /// !! round ?
			centerCol = startCol + Utils.ROUND(weightedSum / totalSum);
			return centerCol;
		}

		private double GetMaxCenter(ImageProcessor.LaserRange range, List<Color> ar, List<Color> br)
		{
			int startCol = range.startCol;
			double centerCol = startCol;
			int endCol = range.endCol;
			int maxMagSq = 0;
			int numSameMax = 0;
			for (int bCol = startCol; bCol <= endCol; bCol++)
			{
				int iCol = bCol;
				int r = br[iCol].R - ar[iCol].R;
				int g = br[iCol].G - ar[iCol].G;
				int b = br[iCol].B - ar[iCol].B;
				int magSq = r * r + g * g + b * b;

				if (magSq > maxMagSq || bCol == 0)
				{
					maxMagSq = magSq;
					centerCol = bCol;
					numSameMax = 0;
				}
				else if (magSq == maxMagSq)
				{
					numSameMax++;
				}
			}

			centerCol += numSameMax / 2.0f;

			return centerCol;
		}
		private List<PixelLocation> SubProcess(Bitmap before, Bitmap after, Bitmap debuggingImage,
				 double laserThreshold, ref int firstRowLaserCol, ref int numSuspectedBadLaserLocations)
		{
			List<PixelLocation> laserLocations = new List<PixelLocation>(before.Height);

			Rectangle clipRect = new Rectangle(0, 0, before.Width, before.Height);

			int width = before.Width;
			int height = before.Height;
			int rowStep = width;

			int numLocations = 0;

			int numMerged = 0;

			// The location that we last detected a laser line
			int prevLaserCol = firstRowLaserCol;

			List<List<Color>> ar = before.GetPixels();
			List<List<Color>> br = after.GetPixels();

			/** The LaserRanges for each column */
			LaserRange[] laserRanges = new LaserRange[before.Height + 1];

			//            for (int y = 0; y < height && numLocations < height; y++)
			for (int y = clipRect.Top; y < clipRect.Bottom && numLocations < height; y++)
			{
				Color[] debugColor = new Color[width];
				List<Color> arLine = ar[y];
				List<Color> brLine = br[y];

				// The column that the laser started and ended on
				int numLaserRanges = 0;
				laserRanges[numLaserRanges].startCol = -1;
				laserRanges[numLaserRanges].endCol = -1;

				int imageColumn = 0;
				//		        for (int  x = 0; x < rowStep; x += 1)
				/**/
				for (int x = clipRect.Left; x < clipRect.Right; x += 1)
				{
					/**/
					imageColumn = x;
					// Perform image subtraction
					int r = brLine[x].R - arLine[x].R;
					int magSq = r * r;
					byte mag = (byte)(255.0f * (magSq * 0.000015379f));
					if (debuggingImage != null)
					{
						if (mag > laserThreshold)
							debugColor[x] = Color.FromArgb(255, mag, mag, mag);
						else if (magSq > 0)
							debugColor[x] = Color.FromArgb(255, mag, mag, 0);
						else
							debugColor[x] = Color.Black;
					}

					// Compare it against the threshold
					if (mag > laserThreshold)
					{
						// The start of pixels with laser in them
						if (laserRanges[numLaserRanges].startCol == -1)
						{
							laserRanges[numLaserRanges].startCol = imageColumn;
						}

					}
					// The end of pixels with laser in them
					else if (laserRanges[numLaserRanges].startCol != -1)
					{
						int laserWidth = imageColumn - laserRanges[numLaserRanges].startCol;
						if (laserWidth <= m_maxLaserWidth && laserWidth >= m_minLaserWidth)
						{
							// If this range was real close to the previous one, merge them instead of creating a new one
							bool wasMerged = false;
							if (numLaserRanges > 0)
							{
								int rangeDistance = laserRanges[numLaserRanges].startCol - laserRanges[numLaserRanges - 1].endCol;
								if (rangeDistance < RANGE_DISTANCE_THRESHOLD)
								{
									laserRanges[numLaserRanges - 1].endCol = imageColumn;
									laserRanges[numLaserRanges - 1].centerCol = Utils.ROUND((laserRanges[numLaserRanges - 1].startCol + laserRanges[numLaserRanges - 1].endCol) / 2);
									wasMerged = true;
									numMerged++;
								}
							}

							// Proceed to the next laser range
							if (!wasMerged)
							{
								// Add this range as a candidate
								laserRanges[numLaserRanges].endCol = imageColumn;
								laserRanges[numLaserRanges].centerCol = Utils.ROUND((laserRanges[numLaserRanges].startCol + laserRanges[numLaserRanges].endCol) / 2);

								numLaserRanges++;
							}

							// Reinitialize the range
							laserRanges[numLaserRanges].startCol = -1;
							laserRanges[numLaserRanges].endCol = -1;
						}
						// There was a false positive
						else
						{
							laserRanges[numLaserRanges].startCol = -1;
						}
					}

					// Go from image components back to image pixels
					imageColumn++;

				} // foreach column

				if (debuggingImage != null)
					debuggingImage.SetRowColors(y, debugColor);

				// If we have a valid laser region
				if (numLaserRanges > 0)
				{
					int rangeChoice = DetectBestLaserRange(laserRanges, numLaserRanges, prevLaserCol);
					prevLaserCol = laserRanges[rangeChoice].centerCol;

					double centerCol = DetectLaserRangeCenter(laserRanges[rangeChoice], arLine, brLine);

					PixelLocation location = new PixelLocation((double)centerCol, y);
					laserLocations.Add(location);

					// If this is the first row that a laser is detected in, set the firstRowLaserCol member
					if (laserLocations.Count == 1)
					{
						firstRowLaserCol = laserRanges[rangeChoice].startCol;
					}

					// Detect if we think this may have been a bad detection
					if (numLaserRanges > NUM_LASER_RANGE_THRESHOLD)
					{
						numSuspectedBadLaserLocations++;
					}
				}
			} // foreach row

			if (numMerged > 0)
			{
				Debug.WriteLine("Merged " + numMerged + " laser ranges.");
			}
			return laserLocations;
		}


		private double m_laserMagnitudeThreshold;
		private int m_maxLaserWidth;
		private int m_minLaserWidth;

		public eLaserDetectionMode LaserDetectionMode { get; private set; }
		private const int NUM_LASER_RANGE_THRESHOLD = 3;
		private const uint RANGE_DISTANCE_THRESHOLD = 5;
	}
}
