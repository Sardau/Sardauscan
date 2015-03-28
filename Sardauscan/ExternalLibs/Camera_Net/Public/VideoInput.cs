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

    using DirectShowLib;
    #endregion

    /// <summary>
    /// Video input of a capture board.
    /// </summary>
    ///
    /// <remarks>The class is used to describe video input of devices like video capture boards,
    /// which usually provide several inputs.
    /// </remarks>
    /// <author> free5lot (free5lot@yandex.ru) </author>
    /// <version> 2013.10.16 </version>
    public class VideoInput
    {
        /// <summary>
        /// Index of the video input.
        /// </summary>
        public readonly int Index;

        /// <summary>
        /// Type of the video input.
        /// </summary>
        public readonly PhysicalConnectorType Type;

        /// <summary>
        /// Default type of the video input.
        /// </summary>
        public readonly static PhysicalConnectorType PhysicalConnectorType_Default = 0;

        /// <summary>
        /// Constructor for <see cref="VideoInput"/> class.
        /// </summary>
        /// <param name="index">Index of the video input.</param>
        /// <param name="type">Type of the video input.</param>
        public VideoInput(int index, PhysicalConnectorType type)
        {
            Index = index;
            Type = type;
        }

        /// <summary>
        /// Default video input. Used to specify that it should not be changed.
        /// </summary>
        public static VideoInput Default
        {
            get { return new VideoInput(-1, PhysicalConnectorType_Default); }
        }

        /// <summary>
        /// Makes a clone of video input.
        /// </summary>
        /// <remarks>Clone is not connected with original object via refs.</remarks>
        /// <returns>Clone of object</returns>
        public VideoInput Clone()
        {
            return new VideoInput(this.Index, this.Type);
        }
    }
}
