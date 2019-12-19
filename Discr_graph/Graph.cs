using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discr_graph
{
    [Serializable]
    class Graph
    {
        private int count;
        private int[,] matrix;
        public int[,] Matrix
        {
            get 
            {
                if (isOrGraph)
                    return matrix;
                else return GetSymmMatrix();
            }
            set
            {
                if (value.Length != count)
                    throw new ArgumentException("Нельзя установить позиции. Неверное число элементов");
                else matrix = value;
            }
        }
        public int Count { get { return count; } }
        public int Lines { get { return GetLines(); } }

        public bool empty;
        public bool isOrGraph;
        public bool weighted;

        public bool fixedPositions;

        public string[] names;
        public Position[] Positions
        {
            get { return positions; }
            set
            {
                if (value.Length != count)
                    throw new ArgumentException("Нельзя установить позиции. Неверное число элементов");
                else positions = value;
            }
        }
        Position[] positions;

        public Graph(int n)
        {
            count = n;
            matrix = new int[n, n];
            names = new string[n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = 0;
                }
                names[i] = (i + 1).ToString();
            }
            empty = false;
            fixedPositions = false;
            isOrGraph = false;
            positions = null;
        }
        public void TruncPos()
        {
            positions = null;
        }
        public Graph(int[,] m, bool isOrGR)
        {
            if (m.GetLength(0) != m.GetLength(1)) throw new ArgumentException("Инвалидная матрица");
            count = m.GetLength(0);
            matrix = m;
            names = new string[count];

            for (int i = 0; i < count; i++)
            {
                names[i] = (i + 1).ToString();
            }
            empty = false;
            fixedPositions = false;
            isOrGraph = isOrGR;
            positions = null;
        }
        public Graph(int n, bool random)
        {
            Random rand = new Random();
            count = n;
            matrix = new int[n, n];
            names = new string[n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                        if (rand.Next(0, 2) == 1)
                            matrix[i, j] = rand.Next(0, 99);
                        else
                            matrix[i, j] = 0;
                }
                names[i] = (i + 1).ToString();
            }
            empty = false;
            fixedPositions = false;
            isOrGraph = true;
            positions = null;
        }
        public Graph(int n, int full)
        {
            Random rand = new Random();
            count = n;
            matrix = new int[n, n];
            names = new string[n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                        matrix[i, j] = 1;
                    else
                        matrix[i, j] = 0;
                }
                names[i] = (i + 1).ToString();
            }
            empty = false;
            fixedPositions = false;
            isOrGraph = false;
            positions = null;
        }
        private int GetLines()
        {
            int c = 0;
            for (int i = 0; i < count; i++)
            {
                int j = 0;
                if (!isOrGraph)
                    j = i;
                for (; j < count; j++)
                {
                    if (matrix[i, j] != 0) c++;
                }
            }
            return c;
        }
        private int[,] GetSymmMatrix()
        {
            int[,] res = new int[count, count];


            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    if (i <= j)
                        res[i, j] = matrix[i, j];
                    else
                        res[i, j] = matrix[j, i];
                }
            }
            return res;
        }
    }
}
