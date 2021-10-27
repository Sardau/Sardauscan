#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
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

		public ImageProcessor(double magnitudeThreshold, int minLaserWidth, int maxLaserWidth, eLaserDetectionMode detectionMode)
		{
			m_laserMagnitudeThreshold = magnitudeThreshold;
			m_maxLaserWidth = maxLaserWidth;
			m_minLaserWidth = minLaserWidth;
			this.LaserDetectionMode = detectionMode;
		}


		public ImageProcessor(double magnitudeThreshold, int minLaserWidth, int maxLaserWidth)
			: this(magnitudeThreshold, minLaserWidth, maxLaserWidth, (eLaserDetectionMode)Settings.Get<Settings>().Read(Settings.SCANNER, Settings.CENTERDETECTIONMODE, eLaserDetectionMode.MassCenter))
		{
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
        public List<PointF> Process(Bitmap before, Bitmap after, Bitmap debuggingImage)
		{

			LockBitmap b = new LockBitmap(before);
			LockBitmap d = debuggingImage != null ? new LockBitmap(debuggingImage) : null;
            List<PointF> ret = Process(b, after, d);
			b.UnlockBits();
			if(d!=null)
				d.UnlockBits();

			return ret;
		}


        public List<PointF> Process(LockBitmap before, Bitmap after, LockBitmap debuggingImage)
		{

			LockBitmap a = new LockBitmap(after);
            List<PointF> ret = SubProcess(before, a, debuggingImage, m_laserMagnitudeThreshold);

			a.UnlockBits();
			return ret;
		}


		private int DetectBestLaserRange(LaserRange[] ranges, int numRanges, int prevLaserCol)
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
		private double DetectLaserRangeCenter(LaserRange range, double[] magSquare)
		{
			switch (LaserDetectionMode)
			{
				case eLaserDetectionMode.MassCenter:
					return GetMassCenter(range, magSquare);
				case eLaserDetectionMode.MassHarmonicCenter:
					return GetMassHarmonicCenter(range, magSquare);
				case eLaserDetectionMode.QuadricCenter:
					return GetQuadricCenter(range, magSquare);
			}
			return GetMaxCenter(range, magSquare);
		}
		//http://fr.wikipedia.org/wiki/Moyenne_quadratique
		private double GetQuadricCenter(LaserRange range, double[] magSquare)
		{
			int startCol = range.startCol;
			int endCol = range.endCol;

			double count = 0;
			double sum = 0;
			for (int col = startCol; col < endCol; col++)
			{
				count++;
				sum += col * col;
			}
			return Math.Sqrt(sum / count);

		}

		//http://fr.wikipedia.org/wiki/Moyenne_harmonique_pond%C3%A9r%C3%A9e
		private double GetMassHarmonicCenter(LaserRange range, double[] magSquare)
		{
			int startCol = range.startCol;
			int endCol = range.endCol;

			double numerator = 0;
			double denominator = 0;
			for (int col = startCol; col < endCol; col++)
			{
				double mag = magSquare[col];
				if (mag != 0)
				{
					numerator += mag;
					denominator += mag / col;
				}
			}
			return numerator / denominator;

		}
		private double GetMassCenter(LaserRange range, double[] magSquare)
		{
			int startCol = range.startCol;
			double centerCol = startCol;
			int endCol = range.endCol;

			double totalSum = 0.0;
			double weightedSum = 0.0;
			int cCol = 0;
			for (int bCol = startCol; bCol < endCol; bCol++)
			{
				double mag =magSquare[bCol];
				totalSum += mag;
				weightedSum += mag * cCol;

				cCol++;
			}

			// Compute the center of mass /// !! round ?
			centerCol = startCol + Utils.ROUND(weightedSum / totalSum);
			return centerCol;
		}

		private double GetMaxCenter(LaserRange range, double[] magSquare)
		{
			int startCol = range.startCol;
			double centerCol = startCol;
			int endCol = range.endCol;
			double maxMagSq = 0;
			int numSameMax = 0;
			for (int bCol = startCol; bCol <= endCol; bCol++)
			{
				double magSq = magSquare[bCol];
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
		private List<PointF> SubProcess(LockBitmap before, LockBitmap after, LockBitmap debuggingImage,
				 double laserThreshold)
		{
			int firstRowLaserCol = before.Width / 2;
            List<PointF> laserLocations = new List<PointF>(before.Height);


			int left_Clip = 0;
			int top_Clip = 0;
			int right_Clip = left_Clip + before.Width;
			int bottom_Clip = top_Clip + before.Height;

			int width = before.Width;
			int height = before.Height;
			int rowStep = width;

			int numLocations = 0;

			int numMerged = 0;

			// The location that we last detected a laser line
			int prevLaserCol = firstRowLaserCol;


			/** The LaserRanges for each column */
			LaserRange[] laserRanges = new LaserRange[before.Height + 1];

			double[] magSquare = new double[before.Width];

			//            for (int y = 0; y < height && numLocations < height; y++)
			for (int y = top_Clip; y < bottom_Clip && numLocations < height; y++)
			{

				// The column that the laser started and ended on
				int numLaserRanges = 0;
				laserRanges[numLaserRanges].startCol = -1;
				laserRanges[numLaserRanges].endCol = -1;

				//		        for (int  x = 0; x < rowStep; x += 1)
				/**/
				for (int x = left_Clip; x < right_Clip; x += 1)
				{
					/**/
					// Perform image subtraction
					int r = before.GetRed(x, y) - after.GetRed(x, y);
					int magSq = r * r;
					magSquare[x] = magSq;
					byte mag = (byte)(255.0f * (magSq * 0.000015379f));
					if (debuggingImage != null)
					{
						if (mag > laserThreshold)
							debuggingImage.SetPixel(x,y, Color.FromArgb(255, mag, mag, mag));
						else if (magSq > 0)
							debuggingImage.SetPixel(x,y, Color.FromArgb(255, mag, mag, 0));
						else
							debuggingImage.SetPixel(x,y, Color.Black);
					}

					// Compare it against the threshold
					if (mag > laserThreshold)
					{
						// The start of pixels with laser in them
						if (laserRanges[numLaserRanges].startCol == -1)
						{
							laserRanges[numLaserRanges].startCol = x;
						}

					}
					// The end of pixels with laser in them
					else if (laserRanges[numLaserRanges].startCol != -1)
					{
						int laserWidth = x - laserRanges[numLaserRanges].startCol;
						if (laserWidth <= m_maxLaserWidth && laserWidth >= m_minLaserWidth)
						{
							// If this range was real close to the previous one, merge them instead of creating a new one
							bool wasMerged = false;
							if (numLaserRanges > 0)
							{
								int rangeDistance = laserRanges[numLaserRanges].startCol - laserRanges[numLaserRanges - 1].endCol;
								if (rangeDistance < RANGE_DISTANCE_THRESHOLD)
								{
									laserRanges[numLaserRanges - 1].endCol = x;
									laserRanges[numLaserRanges - 1].centerCol =(laserRanges[numLaserRanges - 1].startCol + laserRanges[numLaserRanges - 1].endCol) / 2;
									wasMerged = true;
									numMerged++;
								}
							}

							// Proceed to the next laser range
							if (!wasMerged)
							{
								// Add this range as a candidate
								laserRanges[numLaserRanges].endCol = x;
								laserRanges[numLaserRanges].centerCol = (laserRanges[numLaserRanges].startCol + laserRanges[numLaserRanges].endCol) / 2;

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
				} // foreach column


				// If we have a valid laser region
				if (numLaserRanges > 0)
				{
					int rangeChoice = DetectBestLaserRange(laserRanges, numLaserRanges, prevLaserCol);
					prevLaserCol = laserRanges[rangeChoice].centerCol;

					double centerCol = DetectLaserRangeCenter(laserRanges[rangeChoice],magSquare);

                    PointF location = new PointF((float)centerCol, y);
					laserLocations.Add(location);

					// If this is the first row that a laser is detected in, set the firstRowLaserCol member
					if (laserLocations.Count == 1)
					{
						firstRowLaserCol = laserRanges[rangeChoice].startCol;
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
