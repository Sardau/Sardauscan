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
using Sardauscan.Core.Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.ComponentModel;

namespace Sardauscan.Core.ProcessingTask
{
    /// <summary>
    /// Strip result
    /// </summary>
    public class StripResult
    {
        /// <summary>
        /// Create a strip result for 2 Scanline (with same Nbr of Points)
        /// </summary>
        /// <param name="prev"></param>
        /// <param name="current"></param>
        public StripResult(ScanLine prev, ScanLine current)
        {
            if (prev.Count != current.Count)
                throw new Exception(" previous and current pointlist doesn't have the same point count");
            Previous = prev;
            Current = current;
            Result = new ScanSlice(Previous.Count + current.Count);
            //GL.Begin(PrimitiveType.LineStrip);
            int count = Math.Min(Previous.Count, current.Count);
            for (int i = 0; i < count; i++)
            {
                Result.Add(Previous[i]);
                Result.Add(current[i]);
            }
            AbstractMeshBuilder.AdjustNormalFromTriangleStrip(Result);
        }
        /// <summary>
        /// Previous ScanLine
        /// </summary>
        public ScanLine Previous;
        /// <summary>
        /// Current ScanLine
        /// </summary>
        public ScanLine Current;
        /// <summary>
        /// Scan*SLICE* resulting
        /// </summary>
        public ScanSlice Result;
    }
    /// <summary>
    /// Class for Abstract Mesh builder
    /// </summary>
    public abstract class AbstractMeshBuilder : AbstractProcessingTask
    {

        /// <summary>
        /// Ctor
        /// </summary>
        public AbstractMeshBuilder()
        {
        }
        /// <summary>
        /// Do the task function override
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override ScanData DoTask(ScanData source)
        {
            ScanData ret = new ScanData(source.Count);
            UpdatePercent(0, ret);
            source.Sort();
            int count = source.Count;
            //count = 2;
            //primitiveType = PrimitiveType.LineStrip;
            ScanLine top = new ScanLine(count);
            ScanLine bottom = new ScanLine(count);

            ScanLine prev = source[0];
            top.Add(prev.First());
            bottom.Add(prev.Last());

            for (int i = 1; i <= count; i++)
            {
                if (this.CancelPending) return source;
                ScanLine current = source[i % count];
                StripResult strip = CreateStrip(prev, current);

                top.Add(strip.Current.First());
                bottom.Add(strip.Current.Last());

                prev = strip.Current;

                ret.Add(strip.Result);
                UpdatePercent((int)((100 * i) / count), ret);
            }
            if (count <= 2)
                return ret;
            Point3D topcenter = Point3D.Average(top);
            Point3D bottomcenter = Point3D.Average(bottom);
            for (int i = 0; i < ret.Count; i++)
            {
                ret[i].Insert(0, topcenter);
                ret[i].Add(bottomcenter);
                AdjustNormalFromTriangleStrip(ret[i]);
            }
            UpdatePercent(100, ret);
            return ret;

        }

        /// <summary>
        /// Create a StripResult from 2 ScanLine
        /// </summary>
        /// <param name="previous"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        protected abstract StripResult CreateStrip(ScanLine previous, ScanLine current);
        /// <summary>
        /// In data are ScanLines
        /// </summary>
        public override eTaskItem In
        {
            get { return eTaskItem.ScanLines; }
        }
        /// <summary>
        /// OutData is a Mesh
        /// </summary>
        public override eTaskItem Out
        {
            get { return eTaskItem.Mesh; }
        }
        /// <summary>
        /// Create Top and Bottom of mesh
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="invert"></param>
        /// <returns></returns>
        protected ScanSlice CreateForTopBottom(Point3DList pts, bool invert = false)
        {
            int count = pts.Count;
            if (count < 2)
                return new ScanSlice(pts);
            ScanSlice outerList = new ScanSlice(count);
            double x = 0;
            double y = 0;
            double z = 0;
            for (int i = 0; i < count + 1; i++)
            {
                Point3D p = pts[i % count];
                if (i == 0 || (pts[i - 1].Position - p.Position).LengthFast != 0)
                {
                    outerList.Add(p);
                    if (i < count)
                    {
                        x += p.Position.X;
                        y += p.Position.Y;
                        z += p.Position.Z;
                    }
                }
            }
            Vector3d center = new Vector3d((double)(x / count), (double)(y / count), (double)(z / count));

            count = outerList.Count;
            if (count < 2)
                return outerList;

            ScanSlice ret = new ScanSlice(count * 2);
            int idx = 0;
            for (idx = 0; idx < count; idx++)
            {
                Point3D pt = outerList[idx];
                if (invert)
                    ret.Add(new Point3D(center, pt.Normal, pt.Color));
                ret.Add(pt);
                if (!invert)
                    ret.Add(new Point3D(center, pt.Normal, pt.Color));
            }

            return ret;
        }
        /// <summary>
        /// Adjust normals for a "trianglestrip" Point list
        /// </summary>
        /// <param name="points"></param>
        public static void AdjustNormalFromTriangleStrip(Point3DList points)
        {
            int count = points.Count;
            if (count < 3)
                return;
            Vector3d normal = new Vector3d();
            for (int i = 0; i < count - 2; i++)
            {
                Vector3d v0 = points[i].Position;
                Vector3d v1 = points[i + 1].Position;
                Vector3d v2 = points[i + 2].Position;
                normal = -Triangle3D.CalculateNormal(v0, v1, v2);
                points[i].Normal = normal;
            }
            points[count - 1].Normal = normal;
            points[count - 2].Normal = normal;
        }

        /// <summary>
        /// Clone the task
        /// </summary>
        /// <returns></returns>
        public override AbstractProcessingTask Clone()
        {
            AbstractMeshBuilder ret = (AbstractMeshBuilder)Activator.CreateInstance(this.GetType());
            return ret;
        }



    }

}
