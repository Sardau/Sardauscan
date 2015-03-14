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
			if (Dirty)
				Update();
			foreach (IScene3DPart part in this)
				part.Render(ref context);
		}
	}
}
