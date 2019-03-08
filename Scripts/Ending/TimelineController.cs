using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TimelineController : MonoBehaviour {

    // temp variable, handle this in gamemanager maybe?
    public bool parachuted = false;
    public GameObject parachute;
    public PlayableDirector singleWinTL;
    public PlayableDirector singleLoseTL;
    public Image bg;
    // The Default Gameplay Camera to disable to run the ending scenes
    public GameObject ingameUI;
    // insert the players from first place to last place
    public GameObject[] playersInEnding;
    public List<CharacterMaterialInfo> CharacterMaterials = new List<CharacterMaterialInfo>();

    private void Start()
    {
        //bg.canvasRenderer.SetAlpha(0.01f);
    }

    public void Awake()
    {
        if (ingameUI != null)
            ingameUI.SetActive(false);

        Debug.Log("control disabled");
        PlayerController.DisableControl();
        List<PlayerInfo> Players = FindObjectOfType<MenuPlayers>().CurrentPlayers;
        Players = Players.OrderByDescending(s => s.CountScore()).ToList();
        if (FindObjectOfType<MenuPlayers>().AnyoneHasParachute())
        {
            int index = 0;
            List<PlayerInfo> temp = new List<PlayerInfo>(Players);
            index = GetParachuteIndex(temp);
            temp.RemoveAt(index);
            temp = temp.OrderByDescending(s => s.CountScore()).ToList();
            index = GetParachuteIndex(Players);
            temp.Insert(0, Players[index]);
            Players = temp;
        }
        Debug.Log(Players[0].CurrentColor.ToString() +" " + Players[0].Playerid.ToString() + " " + Players[0].Hasparachute);
        for (int i = 0; i < Players.Count; i++)
        {
            playersInEnding[i].SetActive(true);
            // vaihda värit oikeiksi (voittajalle voittajapelaajan värit ...)
            // ehkä metodi MenuPlayersiin, josta saa irrotettua, kuka pelaajista omistaa suurimman pistesaldon jne
            int materialIndex = 0;
            for (int y = 0; y < CharacterMaterials.Count; y++)
            {
                if (Players[i].CurrentColor == CharacterMaterials[y].BindingColor) { 
                    materialIndex = y;
                    break;
                }

            }
            playersInEnding[i].GetComponentInChildren<Renderer>().material = CharacterMaterials[materialIndex].CharacterMat;//sijalla i+1 olevan pelaajan väri;


        }
        parachuted = FindObjectOfType<MenuPlayers>().AnyoneHasParachute();
        if (parachuted)
        {
            parachute.GetComponent<SpriteRenderer>().enabled = true;
        }
        Play(parachuted);
    }

    private int GetParachuteIndex(List<PlayerInfo> curPlayers)
    {
        for(int i = 0; i < curPlayers.Count; i++)
        {
            if (curPlayers[i].Hasparachute)
                return i;
        }
        return -1;
    }

    public void Activate()
    {
        
    }

    public void Play(bool parachuted)
    {
        // if player has parachute, play the "win" scene
        if (parachuted)
        {
            singleWinTL.Play();
        }
        // otherwise play the "lose" scene
        else
        {
            singleLoseTL.Play();
        }
    }
}
[System.Serializable]
public class CharacterMaterialInfo
{
    public Material CharacterMat;
    public PlayerInfo.PlayerColors BindingColor;
}


