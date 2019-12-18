using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Discr_graph
{
    class GD2
    {
        public static int offsetX = 0;
        public static int offsetY = 0;
        public static double radiusOut = 300;
        public static int size = 90;
        public static int fontSize = 24;
        public static int weightsfontSize = 22;
        public static float strokeWidth = 4.5f;
        public static Color fillcolor = Color.White;
        public static Color hilightColor = Color.Red;
        public static Color arrowColor = Color.Blue;
        public static Color outlineColor = Color.Blue;
        public static Color namesColor = Color.Blue;
        public static Color weightsColor = Color.Blue;

        public static void DrawGraph(PaintEventArgs e, Graph g, bool weights)
        {
            if (g.Positions == null)
                CalcPositions(g);
            DrawLines(e, g);
            DrawNodes(e, g);
            DrawNames(e, g);
            if (weights)
                DrawWeights(e, g);
        }
        public static void DrawGraph(PaintEventArgs e, Graph g, List<int> inds)
        {
            if (g.Positions == null)
                CalcPositions(g);
            DrawLines(e, g, inds);
            DrawNodes(e, g, inds);
            DrawNames(e, g, inds);
            DrawWeights(e, g, inds);
        }

        public static void DrawCircle(PaintEventArgs e, int size, int x, int y, Color c)
        {
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(c);
            e.Graphics.FillEllipse(myBrush, new Rectangle(x + offsetX - size / 2, y + offsetY - size / 2, size, size));
            myBrush.Dispose();
            //  formGraphics.Dispose();
        }
        public static void DrawLine(PaintEventArgs e, int x1, int y1, int x2, int y2, Color c)
        {
            System.Drawing.Pen myBrush = new Pen(c, strokeWidth);
            e.Graphics.DrawLine(myBrush, x1 + offsetX, y1 + offsetY, x2 + offsetX, y2 + offsetY);
            myBrush.Dispose();
            //  formGraphics.Dispose();
        }
        public static void DrawLine(PaintEventArgs e, Position a, Position b, Color c)
        {
            System.Drawing.Pen myBrush = new Pen(c, strokeWidth);
            e.Graphics.DrawLine(myBrush, (int)a.X + offsetX, (int)a.Y + offsetY, (int)b.X + offsetX, (int)b.Y + offsetY);
            myBrush.Dispose();
            //  formGraphics.Dispose();
        }
        public static void DrawArrow(PaintEventArgs e, Position a, Position b, Color c)
        {
            System.Drawing.Drawing2D.AdjustableArrowCap bigArrow = new System.Drawing.Drawing2D.AdjustableArrowCap(5, 5);

            System.Drawing.Pen myBrush = new Pen(c, strokeWidth);
            myBrush.CustomEndCap = bigArrow;

            //  myBrush.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            Position z = GetArrowEnd(b, a, size / 2);
            e.Graphics.DrawLine(myBrush, (int)a.X + offsetX, (int)a.Y + offsetY, (int)z.X + offsetX, (int)z.Y + offsetY);
            myBrush.Dispose();
        }
        private static Position GetArrowEnd(Position a, Position b, double offset)
        {
            double l = Math.Sqrt((b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y));
            var dirX = (b.X - a.X) / l;
            var dirY = (b.Y - a.Y) / l;
            dirX *= offset;
            dirY *= offset;
            return new Position(dirX + a.X, dirY + a.Y);
        }


        public static void Clear(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Transparent);
        }
        public static void CalcPositions(Graph g)
        {
            Position[] Npositions = new Position[g.Count];
            int k = 0;
            for (double i = 0; i < 360; i += (double)(360 / (double)g.Count))
            {
                double heading = i * 3.1415926535897932384626433832795 / 180;
                if (k < g.Count)
                    Npositions[k] = new Position(Math.Cos(heading) * radiusOut, Math.Sin(heading) * radiusOut);
                k++;
            }
            g.Positions = Npositions;
        }
        public static void DrawNodes(PaintEventArgs e, Graph g)
        {
            for (int i = 0; i < g.Count; i++)
            {
                DrawCircle(e, size, (int)g.Positions[i].X, (int)g.Positions[i].Y, fillcolor);
                DrawCircleStroke(e, size, (int)g.Positions[i].X, (int)g.Positions[i].Y, outlineColor);
            }
        }
        public static void DrawNodes(PaintEventArgs e, Graph g, List<int> inds)
        {
            Color Col = fillcolor;
            for (int i = 0; i < g.Count; i++)
            {
                if (inds.Contains(i))
                    Col = hilightColor;
                else
                    Col = outlineColor;

                DrawCircle(e, size, (int)g.Positions[i].X, (int)g.Positions[i].Y, fillcolor);
                DrawCircleStroke(e, size, (int)g.Positions[i].X, (int)g.Positions[i].Y, Col);
            }
        }

        public static void DrawNames(PaintEventArgs e, Graph g)
        {
            Font drawFont = new Font("Arial", fontSize);
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(namesColor);
            for (int i = 0; i < g.Count; i++)
            {
                e.Graphics.DrawString(g.names[i], drawFont, myBrush, (float)(g.Positions[i].X + offsetX - (g.names[i].Length * fontSize * 0.6)), (float)(g.Positions[i].Y + offsetY - (fontSize * 0.62)));
            }
            myBrush.Dispose();
        }
        public static void DrawNames(PaintEventArgs e, Graph g, List<int> inds)
        {
            Color Col = fillcolor;
            Font drawFont = new Font("Arial", fontSize);
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(namesColor);
            for (int i = 0; i < g.Count; i++)
            {

                if (inds.Contains(i))
                    Col = hilightColor;
                else
                    Col = namesColor;
                myBrush.Color = Col;
                e.Graphics.DrawString(g.names[i], drawFont, myBrush, (float)(g.Positions[i].X + offsetX - (g.names[i].Length * fontSize * 0.6)), (float)(g.Positions[i].Y + offsetY - (fontSize * 0.62)));
            }
        }
        public static void DrawLines(PaintEventArgs e, Graph g)
        {
            if (!g.isOrGraph)
            {
                DrawLinesNOR(e, g);
            }
            else
            {
                DrawLinesOR(e, g);
            }
        }
        public static void DrawLines(PaintEventArgs e, Graph g, List<int> inds)
        {
            if (!g.isOrGraph)
            {
                DrawLinesNOR(e, g, inds);
            }
            else
            {
                DrawLinesOR(e, g, inds);
            }
        }

        private static void DrawLinesNOR(PaintEventArgs e, Graph g)
        {
            for (int i = 0; i < g.Count; i++)
            {
                for (int j = i + 1; j < g.Count; j++)
                {
                    if (g.Matrix[i, j] != 0)
                        DrawLine(e, g.Positions[i], g.Positions[j], arrowColor);
                }
            }
        }
        private static void DrawLinesNOR(PaintEventArgs e, Graph g, List<int> inds)
        {
            Color Col = fillcolor;
            for (int i = 0; i < g.Count; i++)
            {

                for (int j = i + 1; j < g.Count; j++)
                {
                    if (inds.Contains(i) && inds.Contains(j))
                        Col = hilightColor;
                    else
                        Col = outlineColor;

                    if (g.Matrix[i, j] != 0)
                        DrawLine(e, g.Positions[i], g.Positions[j], Col);
                }

            }
        }
        private static void DrawLinesOR(PaintEventArgs e, Graph g)
        {
            for (int i = 0; i < g.Count; i++)
            {
                for (int j = 0; j < g.Count; j++)
                {
                    if (g.Matrix[i, j] != 0)
                    {
                        DrawArrow(e, g.Positions[i], g.Positions[j], arrowColor);
                    }
                }
            }
        }
        private static void DrawLinesOR(PaintEventArgs e, Graph g, List<int> inds)
        {
            Color Col = fillcolor;
            for (int i = 0; i < g.Count; i++)
            {
                {
                    for (int j = 0; j < g.Count; j++)
                    {
                        if (inds.Contains(i) && inds.Contains(j))
                            Col = hilightColor;
                        else
                            Col = arrowColor;

                        if (g.Matrix[i, j] != 0)
                            DrawArrow(e, g.Positions[i], g.Positions[j], Col);
                    }
                }

            }
        }

        private static void DrawWeights(PaintEventArgs e, Graph g)
        {
            for (int i = 0; i < g.Count; i++)
            {
                int j = 0;
                if (!g.isOrGraph) j = i;
                for (; j < g.Count; j++)
                {
                    if (g.Matrix[i, j] != 0)
                    {
                        Position wPos = GetArrowEnd(g.Positions[j], g.Positions[i], size / 2 + 45);
                        DrawCircle(e, (int)(size * 0.37), (int)(wPos.X), (int)(wPos.Y), Color.White);
                        DrawString(e, (float)wPos.X + offsetX, (float)wPos.Y + offsetY, g.Matrix[i, j].ToString(), weightsColor);
                    }
                }
            }
        }
        private static void DrawWeights(PaintEventArgs e, Graph g, List<int> inds)
        {
            Color Col = fillcolor;
            for (int i = 0; i < g.Count; i++)
            {


                int j = 0;
                if (!g.isOrGraph) j = i;
                for (; j < g.Count; j++)
                {
                    if (inds.Contains(i) && inds.Contains(j))
                    {
                        if (g.Matrix[i, j] != 0)
                        {
                            Position wPos = GetArrowEnd(g.Positions[j], g.Positions[i], size / 2 + 45);
                            DrawCircle(e, (int)(size * 0.37), (int)(wPos.X), (int)(wPos.Y), Color.White);
                            DrawString(e, (float)wPos.X + offsetX, (float)wPos.Y + offsetY, g.Matrix[i, j].ToString(),hilightColor);
                        }
                    }
                }


            }
        }
        private static void DrawString(PaintEventArgs e, float x, float y, string msg, Color col)
        {
            Font drawFont = new Font("Arial",weightsfontSize);
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(col);
            e.Graphics.DrawString(msg, drawFont, myBrush, x - (float)(msg.Length * (fontSize - 4) * 0.5), y - (float)(fontSize * 0.7));
            myBrush.Dispose();
        }


        public static void DrawCircleStroke(PaintEventArgs e, int size, int x, int y, Color c)
        {
            System.Drawing.Pen myBrush = new Pen(c, strokeWidth);
            // System.Drawing.Graphics formGraphics;
            // formGraphics = form.CreateGraphics();
            e.Graphics.DrawEllipse(myBrush, new Rectangle(x + offsetX - size / 2, y + offsetY - size / 2, size, size));
            myBrush.Dispose();
            //  formGraphics.Dispose();
        }

    }
}
