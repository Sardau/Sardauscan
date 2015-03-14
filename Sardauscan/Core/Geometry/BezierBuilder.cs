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

namespace Sardauscan.Core.Geometry
{
	/// <summary>
	/// Class to help with bezier curve
	/// based on Herman Tulleken article
	/// http://devmag.org.za/2011/04/05/bzier-curves-a-tutorial/
  /// http://devmag.org.za/2011/06/23/bzier-path-algorithms/
	/// </summary>
    public class BezierBuilder
    {

        // This corresponds to about 172 degrees, 8 degrees from a traight line
        private const double DIVISION_THRESHOLD = -0.99f;

			   // the list of curve control points
        private Point3DList controlPoints;

        private int curveCount; //how many bezier curves in this path?

        /**
            Constructs a new empty Bezier curve. Use one of these methods
            to add points: SetControlPoints, Interpolate, SamplePoints.
        */
        public BezierBuilder(int segmentPerCurve = 10, double minSquareDistance = 0.01f)
        {
            controlPoints = new Point3DList();
            SEGMENTS_PER_CURVE = segmentPerCurve;
            MINIMUM_SQR_DISTANCE = minSquareDistance;
        }

        readonly int SEGMENTS_PER_CURVE;
        readonly double MINIMUM_SQR_DISTANCE;        

        /**
            Sets the control points of this Bezier path.
            Points 0-3 forms the first Bezier curve, points 
            3-6 forms the second curve, etc.
        */
        public void SetControlPoints(Point3DList newControlPoints)
        {
            controlPoints.Clear();
            controlPoints.AddRange(newControlPoints);
            curveCount = (controlPoints.Count - 1) / 3;
        }

        /**
            Returns the control points for this Bezier curve.
        */
        public Point3DList GetControlPoints()
        {
            return controlPoints;
        }

        /**
            Calculates a Bezier interpolated path for the given points.
        */
        public void Interpolate(Point3DList segmentPoints, double scale)
        {
            controlPoints.Clear();

            if (segmentPoints.Count < 2)
            {
                return;
            }

            for (int i = 0; i < segmentPoints.Count; i++)
            {
                if (i == 0) // is first
                {
                    Point3D p1 = segmentPoints[i];
                    Point3D p2 = segmentPoints[i + 1];

                    controlPoints.Add(p1);
                    controlPoints.Add(Point3DList.Interpolate(p1,p2,scale));
                    
                }
                else if (i == segmentPoints.Count - 1) //last
                {
                    Point3D p0 = segmentPoints[i - 1];
                    Point3D p1 = segmentPoints[i];

                    controlPoints.Add(Point3DList.Interpolate(p0,p1, scale));
                    controlPoints.Add(p1);

                }
                else
                {
                    Point3D p0 = segmentPoints[i - 1];
                    Point3D p1 = segmentPoints[i];
                    Point3D p2 = segmentPoints[i + 1];
                    Vector3d tp = (p2.Position - p0.Position).Normalized();

                    Vector3d pos0 = p1.Position - scale * tp * (p1.Position - p0.Position).Length;
                    Vector3d pos1 = p1.Position + scale * tp * (p2.Position - p1.Position).Length;


                    Vector3d tn = (p2.Normal - p0.Normal).Normalized();
                    Vector3d norm0 = p1.Normal - scale * tn * (p1.Normal - p0.Normal).Length;
                    Vector3d norm1 = p1.Normal + scale * tn * (p2.Normal - p1.Normal).Length;



                    controlPoints.Add(new Point3D(pos0,norm0,p0.Color.GetStepColor(p1.Color,scale)));
                    controlPoints.Add(p1);
                    controlPoints.Add(new Point3D(pos1, norm1, p1.Color.GetStepColor(p2.Color,scale)));
                }
            }

            curveCount = (controlPoints.Count - 1) / 3;
        }

        /**
            Sample the given points as a Bezier path.
        */
        public void SamplePoints(Point3DList sourcePoints, double minSqrDistance, double maxSqrDistance, double scale)
        {
            if (sourcePoints.Count < 2)
            {
                return;
            }

            Stack<Point3D> samplePoints = new Stack<Point3D>();

            samplePoints.Push(sourcePoints[0]);

            Point3D potentialSamplePoint = sourcePoints[1];

            int i = 2;

            for (i = 2; i < sourcePoints.Count; i++)
            {
                if (
                    ((potentialSamplePoint.Position - sourcePoints[i].Position).LengthSquared > minSqrDistance) &&
                    ((samplePoints.Peek().Position - sourcePoints[i].Position).LengthSquared > maxSqrDistance))
                {
                    samplePoints.Push(potentialSamplePoint);
                }

                potentialSamplePoint = sourcePoints[i];
            }

            //now handle last bit of curve
            Point3D p1 = samplePoints.Pop(); //last sample point
            Point3D p0 = samplePoints.Peek(); //second last sample point


            Vector3d posT = (p0.Position - potentialSamplePoint.Position).Normalized();
            double pos_d2 = (potentialSamplePoint.Position - p1.Position).Length;
            double pos_d1 = (p1.Position - p0.Position).Length;
            double pos_scale =((pos_d1 - pos_d2) / 2f);
            Vector3d pos_ = p1.Position + posT * pos_scale;

            Vector3d normT = (p0.Normal - potentialSamplePoint.Normal).Normalized();
            double norm_d2 = (potentialSamplePoint.Normal - p1.Normal).Length;
            double norm_d1 = (p1.Normal - p0.Normal).Length;
            Vector3d norm_ = p1.Normal + normT * ((norm_d1 - norm_d2) / 2);

            
            samplePoints.Push(new Point3D(pos_,norm_,p1.Color.GetStepColor(p0.Color,pos_scale)));
            samplePoints.Push(potentialSamplePoint);
            Point3DList l = new Point3DList(samplePoints.Count);
            l.AddRange(samplePoints);
            Interpolate(l, scale);
        }

        /**
            Caluclates a point on the path.
        
            @param curveIndex The index of the curve that the point is on. For example, 
            the second curve (index 1) is the curve with controlpoints 3, 4, 5, and 6.
        
            @param t The paramater indicating where on the curve the point is. 0 corresponds 
            to the "left" point, 1 corresponds to the "right" end point.
        */
        public Point3D CalculateBezierPoint(int curveIndex, double t)
        {
            int nodeIndex = curveIndex * 3;

            Point3D p0 = controlPoints[nodeIndex];
            Point3D p1 = controlPoints[nodeIndex + 1];
            Point3D p2 = controlPoints[nodeIndex + 2];
            Point3D p3 = controlPoints[nodeIndex + 3];

            return CalculateBezierPoint(t, p0, p1, p2, p3);
        }

        /**
            Gets the drawing points. This implementation simply calculates a certain number
            of points per curve.
        */
        public Point3DList GetDrawingPoints0()
        {
            Point3DList drawingPoints = new Point3DList();

            for (int curveIndex = 0; curveIndex < curveCount; curveIndex++)
            {
                if (curveIndex == 0) //Only do this for the first end point. 
                //When i != 0, this coincides with the 
                //end point of the previous segment,
                {
                    drawingPoints.Add(CalculateBezierPoint(curveIndex, 0));
                }

                for (int j = 1; j <= SEGMENTS_PER_CURVE; j++)
                {
                    double t = j / (double)SEGMENTS_PER_CURVE;
                    drawingPoints.Add(CalculateBezierPoint(curveIndex, t));
                }
            }

            return drawingPoints;
        }

        /**
            Gets the drawing points. This implementation simply calculates a certain number
            of points per curve.

            This is a lsightly different inplementation from the one above.
        */
        public Point3DList GetDrawingPoints1()
        {
            Point3DList drawingPoints = new Point3DList();

            for (int i = 0; i < controlPoints.Count - 3; i += 3)
            {
                Point3D p0 = controlPoints[i];
                Point3D p1 = controlPoints[i + 1];
                Point3D p2 = controlPoints[i + 2];
                Point3D p3 = controlPoints[i + 3];

                if (i == 0) //only do this for the first end point. When i != 0, this coincides with the end point of the previous segment,
                {
                    drawingPoints.Add(CalculateBezierPoint(0, p0, p1, p2, p3));
                }

                for (int j = 1; j <= SEGMENTS_PER_CURVE; j++)
                {
                    double t = j / (double)SEGMENTS_PER_CURVE;
                    drawingPoints.Add(CalculateBezierPoint(t, p0, p1, p2, p3));
                }
            }

            return drawingPoints;
        }

        /**
            This gets the drawing points of a bezier curve, using recursive division,
            which results in less points for the same accuracy as the above implementation.
        */
        public Point3DList GetDrawingPoints2()
        {
            Point3DList drawingPoints = new Point3DList();

            for (int curveIndex = 0; curveIndex < curveCount; curveIndex++)
            {
                Point3DList bezierCurveDrawingPoints = FindDrawingPoints(curveIndex);

                if (curveIndex != 0)
                {
                    //remove the fist point, as it coincides with the last point of the previous Bezier curve.
                    bezierCurveDrawingPoints.RemoveAt(0);
                }

                drawingPoints.AddRange(bezierCurveDrawingPoints);
            }

            return drawingPoints;
        }

        Point3DList FindDrawingPoints(int curveIndex)
        {
            Point3DList pointList = new Point3DList();

            Point3D left = CalculateBezierPoint(curveIndex, 0);
            Point3D right = CalculateBezierPoint(curveIndex, 1);

            pointList.Add(left);
            pointList.Add(right);

            FindDrawingPoints(curveIndex, 0, 1, pointList, 1);

            return pointList;
        }


        /**
            @returns the number of points added.
        */
        int FindDrawingPoints(int curveIndex, double t0, double t1,
            Point3DList pointList, int insertionIndex)
        {
            Point3D left = CalculateBezierPoint(curveIndex, t0);
            Point3D right = CalculateBezierPoint(curveIndex, t1);

            if ((left.Position - right.Position).LengthSquared < MINIMUM_SQR_DISTANCE)
            {
                return 0;
            }

            double tMid = (t0 + t1) / 2;
            Point3D mid = CalculateBezierPoint(curveIndex, tMid);

            Vector3d leftDirection = (left.Position - mid.Position).Normalized();
            Vector3d rightDirection = (right.Position - mid.Position).Normalized();

            if (Vector3d.Dot(leftDirection, rightDirection) > DIVISION_THRESHOLD || Math.Abs(tMid - 0.5f) < 0.0001f)
            {
                int pointsAddedCount = 0;

                pointsAddedCount += FindDrawingPoints(curveIndex, t0, tMid, pointList, insertionIndex);
                pointList.Insert(insertionIndex + pointsAddedCount, mid);
                pointsAddedCount++;
                pointsAddedCount += FindDrawingPoints(curveIndex, tMid, t1, pointList, insertionIndex + pointsAddedCount);

                return pointsAddedCount;
            }

            return 0;
        }



        /**
            Caluclates a point on the Bezier curve represented with the four controlpoints given.
        */
        private Point3D CalculateBezierPoint(double t, Point3D p0, Point3D p1, Point3D p2, Point3D p3)
        {
            double u = 1 - t;
            double tt = t * t;
            double uu = u * u;
            double uuu = uu * u;
            double ttt = tt * t;

            Vector3d p = uuu * p0.Position; //first term

            p += 3 * uu * t * p1.Position; //second term
            p += 3 * u * tt * p2.Position; //third term
            p += ttt * p3.Position; //fourth term


            Vector3d n = uuu * p0.Normal; //first term

            n += 3 * uu * t * p1.Normal; //second term
            n += 3 * u * tt * p2.Normal; //third term
            n += ttt * p3.Normal; //fourth term

            Vector4 c = ((float)uuu) * p0.Color.ToVector(); //first term

            c += ((float)(3 * uu * t)) * p1.Color.ToVector(); //second term
            c += ((float)(3 * u * tt)) * p2.Color.ToVector(); //third term
            c += ((float)(ttt)) * p3.Color.ToVector(); //fourth term


            return new Point3D(p, n, ColorExtension.ColorFromVector(c));

        }
    }
}