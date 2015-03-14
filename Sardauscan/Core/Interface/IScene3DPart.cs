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
using OpenTK;
using Sardauscan.Core.OpenGL;

namespace Sardauscan.Core.Interface
{
	/// <summary>
	/// Interface for any Scene Part
	/// </summary>
	public interface IScene3DPart
	{
		/// <summary>
		/// Minimal X Y Z position
		/// </summary>
		Vector3d Min { get; }
		/// <summary>
		/// MAximal X Y Z position
		/// </summary>
		Vector3d Max { get; }
		/// <summary>
		/// Is the Scene dirty (modified), tell us if we must call Update (to update min-max etc)
		/// </summary>
		bool Dirty { get; }

		/// <summary>
		/// Clear the current Scene part
		/// </summary>
		void Clear();
		/// <summary>
		/// Update (min-max- etc)
		/// </summary>
		void Update();
		/// <summary>
		/// Render the part for with given Rendering context
		/// </summary>
		/// <param name="context"></param>
		void Render(ref RenderingContext context);
	}

	/// <summary>
	/// IScene3D interface Extentions
	/// </summary>
	public static class IScene3DPartExt
	{
		/// <summary>
		/// Get the center of the part
		/// </summary>
		/// <param name="part"></param>
		/// <returns></returns>
		public static Vector3d Center(this IScene3DPart part)
		{
			return new Vector3d((part.Min.X + part.Max.X) / 2.0f, (part.Min.Y + part.Max.Y) / 2.0f, (part.Min.Z + part.Max.Z) / 2.0f);
		}
		/// <summary>
		///  Get the size of the part
		/// </summary>
		/// <param name="part"></param>
		/// <returns></returns>
		static public double Size(this IScene3DPart part)
		{

			double mx = Math.Abs(part.Min.X - part.Max.X);
			double my = Math.Abs(part.Min.Y - part.Max.Y);
			double mz = Math.Abs(part.Min.Z - part.Max.Z);
			return Math.Max(mx, Math.Max(my, mz));
		}
	}
}
