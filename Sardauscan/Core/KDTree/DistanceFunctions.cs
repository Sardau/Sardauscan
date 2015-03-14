using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Sardauscan.Core.KDTree
{
	/// <summary> 
	/// An interface which enables flexible distance functions. 
	/// </summary> 
	public interface DistanceFunctions
	{
		/// <summary> 
		/// Compute a distance between two n-dimensional points. 
		/// </summary> 
		/// <param name="p1">The first point.</param> 
		/// <param name="p2">The second point.</param> 
		/// <returns>The n-dimensional distance.</returns> 
		double Distance(Vector3d p1, Vector3d p2);

		/// <summary> 
		/// Find the shortest distance from a point to an axis aligned rectangle in n-dimensional space. 
		/// </summary> 
		/// <param name="point">The point of interest.</param> 
		/// <param name="min">The minimum coordinate of the rectangle.</param> 
		/// <param name="max">The maximum coorindate of the rectangle.</param> 
		/// <returns>The shortest n-dimensional distance between the point and rectangle.</returns> 
		double DistanceToRectangle(Vector3d point, double[] min, double[] max);
	}

	/// <summary> 
	/// A distance function for our KD-Tree which returns squared euclidean distances. 
	/// </summary> 
	public class SquareEuclideanDistanceFunction : DistanceFunctions
	{
		/// <summary> 
		/// Find the squared distance between two n-dimensional points. 
		/// </summary> 
		/// <param name="p1">The first point.</param> 
		/// <param name="p2">The second point.</param> 
		/// <returns>The n-dimensional squared distance.</returns> 
		public double Distance(Vector3d p1, Vector3d p2)
		{
			return (p1-p2).LengthFast;
		}

		/// <summary> 
		/// Find the shortest distance from a point to an axis aligned rectangle in n-dimensional space. 
		/// </summary> 
		/// <param name="point">The point of interest.</param> 
		/// <param name="min">The minimum coordinate of the rectangle.</param> 
		/// <param name="max">The maximum coorindate of the rectangle.</param> 
		/// <returns>The shortest squared n-dimensional squared distance between the point and rectangle.</returns> 
		public double DistanceToRectangle(Vector3d point, double[] min, double[] max)
		{
			double fSum = 0;
			double fDifference = 0;
			fDifference = 0;
			if (point.X > max[0])
				fDifference = (point.X - max[0]);
			else if (point.X < min[0])
				fDifference = (point.X - min[0]);
			fSum += fDifference * fDifference;

			fDifference = 0;
			if (point.Y > max[1])
				fDifference = (point.Y - max[1]);
			else if (point.Y < min[1])
				fDifference = (point.Y - min[1]);
			fSum += fDifference * fDifference;

			fDifference = 0;
			if (point.Z > max[2])
				fDifference = (point.Z - max[2]);
			else if (point.Z < min[2])
				fDifference = (point.Z - min[2]);
			fSum += fDifference * fDifference;

			return fSum;
		}
	}
}
