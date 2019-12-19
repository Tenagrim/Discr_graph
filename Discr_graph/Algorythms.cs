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

    }
}
