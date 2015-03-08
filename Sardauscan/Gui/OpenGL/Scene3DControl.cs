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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.IO;
using Sardauscan.Core;
using Sardauscan.Core.Interface;
using Sardauscan.Core.IO;
using Sardauscan.Core.Geometry;
using Sardauscan.Gui.OpenGL;
using Sardauscan.Core.OpenGL;
using Sardauscan.Gui.Forms;



namespace Sardauscan.Gui.OpenGL
{
    public partial class Scene3DControl : OpenTK.GLControl, IScene3DViewer
    {

         #region Properties
        protected bool m_ShowDefaultScene;
        [Browsable(true)]
        [BindableAttribute(true)]
        public bool ShowDefaultScene { get { return m_ShowDefaultScene; } set { m_ShowDefaultScene = value; } }
        protected bool m_ShowFullOnClick;
        [Browsable(true)]
        [BindableAttribute(true)]
        public bool ShowFullOnClick { get { return m_ShowFullOnClick; } set { m_ShowFullOnClick = value; } }

        [Browsable(true)]
        [BindableAttribute(true)]
        public bool ShowSettingsButton { get { return this.SettingsButton.Visible; } set { this.SettingsButton.Visible = value; Invalidate(); } }

        public bool ShowSceneColor { get { return ViewerConfig.ShowSceneColor; } }
        public bool BoundingBox { get { return ViewerConfig.BoundingBox; } }
        public float TableRadius { get { return ViewerConfig.TableRadius; } }
        public float TableHeight { get { return ViewerConfig.TableHeight; } }
        public bool Lightning { get { return ViewerConfig.Lightning; } }
        public bool Smooth { get { return ViewerConfig.Smooth; } }
        public bool Projection { get { return ViewerConfig.Projection; } }
        

        [Browsable(true)]
        [BindableAttribute(true)]
        public GLViewerConfig ViewerConfig;
				public override Color BackColor { get { return SkinInfo.View3DBackColor; } set { base.BackColor = SkinInfo.View3DBackColor; } }
				public override Color ForeColor { get { return SkinInfo.View3DForeColor; } set { base.ForeColor = SkinInfo.View3DForeColor; } }
				 #endregion


        bool loaded = false;
        public DragBallNavigator Drag {get;protected set;}
				/// <summary>
				/// Default ctor
				/// </summary>
				public Scene3DControl()
        {
            InitializeComponent();
            Scene = new Scene3D();
            Drag = new DragBallNavigator(this);
            ViewerConfig = new GLViewerConfig();
            ViewerConfig.LoadDefault();

        }

        #region Scene
				Scene3D _Scene = null;
        public Scene3D Scene
				{
					get{return _Scene;}
					set {
						_Scene = value;
						Refresh();
					}
				}

        #endregion

        private void Object3DView_Load(object sender, EventArgs e)
        {

					loaded = !this.IsDesignMode();
            SetupViewport();
						if (!this.IsDesignMode())
            {
                if (ShowDefaultScene)
                    LoadDefaultScene();
            }
        }


        private void SetupViewport()
        {
            if (!loaded)
                return;
            int w = Width;
            int h = Height;
            GL.Viewport(0, 0, w, h);
            GL.Enable(EnableCap.DepthTest);
            GL.LoadIdentity();
            GL.ClearColor(BackColor);
						GL.DepthFunc(DepthFunction.Less);
            GL.End();
						//GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);
						//GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
	

        }
        public static Matrix4 MatrixFromYawPitchRoll(double yaw, double pitch, double roll)
        {
            return Matrix4.CreateFromQuaternion(QuaternionFromYawPitchRoll(yaw, pitch, roll));
        }

        public static Quaternion QuaternionFromYawPitchRoll(double yaw, double pitch, double roll)
        {
            Quaternion result = Quaternion.Identity;
            float num9 = (float)roll * 0.5f;
            float num6 = (float)Math.Sin((double)num9);
            float num5 = (float)Math.Cos((double)num9);
            float num8 = (float)pitch * 0.5f;
            float num4 = (float)Math.Sin((double)num8);
            float num3 = (float)Math.Cos((double)num8);
            float num7 = (float)yaw * 0.5f;
            float num2 = (float)Math.Sin((double)num7);
            float num = (float)Math.Cos((double)num7);
            result.X = ((num * num4) * num5) + ((num2 * num3) * num6);
            result.Y = ((num2 * num3) * num5) - ((num * num4) * num6);
            result.Z = ((num * num3) * num6) - ((num2 * num4) * num5);
            result.W = ((num * num3) * num5) + ((num2 * num4) * num6);
            return result;
        }
        private void Render(object sender, PaintEventArgs e)
        {
        
            
            if (!loaded)
                return;
            MakeCurrent();
						SetupViewport();
            //float size = Scene.Size();

            float size = Scene.Max.Y - Scene.Min.Y;

            float SceneYOffset = -Scene.Min.Y - size / 2f;

            float defaultzoom = (float)Math.Sqrt(Math.Pow(TableRadius , 2) + Math.Pow(TableHeight, 2));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();
            var aspect_ratio = Width / (float)Height;
            var projection = Matrix4.CreatePerspectiveFieldOfView(
                          MathHelper.PiOver4, float.Parse(aspect_ratio.ToString()), 1f, 1024);

             
            
            //            var modelview = Matrix4.LookAt(0, size / 2 , - DragZoom * defaultzoom * 2f,
//                                            0,TableHeight/2, 0,  0, size, 0);
            var h = size / 2f ;
            var modelview = Matrix4.LookAt(0, h, Math.Max(0.00000001f, -(- Drag.Zoom * defaultzoom * 2)),
                                            0, h, 0, 0, size, 0);

            GL.MatrixMode(MatrixMode.Projection);
            if (Projection)
                GL.LoadMatrix(ref projection);
            else
            {
                GL.LoadIdentity();
                float x = Math.Max(TableRadius*2,TableHeight) * Drag.Zoom;
                float y = x * ((float)Height / (float)Width);
                GL.Ortho(-x,x, -y, y, 0.0, 10000.0);
            }
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);

            #region LIGHT
            if (Lightning)
            {
                float[] lightPos = new float[] { -TableRadius * 2f, TableRadius * 2f, TableHeight * 2f, 1.0f };
                GL.PointSize(5);
                GL.Disable(EnableCap.Lighting);
                GL.Begin(PrimitiveType.Points);
                GL.Color3(Color.Yellow);
                GL.Vertex3(lightPos);
                GL.End();
                GL.PointSize(1);

                float[] light_ambient = { 0.2f, 0.2f, 0.2f, 1.0f };
                float[] light_diffuse = { 1.0f, 1.0f, 1.0f, 1.0f };
                float[] light_specular = { 1.0f, 1.0f, 1.0f, 1.0f };
                float[] spotdirection = { 0.0f, 0.0f, -1.0f };

                GL.Light(LightName.Light0, LightParameter.Ambient, light_ambient);
                GL.Light(LightName.Light0, LightParameter.Diffuse, light_diffuse);
                GL.Light(LightName.Light0, LightParameter.Specular, light_specular);

                GL.Enable(EnableCap.Lighting);
                GL.Light(LightName.Light0, LightParameter.Position, lightPos);
                GL.Enable(EnableCap.Light0);

            }
            #endregion

           //GL.Scale(DragZoom, DragZoom, DragZoom);
            GL.Translate(Drag.Pane.X, -Drag.Pane.Y +size/2f, 0);
            Matrix4 m = MatrixFromYawPitchRoll(-Math.PI * Drag.Angle.X / 360.0f, -Math.PI * Drag.Angle.Y / 360.0f, 0);
            GL.MultMatrix(ref m);
            // Model space
            #region Box/Axis
            GL.Disable(EnableCap.Lighting);
            Vector3 scannerCenter = new Vector3(0, -size/2f, 0);
            DrawScannerBox(ForeColor, scannerCenter);
            Axis(new Vector3(0, 0 , 0), 10);
            #endregion;

            GL.Translate(0, SceneYOffset, 0);
 
            if (BoundingBox)
                DrawBoundingBox(Color.Red,Scene.Min, Scene.Max);
            if (Lightning)
            {
                GL.Enable(EnableCap.Lighting);
                GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);
                GL.Enable( EnableCap.ColorMaterial);
            }


						GL.PointSize(2);
						GL.LineWidth(2);
						GL.PolygonOffset(10, 10);

            RenderingContext context = RenderingContext.From(ViewerConfig);
            context.ApplyFaceDefault();
            GL.ShadeModel(Smooth ? ShadingModel.Smooth : ShadingModel.Flat);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);
            Scene.Render(ref context);
            SwapBuffers();
        }
        #region Draw
        public static void Axis(Vector3 center, float size)
        {
            GL.LineWidth(2f);
            GL.Translate(center.X, center.Y, center.Z);
            GL.Begin(PrimitiveType.Lines);

            GL.Color3(1f, 0f, 0f);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(size, 0f, 0f);

            GL.Color3(0f, 1f, 0f);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(0f, size, 0f);

            GL.Color3(0f, 0f, 1f);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(0f, 0f, size);

            GL.End();
            GL.Translate(-center.X, -center.Y, -center.Z);
            GL.LineWidth(1f);
        }
        public static void DrawBoundingBox(Color color, Vector3 min, Vector3 max)
        {
            Vector3 pos = min;
            Vector3 size = new Vector3(max.X - min.X, max.Y - min.Y, max.Z - min.Z);
            WireCube(color, pos, size);
        }
        public void DrawScannerBox(Color color, Vector3 center)
        {
            Vector3 size = new Vector3(TableRadius * 2f, TableHeight, TableRadius * 2f);
            Vector3 pos = new Vector3(center.X - size.X / 2f, center.Y, center.Z - size.Z / 2f);
            WireCube(color, pos, size);
            DrawFloor(color, pos , size);
        }
        public static void WireCube(Color color, Vector3 position,Vector3 size)
        {
            Vector3 s = new Vector3(size);
            Vector3 p = new Vector3(position);

            GL.Color3(color);
            // Bottom
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex3(p.X    , p.Y    ,p.Z    );
            GL.Vertex3(p.X+s.X, p.Y    ,p.Z    );
            GL.Vertex3(p.X+s.X, p.Y+s.Y,p.Z    );
            GL.Vertex3(p.X    , p.Y+s.Y,p.Z    );
            GL.End();

            // Top
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex3(p.X    , p.Y    ,p.Z+s.Z);
            GL.Vertex3(p.X+s.X, p.Y    ,p.Z+s.Z);
            GL.Vertex3(p.X+s.X, p.Y+s.Y,p.Z+s.Z);
            GL.Vertex3(p.X    , p.Y+s.Y,p.Z + s.Z);
            GL.End();

            // Vertical
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(p.X    , p.Y    ,p.Z    );
            GL.Vertex3(p.X    , p.Y    ,p.Z+s.Z);
            GL.Vertex3(p.X+s.X, p.Y    ,p.Z    );
            GL.Vertex3(p.X+s.X, p.Y    ,p.Z+s.Z);
            GL.Vertex3(p.X+s.X, p.Y+s.Y,p.Z    );
            GL.Vertex3(p.X+s.X, p.Y+s.Y,p.Z+s.Z);
            GL.Vertex3(p.X    , p.Y+s.Y,p.Z    );
            GL.Vertex3(p.X    , p.Y+s.Y,p.Z+s.Z);
            GL.End();
        }

        public static void DrawFloor(Color color, Vector3 position , Vector3 size, int iter = -1)
        {
            float iterationsX = iter > 0 ? iter : size.X / 10f;
            float iterationsZ = iter > 0 ? iter : size.X / 10f;
            Vector3 p = position;
            GL.Color3(color);

            float deltaX = size.X / iterationsX;
            float deltaZ = size.Z / iterationsZ;

            GL.Begin(PrimitiveType.Lines);
            for (var x = 0; x < iterationsX; x++)
            {
                GL.Vertex3(p.X + x * deltaX, p.Y, p.Z);
                GL.Vertex3(p.X + x * deltaX, p.Y, p.Z + size.Z);
            }
            for (var z = 0; z < iterationsZ; z++)
            {
                GL.Vertex3(p.X, p.Y, p.Z + z * deltaZ);
                GL.Vertex3(p.X+size.X, p.Y, p.Z + z * deltaZ);
            }

            GL.End();
        }
        protected void DrawScanner()
        {
            int step = 1;
            int len = (int)(360 / step);
            Vector3[] v = new Vector3[len];
            float tr = TableRadius;
            float h = -TableHeight;
            for (int ang = 0; ang < 360; ang += step)
            {
                float rad = Utils.DEGREES_TO_RADIANS(ang);
                float x = (float)Math.Sin(rad);
                float y = (float)Math.Cos(rad);
                v[ang] = new Vector3(x, y, 0);
            }
            GL.Enable(EnableCap.Lighting);
            GL.Color3(this.BackColor);
            GL.Begin(PrimitiveType.Polygon);
            for (int i = 0; i < len; i++)
            {
                GL.Normal3(0, 0, -1);
                GL.Vertex3(v[i].X * tr, v[i].Y * tr, 0);
            }
            GL.End();

            GL.Begin(PrimitiveType.Polygon);
            for (int i = 0; i < len; i++)
            {
                GL.Normal3(0, 0, 1);
                GL.Vertex3(v[i].X * tr, v[i].Y * tr, h);
            }
            GL.End();

            GL.Begin(PrimitiveType.TriangleStrip);
            for (int i = 0; i < len; i++)
            {
                GL.Normal3(v[i].X, v[i].Y, 0);
                GL.Vertex3(v[i].X * tr, v[i].Y * tr, 0);
                GL.Vertex3(v[i].X * tr, v[i].Y * tr, h);
            }
            GL.Normal3(v[0].X, v[0].Y, 0);
            GL.Vertex3(v[0].X * tr, v[0].Y * tr, 0);
            GL.Vertex3(v[0].X * tr, v[0].Y * tr, h);
            GL.End();
            GL.ClearDepth(10000000);
        }

        #endregion

        private void LoadDefaultScene()
        {
					string path = Path.Combine(Program.UserDataPath, "startup" + ScanDataIO.DefaultExtention);
            if (File.Exists(path))
            {
                ScanData Points = ScanDataIO.Read(path);

                Scene.Clear();
                Scene.Add(Points);
                Invalidate();
            }
        }

        protected void InvalidateAll()
        {
            if(loaded)
                Invalidate();
        }


        #region DRAG
        /*
        private void glSurface_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            System.Windows.Forms.MouseEventArgs ev = (e as System.Windows.Forms.MouseEventArgs);
            Point pt =new Point(ev.X, ev.Y);
            if(ev.Button == MouseButtons.Left)
                StartDrag(eDragMode.Rotation, pt);
            else if(ev.Button == System.Windows.Forms.MouseButtons.Right)
                StartDrag(eDragMode.Zoom, pt);
            else if (ev.Button == System.Windows.Forms.MouseButtons.Middle)
                StartDrag(eDragMode.Pane, pt);
            else
            {
                EndDrag();
                InvalidateAll();
            }

        }
        */
        private void glSurface_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            System.Windows.Forms.MouseEventArgs ev = (e as System.Windows.Forms.MouseEventArgs);
            if (!Drag.IsDragging && ev.Button == MouseButtons.Left)
            {
                if (ShowFullOnClick)
                {
                    ShowFull3dObjectForm dlg = new ShowFull3dObjectForm();
                    dlg.View.Drag.Copy(this.Drag);
                    dlg.View.Scene = this.Scene;
                    dlg.ViewerConfig = this.ViewerConfig;
                    dlg.ShowDialog();
                }
            }

        }
        /*
        private void glSurface_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            System.Windows.Forms.MouseEventArgs ev = (e as System.Windows.Forms.MouseEventArgs);
            MoveDrag(new Point(ev.X, ev.Y));
            InvalidateAll();

        }
         * */
        #endregion


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Drag.Init();
            InvalidateAll();
        }

        private void Scene3DView_Resize(object sender, EventArgs e)
        {
               SetupViewport();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.Scene = new Scene3D();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            GLViewerConfigForm dlg = new GLViewerConfigForm();
            Point p = HomeButton.PointToScreen(new Point(HomeButton.Width,0));
            dlg.Location = p;
            dlg.Config = this.ViewerConfig;
						if (dlg.ShowDialog()== DialogResult.OK)
            {
                this.ViewerConfig = dlg.Config;
                this.ViewerConfig.SaveDefault();
                Refresh();
            }

        }
    }
}
