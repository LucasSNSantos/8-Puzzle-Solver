using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberBox
{
    public GameObject Instance { get; set; }
    public int Index = 0;
    public int XPos = 0;
    public int YPos = 0;

    public NumberBox(GameObject instance, int index)
    {
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


}
