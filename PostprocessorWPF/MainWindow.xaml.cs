
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using PostprocessorWPF.Backend;
using SharpGL;
using SharpGL.SceneGraph;

namespace PostprocessorWPF
{
    public partial class MainWindow
    {
        private readonly Builder _builder = new Builder();
        private readonly List<Button> _buttons = new List<Button>();
        //private readonly GLControl _glControl = new GLControl(GraphicsMode.Default);
        private bool _loaded;

        public MainWindow()
        {
            InitializeComponent();

            //GLControl glControl
            //_glControl.Load += GlControl_Load;
            //_glControl.Paint += GlControl_Paint;

            //gridPanel.Children.Add(_glControl);
        }

        private void OpenGLControl_OpenGLInitialized(object sender, OpenGLEventArgs args)
        {
            var gl = args.OpenGL;
            gl.ClearColor(Globals.BackColor.R, Globals.BackColor.G, Globals.BackColor.B, Globals.BackColor.A);
        }

        private void OpenGLControl_OpenGLDraw(object sender, OpenGLEventArgs args)
        {
            var gl = args.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.Begin(OpenGL.GL_TRIANGLES);
            gl.Color(0f, 1f, 0f);
            gl.Vertex(-1f, -1f);
            gl.Vertex(0f, 1f);
            gl.Vertex(1f, -1f);
            gl.End();
        }

        private void OpenGLControl_Resized(object sender, OpenGLEventArgs args)
        {
        }

        //private void GlControl_Load(object sender, EventArgs e)
        //{
        //    LoadGl();
        //}

        //public void LoadGl()
        //{
        //    GL.ClearColor(Color.Gray);

        //    GL.Enable(EnableCap.DepthTest);
        //    var p = Matrix4.CreatePerspectiveFieldOfView((float)(80 * Math.PI / 180), 1, 0.1F, 350);
        //    GL.MatrixMode(MatrixMode.Projection);
        //    GL.LoadMatrix(ref p);

        //    var modelview = Matrix4.LookAt(3, -3, 3, 0, 0, 0, 0, 0, 1);

        //    GL.MatrixMode(MatrixMode.Modelview);
        //    GL.LoadMatrix(ref modelview);

        //    _loaded = true;
        //    _glControl.VSync = true;


        //    _glControl.Invalidate();
        //}

        //private void GlControl_Paint(object sender, PaintEventArgs e)
        //{
        //    if (!_loaded) return;

        //    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        //    try
        //    {
        //        _builder.Draw();

        //        //оси
        //        GL.Color3(255,255,255);
        //        GL.Begin(BeginMode.Lines);
        //        GL.Vertex3(0, 0, 0);
        //        GL.Vertex3(100, 0, 0);
        //        GL.Vertex3(0, 0, 0);
        //        GL.Vertex3(0, 100, 0);
        //        GL.Vertex3(0, 0, 0);
        //        GL.Vertex3(0, 0, 100);
        //        GL.End();

        //        _glControl.SwapBuffers();
        //    }
        //    catch (Exception)
        //    {
        //        // ignored
        //    }
        //}
    }
}