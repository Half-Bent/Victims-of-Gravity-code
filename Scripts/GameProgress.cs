using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public class GameProgress : MonoBehaviour {

    private class ProgressTimers
    {
        public float first = 0;
        public float Secound = 0;
        private float firstProgres = 0;
        private bool DontUpdateFirst = false;
        public bool Transition = false;

        public float GetProgress()
        {
            if (DontUpdateFirst && !Transition)
                return (firstProgres + Secound) / 2;
            else
                return (first + Secound) / 2;
        }

        public void SetFirstToZero()
        {
            firstProgres = 1;
            first = 0;
            DontUpdateFirst = true;
        }
    }

    public GameObject World;
    public GameObject Surface;
    public GameObject FadeSprite;
    public GameObject EndGameobject;
    public float FadeTime;
    public float ProgressGlobal
    {
        get { return CurProgress.GetProgress(); }
    }

    private ProgressTimers CurProgress;
    private bool CloudTransition;
    private bool fadeCorotinOngoing = false;
    private float startTime;
    [SerializeField]
    private float time = 0;
    private GameSceneManager manager;
	// Use this for initialization
	void Start () {
        startTime = Time.time;
        manager = FindObjectOfType<GameSceneManager>();
        manager.StopSpawn = false;
        CurProgress = new ProgressTimers()
        {
            first = 0,
            Secound = 0
        };

    }

    bool SkipScene = false;

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.KeypadEnter))
        {
            World.GetComponent<MeshRenderer>().enabled = false;
            Surface.SetActive(true);
            SkipScene = true;
        }
        if (!CloudTransition) {
            if (World.GetComponent<MeshRenderer>().enabled)
            {
                if (CurProgress.first >= 1 && !CurProgress.Transition)
                {
                    CloudTransition = true;
                    manager.StopSpawn = CloudTransition;
                    CurProgress.SetFirstToZero();
                    CurProgress.Transition = true;
                }
                CurProgress.first = World.GetComponent<ScaleWorld>().GetCurrentProgress();

            }
            else if (Surface.activeSelf)
            {
                CurProgress.Secound = Surface.GetComponent<ScaleWorld>().GetCurrentProgress();
                if (SkipScene)
                    CurProgress.Secound = 1.1f;
                if(CurProgress.Secound >= 1)
                {
                    ///TODO add ending
                    EndGameobject.SetActive(true);
                    manager.StopSpawn = true;
                    manager.StopScore = true;
                    DeleteObjects();
                }
            }
        }

        if (CloudTransition)
        {
            
            time += Time.deltaTime;
            if (time >= FadeTime)
            {
                CloudTransition = false;
                manager.StopSpawn = CloudTransition;
                time = 0;
                Surface.GetComponent<ScaleWorld>().enabled = true;
                CurProgress.Transition = false;
                CurProgress.SetFirstToZero();
                Debug.Log("Transition ended");
            }

            if(time > FadeTime / 3 && !fadeCorotinOngoing)
            {
                StartCoroutine(WhiteFade(FadeTime / 3.0f));
                fadeCorotinOngoing = true;
                Debug.Log("Fade going");
            }

            if (time > FadeTime / 2.0f && !Surface.activeSelf)
            {
               
                Surface.SetActive(true);
                World.GetComponent<MeshRenderer>().enabled = false;
            }

        }
    }
    private IEnumerator WhiteFade(float fadeTime)
    {
        float time = 0;
        float tim = FadeTime;
        SpriteRenderer targetSprite = FadeSprite.GetComponent<SpriteRenderer>();
        while (time < 1)
        {
            targetSprite.color = new Color(targetSprite.color.r, targetSprite.color.g, targetSprite.color.b, Mathf.Lerp(targetSprite.color.a, 1, time));
            time += Time.deltaTime * (1f / (tim / 2));
            yield return null;
        }
        yield return null;
        time = 0;
        while (time < 1)
        {
            targetSprite.color = new Color(targetSprite.color.r, targetSprite.color.g, targetSprite.color.b, Mathf.Lerp(targetSprite.color.r, 0, time));
            time += Time.deltaTime * (1f / (tim / 2));
            yield return null;
        }
    }

    private void DeleteObjects()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("ObstacleObj");
        for(int i = 0; i < obj.Length; i++)
        {
            if (obj[i].activeSelf)
                Destroy(obj[i]);
        }
    }
}



/// <summary>
/// toteutetaan kunnolla jos saadaan tähän enemmän kuin 1 pelaaja
/// </summary>
[System.Serializable]
public class PlayerInfo
{
    [System.Flags]
    public enum Planets
    {
        Earth = 0x1,
        Moon = 0x2,
        Mars = 0x4,
        Sun = 0x8
    }

    public enum PlayerColors : byte
    {
        Blue = 0x1,
        Green,
        Orange,
        Purple,
        Dark
    }

    public Planets WonPlanets;

    public int ConquestScore = 0;

    public bool WonTheRound = false;

    public bool IsAlive { get; private set; }

    public GameObject PlayerScoreUI;

    public List<ScoreInformation> Scores
    {
        get { return score; }
        set { score = value; }
    }

    public PlayerColors CurrentColor
    {
        get { return currentColor; }
        private set { currentColor = value; }
    }

    public XboxController Playerid
    {
        get { return PlayerID; }
    }

    public bool UseKeyboard;

    [SerializeField]
    private XboxController PlayerID;

    [SerializeField]
    private PlayerColors currentColor;

    [SerializeField]
    private List<ScoreInformation> score;

    public bool Hasparachute = false;

    public GameSceneManager manager;

    public PlayerInfo(XboxController ID, PlayerColors playerColors)
    {
        IsAlive = true;
        PlayerID = ID;
        CurrentColor = playerColors;
        Scores = new List<ScoreInformation>();
    }

    public void AddScore(ScoreInformation info)
    {
        if(!manager.StopScore)
            Scores.Add(info);
    }

    public int CountScore()
    {
        int sum = 0;
        for(int i = 0; i < Scores.Count; i++)
        {
            sum += Scores[i].Score;
        }
        return sum;
    }

    public void SetDead()
    {
        IsAlive = false;
    }

    public Color GetColor()
    {
        switch (CurrentColor)
        {
            case PlayerColors.Blue:
                return Color.blue;
            case PlayerColors.Dark:
                return Color.black;
            case PlayerColors.Green:
                return Color.green;
            case PlayerColors.Orange:
                return new Color(1.2F, 0.64F, 0F);
            case PlayerColors.Purple:
                return new Color(0.33f, 0.10f, 0.54f);
        }
        ///Vain jos syystä x väriä ei löydy
        return Color.white;
    }
}

[System.Serializable]
public class ScoreInformation
{
    public int Score;
    public FlyerProperties.ObstacleType obsType;

    public override string ToString()
    {
        return "Score: " + Score + " ObstacleType " + obsType.ToString();
    }
}