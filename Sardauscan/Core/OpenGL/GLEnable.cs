using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sardauscan.Core.OpenGL
{
	class GLEnable : IDisposable
	{
		public GLEnable(EnableCap cap)
		{
			Cap=cap;
			GL.Enable(Cap);
		}
		private EnableCap Cap;
		public void Dispose()
		{
			GL.Disable(Cap);
		}
	}
}
