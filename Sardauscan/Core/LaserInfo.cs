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
using Sardauscan.Core.Interface;
using System.Drawing;
using Sardauscan.Core.Geometry;

namespace Sardauscan.Core
{
    /// <summary>
    /// Laser information
    /// </summary>
    public class LaserInfo
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cameraPos"></param>
        /// <param name="tableSize"></param>
        public LaserInfo(int id, Vector3d cameraPos, SizeF tableSize)
        {
            Id = id;
            Vector3d loc = new Vector3d();
            Settings settings = Settings.Get<Settings>();
            loc.X = settings.Read(Settings.LASER(Id), Settings.X, 9.5f);
            loc.Y = settings.Read(Settings.LASER(Id), Settings.Y, 27.0f);
            loc.Z = settings.Read(Settings.LASER(Id), Settings.Z, 7.0f);
            DefaultColor = settings.Read(Settings.LASER(Id), Settings.DEFAULTCOLOR, LaserInfo.GetDefaultColor(Id));


            Location = loc;

            Mapper = new LocationMapper(Location, cameraPos, tableSize);

            Correction = new LaserCorrection();
            Correction.LoadFromSettings(Id);

        }
        /// <summary>
        /// Map points
        /// </summary>
        /// <param name="laserLocations"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public ScanLine MapPoints(List<PointF> laserLocations, Bitmap image, bool useCorrection)
        {
            Point3DList points = Mapper.MapPoints(laserLocations, image, DefaultColor);
            ScanLine ret = new ScanLine(Id, points.Count);
            if (useCorrection)
            {
                Matrix4d m = Correction.GetMatrix();
                int count = points.Count;
                for (int i = 0; i < count; i++)
                {
                    Point3D p = points[i];
                    Vector3d v = Vector3d.Transform(p.Position, m);
                    Vector3d n = Vector3d.Transform(p.Normal, m);
                    n.Normalize();
                    ret.Add(new Point3D(v, n, p.Color));
                }
            }
            else
            {
                ret.Add(points);
            }
            return ret;
        }
        /// <summary>
        /// Laser ID
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// Laser Location
        /// </summary>
        public Vector3d Location { get; private set; }

        protected LocationMapper Mapper { get; private set; }
        /// <summary>
        /// Laser default Texture color
        /// </summary>
        public Color DefaultColor = Color.White;

        private static Color[] _DefaultColor = new Color[] { Color.Red, Color.Lime, Color.Blue, Color.Cyan, Color.Magenta, Color.Yellow, Color.White };
        public static Color GetDefaultColor(int index)
        {
            if (index < 0)
                return Color.Transparent;
            return _DefaultColor[index % _DefaultColor.Length];
        }
        public LaserCorrection Correction;
    }
}
