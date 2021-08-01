using Chitataunga.Algorithms.Common.DataStructures;
using System;
using System.Collections.Generic;

namespace Chitataunga.Algorithms.Graph
{
    /// <summary>
    /// Undirected graph
    /// </summary>
    public class Graph
    {
        private readonly int _V;
        private int _E;
        private RandomBag<int>[] _adj; // adjascency lists
        public Graph(int v)
        {
            _V = v;
            _E = 0;
            _adj = new RandomBag<int>[_V];
            for(int _v = 0; _v < _V; _v++)
            {
                _adj[v] = new RandomBag<int>();
            }
        }
        public Graph(int v, int e)
        {
            _V = v;
            _E = e;
            _adj = new RandomBag<int>[_V];
            for (int _v = 0; _v < _V; _v++)
            {
                _adj[v] = new RandomBag<int>();
            }
        }

        public int E() => _E;
        public int V() => _V;

        public void AddEdge(int v, int w)
        {
            _adj[v].Add(w);

            _adj[w].Add(v);
            _E++;
        }

        public IEnumerable<int>  Adj(int v)
        {
            return _adj[v];
        }

        public override string ToString()
        {

            string s = _V + " vertices, " + _E + " edges\n";
            for (int v = 0; v < _V; v++)
            {
                s += v + ": ";
                foreach (int w in this.Adj(v))
                    s += w + " ";
                s += "\n";
            }
            return s;
        }
    }
}
