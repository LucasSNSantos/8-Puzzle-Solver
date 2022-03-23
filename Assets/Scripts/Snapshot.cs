using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Snapshot : List<NumberBox>, IEquatable<Snapshot>
{
    public Snapshot()
    {

    }

    public Snapshot(List<NumberBox> collection)
    {
        foreach (var item in collection)
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

    public int[,] GetAsMatrix()
    {
        int[,] snapMatrix = new int[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var snap = this.Where(x => x.XPos == i && x.YPos == j).FirstOrDefault();

                snapMatrix[i, j] = snap.Index;
            }
        }

        return snapMatrix;
    }

    public string Print()
    {
        int c = 0;
        string fullstring = "";
        foreach (var node in this)
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