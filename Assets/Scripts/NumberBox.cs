using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberBox : IComparable<NumberBox>
{
    public Astar<NumberBox> MyPuzzleSolver;
    public GameObject Instance { get; set; }
    public int Index = 0;
    public int XPos = 0;
    public int YPos = 0;
    public int CurrentFullCost;

    public NumberBox(GameObject instance, Astar<NumberBox> myPuzzleSolver, int index)
    {
        MyPuzzleSolver = myPuzzleSolver;
        Instance = instance;
        Index = index;
    }

    public void Init(int x, int y, Sprite sprite)
    {
        Instance.GetComponent<SpriteRenderer>().sprite = sprite;
        UpdatePosition(x, y);
    }

    public void UpdatePosition(int x, int y)
    {
        XPos = x;
        YPos = y;
    }

    public override bool Equals(object obj)
    {
        NumberBox objBox = obj as NumberBox;

        if (objBox.XPos == XPos && objBox.YPos == YPos && objBox.Index == Index)
        {
            return true;
        }

        return false;
    }

    public int CompareTo(NumberBox other)
    {
        return CurrentFullCost.CompareTo(other.CurrentFullCost);
    }
}
