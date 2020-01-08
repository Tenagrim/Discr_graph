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
            const int inf = 10000;
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
                minput[i] = inf;
                met[i] = 1;
            }
            minput[startNode] = 0;
            do
            {
                index = inf;
                min = inf;
                for (int i = 0; i < g.Count; i++)
                {
                    if ((met[i] == 1) && (minput[i] < min))
                    {
                        min = minput[i];
                        index = i;
                    }
                }
                if (index != inf)
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
            } while (index < inf);

            foreach (var i in minput)
                pathsLengths.Add(i);

            for (int n = 1; n <= g.Count; n++)
            {
                end = n;
                met[startNode] = end;
                end--;
                k = 1;
                weight = minput[end];
                if (weight != inf)
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

        static private string InternalArray(int[,] graph)
        {
            string str = "";
            for (int i = 0; i < graph.GetLength(0); i++)
            {
                bool f = false;
                for (int j = 0; j < graph.GetLength(1); j++)
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
                return str.Remove(str.Length - 1);
            else return "";
        }
        static private string ExternalArray(int[,] graph)
        {
            string str = "";
            for (int i = 0; i < graph.GetLength(0); i++)
            {
                bool f = false;
                for (int j = 0; j < graph.GetLength(1); j++)
                    if (graph[i, j] != 0) f = true;
                str += i;
                if (f)
                    for (int j = 0; j < graph.GetLength(1); j++)
                        if (graph[i, j] != 0) str += "v" + j;
                str += " ";
            }
            if (str.Length - 1 > 0)
                return str.Remove(str.Length - 1);
            else return "";
        }
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
        }
        static private bool MayBeAbsorbed(string str1, string str2)
        {
            for (int i = 0; i < str2.Length; i++)
            {
                if (!str1.Contains(str2[i]))
                    return false;
            }
            return true;
        }
        static private string[] DNF(string disukt)
        {
            string[] arr = disukt.Split(' ');
            string[][] arr2 = new string[arr.Length][];
            for (int i = 0; i < arr.Length; i++)
                arr2[i] = arr[i].Split('v');
            for (int i = 0; i < arr2.Length - 1; i++)
            {
                List<string> list = new List<string>();
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
                            f = MayBeAbsorbed(SortString(temp), list[s]);
                            if (f) break;
                        }
                        if (!f)
                            list.Add(SortString(temp));
                    }
                }
                arr2[i + 1] = list.ToArray();
            }
            for (int i = 0; i < arr2[arr2.Length - 1].Length; i++)
            {
                for (int j = 0; j < arr2[arr2.Length - 1].Length; j++)
                {
                    if (arr2[arr2.Length - 1][j] != "" && j != i && MayBeAbsorbed(arr2[arr2.Length - 1][i], arr2[arr2.Length - 1][j]))
                    {
                        arr2[arr2.Length - 1][i] = "";
                    }
                }
            }
            List<string> rez = new List<string>();
            for (int i = 0; i < arr2[arr2.Length - 1].Length; i++)
                if (arr2[arr2.Length - 1][i] != "")
                    rez.Add(arr2[arr2.Length - 1][i]);
            return rez.ToArray();
        }
        static private string[] RevDNF(string[] arr, int n)
        {
            string[] rez = new string[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                rez[i] = "";
                for (int j = 0; j < n; j++)
                {
                    if (!arr[i].Contains(j.ToString()))
                        rez[i] += j;
                }
            }
            return rez;
        }
        static public List<List<int>> Cores(int[,] graph)
        {
            List<string> rez = new List<string>();
            List<List<int>> res = null;
            string[] str1 = DNF(InternalArray(graph));
            str1 = RevDNF(str1, graph.GetLength(0));
            string[] str2 = DNF(ExternalArray(graph));
            for (int i = 0; i < str1.Length; i++)
                for (int j = 0; j < str2.Length; j++)
                    if (str1[i] == str2[j]) rez.Add(str2[j]);
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

        private struct Edge
        {
            public int v1, v2, w;
            public Edge(int v1, int v2, int w)
            {
                this.v1 = v1;
                this.v2 = v2;
                this.w = w;
            }
        }
        static private List<Edge> GetListEdges(int[,] graph)
        {
            List<Edge> myEdges = new List<Edge>();
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
        }

        static private List<int> GetListVertex(int[,] graph)
        {
            List<int> myVertex = new List<int>();
            for (int i = 0; i < graph.GetLength(0); i++)
                myVertex.Add(i);
            return myVertex;
        }
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
        }
        static public int WeightOstov(int[,] graph)
        {
            int sum = 0;
            for (int i = 0; i < graph.GetLength(0); i++)
                for (int j = 0; j < graph.GetLength(0); j++)
                    sum += graph[i, j];
            return sum / 2;
        }
        static private int GetMinEdge(List<Edge> notUsedEdges, List<int> usedVertex)
        {
            int index = -1;
            int minW = 1000000;
            for (int i = 0; i < notUsedEdges.Count; i++)
            {
                if (usedVertex.Count == 0)
                {
                    if (notUsedEdges[i].w <= minW)
                    {
                        minW = notUsedEdges[i].w;
                        index = i;
                    }
                }
                else if ((!usedVertex.Contains(notUsedEdges[i].v1) && usedVertex.Contains(notUsedEdges[i].v2)) || (!usedVertex.Contains(notUsedEdges[i].v2) && usedVertex.Contains(notUsedEdges[i].v1)))
                {
                    if (notUsedEdges[i].w <= minW)
                    {
                        minW = notUsedEdges[i].w;
                        index = i;
                    }
                }
            }
            return index;
        }
        static public List<ValueTuple<int, int>> Prim(int[,] graph)
        {
            List<Edge> rez = new List<Edge>();
            List<Edge> notUsedEdges = GetListEdges(graph);
            List<int> notUsedVertex = GetListVertex(graph);
            List<int> usedVertex = new List<int>();
            int t = GetMinEdge(notUsedEdges, usedVertex);
            rez.Add(notUsedEdges[t]);
            notUsedVertex.Remove(notUsedEdges[t].v1);
            notUsedVertex.Remove(notUsedEdges[t].v2);
            usedVertex.Add(notUsedEdges[t].v1);
            usedVertex.Add(notUsedEdges[t].v2);
            notUsedEdges.RemoveAt(t);
            while (notUsedVertex.Count > 0)
            {
                int index = GetMinEdge(notUsedEdges, usedVertex);
                if (index == -1) throw new Exception("Ошибка построения Остова");
                rez.Add(notUsedEdges[index]);
                notUsedVertex.Remove(notUsedEdges[index].v1);
                notUsedVertex.Remove(notUsedEdges[index].v2);
                usedVertex.Add(notUsedEdges[index].v1);
                usedVertex.Add(notUsedEdges[index].v2);
                notUsedEdges.RemoveAt(index);
            }
            return GetEdges(rez);
        }
        static List<ValueTuple<int, int>> GetEdges(List<Edge> ed)
        {
            List<ValueTuple<int, int>> res = new List<(int, int)>();

            foreach (Edge i in ed)
            {
                res.Add(new ValueTuple<int, int>(i.v1, i.v2));
            }
            return res;
        }

        /////////////////////////////////////////////////////////////////////////////////////////
        ///
        static string[] verticeName;

        static void SetVerticeNames(ref string[] arr)
        {
            Console.WriteLine();
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write($"Введите название вершины {i + 1}: ");
                arr[i] = Console.ReadLine();
            }
            Console.WriteLine();
        } // задание навзаний вершин

        static void CorrectInputOfMatrixMember(out int var) // Проверка на ввод числа в матрице смежностиа
        {
            do
            {
                while (!int.TryParse(Console.ReadLine(), out var))
                {
                    Console.WriteLine("Ошибка ввода. Введите число ");
                }

                if (var < 0 || var > 1) Console.WriteLine("Ошибка ввода. Введите 1, если есть путь, либо 0, если пути нет");

            } while (var < 0 || var > 1);
        }
        static void CorrectInputofNumber(out int var) // Проверка на ввод числа вершин графа
        {
            do
            {
                while (!int.TryParse(Console.ReadLine(), out var))
                {
                    Console.WriteLine("Ошибка ввода. Введите число ");
                }
                if (var < 1 || var > 9) Console.WriteLine("Ошибка ввода. Введите число больше 0 и меньше 10");
            } while (var < 1 && var > 9);
        }
        static int[,] AdjacencyMatrix(int n) // Формирование матрицы смежности
        {
            int[,] arr = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                arr[i, i] = 0;
                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                    {
                        Console.Write($"{ i + 1 } -> { j + 1 }: ");
                        CorrectInputOfMatrixMember(out arr[i, j]);
                    }
                }
            }
            ShowAdjacencyMatrix(ref arr);
            return arr;
        }
        static void ShowAdjacencyMatrix(ref int[,] arr)
        {
            Console.WriteLine("\nМатрица смежности:");
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    Console.Write(arr[i, j] + " ");
                }
                Console.WriteLine("\n");
            }
        } // Матрица смежности

        static void ShowMPF(ref string[,] arr, bool f, ref string outStr)

        {
            outStr = "";
            outStr += "Ярусно-параллельная форма:\n";
            if (!f)
            {
                for (int i = 0; i < arr.GetLength(0); i++)
                {
                    for (int j = 0; j < arr.GetLength(1) && arr[i, j] != null; j++)
                    {
                        outStr += arr[i, j].ToString() + " ";
                    }
                    outStr += "\n";
                }
            }
            else
            {
                // Console.WriteLine("Привести данный граф к ярусно-параллельной форме невозможно, так как он имеет цикл\n");
                throw new ArgumentException("Привести данный граф к ярусно-параллельной форме невозможно, так как он имеет цикл");
            }
        }
        public static void MakeMPF(int[,] Arr, ref string mpf, ref List<List<int>> levels)

        {
            /* массив столбцов, имеющий 3 состояния:
            0 - столбец еще не зачеркнут
            1 - столбец зачеркнут
            2 - столбец был зачеркнут в прошлых итерациях */
            int[,] arr = (int[,])Arr.Clone();
            int[] columns = new int[arr.GetLength(1)];
            for (int i = 0; i < columns.Length; i++)
                columns[i] = 0;
            bool isEnded = false; // конец цикла - все столбцы зачеркнуты
            bool isCycled = false; // есть ли в графе цикл
            int tier = 0; // номер очередного яруса
            string[,] MPF = new string[arr.GetLength(0), arr.GetLength(0)]; // массив вершин в ярусно-параллельной форме
            do
            {
                for (int j = 0; j < arr.GetLength(0); j++)
                {
                    if (columns[j] == 0) // если столбец не зачеркнут
                    {
                        for (int i = 0; i < arr.GetLength(1); i++)
                        {
                            if (arr[i, j] == 0) columns[j] = 1; // зачеркиваем столбец
                            else
                            {
                                columns[j] = 0;
                                break; // не зачеркиваем столбец
                            }
                        }
                    }
                }
                for (int i = 0, k = 0; i < columns.Length; i++)
                {
                    if (columns[i] == 1) // если на этой итерации был зачекнут столбец
                    {
                        for (int j = 0; j < arr.GetLength(1); j++) // зачеркиваем строку(обнуляем ее)
                            arr[i, j] = 0;
                        MPF[tier, k++] = (i + 1).ToString(); // добавляем номер в массив вывода
                    }
                }
                tier++;
                if (tier > arr.GetLength(0))
                {
                    isCycled = true;
                    break;
                }
                isEnded = true;
                for (int i = 0; i < columns.Length; i++)
                {
                    if (columns[i] == 1)

                        columns[i] = 2; // переводим столбец состояние "был зачернут ранее"

                    if (columns[i] == 0)

                        isEnded = false;
                }
            } while (!isEnded);

            ShowMPF(ref MPF, isCycled, ref mpf);
            levels = GetLevels(MPF, isCycled);

        }
        static List<List<int>> GetLevels(string[,] arr, bool f)
        {
            List<List<int>> res = new List<List<int>>();
            if (!f)
            {
                for (int i = 0; i < arr.GetLength(0); i++)
                {
                    res.Add(new List<int>());
                    for (int j = 0; j < arr.GetLength(1) && arr[i, j] != null; j++)
                    {
                        res[i].Add(Int32.Parse(arr[i, j]));
                    }
                }
            }
            else
            {              
               throw new ArgumentException("Привести данный граф к ярусно-параллельной форме невозможно, так как он имеет цикл");
            }
            return res;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////

        public static void FindConnectivityComponents(Graph g, List<List<int>> components)
        {
            List<GraphVertex> gVerticies; // Все вершины графа
            List<GraphVertex> comp; // Компонента связности
            List<GraphVertex> visited; // Посещенные вершины

            gVerticies = g.GetVertices();
            comp = new List<GraphVertex>();
            visited = new List<GraphVertex>();

            components.Clear(); 

            foreach (GraphVertex ver in gVerticies)
            {
                if (!visited.Contains(ver))
                {
                    SFD(ver,comp, visited);
                }
                if (comp.Count != 0)
                {
                    components.Add(getCompList(comp));
                    comp.Clear();
                }
            }          
        }
         static List<int> getCompList(List<GraphVertex> comp)
        {
            List<int> res = new List<int>();

            foreach (var i in comp)
            {
                res.Add(Int32.Parse(i.Name));                
            }

            return res;
        }
         static void SFD(GraphVertex ver, List<GraphVertex> comp, List<GraphVertex> visited)
        {
            visited.Add(ver);
            comp.Add(ver);
            foreach (GraphVertex cv in ver.ConnectedVerticies)
            {
                if (!visited.Contains(cv))
                {
                    SFD(cv, comp, visited);
                }
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////
        public void bronKerbosch(List<List<int>> res, List<GraphVertex> cV, List<GraphVertex> pCV, List<GraphVertex> eV)
        {
            //IEnumerator <GraphVertex> iter = pCV.GetEnumerator();
            //iter.MoveNext();
            if (cV.Count != 0 && pCV.Count == 0 && eV.Count == 0)
            {
                Console.WriteLine("Клика найдена: ");
                printList(cV);
            }
            //while (pCV.Count != 0 && iter.Current != pCV.Last())
            foreach (GraphVertex item in pCV) // Для каждой вершины графа
            {
                List<GraphVertex> singleList = new List<GraphVertex>() { item/*.Current*/ };
                bronKerbosch(setUnion(cV, singleList), setIntersection(pCV, item/*.Current*/.ConnectedVerticies), setIntersection(eV, item./*Current.*/ConnectedVerticies));
                pCV = setDifference(pCV, singleList);
                eV = setUnion(eV, singleList);
                //if (pCV.Count != 0)
                //{
                //    iter.Reset();
                //    iter.MoveNext();
                //}
            }
        }
    }
}
