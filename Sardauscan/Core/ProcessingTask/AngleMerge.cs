using Sardauscan.Core.Geometry;
using Sardauscan.Gui.PropertyGridEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;

namespace Sardauscan.Core.ProcessingTask
{
    public class AngleMerge: AbstractProcessingTask
    {
        public override eTaskItem In {get{return eTaskItem.ScanLines;}}

        public override eTaskItem Out{get{return eTaskItem.ScanLines;}}
        public override string Name { get { return "Merge Angle"; } }
        public override string DisplayName { get { return "Angle"; } }
        public override eTaskType TaskType { get { return eTaskType.Smooth; } }


        public override AbstractProcessingTask Clone()
        {
            AngleMerge ret = new AngleMerge();
            return ret;
        }
        private double m_Angle = 1.8;
        [Browsable(true)]
        [Description("Merge all angle between angle")]
        [DisplayName("Angle to merge")]
        [TypeConverter(typeof(NumericUpDownTypeConverter))]
        [Editor(typeof(NumericUpDownTypeEditor), typeof(UITypeEditor)), MinMaxAttribute(0.1f, 9f, 0.05f, 2)]
        public double DeltaAngle { get { return m_Angle; } set { m_Angle = value; } }

        public override ScanData DoTask(ScanData source)
        {
            source.Sort();
            ScanData ret = source;
            Dictionary<int, ScanData> laserScanData = new Dictionary<int, ScanData>();
            UpdatePercent(0, ret);
            ret = new ScanData();

            int count = source.Count ;
            double delta_angle = DeltaAngle;
            int step = (int)((count * delta_angle) / 360.0);
            for (double ang = 0; ang < 360; ang += delta_angle)
            {
                int biggestIndex = 0;
                int biggestCount = 0;
                List<ScanLine> lines = GetScanlinesByAngle(ang, ang + delta_angle, source);
                for (int j = 0; j < lines.Count; j++)
                {
                    ScanLine l = lines[j];
                    int cnt = l.Count;
                    if (biggestCount < cnt)
                    {
                        biggestCount = cnt;
                        biggestIndex = j;
                    }
                }
                if(lines.Count>0)
                    ret.Add(AverageLines(biggestCount,biggestIndex, lines));
                UpdatePercent((int)(100 * ang/360), ret);
            }
            UpdatePercent(100, ret);
            return ret;
            
        }
        protected ScanLine AverageLines(int count, int biggestIndex, List<ScanLine> scanlines)
        {
            int numLines = scanlines.Count;
            ScanLine ret = new ScanLine(-1, count);
            for (int i = 0; i < count; i++)
            {
                List<Point3D> list = new List<Point3D>(numLines);
                double refy = scanlines[biggestIndex][i].Position.Y;
                for (int line = 0; line < numLines; line++)
                {
                    list.Add(scanlines[line].GetInterpolateByY(refy, i == 0));
                }
                ret.Add(Point3D.Average(list));
            }
            return ret;
        }
        List<ScanLine> GetScanlinesByAngle(double from, double to,ScanData data)
        {
            List<ScanLine> ret = new List<ScanLine>();
            data.Sort();
            int startindex = -1;
            int cnt = data.Count;/*
            for (int i = 0; i < cnt && startindex == -1; i++) // find first of range
            {
                ScanLine line = data[i];
                double angle = line.Angle;
                while (angle < 0)
                    angle += 360;
                if (angle >= from && angle <= to)
                    startindex = i;
            }
            if (startindex < 0)
                return ret;*/
            startindex =0;
            for (int i = 0; i < cnt; i++) // fill the range
            {
                ScanLine line = data[(i + startindex)%cnt ];
                double angle = line.Angle;
                while (angle < 0)
                    angle += 360;
                if (angle >= from && angle <= to)
                    ret.Add(line);
                if (angle > to)
                    return ret;
            }
            return ret;
        }

    }
}
