using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace Discr_graph
{
    class GD
    {
        public static GLControl ViewPort1;
        public static double radiusOut = 0.8f;
        public static double radius = 0.1f;
        public static void DrawNodes(Graph g)
        {
            DrawBegin();
            if (!g.fixedPositions)
            {
                CalcPositions(g);
                DrawNodes(g.Positions);

            }
            else
            {
                DrawNodes(g.Positions);
            }
            DrawEnd();
        }
        public static void DrawNodes(Position[] ps)
        {
            for (int i = 0; i < ps.Length; i++) 
            {
                DrawCircle(ps[i].X, ps[i].Y, radius, Color.White);
                DrawCircleStroke(ps[i].X, ps[i].Y, radius, Color.Black);
                
            }
        }
        public static void CalcPositions(Graph g)
        {
            Position[] Npositions = new Position[g.Count];
            int k = 0;
            for (int i = 0; i <= 360; i += 360 / g.Count)
            {
                double heading = i * 3.1415926535897932384626433832795 / 180;
                Npositions[k].X = Math.Cos(heading) * radius;
                Npositions[k].Y = Math.Sin(heading) * radius;
            }
            g.Positions = Npositions;
        }
        public static void DrawCircle(double x, double y, double radius, Color c)
        {
            int sides = 20;

            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color3(c);
            GL.Vertex2(x, y);
            for (int i = 0; i <= 360; i += 360 / sides)
            {
                double heading = i * 3.1415926535897932384626433832795 / 180;
                GL.Vertex2(x + Math.Cos(heading) * radius, y + Math.Sin(heading) * radius);
            }
            GL.End();
        }

        public static void DrawCircleStroke(double x, double y, double radius, Color c)
        {
            int sides = 20;

            GL.LineWidth(2.7f);
            GL.Color3(c);
            GL.Begin(PrimitiveType.LineLoop);

            // GL.Vertex2(x, y);

            for (int i = 0; i <= 360; i += 360 / sides)
            {
                double heading = i * 3.1415926535897932384626433832795 / 180;
                GL.Vertex2(x + Math.Cos(heading) * radius, y + Math.Sin(heading) * radius);
            }
            GL.End();
        }
        public static void DrawBegin()
        {
            GL.ClearColor(Color.White);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Viewport(0, 0, ViewPort1.Width, ViewPort1.Width);
        }
        public static void DrawEnd()
        {
            ViewPort1.SwapBuffers();
        }


        public static void DrawNCircles(int n, Color c)
        {
            for (int i = 0; i < n; i++)
            {
                for (int a = 0; a < 360; a += 360 / n)
                {
                    double heading = a * 3.1415926535897932384626433832795 / 180;
                    DrawCircle(Math.Cos(heading) * radiusOut, Math.Sin(heading) * radiusOut, 0.1f, c);
                }
            }
        }
        public static void DrawNCirclesStroke(int n, Color c)
        {
            for (int i = 0; i < n; i++)
            {
                for (int a = 0; a < 360; a += 360 / n)
                {
                    double heading = a * 3.1415926535897932384626433832795 / 180;
                    DrawCircleStroke(Math.Cos(heading) * radiusOut, Math.Sin(heading) * radiusOut, 0.1f, c);
                }
            }
        }


    }
}
