using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;
/// <summary>
/// pelaajien valinta
/// </summary>
public class CharacterSelectMenu : MonoBehaviour {

    public List<MenuCharacterPlace> characters = new List<MenuCharacterPlace>();
    public MenuPlayers PlayersInformation;
    private int characterCount = 0;
    [SerializeField]
    private int ControllerCount;


   private class menuCharacterInfo
    {
        public int index;
        public XboxController id;
    }

    menuCharacterInfo[] character;

    private void Awake()
    {
        XCI.DEBUG_LogControllerNames();
        character = new[]
        {
            new menuCharacterInfo
            {
                index = -1,
                id = XboxController.First
            },
            new menuCharacterInfo
            {
                index = -1,
                id = XboxController.Second
            },
            new menuCharacterInfo
            {
                index = -1,
                id = XboxController.Third
            },
            new menuCharacterInfo
            {
                index = -1,
                id = XboxController.Fourth
            },
        };
    }

    // Use this for initialization
    void Start () {
        PlayersInformation = FindObjectOfType<MenuPlayers>();
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!characters[0].InUse)
                characters[0].SetActive(XboxController.First, true);
            else
                characters[0].Disable();
        }

		if(XCI.GetButtonDown(XboxButton.A, XboxController.First))
        {
            if (!characters[0].InUse)
                characters[0].SetActive(XboxController.First, false);
            else
                characters[0].Disable();
        }

        if(XCI.GetButtonDown(XboxButton.A, XboxController.Second))
        {
            if (!characters[1].InUse)
                characters[1].SetActive(XboxController.Second, false);
            else
                characters[1].Disable();
        }

        if (XCI.GetButtonDown(XboxButton.A, XboxController.Third))
        {
            if (!characters[2].InUse)
                characters[2].SetActive(XboxController.Third, false);
            else
                characters[2].Disable();
        }

        if (XCI.GetButtonDown(XboxButton.A, XboxController.Fourth))
        {
            if (!characters[3].InUse)
                characters[3].SetActive(XboxController.Fourth, false);
            else
                characters[3].Disable();
        }
    }


    public void PlayGame()
    {
        foreach(MenuCharacterPlace chars in characters)
        {
            if (chars.InUse)
            {
                Debug.Log(chars.currentController + " " + chars.PlayerColor);
                PlayersInformation.CurrentPlayers.Add(new PlayerInfo(chars.currentController, chars.PlayerColor));
                PlayersInformation.CurrentPlayers[PlayersInformation.CurrentPlayers.Count - 1].manager = PlayersInformation.GetComponent<GameSceneManager>();
                if (chars.UseKeyboard)
                {
                    PlayersInformation.CurrentPlayers[PlayersInformation.CurrentPlayers.Count - 1].UseKeyboard = true;
                }
            }
        }

    }
}
[System.Serializable]
public class MenuCharacterPlace
{
    //Menussa käytetään
    public GameObject CharacterInfo;
    public GameObject ControllerSprite;
    public GameObject KeyboardSprite;
    //

    public PlayerInfo.PlayerColors PlayerColor;

    //public MenuPlayerColorSelect menuPlayer;
    public XboxController currentController;
    public bool InUse = false;
    public bool UseKeyboard;
    public void SetTextAndPlace(int id)
    {
    }
    public void SetActive(XboxController id, bool useKeyboard)
    {
        currentController = id;
        CharacterInfo.SetActive(true);
        InUse = true;
        Debug.Log(id + " IsActive");
        UseKeyboard = useKeyboard;
        if (!useKeyboard)
        {
            ControllerSprite.SetActive(true);
        }
        else
        {
            KeyboardSprite.SetActive(true);
        }
    }
    public void Disable()
    {
        currentController = XboxController.Any;
        CharacterInfo.SetActive(false);
        InUse = false;
        if (ControllerSprite.activeSelf)
            ControllerSprite.SetActive(false);
        if (KeyboardSprite.activeSelf)
            KeyboardSprite.SetActive(false);
    }
}