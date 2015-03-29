using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sardauscan.Core.Interface;
using OpenTK;
using Sardauscan.Core.OpenGL;

namespace Sardauscan.Core
{
	/// <summary>
	/// Ovject representing a Scan data or result
	/// </summary>
	public class ScanData : DirtyAvareList<ScanLine>, IScene3DPart
	{
		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="capacity"></param>
		public ScanData(int capacity = 0)
			: base(capacity)
		{
		}
		/// <summary>
		/// Is it a mesh
		/// </summary>
		public bool IsMesh
		{
			get
			{
				for (int i = 0; i < Count; i++)
					if (this[i].IsFaces)
						return true;
				return false;
			}
		}
		/// <summary>
		/// Get the number of points
		/// </summary>
		/// <returns></returns>
		public int PointCount()
		{
			int c = 0;
			for (int i = 0; i < Count; i++)
				c += this[i].Count;
			return c;
		}

		protected Vector3d m_Min;
		protected Vector3d m_Max;
		/// <summary>
		/// Minimum X Y Z position
		/// </summary>
		public Vector3d Min
		{
			get
			{
				if (Dirty)
					Update();
				return m_Min;
			}
		}

		/// <summary>
		/// Maximum X Y Z position
		/// </summary>
		public Vector3d Max
		{
			get
			{
				if (Dirty)
					Update();
				return m_Max;

			}
		}
		/// <summary>
		/// Is the data modified since last update
		/// </summary>
		public override bool Dirty
		{
			get
			{
				if (base.Dirty)
					return true;
				for (int i = 0; i < Count; i++)
				{
					if (this[i].Dirty)
						return true;
				}
				return false;
			}
		}
		/// <summary>
		/// Update the ScanData bounding values
		/// </summary>
		public override void Update()
		{
			if (this.Count == 0)
			{
				m_Min = new Vector3d(double.NaN, 0, 0);
				m_Max = new Vector3d(double.NaN, 0, 0);
			}
			else
			{
				for (int i = 0; i < Count; i++)
				{
					ScanLine part = this[i];
					if (part.Dirty)
						part.Update();

				}
				IScene3DPart p = this[0];
				m_Min = new Vector3d(p.Min);
				m_Max = new Vector3d(p.Max);

				for (int i = 1; i < Count; i++)
				{
					p = this[i];
					if (p.Min.IsValid())
					{
						m_Min.X = Math.Min(m_Min.X, p.Min.X);
						m_Min.Y = Math.Min(m_Min.Y, p.Min.Y);
						m_Min.Z = Math.Min(m_Min.Z, p.Min.Z);
					}
					if (p.Max.IsValid())
					{
						m_Max.X = Math.Max(m_Max.X, p.Max.X);
						m_Max.Y = Math.Max(m_Max.Y, p.Max.Y);
						m_Max.Z = Math.Max(m_Max.Z, p.Max.Z);
					}
				}
			}
			base.Update();
		}
		/// <summary>
		/// Render the ScanData to OpenGL
		/// </summary>
		/// <param name="context"></param>
		public void Render(ref RenderingContext context)
		{
			if (Dirty)
				Update();
			for (int i = 0; i < Count; i++)
				this[i].Render(ref context);
		}

		/// <summary>
		/// Get the nearest line to a given angle
		/// </summary>
		/// <param name="angle"></param>
		/// <returns></returns>
		public ScanLine GetNearestLine(double angle)
		{
			double refAngle = angle;
			double dist = double.MaxValue;
			ScanLine ret = null;
			for (int i = 0; i < this.Count; i++)
			{
				double curAngle = this[i].Angle;
				double dx = Math.Abs(Utils.DeltaAngle(refAngle, curAngle));
				if (dx < dist)
				{
					ret = this[i];
					dist = dx;
				}
			}
			return ret;
		}

		public void Transform(Matrix4d mat)
		{
			for (int i = 0; i < Count; i++)
				this[i].Transform(mat);

		}

        public int GetNumVertices()
        {
            return this.PointCount();
        }


	}
}
