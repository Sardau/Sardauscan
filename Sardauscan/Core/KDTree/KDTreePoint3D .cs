using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sardauscan.Core.Geometry;
using OpenTK;

namespace Sardauscan.Core.KDTree
{
    /// <summary> 
    /// A KDTree class represents the root of a variable-dimension KD-Tree. 
    /// </summary> 
    /// <typeparam name="T">The generic data type we want this tree to contain.</typeparam> 
    /// <remarks>This is based on this: https://bitbucket.org/rednaxela/knn-benchmark/src/tip/ags/utils/dataStructures/trees/thirdGenKD/ </remarks> 
    public class KDTreePoint3D : KDNode 
    { 
        /// <summary> 
        /// Create a new KD-Tree given a number of dimensions. 
        /// </summary> 
        /// <param name="iDimensions">The number of data sorting dimensions. i.e. 3 for a 3D point.</param> 
        public KDTreePoint3D() 
            : base(24) 
        { 
        } 

 
        /// <summary> 
        /// Create a new KD-Tree given a number of dimensions and initial bucket capacity. 
        /// </summary> 
        /// <param name="iDimensions">The number of data sorting dimensions. i.e. 3 for a 3D point.</param> 
        /// <param name="iBucketCapacity">The default number of items that can be stored in each node.</param> 
				public KDTreePoint3D(int iBucketCapacity) 
            : base(iBucketCapacity) 
        { 
        } 
 
        /// <summary> 
        /// Get the nearest neighbours to a point in the kd tree using a square euclidean distance function. 
        /// </summary> 
        /// <param name="tSearchPoint">The point of interest.</param> 
        /// <param name="iMaxReturned">The maximum number of points which can be returned by the iterator.</param> 
        /// <param name="fDistance">A threshold distance to apply.  Optional.  Negative values mean that it is not applied.</param> 
        /// <returns>A new nearest neighbour iterator with the given parameters.</returns> 
        public NearestNeighbour NearestNeighbors(Vector3d tSearchPoint, int iMaxReturned, double fDistance = -1) 
        { 
            DistanceFunctions distanceFunction = new SquareEuclideanDistanceFunction(); 
            return NearestNeighbors(tSearchPoint, distanceFunction, iMaxReturned, fDistance); 
        } 
 
        /// <summary> 
        /// Get the nearest neighbours to a point in the kd tree using a user defined distance function. 
        /// </summary> 
        /// <param name="tSearchPoint">The point of interest.</param> 
        /// <param name="iMaxReturned">The maximum number of points which can be returned by the iterator.</param> 
        /// <param name="kDistanceFunction">The distance function to use.</param> 
        /// <param name="fDistance">A threshold distance to apply.  Optional.  Negative values mean that it is not applied.</param> 
        /// <returns>A new nearest neighbour iterator with the given parameters.</returns> 
				public NearestNeighbour NearestNeighbors(Vector3d tSearchPoint, DistanceFunctions kDistanceFunction, int iMaxReturned, double fDistance) 
        {
					return new NearestNeighbour(this, tSearchPoint, kDistanceFunction, iMaxReturned, fDistance); 
        }
				public IList<Point3D> FindNearest(IList<Point3D> pointsSource, IList<Point3D> pointsTarget, int keepOnlyNearestPoints)
				{
					IList<Point3D> nearestNeighbours = new List<Point3D>();
					int iMax = 1;
					List<double> listDistances = new List<double>();
					//double fThreshold = kdTreeNeighbourThreshold;
					for (int i = 0; i < pointsSource.Count; i++)
					{
						Point3D p = pointsSource[i];
						// Perform a nearest neighbour search around that point.
						NearestNeighbour pIter = null;
						pIter = NearestNeighbors(p.Position,iMax,-1);
						while (pIter.MoveNext())
						{
							Point3D cp = pIter.Current;
							listDistances.Add(pIter.CurrentDistance);
							nearestNeighbours.Add(cp);
							break;
						}

					}

					if (keepOnlyNearestPoints > 0)
						RemovePointsWithDistanceGreaterThanAverage(listDistances, pointsSource, nearestNeighbours);



					return nearestNeighbours;
				}
				public double GetAverage(List<double> source)
				{
					double[] temp1 = source.ToArray();
					Array.Sort(temp1);
					return temp1[temp1.Length / 2];
				}

				private void RemovePointsWithDistanceGreaterThanAverage(List<double> listDistances, IList<Point3D> pointsSource, IList<Point3D> pointsTarget)
				{
					double median = GetAverage(listDistances);

					for (int i = listDistances.Count - 1; i >= 0; i--)
					{
						if (listDistances[i] > median)
						{
							pointsSource.RemoveAt(i);
							pointsTarget.RemoveAt(i);

						}
					}
				}
 
    } 
} 
