//https://github.com/xuyuandong/ICP/blob/master/ICP.cpp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Sardauscan.Core.Geometry;

namespace Sardauscan.Core.ICP
{
	public class ICP
	{

		public ICP(int controlnum = 1000, float thre = 0.01f, int iter = 10)
		{
			cono = controlnum;
			threshold = thre;
			iterate = iter;
			//
			contP = new Vector3[cono];
			contQ = new Vector3[cono];
			index = new int[cono];
		}
		//public 	~ICP();
			public void Run(Point3DList varP, Point3DList varQ)
		{
			VarrQ = varQ;
			VarrP = varP;
			IniTransmat();
			Sample();
			//
			float err = Closest();
			//cout << "initial error = " << err << endl;
			//
			for (int i = 0; i < iterate; i++)
			{
				UpdateTransform();
				T = Matrix4.Mult(T, W);
				UpData();
				float newerr = Closest();
				//			cout << "iterate times = " << i << endl;
				//			cout << "error = " << newerr << endl;
				float delta = Math.Abs(err - newerr) / cono;
				//			cout << "delta = " << delta << endl;
				if (delta < threshold)
					break;
				err = newerr;

			}
			ApplyAll();
		}

		private void UpdateTransform()
		{
			LandmarkTransform land = new LandmarkTransform();
			land.SourceLandmarks = contP;
			land.TargetLandmarks = contQ;
			land.Update();
			W = land.Matrix;
		}
		private void IniTransmat() //init translate and rotate matrix
		{
			T = Matrix4.Identity;
		}
		private void Sample() //sample control points
		{
			//cout<<"sample control points from P"<<endl;
			//
			int N = VarrP.Count;
			bool[] flag = new bool[N];
			//assert(flag != NULL);
			for (int i = 0; i < N; i++)
				flag[i] = false;
			//sample control points index
			Random rnd = new Random(DateTime.Now.Millisecond);
			//		srand((unsigned)time(NULL));
			for (int i = 0; i < cono ; i++)
			{
				while (true)
				{
					int sam = rnd.Next(N);
					if (!flag[sam])
					{
						index[i] = sam;
						flag[sam] = true;
						break;
					}
				}
			}
			for (int i = 0; i < cono; i++)
			{
				Point3D v = VarrP[index[i]];
				contP[i] = new Vector3(v.Position);
			}
		}
		private float Closest() // find corresponding points and return error
		{
			//cout<<"find closest points and error"<<endl;
			//
			float error = 0.0f;
			for(int i=0;i<cono;i++)
			{
				float maxdist = 100.0f;
				index[i] = 0;
				for(int j=0;j<VarrQ.Count;j++)
				{
					float dist = (contP[i]-VarrQ[j].Position).LengthFast;
					if(dist < maxdist)
					{
						maxdist = dist;
						index[i] = j;
					}
				}
				Vector3 v = VarrQ[index[i]].Position;
				contQ[i] = new Vector3(v);
				error += maxdist;
			}
			return error;
		}

			private void UpData()  // update control points coordinate
			{
				//cout<<"update control points in P"<<endl;
				//
				//rotate + translate
				for(int i=0;i<cono;i++)
				{
					Vector3 tmp = new Vector3(0,0,0);
					contP[i] = Vector3.Transform(contP[i], W);

				}
			}
			private	void ApplyAll()
			{
				//cout<<"transform to all data in P"<<endl;
				// make rotate
				for(int i=0; i< VarrP.Count;i++)
				{
					Point3D v = VarrP[i];
					VarrP[i].Position = Vector3.Transform(v.Position, T);
					VarrP[i].Normal = Vector3.Transform(v.Normal, T);
				}
			}


			private	int cono; // control points number
			private	int iterate; //iterate number
			private	float threshold; //stop threshold
			private	Point3DList VarrP; //original points
			private	Point3DList VarrQ;
			private	Vector3[] contP; //control points in P
			private	Vector3[] contQ;
			private int[] index;	//use when sampling control points and in finding corresponding points index
			private Matrix4 T; //Transformation
			private	Matrix4 W;//step Trnasformation
	}

	public class LandmarkTransform
	{

		public OpenTK.Matrix4 Matrix;

		public Point3D[] SourceLandmarks;
		public Point3D[] TargetLandmarks;


		//----------------------------------------------------------------------------
		public LandmarkTransform()
		{
			Matrix = new Matrix4();

		}



		private void FindCentroids(int N_PTS, ref Vector3 source_centroid, refVector3 target_centroid)
		{

			Vector3 p = new Vector3();

			for (int i = 0; i < N_PTS; i++)
			{
				p = this.SourceLandmarks[i].Position;
				source_centroid[0] += p[0];
				source_centroid[1] += p[1];
				source_centroid[2] += p[2];
				p = this.TargetLandmarks[i].Position;
				target_centroid[0] += p[0];
				target_centroid[1] += p[1];
				target_centroid[2] += p[2];
			}
			source_centroid[0] /= N_PTS;
			source_centroid[1] /= N_PTS;
			source_centroid[2] /= N_PTS;
			target_centroid[0] /= N_PTS;
			target_centroid[1] /= N_PTS;
			target_centroid[2] /= N_PTS;
		}
		private void UpdateCorrelationMatrix(int pointIndex, ref Vector3 source_centroid, ref Vector3 target_centroid, float[,] M, ref float scale)
		{

			float sSum = 0.0F, tSum = 0.0F;


			Vector3 s = this.SourceLandmarks[pointIndex].Position;
			s[0] -= source_centroid[0];
			s[1] -= source_centroid[1];
			s[2] -= source_centroid[2];

			Vector3 t = this.TargetLandmarks[pointIndex].Position;
			t[0] -= target_centroid[0];
			t[1] -= target_centroid[1];
			t[2] -= target_centroid[2];
			// accumulate the products s*T(t) into the matrix M
			for (int i = 0; i < 3; i++)
			{
				M[i, 0] += s[i] * t[0];
				M[i, 1] += s[i] * t[1];
				M[i, 2] += s[i] * t[2];

				// for the affine transform, compute ((a.a^t)^-1 . a.b^t)^t.
				// a.b^t is already in M.  here we put a.a^t in AAT.
				//if (this.Mode == VTK_LANDMARK_AFFINE)
				//  {
				//  AAT[i,0] += a[i]*a[0];
				//  AAT[i,1] += a[i]*a[1];
				//  AAT[i,2] += a[i]*a[2];
				//  }
				//}
				// accumulate scale factors (if desired)
				sSum += s[0] * s[0] + s[1] * s[1] + s[2] * s[2];
				tSum += t[0] * t[0] + t[1] * t[1] + t[2] * t[2];
			}


			scale = (float)Math.Sqrt(tSum / sSum);
		}
		private float[,] CreateMatrixForDiag(int pointIndex, Vector3 source_centroid, Vector3 target_centroid, float[,] M, ref float scale)
		{
			//updates M
			UpdateCorrelationMatrix(pointIndex, ref source_centroid, ref target_centroid, M, ref scale);

			// -- build the 4x4 matrix N --

			float[,] Ndata = new float[4, 4];
			float[,] N = new float[4, 4];
			//      float *N[4];
			for (int i = 0; i < 4; i++)
			{
				//for (int j = 0; j < 4; j++)
				//{
				//    N[i, j] = Ndata[i, j];
				//}
				// fill N with zeros
				for (int j = 0; j < 4; j++)
				{
					N[i, j] = 0.0F;
				}

			}
			// on-diagonal elements
			N[0, 0] = M[0, 0] + M[1, 1] + M[2, 2];
			N[1, 1] = M[0, 0] - M[1, 1] - M[2, 2];
			N[2, 2] = -M[0, 0] + M[1, 1] - M[2, 2];
			N[3, 3] = -M[0, 0] - M[1, 1] + M[2, 2];
			// off-diagonal elements
			N[0, 1] = N[1, 0] = M[1, 2] - M[2, 1];
			N[0, 2] = N[2, 0] = M[2, 0] - M[0, 2];
			N[0, 3] = N[3, 0] = M[0, 1] - M[1, 0];

			N[1, 2] = N[2, 1] = M[0, 1] + M[1, 0];
			N[1, 3] = N[3, 1] = M[2, 0] + M[0, 2];
			N[2, 3] = N[3, 2] = M[1, 2] + M[2, 1];

			// -- eigen-decompose N (is symmetric) --
			return N;

		}
		//----------------------------------------------------------------------------
		// Find unit vectors which is perpendicular to this on and to
		// each other.
		public static void Perpendiculars(float[] x, float[] y, float[] z,
																 float theta)
		{
			int dx, dy, dz;
			float x2 = x[0] * x[0];
			float y2 = x[1] * x[1];
			float z2 = x[2] * x[2];
			float r = (float)Math.Sqrt(x2 + y2 + z2);

			// transpose the vector to avoid divide-by-zero error
			if (x2 > y2 && x2 > z2)
			{
				dx = 0; dy = 1; dz = 2;
			}
			else if (y2 > z2)
			{
				dx = 1; dy = 2; dz = 0;
			}
			else
			{
				dx = 2; dy = 0; dz = 1;
			}

			float a = x[dx] / r;
			float b = x[dy] / r;
			float c = x[dz] / r;

			float tmp = (float)Math.Sqrt(a * a + c * c);

			if (theta != 0)
			{
				float sintheta = (float)Math.Sin(theta);
				float costheta = (float)Math.Cos(theta);

				if (y != null)
				{
					y[dx] = (c * costheta - a * b * sintheta) / tmp;
					y[dy] = sintheta * tmp;
					y[dz] = (-a * costheta - b * c * sintheta) / tmp;
				}

				if (z != null)
				{
					z[dx] = (-c * sintheta - a * b * costheta) / tmp;
					z[dy] = costheta * tmp;
					z[dz] = (a * sintheta - b * c * costheta) / tmp;
				}
			}
			else
			{
				if (y != null)
				{
					y[dx] = c / tmp;
					y[dy] = 0;
					y[dz] = -a / tmp;
				}

				if (z != null)
				{
					z[dx] = -a * b / tmp;
					z[dy] = tmp;
					z[dz] = -b * c / tmp;
				}
			}
		}

		private void ChooseFirstFourEigenvalues(ref float w, ref float x, ref float y, ref float z, float[,] eigenvectors, float[] eigenvalues, int N_PTS)
		{

			// first: if points are collinear, choose the quaternion that 
			// results in the smallest rotation.
			if (eigenvalues[0] == eigenvalues[1] || N_PTS == 2)
			{
				Vector3 s0 = this.SourceLandmarks[0];
				Vector3 t0 = this.TargetLandmarks[0];
				Vector3 s1 = this.SourceLandmarks[1];
				Vector3 t1 = this.TargetLandmarks[1];



				float[] ds = new float[3];
				float[] dt = new float[3];

				float rs = 0;
				float rt = 0;
				for (int i = 0; i < 3; i++)
				{
					ds[i] = s1[i] - s0[i];      // vector between points
					rs += ds[i] * ds[i];
					dt[i] = t1[i] - t0[i];
					rt += dt[i] * dt[i];
				}

				// normalize the two vectors
				rs = (float)Math.Sqrt(rs);
				ds[0] /= rs; ds[1] /= rs; ds[2] /= rs;
				rt = (float)Math.Sqrt(rt);
				dt[0] /= rt; dt[1] /= rt; dt[2] /= rt;

				// take dot & cross product
				w = ds[0] * dt[0] + ds[1] * dt[1] + ds[2] * dt[2];
				x = ds[1] * dt[2] - ds[2] * dt[1];
				y = ds[2] * dt[0] - ds[0] * dt[2];
				z = ds[0] * dt[1] - ds[1] * dt[0];

				float r = (float)Math.Sqrt(x * x + y * y + z * z);
				float theta = (float)Math.Atan2(r, w);

				// construct quaternion
				w = (float)Math.Cos(theta / 2);
				if (r != 0)
				{
					r = (float)Math.Sin(theta / 2) / r;
					x = x * r;
					y = y * r;
					z = z * r;
				}
				else // rotation by 180 degrees: special case
				{
					// rotate around a vector perpendicular to ds
					Perpendiculars(ds, dt, null, 0);
					r = (float)Math.Sin(theta / 2);
					x = dt[0] * r;
					y = dt[1] * r;
					z = dt[2] * r;
				}
			}
			else // points are not collinear
			{
				w = eigenvectors[0, 0];
				x = eigenvectors[1, 0];
				y = eigenvectors[2, 0];
				z = eigenvectors[3, 0];
			}
		}
		private Matrix4 CreateMatrixOutOfDiagonalizationResult(float[,] eigenvectors, float[] eigenvalues, int N_PTS, float[] source_centroid, float[] target_centroid, float scale)
		{
			// the eigenvector with the largest eigenvalue is the quaternion we want
			// (they are sorted in decreasing order for us by JacobiN)

			float w = 0;
			float x = 0;
			float y = 0;
			float z = 0;

			//only important if points are collinear - otherwise they are the first largest eigenvalues
			ChooseFirstFourEigenvalues(ref w, ref x, ref y, ref z, eigenvectors, eigenvalues, N_PTS);


			// convert quaternion to a rotation matrix

			float ww = w * w;
			float wx = w * x;
			float wy = w * y;
			float wz = w * z;

			float xx = x * x;
			float yy = y * y;
			float zz = z * z;

			float xy = x * y;
			float xz = x * z;
			float yz = y * z;

			this.Matrix[0, 0] = ww + xx - yy - zz;
			this.Matrix[1, 0] = 2.0f * (wz + xy);
			this.Matrix[2, 0] = 2.0f * (-wy + xz);

			this.Matrix[0, 1] = 2.0f * (-wz + xy);
			this.Matrix[1, 1] = ww - xx + yy - zz;
			this.Matrix[2, 1] = 2.0f * (wx + yz);

			this.Matrix[0, 2] = 2.0f * (wy + xz);
			this.Matrix[1, 2] = 2.0f * (-wx + yz);
			this.Matrix[2, 2] = ww - xx - yy + zz;

			//if (this.Mode != VTK_LANDMARK_RIGIDBODY)
			//  { // add in the scale factor (if desired)
			for (int i = 0; i < 3; i++)
			{
				float val = this.Matrix[i, 0] * scale;
				this.Matrix[i, 0] = val;
				val = this.Matrix[i, 1] * scale;
				this.Matrix[i, 1] = val;

				val = this.Matrix[i, 2] * scale;
				this.Matrix[i, 2] = val;

			}

			//}

			// the translation is given by the difference in the transformed source
			// centroid and the target centroid
			float sx, sy, sz;

			sx = this.Matrix[0, 0] * source_centroid[0] +
					 this.Matrix[0, 1] * source_centroid[1] +
					 this.Matrix[0, 2] * source_centroid[2];
			sy = this.Matrix[1, 0] * source_centroid[0] +
					 this.Matrix[1, 1] * source_centroid[1] +
					 this.Matrix[1, 2] * source_centroid[2];
			sz = this.Matrix[2, 0] * source_centroid[0] +
					 this.Matrix[2, 1] * source_centroid[1] +
					 this.Matrix[2, 2] * source_centroid[2];

			this.Matrix[0, 3] = target_centroid[0] - sx;
			this.Matrix[1, 3] = target_centroid[1] - sy;
			this.Matrix[2, 3] = target_centroid[2] - sz;

			// fill the bottom row of the 4x4 matrix
			this.Matrix[3, 0] = 0.0f;
			this.Matrix[3, 1] = 0.0f;
			this.Matrix[3, 2] = 0.0f;
			this.Matrix[3, 3] = 1.0f;
			return this.Matrix;

		}
		public bool Update()
		{
			/*
		 The solution is based on
		 Berthold K. P. Horn (1987),
		 "Closed-form solution of absolute orientation using unit quaternions,"
		 Journal of the Optical Society of America A, 4:629-642
	 */

			// Original python implementation by David G. Gobbi

			if (this.SourceLandmarks == null || this.TargetLandmarks == null)
			{
				//Identity Matrix
				this.Matrix = new Matrix4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);
				return false;
			}

			// --- compute the necessary transform to match the two sets of landmarks ---



			int N_PTS = this.SourceLandmarks.Length;
			if (N_PTS != this.TargetLandmarks.Length)
			{
				System.Diagnostics.Debug.WriteLine("Error:  Source and Target Landmarks contain a different number of points");
				return false;
			}

			// -- if no points, stop here

			if (N_PTS == 0)
			{
				//Identity Matrix
				this.Matrix = new Matrix4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);
				return false;
			}
			float[] source_centroid = { 0, 0, 0 };
			float[] target_centroid = { 0, 0, 0 };

			FindCentroids(N_PTS, source_centroid, target_centroid);

			///-------------------------------
			// -- if only one point, stop right here

			if (N_PTS == 1)
			{
				this.Matrix = new Matrix4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);
				Matrix[0, 3] = target_centroid[0] - source_centroid[0];
				this.Matrix[1, 3] = target_centroid[1] - source_centroid[1];
				this.Matrix[2, 3] = target_centroid[2] - source_centroid[2];
				return true;
			}

			// -- build the 3x3 matrix M --

			float[,] M = new float[3, 3];
			float[,] AAT = new float[3, 3];

			for (int i = 0; i < 3; i++)
			{
				AAT[i, 0] = M[i, 0] = 0.0F; // fill M with zeros
				AAT[i, 1] = M[i, 1] = 0.0F;
				AAT[i, 2] = M[i, 2] = 0.0F;
			}
			int pt;


			for (pt = 0; pt < N_PTS; pt++)
			{

				float scale = 0F;
				float[,] N = CreateMatrixForDiag(pt, source_centroid, target_centroid, M, ref scale);

				float[,] eigenvectorData = new float[4, 4];
				//float *eigenvectors[4],eigenvalues[4];
				float[,] eigenvectors = new float[4, 4];
				float[] eigenvalues = new float[4];



				//for (int i = 0; i < 4; i++)
				//{
				//    for (int j = 0; j < 4; j++)
				//    {
				//        eigenvectors[i, j] = eigenvectorData[i, j];
				//    }
				//}


				vtkJacobiN(N, 4, eigenvalues, eigenvectors);

				//returns this.Matrix
				CreateMatrixOutOfDiagonalizationResult(eigenvectors, eigenvalues, N_PTS, source_centroid, target_centroid, scale);


				//this.Matrix.Modified();


			}
			return true;
		}
		private static int VTK_MAX_ROTATIONS = 20;
		public static void VTK_ROTATE(float[,] a, int i, int j, int k, int l, float tau, float s)
		{
			float g = a[i, j];
			float h = a[k, l];
			a[i, j] = g - s * (h + g * tau);
			a[k, l] = h + s * (g - h * tau);

		}

		public static int vtkJacobiN(float[,] a, int n, float[] w, float[,] v)
		{
			int i, j, k, iq, ip, numPos;
			float tresh;
			float theta, tau, t, sm, s, h, g, c, tmp;
			float[] bspace = new float[4];
			float[] zspace = new float[4];

			float[] b = bspace;
			float[] z = zspace;

			// only allocate memory if the matrix is large
			if (n > 4)
			{
				b = new float[n];
				z = new float[n];
			}

			// initialize
			for (ip = 0; ip < n; ip++)
			{
				for (iq = 0; iq < n; iq++)
				{
					v[ip, iq] = 0.0f;
				}
				v[ip, ip] = 1.0f;
			}
			for (ip = 0; ip < n; ip++)
			{
				b[ip] = w[ip] = a[ip, ip];
				z[ip] = 0.0f;
			}

			// begin rotation sequence
			for (i = 0; i < VTK_MAX_ROTATIONS; i++)
			{
				sm = 0.0f;
				for (ip = 0; ip < n - 1; ip++)
				{
					for (iq = ip + 1; iq < n; iq++)
					{
						sm += (float)Math.Abs(a[ip, iq]);
					}
				}
				if (sm == 0.0)
				{
					break;
				}

				if (i < 3)                                // first 3 sweeps
				{
					tresh = 0.2f * sm / (n * n);
				}
				else
				{
					tresh = 0.0f;
				}

				for (ip = 0; ip < n - 1; ip++)
				{
					for (iq = ip + 1; iq < n; iq++)
					{
						g = (float)(100.0 * Math.Abs(a[ip, iq]));

						// after 4 sweeps
						if (i > 3 && (Math.Abs(w[ip]) + g) == Math.Abs(w[ip])
						&& (Math.Abs(w[iq]) + g) == Math.Abs(w[iq]))
						{
							a[ip, iq] = 0.0f;
						}
						else if (Math.Abs(a[ip, iq]) > tresh)
						{
							h = w[iq] - w[ip];
							if ((Math.Abs(h) + g) == Math.Abs(h))
							{
								t = (a[ip, iq]) / h;
							}
							else
							{
								theta = (float)(0.5 * h / (a[ip, iq]));
								t = (float)(1.0 / (Math.Abs(theta) + Math.Sqrt(1.0 + theta * theta)));
								if (theta < 0.0)
								{
									t = -t;
								}
							}
							c = (float)(1.0 / Math.Sqrt(1 + t * t));
							s = t * c;
							tau = (float)(s / (1.0 + c));
							h = t * a[ip, iq];
							z[ip] -= h;
							z[iq] += h;
							w[ip] -= h;
							w[iq] += h;
							a[ip, iq] = 0.0f;

							// ip already shifted left by 1 unit
							for (j = 0; j <= ip - 1; j++)
							{
								VTK_ROTATE(a, j, ip, j, iq, tau, s);
							}
							// ip and iq already shifted left by 1 unit
							for (j = ip + 1; j <= iq - 1; j++)
							{
								VTK_ROTATE(a, ip, j, j, iq, tau, s);
							}
							// iq already shifted left by 1 unit
							for (j = iq + 1; j < n; j++)
							{
								VTK_ROTATE(a, ip, j, iq, j, tau, s);
							}
							for (j = 0; j < n; j++)
							{
								VTK_ROTATE(v, j, ip, j, iq, tau, s);
							}
						}
					}
				}

				for (ip = 0; ip < n; ip++)
				{
					b[ip] += z[ip];
					w[ip] = b[ip];
					z[ip] = 0.0f;
				}
			}

			//// this is NEVER called
			if (i >= VTK_MAX_ROTATIONS)
			{
				//System.Windows.Forms.MessageBox.Show("Jacobi: Error extracting eigenfunctions");
				System.Diagnostics.Debug.WriteLine("Error in Jacobi: No eigenfunctions");
				return 0;
			}

			// sort eigenfunctions                 these changes do not affect accuracy 
			for (j = 0; j < n - 1; j++)                  // boundary incorrect
			{
				k = j;
				tmp = w[k];
				for (i = j + 1; i < n; i++)                // boundary incorrect, shifted already
				{
					if (w[i] >= tmp)                   // why exchage if same?
					{
						k = i;
						tmp = w[k];
					}
				}
				if (k != j)
				{
					w[k] = w[j];
					w[j] = tmp;
					for (i = 0; i < n; i++)
					{
						tmp = v[i, j];
						v[i, j] = v[i, k];
						v[i, k] = tmp;
					}
				}
			}
			// insure eigenvector consistency (i.e., Jacobi can compute vectors that
			// are negative of one another (.707,.707,0) and (-.707,-.707,0). This can
			// reek havoc in hyperstreamline/other stuff. We will select the most
			// positive eigenvector.
			int ceil_half_n = (n >> 1) + (n & 1);
			for (j = 0; j < n; j++)
			{
				for (numPos = 0, i = 0; i < n; i++)
				{
					if (v[i, j] >= 0.0)
					{
						numPos++;
					}
				}
				//    if ( numPos < ceil(float(n)/float(2.0)) )
				if (numPos < ceil_half_n)
				{
					for (i = 0; i < n; i++)
					{
						v[i, j] *= -1.0f;
					}
				}
			}

			if (n > 4)
			{
				b = null;
				z = null;
			}
			return 1;
		}
 

		//------------------------------------------------------------------------

		//----------------------------------------------------------------------------
		void Inverse()
		{
			Vector3[] tmp1 = this.SourceLandmarks;
			Vector3[] tmp2 = this.TargetLandmarks;
			this.TargetLandmarks = tmp1;
			this.SourceLandmarks = tmp2;

		}




	}

}
