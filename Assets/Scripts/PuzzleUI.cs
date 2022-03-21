using UnityEngine;
using UnityEngine.UI;

public class PuzzleUI : MonoBehaviour
{
    public Button ShuffleButton;
    public Button SolveButton;

    public Puzzle MyPuzzle;

    public SpriteRenderer CurrentRenderer;

    public Image CurrentSelect;
    public Sprite[] Sprites;

    public Transform SpriteContainer;

    public GameObject Selector;
    public GameObject ObjectPrefab;

    private Camera MyCamera;

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

        MyCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = MyCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                CurrentRenderer = hit.collider.GetComponent<SpriteRenderer>();
                Selector.SetActive(false);
            }
        }
    }
}
