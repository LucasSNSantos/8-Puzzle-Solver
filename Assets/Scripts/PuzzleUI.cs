using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleUI : MonoBehaviour
{
    public Button ShuffleButton;
    public Button SolveButton;

    public Puzzle MyPuzzle;

    public Image SelectedPiece;

    public SpriteRenderer CurrenteRenderer;

    public Sprite[] Sprites;

    public Transform SpriteContainer;

    public GameObject ObjectPrefab;

    private Camera camera;

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

        foreach(var sprite in Sprites)
        {
            var obj = Instantiate(ObjectPrefab, SpriteContainer, false);
            obj.GetComponent<Image>().sprite = sprite;
        }
    }

    private void Update()
    { 
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = Input.mousePosition;
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject.name);
            }
        }
        
    }
}
