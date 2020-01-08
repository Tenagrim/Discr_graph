using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discr_graph
{
    class GraphEdge // Ребро
    {

        /// Связанная вершина
        public GraphVertex ConnectedVertex { get; }

        /// Вес ребра
        public int EdgeWeight { get; }

        /// Конструктор, связанная вершина, вес
        public GraphEdge(GraphVertex connectedVertex, int weight)
        {
            ConnectedVertex = connectedVertex;
            EdgeWeight = weight;
        }

    }
}
