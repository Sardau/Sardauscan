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
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Sardauscan.Core;
using Sardauscan.Core.Interface;
using Sardauscan.Core.OpenGL;

namespace Sardauscan.Core.Geometry
{
	/// <summary>
	/// Scene 3D Object
	/// </summary>
	public class Scene3D : DirtyAvareList<IScene3DPart>, IScene3DPart
	{

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
		/// Is the Scened Dirty (modified)
		/// </summary>
		public override bool Dirty
		{
			get
			{
				if (base.Dirty)
					return true;
				foreach (IScene3DPart part in this)
				{
					if (part.Dirty)
						return true;
				}
				return false;
			}
		}
		/// <summary>
		/// Update the Scene info id modified
		/// </summary>
		public override void Update()
		{
			if (this.Count == 0 || this[0] == null)
			{
				m_Min = new Vector3d(double.NaN, 0, 0);
				m_Max = new Vector3d(double.NaN, 0, 0);
			}
			else
			{
				IScene3DPart p = null;
				for (int i = 1; i < Count; i++)
				{
					p = this[i];
					if (p.Dirty)
						p.Update();

				}
				p = this[0];
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
		/// Render the Scene
		/// </summary>
		/// <param name="context"></param>
		public void Render(ref RenderingContext context)
		{
            try
            {
                if (Dirty)
                    Update();
                foreach (IScene3DPart part in this)
                    part.Render(ref context);
            }
            catch
            { }
		}
        public int GetNumVertices()
        {
            int count = 0;
            for (int i = 0; i < Count; i++)
                count += this[i].GetNumVertices();
            return count;
        }

	}
}
