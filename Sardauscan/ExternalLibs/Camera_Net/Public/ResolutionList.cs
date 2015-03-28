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
    /// ResolutionList class for <see cref="Camera"/>.
    /// </summary>
    /// <remarks>This class is inherited from List<Resolution> class.</remarks>
    /// 
    /// <author> free5lot (free5lot@yandex.ru) </author>
    /// <version> 2013.10.16 </version>
    public class ResolutionList : List<Resolution>
    {
        /// <summary>
        /// Adds resolution to collection if it doesn't already exist in it
        /// </summary>
        /// <param name="item">Resolution should be added if it's new.</param>
        /// <returns>True if was added, False otherwise</returns>
        public bool AddIfNew(Resolution item)
        {
            if ( this.Contains(item) )
                return false;

            this.Add(item);
            return true;
        }
    }

}
