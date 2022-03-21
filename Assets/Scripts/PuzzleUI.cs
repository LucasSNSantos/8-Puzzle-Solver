using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleUI : MonoBehaviour
{
    public Button ShuffleButton;
    public Button SolveButton;

    public Puzzle MyPuzzle;

    void Start()
    {
        // add os eventos dos btns
        ShuffleButton.onClick.AddListener(() =>
        {
            MyPuzzle.ShuffleAction();
        });

        SolveButton.onClick.AddListener(() =>
        {
            MyPuzzle.SolveAction();
        });
    }
}
