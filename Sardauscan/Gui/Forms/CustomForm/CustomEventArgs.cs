#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#region BASED ON Custom Border Forms - Copyright (C) 2005 Szymon Kobalczyk

// Custom Border Forms
// Copyright (C) 2005 Szymon Kobalczyk
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.

// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//
// Szymon Kobalczyk (http://www.geekswithblogs.com/kobush)

#endregion
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;

namespace Sardauscan.Gui.Forms.CustomForm
{
    #region NonClientMouseEventArgs

#if !DEBUGFORM
    [DebuggerStepThrough]
#endif
    public class NonClientMouseEventArgs : System.Windows.Forms.MouseEventArgs
    {
        #region Variables

        private int _hitTest;
        private bool _handled;

        #endregion

        #region Constructor

        public NonClientMouseEventArgs(System.Windows.Forms.MouseButtons button, int clicks, int x, int y, int delta, int hitTest)
            : base(button, clicks, x, y, delta)
        {
            _hitTest = hitTest;
        }

        #endregion

        #region Properties

        public int HitTest
        {
            get { return _hitTest; }
            set { _hitTest = value; }
        }

        public bool Handled
        {
            get { return _handled; }
            set { _handled = value; }
        }

        #endregion
    }

    #endregion

    #region NonClientPaintEventArgs

#if !DEBUGFORM
    [DebuggerStepThrough]
#endif
    public class NonClientPaintEventArgs : EventArgs
    {
        #region Variables

        private Rectangle _bounds;
        private Region _clipRegion;
        private Graphics _graphics;

        #endregion

        #region Constructor

        public NonClientPaintEventArgs(Graphics g, Rectangle bounds, Region clipRegion)
        {
            _graphics = g;
            _bounds = bounds;
            _clipRegion = clipRegion;
        }

        #endregion

        #region Properties

        public Rectangle Bounds
        {
            get { return _bounds; }
        }

        public Region ClipRegion
        {
            get { return _clipRegion; }
        }

        public Graphics Graphics
        {
            get { return _graphics; }
        }

        #endregion
    }

    #endregion

}
