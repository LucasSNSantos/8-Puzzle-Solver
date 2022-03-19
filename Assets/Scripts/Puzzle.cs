using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public GameObject boxPrefab;

    public List<NumberBox> Boxes
    {
        get { return EightPuzzle.Nodes; }
    }
    public List<NumberBox> Origin;

    public Sprite[] sprite;

    public Astar<NumberBox> EightPuzzle;

    public float TempoMove = 0.5f;

    public bool IsSolved = false;

    void Start()
    {
        EightPuzzle = new EightPuzzle(3);

        Init();

        StartCoroutine(Shuffle(50));
    }

    public void ResetAll()
    {
        foreach(var obj in Boxes)
        {
            Destroy(obj.Instance);
        }
        Boxes.Clear();
    }

    public void Init()
    {

        for (int i = 0; i <= 8; i++)
        {
            GameObject go = Instantiate(boxPrefab, Vector2.zero, Quaternion.identity);
            Boxes.Add(new NumberBox(go, i));
        }

        Queue<NumberBox> qbox = new Queue<NumberBox>(Boxes);

        for (int i = 2; i >= 0; i--)
        {
            for (int j = 0; j < 3; j++)
            {
                var currentBox = qbox.Dequeue();
                
                currentBox.Init(j, i, sprite[currentBox.Index]);

                currentBox.Instance.transform.SetPositionAndRotation(new Vector3(j, i), Quaternion.identity);
            }
        }

        var whereZero = Boxes.Where(x => x.Index == 0).FirstOrDefault();

        whereZero.Instance.GetComponent<SpriteRenderer>().enabled = false;
        EightPuzzle.SetSnapshot(Boxes);
    }

    private IEnumerator Shuffle(int maxCount)
    {
        for (int i = 0; i < maxCount; i++)
        {
            var neighbors = EightPuzzle.GetNeighbors();

            var theChoosenOne = neighbors.OrderBy(x => Random.Range(0, 9999)).FirstOrDefault();

            Swap(theChoosenOne);

            yield return new WaitForSecondsRealtime(TempoMove);
        }
    }

    private IEnumerator Solve()
    {
        // logica para resolver o puzzle
        while(!IsSolved)
        {
            // var neighbors = EightPuzzle.GetNeighbors();

            var theChoosenOne = neighbors.OrderBy(x => Random.Range(0, 9999)).FirstOrDefault();
            
            Swap(theChoosenOne);
            yield return new WaitForSecondsRealtime(TempoMove);
        }
    }

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetAll();
            Init();
        }
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            StartCoroutine(Solve());
        }
    }
}
