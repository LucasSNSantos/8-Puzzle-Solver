using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class PuzzleUI : MonoBehaviour
{
    public Button ShuffleButton;
    public Button SolveButton;
    public Button EditButton;

    public Puzzle MyPuzzle;

    public SpriteRenderer CurrentRenderer;

    public Sprite[] Sprites;
    public Image CurrentSelectUI;

    public Transform SpriteContainerUI;

    public GameObject SelectorContainerUI;
    public GameObject ObjectPrefab;

    public TextMeshProUGUI StepsCounter;

    public bool Editing;

    public GameObject FireworkParticle1;
    public GameObject FireworkParticle2;

    public GameObject CircleFireParticle;

    public bool IsEditing
    {
        get
        {
            return Editing;
        }
        set
        {
            Editing = value;
            HandleEditing();
        }
    }
    private Camera MyCamera;

    void Start()
    {
        // add os eventos dos btns
        ShuffleButton.onClick.AddListener(() =>
        {
            if (IsEditing)
            {
                IsEditing = false;
            }

            if (MyPuzzle.IsSolving) return;
            
            MyPuzzle.ShuffleAction();
        });

        SolveButton.onClick.AddListener(() =>
        {
            if (IsEditing)
            {
                IsEditing = false;
            }
            
            if (MyPuzzle.IsShuffling) return;

            MyPuzzle.SolveAction();
        });

        foreach(var sprite in Sprites)
        {
            var obj = Instantiate(ObjectPrefab, SpriteContainerUI, false);
            obj.GetComponent<Image>().sprite = sprite;
            obj.GetComponent<Button>().onClick.AddListener(() =>
            {
                var selectedSprite = obj.GetComponent<Image>().sprite;
                
                var boxProps = CurrentRenderer.gameObject.GetComponent<BoxProperties>();

                var prevIndex = boxProps.Index;

                var arrayIndex = MyPuzzle.GetSpriteIndexOnList(CurrentRenderer.sprite);

                MyPuzzle.SetOfSprites[arrayIndex] = selectedSprite;

                MyPuzzle.GetSnapshotIndexFromSprite(selectedSprite, out int newIndex);
                boxProps.Index = newIndex;

                CurrentSelectUI.sprite = selectedSprite;
                CurrentRenderer.sprite = selectedSprite;

                Snapshot newGoal = new Snapshot(MyPuzzle.GoalSnapshot);

                var box = newGoal.Where(x => x.Index == prevIndex).FirstOrDefault();

                box.Index = newIndex;

                MyPuzzle.ResetAll();
                MyPuzzle.Init(newGoal);
            });
        }

        EditButton.onClick.AddListener(() => 
        {
            if (MyPuzzle.IsSolving || MyPuzzle.IsShuffling) return;

            IsEditing = !IsEditing;
        });

        MyPuzzle.OnSolved.Add(() =>
        {
            GameObject fire1 = Instantiate(FireworkParticle1, Vector2.one, Quaternion.identity);
            GameObject fire2 = Instantiate(FireworkParticle2, Vector2.one, Quaternion.identity);

            Destroy(fire1, 5f);
            Destroy(fire2, 5f);
        });

        MyCamera = Camera.main;
    }

    private void Update()
    {
        if (IsEditing && Input.GetMouseButtonDown(0))
        {
            var mousePos = MyCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                SelectorContainerUI.SetActive(true);
                CurrentRenderer = hit.collider.GetComponent<SpriteRenderer>();
                CurrentSelectUI.sprite = CurrentRenderer.sprite;
                IsEditing = false;
            }
        }

        if (MyPuzzle.IsSolving)
        {
            UpdateStepCounter(MyPuzzle.CurrentStep, MyPuzzle.Result.Count);

            CircleFireParticle.SetActive(true);
        } else
        {
            CircleFireParticle.SetActive(false);
        }
    }

    private void HandleEditing()
    {
        foreach (var box in MyPuzzle.CurrentSnapshot)
        {
            if (box.Index == MyPuzzle.MyEmpty)
            {
                box.Renderer.enabled = IsEditing;
            }
        }

        MyPuzzle.MountFromSnapshot(MyPuzzle.GoalSnapshot);
    }

    public void UpdateStepCounter(int step, int maxStep)
    {
        StepsCounter.text = $"{step}/{maxStep} passos";
    }

    public void CloseSelector()
    {
        SelectorContainerUI.SetActive(false);
    }
}
