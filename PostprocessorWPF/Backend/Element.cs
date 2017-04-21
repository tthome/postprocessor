using OpenTK.Graphics.OpenGL;

namespace PostprocessorWPF.Backend
{
    public enum Mode
    {
        Solid,
        Wireframe,
        Transparent
    }

    internal class Element
    {
        public Element()
        {
            Points = new Point3D[8];
        }

        public int Id { get; set; }
        public Mode Mode { get; set; }
        public Point3D[] Points { get; set; }
        public Sector Sector { get; set; }

        public void Draw()
        {
            if (Mode == Mode.Solid)
            {
                /*bottom*/
                GL.Color3(2, 2, 2);
                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex3(Points[0].X, Points[0].Y, Points[0].Z);
                GL.Vertex3(Points[2].X, Points[2].Y, Points[2].Z);
                GL.Vertex3(Points[3].X, Points[3].Y, Points[3].Z);
                GL.Vertex3(Points[1].X, Points[1].Y, Points[1].Z);
                GL.End();

                /*left*/
                GL.Color3(38, 94, 62);
                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex3(Points[0].X, Points[0].Y, Points[0].Z);
                GL.Vertex3(Points[4].X, Points[4].Y, Points[4].Z);
                GL.Vertex3(Points[6].X, Points[6].Y, Points[6].Z);
                GL.Vertex3(Points[2].X, Points[2].Y, Points[2].Z);
                GL.End();

                /*right*/
                GL.Color3(8, 8, 38);
                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex3(Points[1].X, Points[1].Y, Points[1].Z);
                GL.Vertex3(Points[3].X, Points[3].Y, Points[3].Z);
                GL.Vertex3(Points[7].X, Points[7].Y, Points[7].Z);
                GL.Vertex3(Points[5].X, Points[5].Y, Points[5].Z);
                GL.End();

                /*top*/
                GL.Color3(220, 35, 5);
                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex3(Points[4].X, Points[4].Y, Points[4].Z);
                GL.Vertex3(Points[5].X, Points[5].Y, Points[5].Z);
                GL.Vertex3(Points[7].X, Points[7].Y, Points[7].Z);
                GL.Vertex3(Points[6].X, Points[6].Y, Points[6].Z);
                GL.End();

                /*front*/
                GL.Color3(100, 4, 200);
                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex3(Points[0].X, Points[0].Y, Points[0].Z);
                GL.Vertex3(Points[1].X, Points[1].Y, Points[1].Z);
                GL.Vertex3(Points[5].X, Points[5].Y, Points[5].Z);
                GL.Vertex3(Points[4].X, Points[4].Y, Points[4].Z);
                GL.End();

                /*back*/
                GL.Color3(23, 109, 1);
                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex3(Points[3].X, Points[3].Y, Points[3].Z);
                GL.Vertex3(Points[2].X, Points[2].Y, Points[2].Z);
                GL.Vertex3(Points[6].X, Points[6].Y, Points[6].Z);
                GL.Vertex3(Points[7].X, Points[7].Y, Points[7].Z);
                GL.End();

                /*ребра*/
                /*bottom*/
                GL.Color3(24, 24, 24);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(Points[0].X, Points[0].Y, Points[0].Z);
                GL.Vertex3(Points[2].X, Points[2].Y, Points[2].Z);
                GL.Vertex3(Points[3].X, Points[3].Y, Points[3].Z);
                GL.Vertex3(Points[1].X, Points[1].Y, Points[1].Z);
                GL.End();

                /*left*/
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(Points[0].X, Points[0].Y, Points[0].Z);
                GL.Vertex3(Points[4].X, Points[4].Y, Points[4].Z);
                GL.Vertex3(Points[6].X, Points[6].Y, Points[6].Z);
                GL.Vertex3(Points[2].X, Points[2].Y, Points[2].Z);
                GL.End();

                /*right*/
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(Points[1].X, Points[1].Y, Points[1].Z);
                GL.Vertex3(Points[3].X, Points[3].Y, Points[3].Z);
                GL.Vertex3(Points[7].X, Points[7].Y, Points[7].Z);
                GL.Vertex3(Points[5].X, Points[5].Y, Points[5].Z);
                GL.End();

                /*top*/
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(Points[4].X, Points[4].Y, Points[4].Z);
                GL.Vertex3(Points[5].X, Points[5].Y, Points[5].Z);
                GL.Vertex3(Points[7].X, Points[7].Y, Points[7].Z);
                GL.Vertex3(Points[6].X, Points[6].Y, Points[6].Z);
                GL.End();

                /*front*/
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(Points[0].X, Points[0].Y, Points[0].Z);
                GL.Vertex3(Points[1].X, Points[1].Y, Points[1].Z);
                GL.Vertex3(Points[5].X, Points[5].Y, Points[5].Z);
                GL.Vertex3(Points[4].X, Points[4].Y, Points[4].Z);
                GL.End();

                /*back*/
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(Points[3].X, Points[3].Y, Points[3].Z);
                GL.Vertex3(Points[2].X, Points[2].Y, Points[2].Z);
                GL.Vertex3(Points[6].X, Points[6].Y, Points[6].Z);
                GL.Vertex3(Points[7].X, Points[7].Y, Points[7].Z);
                GL.End();
            }

            if (Mode == Mode.Wireframe)
            {
                /*ребра*/
                /*bottom*/
                GL.Color3(200, 220, 220);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(Points[0].X, Points[0].Y, Points[0].Z);
                GL.Vertex3(Points[2].X, Points[2].Y, Points[2].Z);
                GL.Vertex3(Points[3].X, Points[3].Y, Points[3].Z);
                GL.Vertex3(Points[1].X, Points[1].Y, Points[1].Z);
                GL.End();

                /*left*/
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(Points[0].X, Points[0].Y, Points[0].Z);
                GL.Vertex3(Points[4].X, Points[4].Y, Points[4].Z);
                GL.Vertex3(Points[6].X, Points[6].Y, Points[6].Z);
                GL.Vertex3(Points[2].X, Points[2].Y, Points[2].Z);
                GL.End();

                /*right*/
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(Points[1].X, Points[1].Y, Points[1].Z);
                GL.Vertex3(Points[3].X, Points[3].Y, Points[3].Z);
                GL.Vertex3(Points[7].X, Points[7].Y, Points[7].Z);
                GL.Vertex3(Points[5].X, Points[5].Y, Points[5].Z);
                GL.End();

                /*top*/
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(Points[4].X, Points[4].Y, Points[4].Z);
                GL.Vertex3(Points[5].X, Points[5].Y, Points[5].Z);
                GL.Vertex3(Points[7].X, Points[7].Y, Points[7].Z);
                GL.Vertex3(Points[6].X, Points[6].Y, Points[6].Z);
                GL.End();

                /*front*/
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(Points[0].X, Points[0].Y, Points[0].Z);
                GL.Vertex3(Points[1].X, Points[1].Y, Points[1].Z);
                GL.Vertex3(Points[5].X, Points[5].Y, Points[5].Z);
                GL.Vertex3(Points[4].X, Points[4].Y, Points[4].Z);
                GL.End();

                /*back*/
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(Points[3].X, Points[3].Y, Points[3].Z);
                GL.Vertex3(Points[2].X, Points[2].Y, Points[2].Z);
                GL.Vertex3(Points[6].X, Points[6].Y, Points[6].Z);
                GL.Vertex3(Points[7].X, Points[7].Y, Points[7].Z);
                GL.End();
            }
        }
    }
}