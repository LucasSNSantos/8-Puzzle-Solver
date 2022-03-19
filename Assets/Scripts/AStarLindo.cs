using System;
using System.Collections.Generic;

//Based on https://www.redblobgames.com/pathfinding/from-star/implementation.html#csharp
namespace Silentor
{
    public interface IWeightedGraph<TNode> where TNode : IEquatable<TNode>
    {
        float Cost(TNode from, TNode to);

        IEnumerable<TNode> Neighbors(TNode node);

        float Heuristic(TNode from, TNode to);
    }

    /// <summary>
    /// Example implementation
    /// </summary>
    public class Graph : IWeightedGraph<(int, int)>
    {
        public float Cost((int, int) from, (int, int) to)
        {
            //Should calculate cost value between neighbor nodes, so it can be joined with Neighbors method
            return Math.Abs(from.Item1 - to.Item1) + Math.Abs(from.Item2 - to.Item2);
        }

        public IEnumerable<(int, int)> Neighbors((int, int) node)
        {
            yield return (node.Item1 + 1, node.Item2);
            yield return (node.Item1 - 1, node.Item2);
            yield return (node.Item1, node.Item2 + 1);
            yield return (node.Item1, node.Item2 - 1);
        }

        public float Heuristic((int, int) from, (int, int) to)
        {
            //Consider fast and problem-aware heuristic
            return Cost(from, to);
        }
    }

    /// <summary>
    /// Naive priority queue implementation for small scale problems
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityQueue<T>
    {
        // I'm using an unsorted array for this example, but ideally this
        // would be a binary heap. There's an open issue for adding a binary
        // heap to the standard C# library: https://github.com/dotnet/corefx/issues/574
        //
        // Until then, find a binary heap class:
        // * https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp
        // * http://visualstudiomagazine.com/articles/2012/11/01/priority-queues-with-c.aspx
        // * http://xfleury.github.io/graphsearch.html
        // * http://stackoverflow.com/questions/102398/priority-queue-in-net

        private readonly List<Tuple<T, float>> _elements = new List<Tuple<T, float>>();

        public int Count => _elements.Count;

        public void Enqueue(T item, float priority)
        {
            _elements.Add(Tuple.Create(item, priority));
        }

        public T Dequeue()
        {
            int bestIndex = 0;

            for (int i = 0; i < _elements.Count; i++)
            {
                if (_elements[i].Item2 < _elements[bestIndex].Item2)
                {
                    bestIndex = i;
                }
            }

            T bestItem = _elements[bestIndex].Item1;
            _elements.RemoveAt(bestIndex);
            return bestItem;
        }
    }

    public class AStarSearch<TGraph, TNode> where TGraph : IWeightedGraph<TNode> where TNode : IEquatable<TNode>
    {
        private TGraph _graph;
        private readonly Dictionary<TNode, TNode> _cameFrom = new Dictionary<TNode, TNode>();
        private readonly Dictionary<TNode, float> _costSoFar = new Dictionary<TNode, float>();


        public AStarSearch(TGraph graph)
        {
            _graph = graph;
        }

        public List<TNode> CreatePath(TNode start, TNode goal)
        {
            _cameFrom.Clear();
            _costSoFar.Clear();

            var frontier = new PriorityQueue<TNode>();
            frontier.Enqueue(start, 0);

            _cameFrom[start] = start;
            _costSoFar[start] = 0;

            int antiCrash = 0;

            while (frontier.Count > 0)
            {
                if (antiCrash++ > 15000)
                {
                    return null;
                }

                var current = frontier.Dequeue();

                //Comment out if your want best possible path
                if (current.Equals(goal))
                    break;

                foreach (var next in _graph.Neighbors(current))
                {
                    var newCost = _costSoFar[current] + _graph.Cost(current, next);
                    if (!_costSoFar.TryGetValue(next, out var storedNextCost) || newCost < storedNextCost)
                    {
                        _costSoFar[next] = newCost;
                        var priority = newCost + _graph.Heuristic(next, goal);
                        frontier.Enqueue(next, priority);
                        _cameFrom[next] = current;
                    }
                }
            }

            //Reconstruct path
            var result = new List<TNode>();
            TNode prev;
            if (_cameFrom.ContainsKey(goal))
            {
                prev = goal;
                result.Add(prev);
            }
            else
            {
                //Path cant be found
                return null;
            }

            do
            {
                prev = _cameFrom[prev];
                result.Add(prev);
            } while (!prev.Equals(start));
            result.Reverse();

            //Path can be found
            return result;
        }
    }
}
