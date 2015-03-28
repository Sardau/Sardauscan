using OpenTK;
using Sardauscan.Core.Geometry;
using Sardauscan.Gui.PropertyGridEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;

namespace Sardauscan.Core.ProcessingTask
{
    public class TextureTask : AbstractLineTask
    {
#if DEBUG
        protected override bool LaunchParallel
        {
            get
            {
                return false;
            }
        }
#endif
        private double m_Brigthness = 0;
        [Browsable(true)]
        [Description("Brigthness")]
        [DisplayName("Brigthness")]
        [TypeConverter(typeof(NumericUpDownTypeConverter))]
        [Editor(typeof(NumericUpDownTypeEditor), typeof(UITypeEditor)), MinMaxAttribute(-128f, 128f, 5f, 0)]
        public double Brigthness { get { return m_Brigthness; } set { m_Brigthness = value; } }

        private double m_Contrast = 0;
        [Browsable(true)]
        [Description("Contrast")]
        [DisplayName("Contrast")]
        [TypeConverter(typeof(NumericUpDownTypeConverter))]
        [Editor(typeof(NumericUpDownTypeEditor), typeof(UITypeEditor)), MinMaxAttribute(-128f, 128f, 5f, 0)]
        public double Contrast { get { return m_Contrast; } set { m_Contrast = value; } }

        private double m_Gamma = 0;
        [Browsable(true)]
        [Description("Gamma")]
        [DisplayName("Gamma")]
        [TypeConverter(typeof(NumericUpDownTypeConverter))]
        [Editor(typeof(NumericUpDownTypeEditor), typeof(UITypeEditor)), MinMaxAttribute(0.25f, 2f, 0.05f, 2)]
        public double Gamma { get { return m_Gamma; } set { m_Gamma = value; } }

        public override ScanLine DoTask(ScanLine source)
        {
            int count =source.Count;
            ScanLine ret = new ScanLine(source.LaserID, count);
            for (int i = 0; i < count;i++ )
            {
                Point3D p = source[i];
                Vector3d pos = p.Position;
                Vector3d norm = p.Normal;
                Color col = p.Color;
                double r = col.R;
                double g = col.G;
                double b = col.B;
                if (m_Brigthness != 0)
                {
                    r = r + m_Brigthness;
                    g = g + m_Brigthness;
                    b = b + m_Brigthness;
                }
                if(m_Contrast!=0)
                {
                    double factor = (259 * (m_Contrast + 255)) / (255 * (259 - m_Contrast));
                    r   = factor * (r   - 128) + 128;
                    g   = factor * (g   - 128) + 128;
                    b   = factor * (b   - 128) + 128;
                }
                if(m_Gamma!=0)
                {
                    double gammaCorrection = 1 / m_Gamma;
                    r = 255 * Math.Pow((r / 255), gammaCorrection);
                    g = 255 * Math.Pow((g / 255), gammaCorrection);
                    b = 255 * Math.Pow((b / 255), gammaCorrection);
                }
                ret.Add(new Point3D(pos, norm, Color.FromArgb(255,Truncate(r),Truncate(g),Truncate(b))));
            }
            return ret;
        }
        byte Truncate(double value)
        {
            return (byte)Math.Min(255,Math.Max(0,value));
        }
        public override string Name { get { return "Texture Adjust"; }}
        public override eTaskType TaskType { get { return eTaskType.Color; } }

    }
}
