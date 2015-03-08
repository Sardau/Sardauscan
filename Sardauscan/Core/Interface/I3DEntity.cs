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

namespace Sardauscan.Core.Interface
{
	/// <summary>
	///  Interface for Trianges recuperation
	/// </summary>
    public interface I3DEntity : IScene3DPart
    {
			/// <summary>
			/// IS this 3d entity a Mesh
			/// </summary>
        bool IsMesh { get; }
			/// <summary>
			/// Get the mesh Triangles
			/// </summary>
			/// <returns></returns>
        List<Triangle3D> GetTriangles();

    }

    public interface I3DEntity<T> : I3DEntity
    {
        T Data { get; }

    }
}
