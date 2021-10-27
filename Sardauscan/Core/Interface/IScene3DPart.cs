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

        int GetNumVertices();
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
