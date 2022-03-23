using Silentor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EightPuzzleGraph : IWeightedGraph<Snapshot>
{
    private readonly NumberBox Empty;

    private readonly int Width;
    private readonly int Height;

    public EightPuzzleGraph(NumberBox empty, int width, int height)
    {
        Empty = empty;
        Width = width;
        Height = height;
    }

    private List<Vector2> GetPositions(Snapshot snapshot, NumberBox shot) => snapshot.Where(x => x.Index == shot.Index).Select(x => new Vector2(x.XPos, x.YPos)).ToList();

    private int GetDistance(Vector2 current, Vector2 goal) => (int)Mathf.Abs(current.x - goal.y) + (int)Mathf.Abs(current.x - goal.y);

    public float Cost(Snapshot from, Snapshot to) => 1;

    public float Heuristic(Snapshot from, Snapshot to)
    {
        int cost = 0;

        var values = new List<NumberBox>(from);

        foreach (var val in values)
        {
            var currentVectores = GetPositions(from, val);
            var goalVectores = GetPositions(to, val);

            var diff = int.MaxValue;

            foreach (var currentVector in currentVectores)
            {
                foreach (var currentGoal in goalVectores)
                {
                    var _diff = GetDistance(currentVector, currentGoal);

                    diff = diff < _diff ? diff : _diff;
                }
            }

            cost += diff;
        }

        return cost;
    }

    public int ToIndex(int x, int y) => Width * y + x;

    public IEnumerable<Snapshot> Neighbors(Snapshot node)
    {
        var positions = GetPositions(node, Empty);

        var neighbors = new List<Snapshot>();

        foreach (var position in positions)
        {
            var movements = new Vector2[]
            {
                Vector2.up + position,
                Vector2.down + position,
                Vector2.right + position,
                Vector2.left + position
            };

            movements = movements.Where(x => x.x >= 0 && x.x < Width && x.y >= 0 && x.y < Height).ToArray();

            foreach (var movement in movements)
            {
                var neighbor = new Snapshot(node);
                //Width * (int)movement.y + (int)movement.x
                var indexMovement = ToIndex((int)movement.x, (int)movement.y);
                var indexPosition = ToIndex((int)position.x, (int)position.y);

                var prevIndex = node[indexMovement].Index;

                neighbor[indexMovement].Index = node[indexPosition].Index;
                neighbor[indexPosition].Index = prevIndex;

                var neighborString = neighbor.Print();
                var nodeSTring = node.Print();

                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }
}