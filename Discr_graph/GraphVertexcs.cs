using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discr_graph
{
    class GraphVertex // Вершина
    {
        /// Название вершины
        public string Name { get; }

        /// Список ребер, соедененных с данной вершиной
        public List<GraphEdge> Edges { get; }
        public List<GraphVertex> ConnectedVerticies { get; }

        /// Конструктор
        public GraphVertex(string vertexName)
        {
            Name = vertexName;
            Edges = new List<GraphEdge>();
            ConnectedVerticies = new List<GraphVertex>();
        }

    }
}
