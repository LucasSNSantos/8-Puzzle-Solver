using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Astar<T>
{
    public List<T> Nodes;
    public List<T> Snapshot;
    public int Length;

    public abstract int HeuristicDistance(T currentNode, T goalNode);

    public Astar(int length)
    {
        Nodes = new List<T>();
        Length = length;
    }

    public abstract void SetSnapshot(List<T> collection);
    public abstract Queue<T> AStar(T start, T goal);

    public Queue<T> BuildPath(Dictionary<T, T> cameFrom, T current)
    {
        if (current == null)
        {
            return new Queue<T>();
        }

        var path = new Queue<T>();

        path.Enqueue(current);

        int count = 0;

        while (current != null && cameFrom[current] != null)
        {
            if (count > 750)
            {
                throw new System.Exception("Timeout");
            }

            current = cameFrom[current];

            if (current != null)
            {
                path.Enqueue(current);
            }
        }

        path.Dequeue();

        return path;
    }

}
