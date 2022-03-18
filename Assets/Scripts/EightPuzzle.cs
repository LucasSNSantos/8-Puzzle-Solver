using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EightPuzzle : Astar<NumberBox>
{
    public EightPuzzle(int length) : base(length)
    {

    }

    public override List<NumberBox> GetNeighbors()
    {
        List<NumberBox> neighbors = new List<NumberBox>();
        var empty = Nodes.Where(x => x.Index == 0).FirstOrDefault();

        if (empty == null)
        {
            throw new System.Exception("erro no empty");
        }

        var up = empty.YPos + 1;
        var down = empty.YPos - 1;
        var right = empty.XPos + 1;
        var left = empty.XPos - 1;

        if (up < Length)
        {
            var upNode = Nodes.Where(x => x.XPos == empty.XPos && x.YPos == up).FirstOrDefault();

            neighbors.Add(upNode);
        }

        if(down >= 0)
        {
            var DNode = Nodes.Where(x => x.XPos == empty.XPos && x.YPos == down).FirstOrDefault();
            neighbors.Add(DNode);
        }

        if(right < Length )
        {
            var RightNode = Nodes.Where(x => x.XPos == right && x.YPos == empty.YPos).FirstOrDefault();

            neighbors.Add(RightNode);
        }

        if (left >= 0)
        {
            var LeftNode = Nodes.Where(x => x.XPos == left && x.YPos == empty.YPos).FirstOrDefault();

            neighbors.Add(LeftNode);
        }




        return neighbors;
    }

    public override void HeuristicDistance()
    {

    }
}
