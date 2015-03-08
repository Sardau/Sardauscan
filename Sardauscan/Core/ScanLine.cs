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

		private float _angle = float.NaN;
		/// <summary>
		/// Angle of the ScanLine
		/// </summary>
		public float Angle
		{
			get
			{
				if (!float.IsNaN(_angle))
					return _angle;

				if (Count <= 0)
					return float.NaN;
				return this[Count / 2].Position.XZProjected_AngleInDegree(new Vector3(0, 0, 1)) + 180;
			}
		}


		private void ForceAngle(float angle)
		{
			_angle = angle;
		}
		/// <summary>
		/// Rotate these scanline around Y axe
		/// </summary>
		/// <param name="angle"></param>
		public void RotateAroundY(float angle)
		{
			float radian = Utils.DEGREES_TO_RADIANS(angle);
			for (int i = 0; i < Count; i++)
				this[i].RotateAroundY(radian);

		}

		public int CompareTo(object obj)
		{
			return this.Angle.CompareTo(((ScanLine)obj).Angle);
		}
	}
}
