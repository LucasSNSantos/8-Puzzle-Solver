using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Silentor;
using Newtonsoft.Json;

[Serializable]
public class Snapshot : List<NumberBox>, IEquatable<Snapshot>
{
    public Snapshot()
    {

    }

    public Snapshot(List<NumberBox> collection)
    {
        foreach(var item in collection)
        {
            Add(new NumberBox(item.Instance, item.Index)
            {
                XPos = item.XPos,
                YPos = item.YPos
            });
        }
    }

    public bool Equals(Snapshot other)
    {
        var thisJson = JsonConvert.SerializeObject(this.ToArray());
        var otherJson = JsonConvert.SerializeObject(other.ToArray());
        var a = thisJson == otherJson;
        return a;
    }

    public string Print()
    {
        int c = 0;
        string fullstring = "";
        foreach(var node in this)
        {
            c++;
            fullstring += $"{node.Index} | ";
            if (c % 3 == 0)
            {
                fullstring += "\t\t";
            }
        }
        return fullstring;
    }
}

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

        foreach(var val in values)
        {
            var currentVectores = GetPositions(from, val);
            var goalVectores = GetPositions(to, val);

            var diff = int.MaxValue;

            foreach(var currentVector in currentVectores)
            {
                foreach(var currentGoal in goalVectores)
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

        foreach(var position in positions)
        {
            var movements = new Vector2[]
            {
                Vector2.up + position,
                Vector2.down + position,
                Vector2.right + position,
                Vector2.left + position
            };

            movements = movements.Where(x => x.x >= 0 && x.x < Width && x.y >= 0 && x.y < Height).ToArray();

            foreach(var movement in movements)
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

                //var nodePosition = node.Where(x => x.XPos == position.x && x.YPos == position.y).FirstOrDefault();
                //var nodeMovement = node.Where(x => x.XPos == movement.x && x.YPos == movement.y).FirstOrDefault();

                //var neighborPosition = neighbor.Where(x => x.XPos == position.x && x.YPos == position.y).FirstOrDefault();
                //var neighborMovement = neighbor.Where(x => x.XPos == movement.x && x.YPos == movement.y).FirstOrDefault();

                //var prevIndex = nodeMovement.Index;

                //neighborMovement.Index = nodePosition.Index;
                //neighborPosition.Index = prevIndex;

                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }
}

public class Puzzle : MonoBehaviour
{
    public GameObject boxPrefab;

    public Snapshot Boxes;

    public Snapshot Origin;

    public Snapshot SnapshotStart;

    public Sprite[] sprite;

    //public EightPuzzle EightPuzzle;

    public EightPuzzleGraph EightPuzzleGraph;

    public AStarSearch<EightPuzzleGraph, Snapshot> Astar;

    public float TempoMove = 0.5f;

    public bool IsSolved = false;
    private bool IsShuffling = false;
    private bool IsSolving = false;

    void Start()
    {
        NumberBox empty = new NumberBox(null, 0);

        EightPuzzleGraph = new EightPuzzleGraph(empty, 3, 3);

        Astar = new AStarSearch<EightPuzzleGraph, Snapshot>(EightPuzzleGraph);

        Init();

        Origin = new Snapshot(Boxes);

        // StartCoroutine(Shuffle(50));
    }

    public void ResetAll()
    {
        foreach(var obj in Boxes)
        {
            if (obj.Instance != null)
            {
                Destroy(obj.Instance);
            }
        }

        Boxes.Clear();
    }

    public void UpdateSnap(Snapshot snapshot)
    {
        foreach(var box in snapshot)
        {
            box.Renderer.sprite = sprite[box.Index];

            box.Renderer.enabled = box.Index != 0;
        }
    }

    public void Init(Snapshot defaultSnapshot = null)
    {
        Boxes = new Snapshot();

        var valores = new int[]
        {
            6, 7, 8,
            3, 4, 5,
            0, 1, 2
        };
        
        foreach (var i in valores)
        {
            GameObject go = Instantiate(boxPrefab, Vector2.zero, Quaternion.identity);

            int newIndex = i;

            if (defaultSnapshot != null)
            {
                newIndex = defaultSnapshot[i].Index;
            }

            Boxes.Add(new NumberBox(go, newIndex));
        }

        MountFromSnapshot(Boxes);
    }

    private void MountFromSnapshot(Snapshot snapshot)
    {
        //snapshot.Reverse();
        //snapshot = snapshot.OrderBy(x => x.Index).ToList() as Snapshot;

        Queue<NumberBox> qbox = new Queue<NumberBox>(snapshot);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var currentBox = qbox.Dequeue(); // snapshot.Where(x => x.XPos == i && x.YPos == j).FirstOrDefault();

                currentBox.Init(j, i, sprite[currentBox.Index]);

                currentBox.Instance.transform.SetPositionAndRotation(new Vector3(j, i), Quaternion.identity);

                currentBox.Instance.GetComponent<SpriteRenderer>().enabled = true;
            }
        }

        var whereZero = snapshot.Where(x => x.Index == 0).FirstOrDefault();

        whereZero.Instance.GetComponent<SpriteRenderer>().enabled = false;
    }

    private IEnumerator Shuffle(int maxCount)
    {
        IsShuffling = true;

        for (int i = 0; i < maxCount; i++)
        {
            var neighbors = EightPuzzleGraph.Neighbors(Boxes);

            foreach(var ney in neighbors)
            {
                print(ney.Print());
            }

            var theChoosenOne = neighbors.OrderBy(x => UnityEngine.Random.Range(0, 9999)).FirstOrDefault();

            UpdateSnap(theChoosenOne);

            Boxes = theChoosenOne;

            //ResetAll();

            //Init(theChoosenOne);

            yield return new WaitForSecondsRealtime(TempoMove);
        }

        SnapshotStart = new Snapshot(Boxes);
        IsShuffling = false;
    }

    private IEnumerator Solve()
    {
        IsSolving = true;
        // logica para resolver o puzzle
        var goal = Origin;
        var start = SnapshotStart;

        while(!IsSolved)
        {
            var path = Astar.CreatePath(start, goal);

            foreach(var snapshot in path)
            {
                MountFromSnapshot(snapshot);

                yield return new WaitForSecondsRealtime(TempoMove);
            }
        }

        IsSolving = false;
    }

    /// <summary>
    /// DEPRECATED
    /// </summary>
    /// <param name="box"></param>
    private void Swap(NumberBox box)
    {
        var whereZero = Boxes.Where(x => x.Index == 0).FirstOrDefault();

        var boxSpriteRenderer = box.Instance.GetComponent<SpriteRenderer>();

        var sprite = boxSpriteRenderer.sprite;

        var previousIndex = box.Index;

        var zeroSpriteRenderer = whereZero.Instance.GetComponent<SpriteRenderer>();

        zeroSpriteRenderer.sprite = sprite;
        zeroSpriteRenderer.enabled = true;
        whereZero.Index = previousIndex;

        boxSpriteRenderer.enabled = false;

        box.Index = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !IsShuffling)
        {
            StartCoroutine(Shuffle(50));
        }

        if(Input.GetKeyDown(KeyCode.R) && !IsSolving)
        {
            StartCoroutine(Solve());
        }
    }
}
