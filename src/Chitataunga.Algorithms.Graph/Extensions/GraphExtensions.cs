using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chitataunga.Algorithms.Graph.Extensions
{
    public static class GraphExtensions
    {
        /// <summary>
        /// computes the degree of vertex v from graph  g
        /// </summary>
        /// <param name="g"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static int Degree(this Graph g, int v)
        {
            int degree = 0;
            foreach (int w in g.Adj(v))
            {
                degree++;
            }
            return degree;
        }

        public static int MaxDegree(this Graph g)
        {
            int max = 0;
            for (int v = 0; v <  g.V(); v++)
            {
                int temp = g.Degree(v);
                if (temp > max) max = temp;
            }
            return max;
        }
        public static int AverageDegree(this Graph g)
        {
            return 2 * g.E() / g.V();
        }
        public static int NumberOfSelfLoops(this Graph g)
        {
            int count = 0;
            for (int v = 0; v < g.V(); v++)
            {
                foreach (int w in g.Adj(v))
                {
                    if (v == w) count++;
                }
            }
            return count;
        }
    }
}
