#region License

/*
Camera_NET - Camera wrapper for directshow for .NET
Copyright (C) 2013
https://github.com/free5lot/Camera_Net

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 3.0 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU LesserGeneral Public 
License along with this library. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

namespace Camera_NET
{
    #region Using directives
    using System;
    using System.Collections.Generic;
    #endregion

    /// <summary>
    /// Resolution class for <see cref="Camera"/>.
    /// </summary>
    /// 
    /// <author> free5lot (free5lot@yandex.ru) </author>
    /// <version> 2013.10.16 </version>
    public class Resolution : IComparable<Resolution>, IEquatable<Resolution>
    {
        /// <summary>
        /// Width of frame of video output.
        /// </summary>
        public int Width { set; get; }

        /// <summary>
        /// Height of frame of video output.
        /// </summary>
        public int Height { set; get; }

        /// <summary>
        /// Constructor for <see cref="Resolution"/> class.
        /// </summary>
        /// <param name="width">Width of frame of video output.</param>
        /// <param name="height">Height of frame of video output.</param>
        public Resolution(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Comparator for IComparable<Resolution>
        /// </summary>
        /// <param name="y">Resolution we should compare to.</param>
        public int CompareTo(Resolution y)
        {
            Resolution x = this;

            if ( x == null)
            {
                if (y == null)
                    return 0; // If x is null and y is null, they're equal. 
                else
                    return -1; // If x is null and y is not null, y is greater. 
            }
            else
            {
                // If x is not null and y is null, x is greater.
                if (y == null)
                    return 1;
            }

            // Main comparation
            // x and y are not null
            if (x.Width > y.Width)
                return 1;
            else
            if (x.Width < y.Width)
                return -1;
            else
                return x.Height.CompareTo(y.Height);  //x.Width == y.Width
        }

        /// <summary>
        /// To String convertion
        /// </summary>
        /// <returns>String the object was converted to</returns>
        public override string ToString()
        {
            // String representation.
            return Width.ToString() + "x" + Height.ToString();
        }

        /// <summary>
        /// Makes a clone of resolution.
        /// </summary>
        /// <remarks>Clone is not connected with original object via refs.</remarks>
        /// <returns>Clone of object</returns>
        public Resolution Clone()
        {
            Resolution copy = new Resolution(Width, Height);
            return copy;
        }

        public bool Equals(Resolution other)
        {
            if (other == null)
                return false;

            if (this.Height != other.Height ||
                this.Width  != other.Width)
                return false;

            return true;
        }
    }


}
