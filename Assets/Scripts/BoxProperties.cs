using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxProperties : MonoBehaviour
{
    public int Index;

    private SpriteRenderer Renderer;

    private static PuzzleUI PuzzleUI;

    private Animation MyAnimation;

    public AnimationClip FlashClip;
    public AnimationClip IdleClip;

    private void Start()
    {
        if (PuzzleUI == null)
        {
            PuzzleUI = FindObjectOfType<PuzzleUI>();
        }

        MyAnimation = GetComponent<Animation>();

        Renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (PuzzleUI != null && PuzzleUI.IsEditing)
        {
            if (MyAnimation.IsPlaying("Idle"))
            {
                MyAnimation.clip = FlashClip;
                MyAnimation.Play();
            }
        } else
        {
            if (MyAnimation.IsPlaying("Flash"))
            {
                MyAnimation.clip = IdleClip;
                MyAnimation.Play();
            }
        }
    }
}
