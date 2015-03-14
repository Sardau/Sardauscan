using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Windows.Media;
using System.Diagnostics;
using OpenTK;
using Sardauscan.Core.Geometry;
using Sardauscan.Core.KDTree;


namespace Sardauscan.Core.ICP
{
	public class IterativeClosestPointTransform
	{
		//int NumberPointsSolution = 100;
		public static List<Vector3d> NormalsSource;
		public static List<Vector3d> NormalsTarget;


		int MaxNumberSolutions = 10;
		public int NumberOfStartTrialPoints = 100000;


		public static bool SimulatedAnnealing = true;
		public static bool FixedTestPoints = false;
		public static int MaximumNumberOfIterations = 100;
		public static bool ResetPoint3DToOrigin = true;
		public static bool DistanceOptimization = true;

		private static IterativeClosestPointTransform instance;


		public Matrix4d Matrix;
		public int NumberOfIterations;

		private IList<Point3D> pointsTransformed;

		//int CheckMeanDistance;
		public double MeanDistance;


		double MaximumMeanDistance;


		public LandmarkTransform LandmarkTransform;
		List<ICPSolution> solutionList;


		public IList<Point3D> PSource;
		public IList<Point3D> PTarget;

		public static void Reset()
		{
			SimulatedAnnealing = false;
			FixedTestPoints = false;
			MaximumNumberOfIterations = 100;
			ResetPoint3DToOrigin = true;
			IterativeClosestPointTransform.DistanceOptimization = false;

		}
		public IterativeClosestPointTransform()//:base(PointerUtils.GetIntPtr(new double[3]), true, true)
		{
			this.PSource = null;
			this.PTarget = null;

			this.LandmarkTransform = new LandmarkTransform();



			this.MaximumMeanDistance = 1.0E-3f;

			this.NumberOfIterations = 0;
			this.MeanDistance = 0.0f;

		}

		public Matrix4d PerformICP(IList<Point3D> myPointsTarget, IList<Point3D> mypointsSource)
		{
			this.PTarget = myPointsTarget;
			this.PSource = mypointsSource;
			return PerformICP();
		}
		public static IterativeClosestPointTransform Instance
		{
			get
			{
				if (instance == null)
					instance = new IterativeClosestPointTransform();
				return instance;

			}
		}
		public IList<Point3D> PTransformed
		{
			get
			{
				return pointsTransformed;
			}
			set
			{
				pointsTransformed = value;
			}
		}



		public void Inverse()
		{
			IList<Point3D> tmp1 = this.PSource;
			this.PSource = this.PTarget;
			this.PTarget = tmp1;
			//this.Modified();
		}

		private KDTreePoint3D Helper_CreateTree(IList<Point3D> pointsTarget)
		{
			KDTreePoint3D kdTree = new KDTreePoint3D();
			kdTree.AddPoint(pointsTarget);
			return kdTree;
		}
		private bool Helper_FindNeighbours(ref IList<Point3D> pointsTarget, ref IList<Point3D> pointsSource, KDTreePoint3D kdTree, int keepOnlyPoints)
		{

			if (!FixedTestPoints)
			{
				pointsTarget = kdTree.FindNearest(pointsSource, pointsTarget, keepOnlyPoints);
				if (pointsTarget.Count != pointsSource.Count)
				{
					MessageBox.Show("Error finding neighbours, found " + pointsTarget.Count.ToString() + " out of " + pointsSource.Count.ToString());
					return false;
				}
			}
			else
			{
				//adjust number of points - for the case if there are outliers
				int min = pointsSource.Count;
				if (pointsTarget.Count < min)
				{
					min = pointsTarget.Count;
					pointsSource.RemoveRange(pointsTarget.Count, pointsSource.Count - min);

				}
				else
				{
					pointsTarget.RemoveRange(pointsSource.Count, pointsTarget.Count - min);
				}

			}
			return true;

		}
		private static double TransformPoints(ref IList<Point3D> myPointsTransformed, IList<Point3D> pointsTarget, IList<Point3D> pointsSource, Matrix4d myMatrix)
		{
			myPointsTransformed = MathUtils.TransformPoints(pointsSource, myMatrix);
			double totaldist = pointsTarget.CalculateTotalDistance(myPointsTransformed);
			double meanDistance = (double)(totaldist / Convert.ToDouble(pointsTarget.Count));
			return meanDistance;

		}
		private Matrix4d Helper_FindTransformationMatrix(IList<Point3D> pointsTarget, IList<Point3D> pointsSource)
		{
		 //if(true)
		//	 return SVD.FindTransformationMatrix(pointsTarget.Copy(),pointsSource.Copy());
			Matrix4d myMatrix;

			MatrixUtilsNew.FindTransformationMatrix(pointsSource, pointsTarget, this.LandmarkTransform);
			myMatrix = LandmarkTransform.Matrix;
			return myMatrix;



		}
		private void Helper_SetNewInterationSets(ref IList<Point3D> pointsTarget, ref IList<Point3D> pointsSource, IList<Point3D> PT, IList<Point3D> PS)
		{
			IList<Point3D> myPointsTransformed = MathUtils.TransformPoints(PS, Matrix);
			pointsSource = myPointsTransformed;
			pointsTarget = PT.Copy();

		}
		/// <summary>
		/// A single ICP Iteration
		/// </summary>
		/// <param name="pointsTarget"></param>
		/// <param name="pointsSource"></param>
		/// <param name="PT"></param>
		/// <param name="PS"></param>
		/// <param name="kdTree"></param>
		/// <returns></returns>
		private bool Helper_ICP_Iteration(ref IList<Point3D> pointsTarget, ref IList<Point3D> pointsSource, IList<Point3D> PT, IList<Point3D> PS, KDTreePoint3D kdTree, int keepOnlyPoints)
		{
			if (!Helper_FindNeighbours(ref pointsTarget, ref pointsSource, kdTree, keepOnlyPoints))
				return true;

			Matrix4d myMatrix = Helper_FindTransformationMatrix(pointsTarget, pointsSource);
			IList<Point3D> myPointsTransformed = MathUtils.TransformPoints(pointsSource, myMatrix);

			if (SimulatedAnnealing)
			{
				this.Matrix = myMatrix;

				double totaldist = pointsTarget.CalculateTotalDistance(myPointsTransformed);
				this.MeanDistance = (double)(totaldist / Convert.ToDouble(pointsTarget.Count));

				//new set:
				pointsSource = myPointsTransformed;
				pointsTarget = PT.Copy();



			}
			else
			{
				Matrix4d.Mult(ref myMatrix, ref this.Matrix, out this.Matrix);
				double totaldist = pointsTarget.CalculateTotalDistance(myPointsTransformed);
				this.MeanDistance = (double)(totaldist / Convert.ToDouble(pointsTarget.Count));
				//Debug.WriteLine("--------------Iteration: " + iter.ToString() + " : Mean Distance: " + MeanDistance.ToString("0.00000000000"));

				if (MeanDistance < this.MaximumMeanDistance) //< Math.Abs(MeanDistance - oldMeanDistance) < this.MaximumMeanDistance)
					return true;

				Helper_SetNewInterationSets(ref pointsTarget, ref pointsSource, PT, PS);
			}
			return false;

		}
		private bool Helper_ICP_Iteration_SA(IList<Point3D> PT, IList<Point3D> PS, KDTreePoint3D kdTree, int keepOnlyPoints)
		{
			try
			{

				//first iteration
				if (solutionList == null)
				{
					solutionList = new List<ICPSolution>();


					if (NumberOfStartTrialPoints > PS.Count)
						NumberOfStartTrialPoints = PS.Count;
					if (NumberOfStartTrialPoints == PS.Count)
						NumberOfStartTrialPoints = PS.Count * 80 / 100;
					if (NumberOfStartTrialPoints < 3)
						NumberOfStartTrialPoints = 3;



					for (int i = 0; i < MaxNumberSolutions; i++)
					{
						ICPSolution myTrial = ICPSolution.SetRandomIndices(NumberOfStartTrialPoints, PS.Count, solutionList);

						if (myTrial != null)
						{
							myTrial.PointsSource = ExtractPoints(PS, myTrial.RandomIndices);
							solutionList.Add(myTrial);
						}
					}
					////test....
					////maxNumberSolutions = 1;
					//ICPSolution myTrial1 = new ICPSolution();
					//for (int i = 0; i < NumberPointsSolution; i++)
					//{
					//    myTrial1.RandomIndices.Add(i);
					//}
					//myTrial1.PointsSource = RandomUtils.ExtractPoints(PS, myTrial1.RandomIndices);
					//solutionList[0] = myTrial1;


				}


				for (int i = 0; i < solutionList.Count; i++)
				{
					IList<Point3D> transformedPoints = null;

					ICPSolution myTrial = solutionList[i];
					Helper_ICP_Iteration(ref myTrial.PointsTarget, ref myTrial.PointsSource, PT, PS, kdTree, keepOnlyPoints);
					myTrial.Matrix = Matrix4d.Mult(myTrial.Matrix, this.Matrix);
					myTrial.MeanDistanceSubset = this.MeanDistance;

					myTrial.MeanDistance = TransformPoints(ref transformedPoints, PT, PS, myTrial.Matrix);

					// solutionList[i] = myTrial;

				}
				if (solutionList.Count > 0)
				{
					solutionList.Sort(new ICPSolutionComparer());
					RemoveSolutionIfMatrixContainsNaN(solutionList);
					if (solutionList.Count == 0)
						System.Windows.Forms.MessageBox.Show("No solution could be found !");

					this.Matrix = solutionList[0].Matrix;
					this.MeanDistance = solutionList[0].MeanDistance;

					if (solutionList[0].MeanDistance < this.MaximumMeanDistance)
					{
						return true;
					}


				}

			}
			catch (Exception err)
			{
				MessageBox.Show("Error in Helper_ICP_Iteration_SA: " + err.Message);
				return false;
			}

			return false;


		}
		private static bool CheckIfMatrixIsOK(Matrix4d myMatrix)
		{
			//ContainsNaN
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					if (double.IsNaN(myMatrix[i, 0]))
						return false;

				}
			}
			return true;

		}
 
		private static void RemoveSolutionIfMatrixContainsNaN(List<ICPSolution> solutionList)
		{
			int iTotal = 0;
			for (int i = solutionList.Count - 1; i >= 0; i--)
			{
				if (!CheckIfMatrixIsOK(solutionList[i].Matrix))
				{
					iTotal++;

					solutionList.RemoveAt(i);
				}
			}
			// Debug.WriteLine("-->Removed a total of: " + iTotal.ToString() + " solutions - because invalid matrixes");
		}

		public static IList<Point3D> ExtractPoints(IList<Point3D> points, List<int> indices)
		{
			try
			{

				IList<Point3D> output = new List<Point3D>();
				for (int i = 0; i < indices.Count; i++)
				{
					int indexPoint = indices[i];
					Point3D p = points[indexPoint];
					output.Add(p);
				}
				return output;
			}
			catch (Exception err)
			{
				MessageBox.Show("Error in RandomUtils.ExtractPoints " + err.Message);
				return null;
			}
		}
		public Matrix4d PerformICP()
		{
			IList<Point3D> PT = PTarget.Copy();
			IList<Point3D> PS = PSource.Copy();
			Point3D pSOrigin = null;
			Point3D pTOrigin = null;

			if (ResetPoint3DToOrigin)
			{
				pTOrigin = PT.ResetToOrigin();
				pSOrigin = PS.ResetToOrigin();
			}

			int keepOnlyPoints = 0;
			if (DistanceOptimization)
				keepOnlyPoints = 3;
			int iter = 0;
			try
			{
				if (!CheckSourceTarget(PT, PS))
					return Matrix4d.Identity;

				IList<Point3D> pointsTarget = PT.Copy();
				IList<Point3D> pointsSource = PS.Copy();

				this.Matrix = Matrix4d.Identity;
				Matrix4d bestMatrix = Matrix4d.Mult(this.Matrix, 1f);
				double bestMeanDistance = (double)(pointsTarget.CalculateTotalDistance(pointsSource) / Convert.ToDouble(pointsTarget.Count));
				double oldMeanDistance = 0;

				KDTreePoint3D kdTreee = Helper_CreateTree(pointsTarget);

				for (iter = 0; iter < MaximumNumberOfIterations; iter++)
				{
					if (SimulatedAnnealing)
					{
						if (Helper_ICP_Iteration_SA(PT, PS, kdTreee, keepOnlyPoints))
							break;
					}
					else
					{
						if (Helper_ICP_Iteration(ref pointsTarget, ref pointsSource, PT, PS, kdTreee, keepOnlyPoints))
							break;
					}
					oldMeanDistance = MeanDistance;
					Debug.WriteLine("--------------Iteration: " + iter.ToString() + " : Mean Distance: " + MeanDistance.ToString("0.00000000000"));
					if (bestMeanDistance > MeanDistance)
					{
						bestMeanDistance = MeanDistance;
						bestMatrix = Matrix4d.Mult(this.Matrix, 1f);
					}
					else return bestMatrix;
					Debug.WriteLine("--------------Iteration: " + iter.ToString() + " : BEST: " + bestMeanDistance.ToString("0.00000000000"));

				}

				Debug.WriteLine("--------****** Solution of ICP after : " + iter.ToString() + " iterations, and Mean Distance: " + MeanDistance.ToString("0.00000000000"));
				return bestMatrix;
				//PTransformed = MathUtils.TransformPoints(PS, Matrix);
				////re-reset vector 
				//if (ResetPoint3DToOrigin)
				//{
				//  PTransformed.AddVector(pTOrigin);
				//  //Vertices.AddVector(PSource, pSOrigin);

				//}

				////DebugWriteUtils.WriteTestOutputPoint3D("Solution of ICP", Matrix, this.PSource, PTransformed, PTarget);

				//return PTransformed;
			}
			catch (Exception err)
			{
				System.Windows.Forms.MessageBox.Show("Error in Update ICP at iteration: " + iter.ToString() + " : " + err.Message);
				return Matrix4d.Identity;

			}

		}
		private static bool CheckSourceTarget(IList<Point3D> myPointsTarget, IList<Point3D> mypointsSource)
		{
			// Check source, target
			if (mypointsSource == null || mypointsSource.Count == 0)
			{
				MessageBox.Show("Source point set is empty");
				System.Diagnostics.Debug.WriteLine("Can't execute with null or empty input");
				return false;
			}

			if (myPointsTarget == null || myPointsTarget.Count == 0)
			{
				MessageBox.Show("Target point set is empty");
				System.Diagnostics.Debug.WriteLine("Can't execute with null or empty target");
				return false;
			}
			return true;
		}


	}

	#region ICPSolution
	public class ICPSolution
	{
		public List<int> RandomIndices;
		public Matrix4d Matrix = Matrix4d.Identity;
		public double MeanDistanceSubset;
		public double MeanDistance;

		public IList<Point3D> PointsTarget;
		public IList<Point3D> PointsSource;
		public IList<Point3D> PointsTransformed;
		public ICPSolution()
		{
			RandomIndices = new List<int>();
		}
		public override string ToString()
		{
			string str = this.MeanDistance.ToString("0.0") + "(" + this.MeanDistanceSubset.ToString("0.0") + ")" + " : Matrix row 1: " + this.Matrix[0, 0].ToString("0.0") + ":" + this.Matrix[0, 1].ToString("0.0") + ":" +
					this.Matrix[0, 2].ToString("0.0") + ":" + this.Matrix[0, 3].ToString("0.0") + ":";

			return str;
		}
		private static bool ListEqual(List<int> a, List<int> b)
		{
			for (int i = 0; i < a.Count; i++)
			{
				if (a[i] != b[i])
					return false;

			}
			return true;

		}
		public static bool IndicesAreNew(List<int> newIndices, List<ICPSolution> solutionsList)
		{
			if (solutionsList.Count == 0)
				return true;

			try
			{

				for (int i = 0; i < solutionsList.Count; i++)
				{
					List<int> indicesOfList = solutionsList[i].RandomIndices;
					//System.Diagnostics.Debug.WriteLine( (indicesOfList[0] - newIndices[0]).ToString() + "; " + (indicesOfList[1] - newIndices[1]).ToString() );

					if (ListEqual(newIndices, indicesOfList))
						return false;

					//if (newIndices.Equals(indicesOfList))
					//    return false;


					//bool isNew = true;
					////because indices are sorted, if one of them is not equal, return
					//for (int j = 0; j < newIndices.Count; j++)
					//{
					//    if (indicesOfList[j] != newIndices[j])
					//    {
					//        isNew = true;
					//    }
					//}
					////if (equal)
					////    return false;
				}
				return true;
			}
			catch (Exception err)
			{
				MessageBox.Show("Error in IndicesAlreadyTried " + err.Message);
				return false;

			}

		}
		/// <summary>
		/// Generates random indices 
		/// </summary>
		/// <param name="MaxIndex"></param>
		/// <param name="numIndices"></param>
		/// <returns></returns>
		public static List<int> UniqueRandomIndices(int numIndices, int MaxIndex)
		{
			List<int> indices = new List<int>();
			try
			{
				//generate Number unique random indices from 0 to MAX


				//SeedRandom();
				//cannot generate more unique numbers than than the size of the set we are sampling
				if (numIndices > MaxIndex)
				{
					MessageBox.Show("SW Call error for UniqueRandomIndices");
					return indices;
				}
				Random rnd = new Random(DateTime.Now.Millisecond);
				for (int i = 0; i < 100000; i++)
				{
					double newRnd = rnd.NextDouble() * (MaxIndex - 1) - 0.5;
					int newIndex = Convert.ToInt32(newRnd);
					if (newIndex < 0)
						newIndex = 0;
					if (newIndex == MaxIndex)
						newIndex = MaxIndex - 1;

					if (!indices.Contains(newIndex))
						indices.Add(newIndex);

					if (indices.Count == numIndices)
						return indices;

				}
				MessageBox.Show("No random Indices are found - please check routine UniqueRandomIndices");
				return indices;
			}
			catch (Exception err)
			{
				MessageBox.Show("Error in UniqueRandomIndices " + err.Message);
				return indices;
			}


		}
		public static List<int> GetRandomIndices(int maxNumber, int myNumberPoints)
		{
			List<int> randomIndices = UniqueRandomIndices(myNumberPoints, maxNumber);
			randomIndices.Sort();

			return randomIndices;
		}
		public static ICPSolution SetRandomIndices(int myNumberPoints, int maxNumber, List<ICPSolution> solutionList)
		{
			int i;
			List<int> randomIndices;
			try
			{
				//set trial points 
				for (i = 0; i < 1000; i++)
				{
					try
					{

						randomIndices = ICPSolution.GetRandomIndices(maxNumber, myNumberPoints);
						if (ICPSolution.IndicesAreNew(randomIndices, solutionList))
						{

							ICPSolution res = new ICPSolution();
							res.RandomIndices = randomIndices;
							return res;


						}
					}
					catch (Exception err)
					{
						MessageBox.Show("Error in SetRandomIndices " + err.Message);
						return null;

					}
				}
				// MessageBox.Show("SetRandomIndices: No indices could be found!!");
				// 1000 trials
				return null;
			}
			catch (Exception err)
			{
				MessageBox.Show("Error in SetRandomIndices " + err.Message);
				return null;

			}

		}
	}
	public class ICPSolutionComparer : IComparer<ICPSolution>
	{

		public int Compare(ICPSolution a, ICPSolution b)
		{
			if (a == null || b == null)
			{
				return 0;
			}
			if (double.IsNaN(a.MeanDistance))
				return 1;
			if (double.IsNaN(b.MeanDistance))
				return -1;

			if (a.MeanDistance < b.MeanDistance)
				return -1;
			else
				return 1;


		}
	}
	#endregion
}