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
        public static int startAngle = 0;
        public static int offsetX = 0;                  // Координата X центра грава на форме (координата Х picturebox-а)
        public static int offsetY = 0;                  // Координата Y центра грава на форме (координата Х picturebox-а)
        public static double radiusOut = 300;           // Радиус расположения вершин
        public static int size = 90;               //90     // Размер вершин
        public static int levelsOffset = 25;       // расстояние между уровнями ярусной формы
        public static int elementInLevelsOffset = 95;
        public static int fontSize = 24;                // Размер шрифта вершин
        public static int weightsfontSize = 22;         // Размер шрифта весов
        public static float strokeWidth = 4.5f;         // Ширина обводки вершин и линий
        public static Color fillcolor = Color.White;    // Цвет заливки вершин
        public static Color hilightColor = Color.Red;   // Цвет выделения
        public static Color arrowColor = Color.Blue;    // Цвет связей
        public static Color outlineColor = Color.Blue;  // Цвет обводки вершин
        public static Color namesColor = Color.Blue;    // Цвет названий вершин
        public static Color weightsColor = Color.Blue;  // Цвет весов
        public static Color[] coresColors = { Color.Magenta, Color.Cyan, Color.LightGreen, Color.Purple, Color.DarkOrange, Color.Chocolate, Color.Brown, Color.Crimson, Color.Firebrick, Color.Maroon }; // Цвета для нескольких ядер

        public static void SetStartAngle(int n)
        {
            startAngle = n;
        }

        public static void DrawGraph(PaintEventArgs e, Graph g, bool weights) // Нарисовать весь граф без выделения weights - рисовать веса или нет
        {
            if (g.Positions == null)
                CalcPositions(g);
            DrawLines(e, g);
            DrawNodes(e, g);
            DrawNames(e, g);
            if (weights)
                DrawWeights(e, g);
        }

        public static void DrawGraph(PaintEventArgs e, Graph g, List<int> inds, bool path) // Нарисовать граф. Вершины из списка выделяются как подграф если path = false или как путь если path = true
        {
            if (g.Positions == null)
                CalcPositions(g);
            DrawLines(e, g, inds, path);
            DrawNodes(e, g, inds);
            DrawNames(e, g, inds);
            if (path)
            {
                if (!g.isOrGraph)
                    DrawWeightsPathNOR(e, g, inds);
                else
                    DrawWeightsPathOR(e, g, inds);
            }
            else
                DrawWeights(e, g, inds);
        }

        public static void DrawGraph(PaintEventArgs e, Graph g, List<List<int>> cores)
        {
            if (g.Positions == null)
                CalcPositions(g);

            DrawLines(e, g, cores);
            DrawNodes(e, g, cores);
            DrawNames(e, g, cores);

        }

        public static void DrawGraph(PaintEventArgs e, Graph g, List<ValueTuple<int, int>> edges, bool weights) // Нарисовать граф и выделить указанные ребра
        {
            if (g.Positions == null)
                CalcPositions(g);
            DrawLines(e, g, edges);
            DrawNodes(e, g, edges);
            DrawNames(e, g, edges);
            if (weights)
                DrawWeights(e, g);
        }
        public static void DrawGraphParallel(PaintEventArgs e, Graph g, List<List<int>> levels, bool weights) // Нарисовать граф и выделить указанные ребра
        {

            CalcPositions(g, levels);
            DrawLines(e, g);
            DrawNodes(e, g);
            DrawNames(e, g);
            if (weights)
                DrawWeights(e, g);
            g.TruncPos();
        }
        private static Color Getcolor(int ind)
        {
            if (ind >= coresColors.Length) ind %= coresColors.Length;
            return coresColors[ind];
        }
        private static void DrawCircle(PaintEventArgs e, int size, int x, int y, Color c)
        {
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(c);
            e.Graphics.FillEllipse(myBrush, new Rectangle(x + offsetX - size / 2, y + offsetY - size / 2, size, size));
            myBrush.Dispose();
            //  formGraphics.Dispose();
        }
        private static void DrawLine(PaintEventArgs e, int x1, int y1, int x2, int y2, Color c)
        {
            System.Drawing.Pen myBrush = new Pen(c, strokeWidth);
            e.Graphics.DrawLine(myBrush, x1 + offsetX, y1 + offsetY, x2 + offsetX, y2 + offsetY);
            myBrush.Dispose();
            //  formGraphics.Dispose();
        }
        private static void DrawLine(PaintEventArgs e, Position a, Position b, Color c)
        {
            System.Drawing.Pen myBrush = new Pen(c, strokeWidth);
            e.Graphics.DrawLine(myBrush, (int)a.X + offsetX, (int)a.Y + offsetY, (int)b.X + offsetX, (int)b.Y + offsetY);
            myBrush.Dispose();
            //  formGraphics.Dispose();
        }
        private static void DrawArrow(PaintEventArgs e, Position a, Position b, Color c)
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
        private static void CalcPositions(Graph g)
        {
            Position[] Npositions = new Position[g.Count];
            int k = 0;
            for (double i = 0; i < 360; i += (double)(360 / (double)g.Count))
            {
                double heading = ((i + startAngle) * 3.1415926535897932384626433832795 / 180);
                if (k < g.Count)
                    Npositions[k] = new Position(Math.Cos(heading) * radiusOut, Math.Sin(heading) * radiusOut);
                k++;
            }
            g.Positions = Npositions;
        }
        private static void CalcPositions(Graph g, List<List<int>> levels)
        {
            Position[] Npositions = new Position[g.Count];
            int k = 0;
            int curX = 0;
            int curY = size / 2 + 15 - offsetY;
            for (int i = 0; i < levels.Count; i++)
            {
                curX = (offsetX - (((levels[i].Count * size) + (levels[i].Count - 1 * elementInLevelsOffset))) - offsetX);
                for (int j = 0; j < levels[i].Count; j++)
                {
                    Npositions[levels[i][j] - 1] = new Position(curX, curY);
                    curX += elementInLevelsOffset + size;
                }
                curY += levelsOffset + size;
            }
            g.Positions = Npositions;
        }
        private static void DrawNodes(PaintEventArgs e, Graph g)
        {
            for (int i = 0; i < g.Count; i++)
            {
                DrawCircle(e, size, (int)g.Positions[i].X, (int)g.Positions[i].Y, fillcolor);
                DrawCircleStroke(e, size, (int)g.Positions[i].X, (int)g.Positions[i].Y, outlineColor);
            }
        }
        private static void DrawNodes(PaintEventArgs e, Graph g, List<int> inds)
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
        private static void DrawNodes(PaintEventArgs e, Graph g, List<int> inds, Color Col)
        {

            for (int i = 0; i < g.Count; i++)
            {
                if (inds.Contains(i))
                {
                    DrawCircle(e, size, (int)g.Positions[i].X, (int)g.Positions[i].Y, fillcolor);
                    DrawCircleStroke(e, size, (int)g.Positions[i].X, (int)g.Positions[i].Y, Col);
                }
            }
        }
        private static void DrawNodes(PaintEventArgs e, Graph g, List<ValueTuple<int, int>> edges)
        {
            Color Col = fillcolor;
            for (int i = 0; i < g.Count; i++)
            {
                foreach (var inds in edges)
                {
                    if (inds.Item1 == i || inds.Item2 == i)
                    {
                        Col = hilightColor;
                        break;
                    }
                    else
                        Col = outlineColor;
                }
                DrawCircle(e, size, (int)g.Positions[i].X, (int)g.Positions[i].Y, fillcolor);
                DrawCircleStroke(e, size, (int)g.Positions[i].X, (int)g.Positions[i].Y, Col);
            }
        }
        private static void DrawNodes(PaintEventArgs e, Graph g, List<List<int>> cores)
        {
            DrawNodes(e, g);
            Color Col = Color.Red;
            for (int i = 0; i < cores.Count; i++)
            {
                Col = Getcolor(i);
                DrawNodes(e, g, cores[i], Col);
                // Col = Color.FromArgb(Col.R - (255 / cores.Count), Col.G + (255 / cores.Count), Col.B + (255 / cores.Count));
            }
        }

        private static void DrawNames(PaintEventArgs e, Graph g)
        {
            Font drawFont = new Font("Arial", fontSize);
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(namesColor);
            for (int i = 0; i < g.Count; i++)
            {
                e.Graphics.DrawString(g.names[i], drawFont, myBrush, (float)(g.Positions[i].X + offsetX - (g.names[i].Length * fontSize * 0.6)), (float)(g.Positions[i].Y + offsetY - (fontSize * 0.62)));
            }
            myBrush.Dispose();
        }
        private static void DrawNames(PaintEventArgs e, Graph g, List<int> inds)
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
        private static void DrawNames(PaintEventArgs e, Graph g, List<int> inds, Color Col)
        {
            Font drawFont = new Font("Arial", fontSize);
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(namesColor);
            for (int i = 0; i < g.Count; i++)
            {
                if (inds.Contains(i))
                {
                    myBrush.Color = Col;
                    e.Graphics.DrawString(g.names[i], drawFont, myBrush, (float)(g.Positions[i].X + offsetX - (g.names[i].Length * fontSize * 0.6)), (float)(g.Positions[i].Y + offsetY - (fontSize * 0.62)));
                }
            }
        }
        private static void DrawNames(PaintEventArgs e, Graph g, List<List<int>> cores)
        {
            Color Col = Color.Red;

            DrawNames(e, g);

            for (int i = 0; i < cores.Count; i++)
            {
                Col = Getcolor(i);
                DrawNames(e, g, cores[i], Col);
            }
            // Col = Color.FromArgb(Col.R - (255 / cores.Count), Col.G + (255 / cores.Count), Col.B + (255 / cores.Count));
        }
        private static void DrawNames(PaintEventArgs e, Graph g, List<ValueTuple<int, int>> edges)
        {
            Color Col = fillcolor;
            Font drawFont = new Font("Arial", fontSize);
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(namesColor);
            for (int i = 0; i < g.Count; i++)
            {
                foreach (var inds in edges)
                {
                    if (inds.Item1 == i || inds.Item2 == i)
                    {
                        Col = hilightColor;
                        break;
                    }
                    else
                        Col = namesColor;
                }
                myBrush.Color = Col;
                e.Graphics.DrawString(g.names[i], drawFont, myBrush, (float)(g.Positions[i].X + offsetX - (g.names[i].Length * fontSize * 0.6)), (float)(g.Positions[i].Y + offsetY - (fontSize * 0.62)));
            }
        }
        private static void DrawLines(PaintEventArgs e, Graph g)
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
        private static void DrawLines(PaintEventArgs e, Graph g, List<int> inds, bool path)
        {
            if (!g.isOrGraph)
            {
                if (path)
                {
                    DrawLinesNOR(e, g);
                    DrawLinesNORPath(e, g, inds);
                }
                else
                    DrawLinesNOR(e, g, inds);
            }
            else
            {
                if (path)
                {
                    DrawLinesOR(e, g);
                    DrawLinesORPath(e, g, inds);
                }
                else
                    DrawLinesOR(e, g, inds);
            }
        }
        private static void DrawLines(PaintEventArgs e, Graph g, List<List<int>> cores)
        {
            DrawLinesOR(e, g);
            DrawLinesOR(e, g, cores);
        }
        private static void DrawLines(PaintEventArgs e, Graph g, List<ValueTuple<int, int>> edges)
        {
            if (!g.isOrGraph)
            {
                DrawLinesNOR(e, g);
                DrawLinesNOR(e, g, edges);
            }
            else
            {
                DrawLinesOR(e, g);
                DrawLinesOR(e, g, edges);
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
            Color Col = outlineColor;
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

        private static void DrawLinesNOR(PaintEventArgs e, Graph g, List<ValueTuple<int, int>> edges)
        {
            for (int i = 0; i < edges.Count; i++)
            {
                if (edges[i].Item1 >= g.Count || edges[i].Item2 >= g.Count) new NoPathException("Пути нет");
                if (g.Matrix[edges[i].Item1, edges[i].Item2] != 0 || g.Matrix[edges[i].Item2, edges[i].Item1] != 0)
                {
                    DrawLine(e, g.Positions[edges[i].Item1], g.Positions[edges[i].Item2], hilightColor);
                }
                else
                {
                    throw new NoPathException("Пути нет");
                }
            }
        }
        private static void DrawLinesNORPath(PaintEventArgs e, Graph g, List<int> inds)
        {
            for (int i = 1; i < inds.Count; i++)
            {
                if (inds[i] >= g.Count) new NoPathException("Пути нет");
                if (g.Matrix[inds[i - 1], inds[i]] != 0 || g.Matrix[inds[i], inds[i - 1]] != 0)
                {
                    DrawLine(e, g.Positions[inds[i - 1]], g.Positions[inds[i]], hilightColor);
                }
                else
                {
                    throw new NoPathException("Пути нет");
                }
            }
        }
        private static void DrawLinesORPath(PaintEventArgs e, Graph g, List<int> inds)
        {
            for (int i = 1; i < inds.Count; i++)
            {
                if (inds[i] >= g.Count) new NoPathException("Пути нет");
                if (g.Matrix[inds[i - 1], inds[i]] != 0)
                {
                    DrawArrow(e, g.Positions[inds[i - 1]], g.Positions[inds[i]], hilightColor);
                }
                else
                {
                    throw new NoPathException("Пути нет");
                }
            }
        }
        private static void DrawLinesOR(PaintEventArgs e, Graph g, List<ValueTuple<int, int>> edges)
        {
            foreach (var edge in edges)
            {
                if (edge.Item1 >= g.Count || edge.Item2 >= g.Count) throw new NoPathException("Ребра нет");
                if (g.Matrix[edge.Item1, edge.Item2] != 0)
                {
                    DrawArrow(e, g.Positions[edge.Item1], g.Positions[edge.Item2], hilightColor);
                }
                else
                {
                    throw new NoPathException("Ребра нет");
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
                        if (g.isOrGraph)
                            DrawArrow(e, g.Positions[i], g.Positions[j], arrowColor);
                        else
                            DrawLine(e, g.Positions[i], g.Positions[j], arrowColor);

                    }
                }
            }
        }
        private static void DrawLinesOR(PaintEventArgs e, Graph g, List<int> inds)
        {
            Color Col = fillcolor;
            for (int i = 0; i < g.Count; i++)
            {

                for (int j = 0; j < g.Count; j++)
                {
                    if (inds.Contains(i) && inds.Contains(j))
                        Col = hilightColor;
                    else
                        Col = arrowColor;

                    if (g.Matrix[i, j] != 0 && i != j)
                        DrawArrow(e, g.Positions[i], g.Positions[j], Col);
                }
            }
        }
        private static void DrawLinesOR(PaintEventArgs e, Graph g, List<int> inds, Color Col)
        {
            for (int i = 0; i < g.Count; i++)
            {

                for (int j = 0; j < g.Count; j++)
                {
                    if (inds.Contains(i) && inds.Contains(j))
                    {

                        if (g.Matrix[i, j] != 0 && i != j)
                        {
                            if (g.isOrGraph)
                                DrawArrow(e, g.Positions[i], g.Positions[j], Col);
                            else
                                DrawLine(e, g.Positions[i], g.Positions[j], Col);
                        }

                    }
                }
            }
        }
        private static void DrawLinesOR(PaintEventArgs e, Graph g, List<List<int>> cores)
        {
            Color Col = Color.FromArgb(255, 0, 0);
            for (int n = 0; n < cores.Count; n++)
            {
                Col = Getcolor(n);
                DrawLinesOR(e, g, cores[n], Col);
                // Col = Color.FromArgb(Col.R - (255 / cores.Count), Col.G + (255 / cores.Count), Col.B + (255 / cores.Count));
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
                            DrawString(e, (float)wPos.X + offsetX, (float)wPos.Y + offsetY, g.Matrix[i, j].ToString(), hilightColor);
                        }
                    }
                }
            }
        }

        private static void DrawWeightsPathNOR(PaintEventArgs e, Graph g, List<int> inds)
        {
            for (int i = 1; i < inds.Count; i++)
            {
                if (inds[i] >= g.Count) new NoPathException("Пути нет");
                if (g.Matrix[inds[i - 1], inds[i]] != 0 || g.Matrix[inds[i], inds[i - 1]] != 0)
                {
                    Position wPos = GetArrowEnd(g.Positions[inds[i]], g.Positions[inds[i - 1]], size / 2 + 45);
                    DrawCircle(e, (int)(size * 0.37), (int)(wPos.X), (int)(wPos.Y), Color.White);
                    DrawString(e, (float)wPos.X + offsetX, (float)wPos.Y + offsetY, g.Matrix[inds[i - 1], inds[i]].ToString(), hilightColor);
                }
                else
                {
                    throw new NoPathException("Пути нет");
                }
            }
        }
        private static void DrawWeightsPathOR(PaintEventArgs e, Graph g, List<int> inds)
        {
            for (int i = 1; i < inds.Count; i++)
            {
                if (inds[i] >= g.Count) new NoPathException("Пути нет");
                if (g.Matrix[inds[i - 1], inds[i]] != 0)
                {
                    Position wPos = GetArrowEnd(g.Positions[inds[i]], g.Positions[inds[i - 1]], size / 2 + 45);
                    DrawCircle(e, (int)(size * 0.37), (int)(wPos.X), (int)(wPos.Y), Color.White);
                    DrawString(e, (float)wPos.X + offsetX, (float)wPos.Y + offsetY, g.Matrix[inds[i - 1], inds[i]].ToString(), hilightColor);
                }
                else
                {
                    throw new NoPathException("Пути нет");
                }
            }
        }
        private static void DrawString(PaintEventArgs e, float x, float y, string msg, Color col)
        {
            Font drawFont = new Font("Arial", weightsfontSize);
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(col);
            e.Graphics.DrawString(msg, drawFont, myBrush, x - (float)(msg.Length * (fontSize - 4) * 0.5), y - (float)(fontSize * 0.7));
            myBrush.Dispose();
        }
        private static void DrawCircleStroke(PaintEventArgs e, int size, int x, int y, Color c)
        {
            System.Drawing.Pen myBrush = new Pen(c, strokeWidth);
            // System.Drawing.Graphics formGraphics;
            // formGraphics = form.CreateGraphics();
            e.Graphics.DrawEllipse(myBrush, new Rectangle(x + offsetX - size / 2, y + offsetY - size / 2, size, size));
            myBrush.Dispose();
            //  formGraphics.Dispose();
        }

    }
    [Serializable]
    class Position
    {
        public double X;
        public double Y;
        public Position(double x, double y)
        {
            X = x;
            Y = y;
        }

    }
    class NoPathException : Exception
    {
        public NoPathException(string msg) : base()
        {
        }
    }
}
