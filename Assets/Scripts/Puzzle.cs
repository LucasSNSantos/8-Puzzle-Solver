using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour
{
    public GameObject BoxPrefab;

    public Snapshot CurrentSnapshot;

    public Snapshot GoalSnapshot;

    public Snapshot FromSnapshot;

    public Sprite[] SetOfSprites;

    public Transform Pivot;

    public EightPuzzleGraph EightPuzzleGraph;

    public float TempoMove = 0.5f;

    public int ShuffleMoves = 50;

    public int MyEmpty = 9;

    public List<int[,]> Result;

    public int CurrentStep = 0;

    public bool IsSolved = false;
    public bool IsShuffling { get; private set; } = false;
    public bool IsSolving { get; private set; } = false;

    public List<UnityAction> OnSolved;

    private void Awake()
    {
        OnSolved = new List<UnityAction>();
    }

    void Start()
    {
        NumberBox empty = new NumberBox(null, MyEmpty);

        EightPuzzleGraph = new EightPuzzleGraph(empty, 3, 3);

        Init();

        GoalSnapshot = new Snapshot(CurrentSnapshot);
    }

    public void ResetAll()
    {
        foreach(var obj in CurrentSnapshot)
        {
            if (obj.Instance != null)
            {
                Destroy(obj.Instance);
            }
        }

        CurrentSnapshot.Clear();
    }

    public void UpdateSnap(Snapshot snapshot)
    {
        foreach(var box in snapshot)
        {
            Sprite currentSprite = GetSpriteFromBox(box);

            box.Renderer.sprite = currentSprite;

            box.Renderer.enabled = box.Index != MyEmpty;
        }
    }

    public void Init(Snapshot defaultSnapshot = null)
    {
        CurrentSnapshot = new Snapshot();

        List<int> valores = new List<int>();

        foreach(var spr in SetOfSprites)
        {
            int indexSprite;
            var res = GetSnapshotIndexFromSprite(spr, out indexSprite);
            if (!res)
            {
                print("deu erro na conversao " + spr.name);
            }
            valores.Add(indexSprite);
        }

        //var valores = new int[]
        //{
        //    6, 7, 8,
        //    3, 4, 5,
        //    0, 1, 2
        //};


        foreach (var i in valores)
        {
            GameObject go = Instantiate(BoxPrefab, Vector2.zero, Quaternion.identity, Pivot);

            int newIndex = i;

            var prop = go.GetComponent<BoxProperties>();

            prop.Index = newIndex;

            CurrentSnapshot.Add(new NumberBox(go, newIndex));
        }

        MountFromSnapshot(CurrentSnapshot);

        GoalSnapshot = new Snapshot(CurrentSnapshot);
    }

    public bool GetSnapshotIndexFromSprite(Sprite spr, out int indexSprite)
    {
        return int.TryParse(spr.name.Split('_').LastOrDefault(), out indexSprite);
    }
    
    public void MountFromSnapshot(Snapshot snapshot)
    {
        Queue<NumberBox> qbox = new Queue<NumberBox>(snapshot);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var currentBox = qbox.Dequeue();
                
                Sprite currentSprite = GetSpriteFromBox(currentBox);

                currentBox.Init(j, i, currentSprite);

                GetSnapshotIndexFromSprite(currentSprite, out int newIndex);

                currentBox.Index = newIndex;

                currentBox.Instance.transform.SetPositionAndRotation(new Vector3(j, i), Quaternion.identity);

                currentBox.Instance.GetComponent<SpriteRenderer>().enabled = true;
            }
        }

        var whereZero = snapshot.Where(x => x.Index == MyEmpty).FirstOrDefault();

        whereZero.Instance.GetComponent<SpriteRenderer>().enabled = false;
    }

    public Sprite GetSpriteFromBox(NumberBox currentBox)
    {
        return SetOfSprites.FirstOrDefault(x => x.name.Split('_').LastOrDefault() == currentBox.Index.ToString());
    }

    public int GetSpriteIndexOnList(Sprite _sprite)
    {
        for(int i = 0; i < SetOfSprites.Length; i++)
        {
            if (SetOfSprites[i] == _sprite)
            {
                return i;
            }
        }
        return -1;
    }

    private IEnumerator Shuffle(int maxCount)
    {
        IsShuffling = true;

        for (int i = 0; i < maxCount; i++)
        {
            var neighbors = EightPuzzleGraph.Neighbors(CurrentSnapshot);

            var theChoosenOne = neighbors.OrderBy(x => UnityEngine.Random.Range(0, 9999)).FirstOrDefault();

            UpdateSnap(theChoosenOne);

            CurrentSnapshot = theChoosenOne;

            yield return new WaitForSecondsRealtime(TempoMove);
        }

        FromSnapshot = new Snapshot(CurrentSnapshot);
        IsShuffling = false;
    }

    private async void AskServer()
    {
        var url = "http://localhost:8000/";

        using (var client = new HttpClient())
        {
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            var baseAddress = url;
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Accept.Add(contentType);

            var data = new Dictionary<string, string>
            {
                {"origin", JsonConvert.SerializeObject(GoalSnapshot.GetAsMatrix()) },
                {"start", JsonConvert.SerializeObject(FromSnapshot.GetAsMatrix())},
                {"empty", MyEmpty.ToString()}
            };

            var jsonData = JsonConvert.SerializeObject(data);
            var contentData = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/", contentData);

            if (response.IsSuccessStatusCode)
            {
                var stringData = await response.Content.ReadAsStringAsync();
                Result = JsonConvert.DeserializeObject<List<int[,]>>(stringData);
                StartCoroutine(Solve());
            }
        }
    }

    private void MountFromServer(int[,] snap)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var currentBox = CurrentSnapshot.Where(x => x.XPos == i && x.YPos == j).FirstOrDefault();

                currentBox.Index = snap[i, j];
            }
        }

        UpdateSnap(CurrentSnapshot);
    }

    private IEnumerator Solve()
    {
        IsSolving = true;

        CurrentStep = 0;

        foreach(var snap in Result)
        {
            MountFromServer(snap);

            CurrentStep++;

            yield return new WaitForSecondsRealtime(TempoMove);
        }

        IsSolved = true;

        foreach(var callback in OnSolved)
        {
            callback?.Invoke();
        }

        IsSolving = false;
    }

    public void ShuffleAction()
    {
        if (IsShuffling) return;
        StartCoroutine(Shuffle(ShuffleMoves));
    }

    public void SolveAction()
    {
        if (IsSolving) return;
        AskServer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShuffleAction();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            SolveAction();
        }
    }
}
