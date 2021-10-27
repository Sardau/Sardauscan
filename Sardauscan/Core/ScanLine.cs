#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sardauscan.Core.Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Sardauscan.Core
{
	/// <summary>
	/// Object representing all the data of a laser scan
	/// </summary>
	[DebuggerDisplay("{LaserID}({Angle}°) {Count}")]
	public class ScanLine : Point3DList, IComparable
	{
		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="laserID"></param>
		/// <param name="capacity"></param>
		public ScanLine(int laserID, int capacity = 0)
			: base(capacity)
		{
			LaserID = laserID;
			DisplayAsLine = true;
		}
		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="laserID"></param>
		/// <param name="points"></param>
		public ScanLine(int laserID, IEnumerable<Point3D> points)
			: this(laserID, points.Count())
		{
			this.AddRange(points);
		}
		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="laserID"></param>
		/// <param name="points"></param>
		public ScanLine(int laserID, Point3DList points)
			: this(laserID, (IEnumerable<Point3D>)points)
		{
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="laserID"></param>
		/// <param name="points"></param>
		public ScanLine(ScanLine other)
			: this(other.LaserID, (Point3DList)other)
		{
			DisplayAsLine = other.DisplayAsLine;
		}

		/// <summary>
		/// Get the openGL drawing primitive
		/// </summary>
		public override PrimitiveType DrawAs
		{
			get
			{
				return DisplayAsLine ? PrimitiveType.LineStrip : PrimitiveType.Points;
			}
		}
		private bool _DisplayAsLine = false;
		/// <summary>
		/// Get set the Render of this line
		/// </summary>
		public bool DisplayAsLine { get { return _DisplayAsLine; } set { _DisplayAsLine = value; } }
		/// <summary>
		/// Is this scanline a composed of face (mesh part)
		/// </summary>
		public virtual bool IsFaces { get { return false; } }
		/// <summary>
		/// LAser ID used to take this scanline
		/// </summary>
		public int LaserID { get; protected set; }

		private double _angle = double.NaN;
		/// <summary>
		/// Angle of the ScanLine
		/// </summary>
		public double Angle
		{
			get
			{

                if (Count <= 0)
                    return double.NaN;

                if(Dirty)
                {
                    Update();
                    List<Point3D> sub = new List<Point3D>();
                    for(int i= Count/4; i <= (Count*3/4);i++)
                        sub.Add(this[i]);
                    Point3D avg = Point3D.Average(sub);
                    _angle = avg.Position.XZProjected_AngleInDegree(new Vector3d(0, 0, 1)) + 180;
                }

				if (!double.IsNaN(_angle))
					return _angle;

                return (this[Count/2].Position.XZProjected_AngleInDegree(new Vector3d(0, 0, 1))) + 180;
			}
		}


		private void ForceAngle(double angle)
		{
			_angle = angle;
		}
		/// <summary>
		/// Rotate these scanline around Y axe
		/// </summary>
		/// <param name="angle"></param>
		public void RotateAroundY(double angle)
		{
			double radian = Utils.DEGREES_TO_RADIANS(angle);
			for (int i = 0; i < Count; i++)
				this[i].RotateAroundY(radian);

		}

		public int CompareTo(object obj)
		{
			return this.Angle.CompareTo(((ScanLine)obj).Angle);
		}
	}
}
