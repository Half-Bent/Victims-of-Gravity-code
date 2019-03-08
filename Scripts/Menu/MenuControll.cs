using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControll : MonoBehaviour {

    [SerializeField]
    private GameObject OptionPanel;
    [SerializeField]
    private GameObject MainPanel;
    [SerializeField]
    private GameObject StageSelectionPanel;
    [SerializeField]
    private GameObject StoryPanel;

    [SerializeField]
    private GameObject TutorialFirstPanel;
    [SerializeField]
    private GameObject TutorialSecondPanel;
    [SerializeField]
    private GameObject TutorialThirdPanel;
    [SerializeField]
    private GameObject TutorialFourthPanel;
    [SerializeField]
    private GameObject TutorialFifthPanel;
    [SerializeField]
    private GameObject TutorialSixthPanel;

    // Sprite groups for different menu screens
    public Graphic[] titleScreen;
    public Graphic[] selectionScreen;
    public Graphic loadingScreen;
    public Graphic storyScreen;

    
    // Description boxes
    public GameObject storybox;
    public GameObject custombox;
    
    // For tracking if pulling out loading screen is required
    private static bool isSceneLoaded;
    private Vector2 loadingScreenOrignalPos;

    private AudioSource audioSource;
    public AudioClip audioClip;

    AsyncOperation async;

    // Use this for initialization
    void Start ()
    {
        audioSource = GetComponent<AudioSource>();

        loadingScreenOrignalPos = loadingScreen.transform.position;
        if (isSceneLoaded)
        {
            StartCoroutine(LoadingScreenPullOut(loadingScreen));
            isSceneLoaded = false;
        }
	}

    // Update is called once per frame
    void Update () {
		
	}

    // UI manipulation

    IEnumerator FadeImage(Graphic sr, bool fadeAway)
    {
        if (fadeAway)
        {
            for(float i = 1; i >= 0; i -= Time.deltaTime*2)
            {
                sr.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
        else
        {
            for(float i = 0; i <= 1; i += Time.deltaTime*2)
            {
                sr.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }




    // Method to pull in or out the loadingscreen
    IEnumerator LoadingScreenPullIn(Graphic screen, int scene)
    {
        screen.transform.position = loadingScreenOrignalPos;
        Vector2 startPos = screen.transform.position;
        async = SceneManager.LoadSceneAsync(scene);
        async.allowSceneActivation = false;
        Rigidbody2D screenRB = screen.GetComponent<Rigidbody2D>();
        screenRB.velocity = new Vector2(-Screen.width * 3, 0.0f);
        isSceneLoaded = true;
        
        while(!async.isDone)
        {
            // Checks when loading screen is in place and then progress
            if (screen.transform.position.x <= (startPos.x - Screen.width * 3.1))
            {
                screenRB.velocity = Vector2.zero;
                if (async.progress == 0.9f)
                    async.allowSceneActivation = true;
            }
            
            yield return null;
        }
    }

    IEnumerator LoadingScreenPullOut(Graphic screen)
    {
        screen.transform.position = new Vector2(screen.transform.position.x - Screen.width * 3, screen.transform.position.y);
        Vector2 startPos = screen.transform.position;
        Rigidbody2D screenRB = screen.GetComponent<Rigidbody2D>();
        screenRB.velocity = new Vector2(-Screen.width * 3, 0.0f);
        bool isPullOutDone = false;
        while(!isPullOutDone)
        {
            // When loadingscreen is out of the way, stop it
            if (screen.transform.position.x <= (startPos.x - Screen.width * 3.5))
            {
                screenRB.velocity = Vector2.zero;
            }
                
            yield return null;
        }
    }




    // Button functions

    public void OnMouseEnterStoryBox()
    {
        storybox.SetActive(true);
    }

    public void OnMouseLeaveStoryBox()
    {
        storybox.SetActive(false);
    }

    public void OnMouseEnterCustomBox()
    {
        custombox.SetActive(true);
    }

    public void OnMouseLeaveCustomBox()
    {
        custombox.SetActive(false);
    }




    public void FadeTitleScreen(bool fade)
    {
        for (int i = 0; i <= titleScreen.Length - 1; i++)
        {
            StartCoroutine(FadeImage(titleScreen[i], fade));
        }
    }

    public void EnableOptions()
    {
        MainPanel.SetActive(false);
        OptionPanel.SetActive(true);
    }

    public void BackOptions()
    {
        MainPanel.SetActive(true);
        OptionPanel.GetComponent<Options>().SavesSettings();
        OptionPanel.SetActive(false);
    }


    public void EnableStageSelection()
    {
        MainPanel.SetActive(false);
        StageSelectionPanel.SetActive(true);
        FadeTitleScreen(true);
        StartCoroutine(FadeImage(selectionScreen[0], false));
    }

    public void BackSelection()
    {
        MainPanel.SetActive(true);
        StageSelectionPanel.SetActive(false);
        for (int i = 0; i <= titleScreen.Length - 1; i++)
        {
            StartCoroutine(FadeImage(titleScreen[i], false));
        }
        StartCoroutine(FadeImage(selectionScreen[0], true));
    }

    public void EnableStory()
    {
        StageSelectionPanel.SetActive(false);
        StoryPanel.SetActive(true);
        StartCoroutine(FadeImage(selectionScreen[0], true));
    }

    public void BackStory()
    {
        StageSelectionPanel.SetActive(true);
        StoryPanel.SetActive(false);
        StartCoroutine(FadeImage(selectionScreen[0], false));
    }


    // Tutorials

    public void EnableTutorialFirst()
    {
        TutorialFirstPanel.SetActive(true);
        StageSelectionPanel.SetActive(false);
        StartCoroutine(FadeImage(selectionScreen[0], true));
    }

    public void EnableTutorialSecond()
    {
        TutorialSecondPanel.SetActive(true);
        TutorialFirstPanel.SetActive(false);
    }

    public void EnableTutorialThird()
    {
        TutorialThirdPanel.SetActive(true);
        TutorialSecondPanel.SetActive(false);
    }

    public void EnableTutorialFourth()
    {
        TutorialFourthPanel.SetActive(true);
        TutorialThirdPanel.SetActive(false);
    }

    public void EnableTutorialFifth()
    {
        TutorialFifthPanel.SetActive(true);
        TutorialFourthPanel.SetActive(false);
    }

    public void EnableTutorialSixth()
    {
        TutorialSixthPanel.SetActive(true);
        TutorialFifthPanel.SetActive(false);
    }

    public void ExitTutorial()
    {
        TutorialFirstPanel.SetActive(false);
        TutorialSecondPanel.SetActive(false);
        TutorialThirdPanel.SetActive(false);
        TutorialFourthPanel.SetActive(false);
        TutorialFifthPanel.SetActive(false);
        TutorialSixthPanel.SetActive(false);

        StageSelectionPanel.SetActive(true);
        StartCoroutine(FadeImage(selectionScreen[0], false));
    }



    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame(int scene)
    {
        if(FindObjectOfType<MenuPlayers>().CurrentPlayers.Count >= 1)
            StartCoroutine(LoadingScreenPullIn(loadingScreen, scene));
        //StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        while (!load.isDone)
        {
            yield return null;
        }
    }

    public void PlaySound()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}