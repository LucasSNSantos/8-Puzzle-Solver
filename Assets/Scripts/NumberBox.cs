using Newtonsoft.Json;
using System;
using UnityEngine;

public class NumberBox
{
    [JsonIgnore]
    public GameObject Instance { get; set; }
    [JsonIgnore]
    public SpriteRenderer Renderer { get; set; }
    public int Index = 0;
    public int XPos = 0;
    public int YPos = 0;

    public NumberBox(GameObject instance, int index)
    {
        Instance = instance;
        if (Instance != null)
        {
            Renderer = Instance.GetComponent<SpriteRenderer>();
        }
        Index = index;
    }

    public void Init(int x, int y, Sprite sprite)
    {
        if (Instance != null)
        {
            Instance.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        UpdatePosition(x, y);
    }

    public void UpdatePosition(int x, int y)
    {
        XPos = x;
        YPos = y;
    }

    public void UpdateIndex(int index, Sprite sprite)
    {
        Index = index;
        if (Instance != null)
        {
            Instance.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
