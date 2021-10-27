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
