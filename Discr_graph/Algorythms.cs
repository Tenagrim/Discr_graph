using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discr_graph
{
    class Algorythms
    {
        public static void Deicstr(Graph g, int startNode, List<List<int>> paths, List<int> pathsLengths)
        {
            int[,] graf = g.Matrix;
            int[] minput = new int[g.Count];
            int[] met = new int[g.Count];
            int k = -1;
            int weight;
            int end;
            int index;
            int min;
            int temp;
            startNode--;

            for (int i = 0; i < g.Count; i++)
            {
                minput[i] = 10000;
                met[i] = 1;
            }
            minput[startNode] = 0;
            do
            {
                index = 10000;
                min = 10000;
                for (int i = 0; i < g.Count; i++)
                {
                    if ((met[i] == 1) && (minput[i] < min))
                    {
                        min = minput[i];
                        index = i;
                    }
                }
                if (index != 10000)
                {
                    for (int i = 0; i < g.Count; i++)
                    {
                        if (graf[index, i] > 0)
                        {
                            temp = min + graf[index, i];
                            if (temp < minput[i])
                            {
                                minput[i] = temp;
                            }
                        }
                    }
                    met[index] = 0;
                }
            } while (index < 10000);

            foreach (var i in minput)
                pathsLengths.Add(i);

            for (int n = 1; n <= g.Count; n++)
            {
                end = n;
                met[startNode] = end;
                end--;
                k = 1;
                weight = minput[end];
                if (weight != 10000)
                {
                    do
                    {
                        for (int i = 0; i < g.Count; i++)
                        {
                            if (graf[i, end] != 0)
                            {
                                temp = weight - graf[i, end];
                                if (temp == minput[i])
                                {
                                    weight = temp;
                                    met[k] = i + 1;
                                    end = i;
                                    k++;
                                }
                            }
                        }
                    } while (end > startNode);


                    paths.Add(new List<int>());
                    for (int i = k - 1; i >= 0; i--)
                    {
                        paths[n - 1].Add(met[i] - 1);
                    }
                    paths[n - 1][paths[n - 1].Count - 1] = n - 1;
                }
                else
                {
                    paths.Add(new List<int>());
                    paths[n - 1].Add(startNode);
                }
            }
        }

        public static void GetCore(Graph g, List<List<int>> cores)
        {

        }

        private static void CalcDNF()
        {

        }
        static private string GetDisunkInternal(int[,] graph)
        {
            string str = "";
            for (int i = 0; i < graph.GetLength(0); i++)
            {
                bool f = false;
                for (int j = 0; j < graph.GetLength(1); j++) // проверяет есть ли единицы в строке
                    if (graph[i, j] != 0) f = true;
                if (f)
                {
                    str += i + "v";
                    for (int j = 0; j < graph.GetLength(1); j++)
                        if (graph[i, j] != 0) str += j;
                    str += " ";
                }
            }
            if (str.Length - 1 > 0)
                return str.Remove(str.Length - 1); // удаляет левый пробел
            else return "";
        } // получает дизъюнкты для внут устойчивости
        static private string GetDisunkExternal(int[,] graph)
        {
            string str = "";
            for (int i = 0; i < graph.GetLength(0); i++)
            {
                bool f = false;
                for (int j = 0; j < graph.GetLength(1); j++) // проверяет есть ли единицы в строке
                    if (graph[i, j] != 0) f = true;
                str += i;
                if (f)
                    for (int j = 0; j < graph.GetLength(1); j++)
                        if (graph[i, j] != 0) str += "v" + j;
                str += " ";
            }
            if (str.Length - 1 > 0)
                return str.Remove(str.Length - 1); // удаляет левый пробел
            else return "";
        } // получает дизъюнкты для внешней устойчивости
        static private string SortString(string str)
        {
            List<string> newList = new List<string>();
            for (int i = 0; i < str.Length; i++)
                if (!newList.Contains(str[i].ToString()))
                    newList.Add(str[i].ToString());
            newList.Sort();
            string rez = "";
            for (int i = 0; i < newList.Count; i++)
                rez += newList[i];
            return rez;
        } // сортирует строку
        static private bool ContainsPogl(string str1, string str2)
        {
            for (int i = 0; i < str2.Length; i++)
            {
                if (!str1.Contains(str2[i]))
                    return false;
            }
            return true;
        } // можно поглотить или нет
        static private string[] GetDNF(string dis)
        {
            string[] arr = dis.Split(' ');
            string[][] arr2 = new string[arr.Length][];
            for (int i = 0; i < arr.Length; i++)
                arr2[i] = arr[i].Split('v');
            for (int i = 0; i < arr2.Length - 1; i++)
            {
                List<string> list = new List<string>(); // храним список конъюнкций
                for (int k = 0; k < arr2[i].Length; k++)
                {
                    for (int h = 0; h < arr2[i + 1].Length; h++)
                    {
                        string temp = arr2[i][k] + arr2[i + 1][h];
                        if (arr2[i][k].Contains(arr2[i + 1][h]))
                            temp = arr2[i][k];
                        else if (arr2[i + 1][h].Contains(arr2[i][k]))
                            temp = arr2[i + 1][h];
                        else
                            temp = arr2[i][k] + arr2[i + 1][h];

                        bool f = false;
                        for (int s = 0; s < list.Count; s++)
                        {

                            f = ContainsPogl(SortString(temp),list[s] );
                            if (f) break;
                                     
                        }
                        if (!f)
                            list.Add(SortString(temp));
                    }
                }
                arr2[i + 1] = list.ToArray();
            }

           //

            for (int i = 0; i < arr2[arr2.Length - 1].Length; i++)
            { // заменяем то, что можно поглотить, на ""
                for (int j = 0; j < arr2[arr2.Length - 1].Length; j++)
                {
                    if (arr2[arr2.Length - 1][j] != "" && j != i && ContainsPogl(arr2[arr2.Length - 1][i], arr2[arr2.Length - 1][j]))
                    {
                        arr2[arr2.Length - 1][i] = "";
                    }
                }
            }


            List<string> rez = new List<string>();
            for (int i = 0; i < arr2[arr2.Length - 1].Length; i++)
                if (arr2[arr2.Length - 1][i] != "")
                    rez.Add(arr2[arr2.Length - 1][i]);
            return rez.ToArray(); // возвращаем последнюю строку массива
        } // упрощает дизъюнкты, приводит к днф
        static private string[] GetObrDNF(string[] arr, int n)
        {
            string[] rez = new string[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                rez[i] = "";
                for (int j = 0; j < n; j++)
                {
                    if (!arr[i].Contains(j.ToString())) // если буквы не содержится в записи, то она добавляется
                        rez[i] += j;
                }
            }
            return rez;
        } // получение недостающих от днф
        static public List<List<int>> GetCore(int[,] graph)
        {
            List<string> rez = new List<string>();
            List<List<int>> res = null;
            string[] str1 = GetDNF(GetDisunkInternal(graph));
            str1 = GetObrDNF(str1, graph.GetLength(0)); 
            string[] str2 = GetDNF(GetDisunkExternal(graph)); 
            for (int i = 0; i < str1.Length; i++)
                for (int j = 0; j < str2.Length; j++)
                    if (str1[i] == str2[j]) rez.Add(str2[i]);
            res = stoList(rez.ToArray());
            return res;
        }
        private static List<List<int>> stoList(string[] arr)
        {
            List<List<int>> res = new List<List<int>>();
            for (int i = 0; i < arr.Length; i++)
            {
                res.Add(new List<int>());
                for (int j = 0; j < arr[i].Length; j++)
                {
                    res[i].Add(Int32.Parse(arr[i][j].ToString()));
                }
            }
            return res;
        }
        static public bool AllVertexConected(int[,] graph)
        {
            bool f = false;
            for (int i = 0; i < graph.GetLength(0); i++)
            {
                f = false;
                for (int j = 0; j < graph.GetLength(0); j++)
                    if (graph[i, j] != 0)
                    {
                        f = true;
                        break;
                    }
                for (int j = 0; j < graph.GetLength(0); j++)
                    if (graph[j, i] != 0)
                    {
                        f = true;
                        break;
                    }
            }
            return f;
        } // все ли вершины связаны
        private struct Edge
        {
            public int v1, v2, w; // v1- вершина 1, v2 - вершина 2, w - ширина
            public Edge(int v1, int v2, int w)
            {
                this.v1 = v1;
                this.v2 = v2;
                this.w = w;
            } // конструктор
        } // ребро
        static private List<Edge> GetListEdges(int[,] graph)
        {
            List<Edge> myEdges = new List<Edge>(); // список рёбер
            for (int i = 0; i < graph.GetLength(0); i++)
                for (int j = i + 1; j < graph.GetLength(0); j++)
                    if (graph[i, j] != 0) myEdges.Add(new Edge(i, j, graph[i, j]));
            for (int i = 0; i < myEdges.Count; i++)
                for (int j = 0; j < myEdges.Count; j++)
                    if (myEdges[i].w < myEdges[j].w)
                    {
                        Edge t = myEdges[i];
                        myEdges[i] = myEdges[j];
                        myEdges[j] = t;
                    }
            return myEdges;
        } // получение списка рёбер из таблицы весов и сортировка по возрастанию
        static private bool IsItTree(int[,] graph)
        {
            List<int> visitedVertex = new List<int>();
            visitedVertex.Add(0); // добавили первую вершину
            bool cicl = false;
            for (int i = 0; i < visitedVertex.Count; i++)
            { // смотрим посещённые вершины (ищем куда дальше можем пойти)
                for (int j = visitedVertex[i] + 1; j < graph.GetLength(0); j++)
                {
                    if (graph[visitedVertex[i], j] != 0)
                    {
                        if (!visitedVertex.Contains(j))
                        {
                            visitedVertex.Add(j);
                        }
                        else
                        {
                            cicl = true; // найден цикл
                            break; // завершаем посещение вершин
                        }
                    }
                }
                if (cicl) break; // если найдет цикл то завершаем посещение вершин
            }
            if (cicl) return false; // если найден цикл, то результат функции отрицательный
            else
            {
                if (visitedVertex.Count == graph.GetLength(0)) return true; // если смогли посетить все вершины, то граф является деревом
                else return false; // иначе не дерево
            }
        }
        static private bool Can_we_make_Graph(List<Edge> edges, int count)
        {
            bool[] visitedVertex = new bool[count];
            for (int i = 0; i < edges.Count; i++)
            {
                if (!visitedVertex[edges[i].v1] || !visitedVertex[edges[i].v2])
                {
                    visitedVertex[edges[i].v1] = true;
                    visitedVertex[edges[i].v2] = true;
                }
                else
                {
                    List<int> a_vertex = new List<int>();
                    List<int> b_vertex = new List<int>();
                    for (int j = 0; j < edges.Count; j++)
                    {
                        if (j != i)
                        {
                            if (edges[j].v1 == edges[i].v1)
                            {
                                a_vertex.Add(edges[j].v2);
                            }
                            if (edges[j].v2 == edges[i].v1)
                            {
                                a_vertex.Add(edges[j].v1);
                            }
                            if (edges[j].v1 == edges[i].v2)
                            {
                                b_vertex.Add(edges[j].v2);
                            }
                            if (edges[j].v2 == edges[i].v2)
                            {
                                b_vertex.Add(edges[j].v1);
                            }
                        }
                    }
                    for (int j = 0; j < a_vertex.Count; j++)
                    {
                        for (int k = 0; k < b_vertex.Count; k++)
                        {
                            if (a_vertex[j] == b_vertex[k])
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        static private List<int> GetVertex(int[,] graph)
        {
            List<int> myVertex = new List<int>();
            for (int i = 0; i < graph.GetLength(0); i++)
                myVertex.Add(i);
            return myVertex;
        } // получение списка вершин
        static private int[,] GetGraphFromEdges(List<Edge> listEdges)
        {
            List<int> vertex = new List<int>();
            for (int i = 0; i < listEdges.Count; i++)
            {
                if (vertex.IndexOf(listEdges[i].v1) == -1)
                    vertex.Add(listEdges[i].v1);
                if (vertex.IndexOf(listEdges[i].v2) == -1)
                    vertex.Add(listEdges[i].v2);
            }
            int[,] graph = new int[vertex.Count, vertex.Count];
            for (int i = 0; i < listEdges.Count; i++)
            {
                graph[listEdges[i].v1, listEdges[i].v2] = listEdges[i].w;
                graph[listEdges[i].v2, listEdges[i].v1] = graph[listEdges[i].v1, listEdges[i].v2];
            }
            return graph;
        } // строит граф из рёбер
        static public int WeightOstov(int[,] graph)
        {
            int sum = 0;
            for (int i = 0; i < graph.GetLength(0); i++)
                for (int j = 0; j < graph.GetLength(0); j++)
                    sum += graph[i, j];
            return sum / 2;
        } // получение веса остова
        static private int NecessaryEdge(List<Edge> notUsedEdges, List<int> usedVertex)
        {
            int index = -1;
            for (int i = 0; i < notUsedEdges.Count; i++)
            {
                if (usedVertex.IndexOf(notUsedEdges[i].v1) == -1 || usedVertex.IndexOf(notUsedEdges[i].v2) == -1)
                {
                    return i;
                }
            }
            return index;
        } // поиск необходимого ребра
        static public List<ValueTuple<int, int>> GetKruskal(int[,] graph)
        {
            List<Edge> rez = new List<Edge>();
            List<Edge> notUsedEdges = GetListEdges(graph); // неиспользованные рёбра
            List<int> notUsedVertex = GetVertex(graph); // неиспользованные вершины
            List<int> usedVertex = new List<int>();
            rez.Add(notUsedEdges[0]); // добавили первое ребро
            notUsedVertex.Remove(notUsedEdges[0].v1);
            notUsedVertex.Remove(notUsedEdges[0].v2);
            usedVertex.Add(notUsedEdges[0].v1);
            usedVertex.Add(notUsedEdges[0].v2);
            notUsedEdges.RemoveAt(0);
            while (notUsedVertex.Count > 0)
            { // пока не используются все вершины
                int index = NecessaryEdge(notUsedEdges, usedVertex);
                if (index == -1) throw new Exception("Ошибка построения Остова");
                rez.Add(notUsedEdges[index]); // добавили неиспользованное ребро
                notUsedVertex.Remove(notUsedEdges[index].v1);
                notUsedVertex.Remove(notUsedEdges[index].v2);
                usedVertex.Add(notUsedEdges[index].v1);
                usedVertex.Add(notUsedEdges[index].v2);
                notUsedEdges.RemoveAt(index); // удалили из списка это же ребро
            }
            for (int i = 0; i < notUsedEdges.Count && !IsItTree(GetGraphFromEdges(rez)); i++)
            { // пока не будет деревом, необходимо ещё добавлять вершины, даже если все вершины использованы
                rez.Add(notUsedEdges[i]); // добавили наименьшее ребро
                if (Can_we_make_Graph(rez, graph.GetLength(0)) && AllVertexConected(GetGraphFromEdges(rez)))
                {
                    return GetEdges(rez);
                }
               // else rez.RemoveAt(rez.Count - 1); // удалили только что добавленное ребро
            }
            return GetEdges(rez);
        } // алгоритм Краскала
        static List<ValueTuple<int, int>> GetEdges(List<Edge> ed)
        {
            List<ValueTuple<int, int>> res = new List<(int, int)>();

            foreach (Edge i in ed)
            {
                res.Add(new ValueTuple<int, int>(i.v1, i.v2));
            }
            return res;
        }
    }
}
