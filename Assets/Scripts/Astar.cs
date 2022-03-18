using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Astar<T>
{
    public List<T> Nodes;
    public int Length;

    public abstract void HeuristicDistance();
    public abstract List<T> GetNeighbors();

    public Astar(int length)
    {
        Nodes = new List<T>();
        Length = length;
    }

    public void RebuildPath(T cameFrom, T current)
    {
        /*
        total_path := {current}
        while current in cameFrom.Keys:
            current := cameFrom[current]
            total_path.prepend(current)
        return total_path
         */
    }

}
