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
using Sardauscan.Core.Geometry;
using OpenTK;
using System.ComponentModel;
using Sardauscan.Gui.PropertyGridEditor;
using System.Drawing.Design;
using System.Diagnostics;
using Sardauscan.Core.Interface;
using Sardauscan.Core;
using System.Drawing;

namespace Sardauscan.Core.ProcessingTask
{
	 /// <summary>
	 /// Apply a K-Mean process to remove the Scanline noises
	 /// </summary>
     [Browsable(true)]
    public class KMean : AbstractLineTask
    {

        private int numClusters = 5;
        [Browsable(true)]
        [Description("Number of Cluster")]
        [DisplayName("Number of Cluster")]
        [TypeConverter(typeof(NumericUpDownTypeConverter))]
        [Editor(typeof(NumericUpDownTypeEditor), typeof(UITypeEditor)), MinMaxAttribute(2, 30)]
        public int NumClusters { get { return numClusters; } set { numClusters = value; } }

        private int rejectPercent = 5;
        [Browsable(true)]
        [Description("if cluster has less than this percentage of point, the points in the cluster are rejected")]
        [DisplayName("Reject percentage")]
        [TypeConverter(typeof(NumericUpDownTypeConverter))]
        [Editor(typeof(NumericUpDownTypeEditor), typeof(UITypeEditor)), MinMaxAttribute(1, 50)]
        public int RejectPercent { get { return rejectPercent; } set { rejectPercent = value; } }

#if DEBUG
        private bool m_ColoriseOnly = false;
        [Browsable(true)]
        [Description("only colorise rejected")]
        [DisplayName("ColoriseOnly")]
        public bool ColoriseOnly { get { return m_ColoriseOnly; } set { m_ColoriseOnly = value; } }
#endif        

        protected override bool LaunchParallel
        {
            get
            {
                return false;
            }
        }

        static double Distance(Vector3 tuple, Vector3 vector)
        {
            // Euclidean distance between an actual data tuple and a cluster mean or centroid
            double sumSquaredDiffs = 0.0;
            //sumSquaredDiffs += Math.Pow((tuple.X - vector.X), 2);
            sumSquaredDiffs += Math.Pow((tuple.Y - vector.Y), 2);
            //sumSquaredDiffs += Math.Pow((tuple.Z - vector.Z), 2);
            return Math.Sqrt(sumSquaredDiffs);
        }

        static double DistanceOutlier(Vector3 tuple, Vector3 vector)
        {
            // Euclidean distance between an actual data tuple and a cluster mean or centroid
            double sumSquaredDiffs = 0.0;
            sumSquaredDiffs += Math.Pow((tuple.X - vector.X), 2);
            //sumSquaredDiffs += Math.Pow((tuple.Y - vector.Y), 2);
            sumSquaredDiffs += Math.Pow((tuple.Z - vector.Z), 2);
            return Math.Sqrt(sumSquaredDiffs);
        }

        public override ScanLine DoTask(ScanLine source)
        {

            try
            {
                //Debug.WriteLine("\nBegin outlier data detection using k-means clustering demo\n");

                //Debug.WriteLine("Loading all (height-weight) data into memory");
                string[] attributes = new string[] { "X", "Y", "Z" };
                Vector3[] rawData = new Vector3[source.Count];  // in most cases data will be in a text file or SQl table

                for (int i = 0; i < rawData.Length; i++)
                    rawData[i] = new Vector3(source[i].Position.X, source[i].Position.Y, source[i].Position.Z );

                //Debug.WriteLine("\nRaw data:\n");
                //ShowMatrix(rawData, rawData.Length, true);

                int numAttributes = attributes.Length;  // 2 in this demo (height,weight)
                int numClusters = NumClusters;  // vary this to experiment (must be between 2 and number data tuples)
                int maxCount = 30;  // trial and error

                //Debug.WriteLine("\nBegin clustering data with k = " + numClusters + " and maxCount = " + maxCount);
                int[] clustering = Cluster(rawData, numClusters, numAttributes, maxCount);
                //Debug.WriteLine("\nClustering complete");

                //Debug.WriteLine("\nClustering in internal format: \n");
                //ShowVector(clustering, true);  // true -> newline after display

                //Debug.WriteLine("\nClustered data:");
                //ShowClustering(rawData, numClusters, clustering, true);


                double maxDist = Distance(source.Max,source.Min) / (50);
                List<int> outlier = Outlier(rawData, clustering, maxDist);
                //Debug.WriteLine("Outlier for cluster 0 is:");
                //ShowVector(outlier, true);
                //Debug.WriteLine("\nEnd demo\n");
                List<int> count = new List<int>(numClusters);
                for (int i = 0; i < numClusters; i++)
                    count.Add(0);
                for (int i = 0; i < clustering.Length; i++)
                {
                    count[clustering[i]]++;
                }

                int pointCount = source.Count;
                List<bool> clusterOk = new List<bool>(numClusters);
                for (int i = 0; i < numClusters; i++)
                {
                    float pct =  (100f*count[i])/pointCount;
                    clusterOk.Add(pct >= this.RejectPercent);
                }

                ScanLine ret = new ScanLine(source.LaserID, source.Count);
                for (int i = 0; i < source.Count; i++)
                {
                    Point3D sp = source[i];
                    int clusterIndex = clustering[i];
                    Color col = sp.Color;
#if DEBUG
                    if (ColoriseOnly)
                    {
                        float clamp = ((1.0f * clusterIndex) / (numClusters - 1));
                        col = ColorExtension.ColorFromVector(new Vector4(clamp, clamp, clamp, clamp));
                        if (!clusterOk[clusterIndex])
                            col = Color.Green;
                        else if (outlier.Contains(i))
                            col = Color.Red;
                        ret.Add(new Point3D(sp.Position, sp.Normal, col));
                    }
                    else  if (clusterOk[clusterIndex] && !outlier.Contains(i))
#endif
                        ret.Add(new Point3D(sp.Position, sp.Normal, col));
                }

                return ret;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return source;
        } // Main


        static int[] InitClustering(int numTuples, int numClusters, int randomSeed)
        {
            // assign each tuple to a random cluster, making sure that there's at least
            // one tuple assigned to every cluster
            Random random = new Random(randomSeed);
            int[] clustering = new int[numTuples];

            // assign first numClusters tuples to clusters 0..k-1
            for (int i = 0; i < numClusters; ++i)
                clustering[i] = i;
            // assign rest randomly
            for (int i = numClusters; i < clustering.Length; ++i)
                clustering[i] = random.Next(0, numClusters);
            return clustering;
        }

        static Vector3[] Allocate(int numClusters)
        {
            // helper allocater for means[][] and centroids[][]
            Vector3[] result = new Vector3[numClusters];
            for (int k = 0; k < numClusters; ++k)
                result[k] = new Vector3();
            return result;
        }

        static void UpdateMeans(Vector3[] rawData, int[] clustering, Vector3[] means)
        {
            // assumes means[][] exists. consider making means[][] a ref parameter
            int numClusters = means.Length;
            // zero-out means[][]
            for (int k = 0; k < means.Length; ++k)
                    means[k]= new Vector3(0,0,0);

            // make an array to hold cluster counts
            int[] clusterCounts = new int[numClusters];

            // walk through each tuple, accumulate sum for each attribute, update cluster count
            for (int i = 0; i < rawData.Length; ++i)
            {
                int cluster = clustering[i];
                ++clusterCounts[cluster];

                means[cluster].X += rawData[i].X;
                means[cluster].Y += rawData[i].Y;
                means[cluster].Z += rawData[i].Z;
            }

            // divide each attribute sum by cluster count to get average (mean)
            for (int k = 0; k < means.Length; ++k)
            {
                int c = clusterCounts[k];
                means[k].X /= c;  // will throw if count is 0. consider an error-check
                means[k].Y /= c;  // will throw if count is 0. consider an error-check
                means[k].Z /= c;  // will throw if count is 0. consider an error-check
            }

            return;
        } // UpdateMeans

        static Vector3 ComputeCentroid(Vector3[] rawData, int[] clustering, int cluster, Vector3[] means)
        {
            // the centroid is the actual tuple values that are closest to the cluster mean
            Vector3 centroid = new Vector3();
            double minDist = double.MaxValue;
            for (int i = 0; i < rawData.Length; ++i) // walk thru each data tuple
            {
                int c = clustering[i];  // if curr tuple isn't in the cluster we're computing for, continue on
                if (c != cluster) continue;

                double currDist = Distance(rawData[i], means[cluster]);  // call helper
                if (currDist < minDist)
                {
                    minDist = currDist;
                    centroid = new Vector3(rawData[i]);
                }
            }
            return centroid;
        }

        static void UpdateCentroids(Vector3[] rawData, int[] clustering, Vector3[] means, Vector3[] centroids)
        {
            // updates all centroids by calling helper that updates one centroid
            for (int k = 0; k < centroids.Length; ++k)
            {
                Vector3 centroid = ComputeCentroid(rawData, clustering, k, means);
                centroids[k] = centroid;
            }
        }

        static int MinIndex(double[] distances)
        {
            // index of smallest value in distances[]
            int indexOfMin = 0;
            double smallDist = distances[0];
            for (int k = 0; k < distances.Length; ++k)
            {
                if (distances[k] < smallDist)
                {
                    smallDist = distances[k]; indexOfMin = k;
                }
            }
            return indexOfMin;
        }

        static bool Assign(Vector3[] rawData, int[] clustering, Vector3[] centroids)
        {
            // assign each tuple to best cluster (closest to cluster centroid)
            // return true if any new cluster assignment is different from old/curr cluster
            // does not prevent a state where a cluster has no tuples assigned. see article for details
            int numClusters = centroids.Length;
            bool changed = false;

            double[] distances = new double[numClusters]; // distance from curr tuple to each cluster mean
            for (int i = 0; i < rawData.Length; ++i)      // walk thru each tuple
            {
                for (int k = 0; k < numClusters; ++k)       // compute distances to all centroids
                    distances[k] = Distance(rawData[i], centroids[k]);

                int newCluster = MinIndex(distances);  // find the index == custerID of closest 
                if (newCluster != clustering[i]) // different cluster assignment?
                {
                    changed = true;
                    clustering[i] = newCluster;
                } // else no change
            }
            return changed; // was there any change in clustering?
        } // Assign

        static int[] Cluster(Vector3[] rawData, int numClusters, int numAttributes, int maxCount)
        {
            bool changed = true;
            int ct = 0;

            int numTuples = rawData.Length;
            int[] clustering = InitClustering(numTuples, numClusters, 0);  // 0 is a seed for random
            Vector3[] means = Allocate(numClusters);       // just makes things a bit cleaner
            Vector3[] centroids = Allocate(numClusters);
            UpdateMeans(rawData, clustering, means);                       // could call this inside UpdateCentroids instead
            UpdateCentroids(rawData, clustering, means, centroids);

            while (changed == true && ct < maxCount)
            {
                ++ct;
                changed = Assign(rawData, clustering, centroids); // use centroids to update cluster assignment
                UpdateMeans(rawData, clustering, means);  // use new clustering to update cluster means
                UpdateCentroids(rawData, clustering, means, centroids);  // use new means to update centroids
            }
            //ShowMatrix(centroids, centroids.Length, true);  // show the final centroids for each cluster
            return clustering;
        }

        static List<int> Outlier(Vector3[] rawData, int[] clustering, double maxDistance)
        {

            List<int> outlier = new List<int>();

            Vector3[] means = Allocate(clustering.Length);
            Vector3[] centroids = Allocate(clustering.Length);
            UpdateMeans(rawData, clustering, means);
            UpdateCentroids(rawData, clustering, means, centroids);

            for (int i = 0; i < rawData.Length; ++i)
            {
                int c = clustering[i];
                double dist = DistanceOutlier(rawData[i], centroids[c]);
                if (dist > maxDistance)
                {
                    outlier.Add(i);
                }
            }
            return outlier;
        }

        // display routines below

        static void ShowMatrix(double[][] matrix, int numRows, bool newLine)
        {
            for (int i = 0; i < numRows; ++i)
            {
                Debug.Write("[" + i.ToString().PadLeft(2) + "]  ");
                for (int j = 0; j < matrix[i].Length; ++j)
                    Debug.Write(matrix[i][j].ToString("F1") + "  ");
                Debug.WriteLine("");
            }
            if (newLine == true) Debug.WriteLine("");
        } // ShowMatrix

        static void ShowVector(int[] vector, bool newLine)
        {
            for (int i = 0; i < vector.Length; ++i)
                Debug.Write(vector[i] + " ");
            Debug.WriteLine("");
            if (newLine == true) Debug.WriteLine("");
        }

        static void ShowVector(Vector3 vector, bool newLine)
        {
            Debug.WriteLine(string.Format("{0:0.00} {1:0.00} {2:0.00}", vector.X, vector.Y, vector.Z));
            if (newLine == true) Debug.WriteLine("");
        }

        static void ShowClustering(Vector3[] rawData, int numClusters, int[] clustering, bool newLine)
        {
            Debug.WriteLine("-----------------");
            for (int k = 0; k < numClusters; ++k) // display by cluster
            {
                for (int i = 0; i < rawData.Length; ++i) // each tuple
                {
                    if (clustering[i] == k) // curr tuple i belongs to curr cluster k.
                    {
                        Debug.Write("[" + i.ToString().PadLeft(2) + "]");
                        Debug.WriteLine(string.Format("{0:0.00} {1:0.00} {2:0.00}", rawData[i].X,rawData[i].Y,rawData[i].Z));
                    }
                }
                Debug.WriteLine("-----------------");
            }
            if (newLine == true) Debug.WriteLine("");
        }

        public override string Name
        {
            get { return "K-Mean Filter"; }
        }
    } // class

}
