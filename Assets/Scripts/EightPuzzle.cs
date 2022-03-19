using PriorityQueue;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EightPuzzle //: Astar<NumberBox>
{
    //public EightPuzzle(int length) : base(length)
    //{

    //}

    //public override Queue<NumberBox> AStar(NumberBox start, NumberBox goal)
    //{
    //    var cameFrom = new Dictionary<NumberBox, NumberBox>();

    //    var startCost = new Dictionary<NumberBox, int>
    //    {
    //        { start, 0 }
    //    };

    //    var fullCost = new Dictionary<NumberBox, int>
    //    {
    //        { start, HeuristicDistance(start, goal) }
    //    };

    //    var openSet = new PriorityQueue<NumberBox>();

    //    openSet.Enqueue(start);

    //    var count = 0;

    //    while(openSet.Count > 0)
    //    {
    //        if (count++ > 3000) throw new System.Exception("Loop infinito");

    //        var current = openSet.Dequeue();

    //        if (current == null)
    //        {
    //            throw new System.Exception("Impossivel de alcancar");
    //        }

    //        if(current.Equals(goal))
    //        {
    //            return BuildPath(cameFrom, current);
    //        }

    //        var currentNeighbors = GetNeighbors(current);

    //        foreach(var neighbor in currentNeighbors)
    //        {
    //            const int infinity = int.MaxValue / 2;

    //            var neighborStartCost = startCost.ContainsKey(current) ? startCost[current] : infinity;

    //            var weight = GetDistance(current, neighbor);

    //            neighborStartCost += weight;

    //            var neighborValue = startCost.ContainsKey(neighbor) ? startCost[neighbor] : infinity;

    //            if (neighborValue >= infinity || neighborStartCost < neighborValue)
    //            {
    //                cameFrom.Add(neighbor, current);

    //                startCost[neighbor] = neighborStartCost;

    //                fullCost[neighbor] = neighborStartCost + HeuristicDistance(neighbor, goal);

    //                //neighbor.CurrentFullCost = fullCost[neighbor];

    //                if (!openSet.Contains(neighbor))
    //                {
    //                    openSet.Enqueue(neighbor);
    //                }
    //            }

    //        }
    //    }

    //    throw new System.Exception("No impossivel de ser alcancado");
    //}

    //public int GetDistance(NumberBox from, NumberBox to)
    //{
    //    return HeuristicDistance(from, to);
    //}

    //public List<NumberBox> GetNeighbors(NumberBox box = null)
    //{
    //    List<NumberBox> neighbors = new List<NumberBox>();

    //    NumberBox empty;

    //    if (box != null)
    //    {
    //        empty = box;
    //    } else
    //    {
    //        empty = Nodes.Where(x => x.Index == 0).FirstOrDefault();
    //    }
        
    //    if (empty == null)
    //    {
    //        throw new System.Exception("erro no empty");
    //    }

    //    var up = empty.YPos + 1;
    //    var down = empty.YPos - 1;
    //    var right = empty.XPos + 1;
    //    var left = empty.XPos - 1;

    //    if (up < Length)
    //    {
    //        var upNode = Nodes.Where(x => x.XPos == empty.XPos && x.YPos == up).FirstOrDefault();

    //        neighbors.Add(upNode);
    //    }

    //    if (down >= 0)
    //    {
    //        var DNode = Nodes.Where(x => x.XPos == empty.XPos && x.YPos == down).FirstOrDefault();
    //        neighbors.Add(DNode);
    //    }

    //    if (right < Length)
    //    {
    //        var RightNode = Nodes.Where(x => x.XPos == right && x.YPos == empty.YPos).FirstOrDefault();

    //        neighbors.Add(RightNode);
    //    }

    //    if (left >= 0)
    //    {
    //        var LeftNode = Nodes.Where(x => x.XPos == left && x.YPos == empty.YPos).FirstOrDefault();

    //        neighbors.Add(LeftNode);
    //    }
    //    return neighbors;
    //}

    //public override int HeuristicDistance(NumberBox current, NumberBox goal)
    //{
    //    //var goal = Snapshot.Where(x => x.Index == current.Index).FirstOrDefault();
    //    var manhattanDistance = Mathf.Abs(current.XPos - goal.YPos) + Mathf.Abs(current.YPos - goal.XPos);
    //    return manhattanDistance;
    //}

    //public override void SetSnapshot(List<NumberBox> collection)
    //{
    //    Snapshot = new List<NumberBox>(collection);
    //}
}
