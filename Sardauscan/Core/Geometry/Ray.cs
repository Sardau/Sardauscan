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
using OpenTK;

namespace Sardauscan.Core.Geometry
{
	/// <summary>
	/// Class defining a Ray
	/// </summary>
	public class Ray
	{
		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="origin"></param>
		/// <param name="direction"></param>
		public Ray(Vector3d origin, Vector3d direction)
		{
			Origin = origin;
			Direction = direction;
		}
		/// <summary>
		/// Origin of the Ray
		/// </summary>
		public Vector3d Origin;
		//Direction vector of the ray
		public Vector3d Direction;
	};
}
