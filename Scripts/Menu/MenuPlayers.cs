using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Käytetään single ja multiplayerissä eli sisältää ohjainten tiedot ja värit
/// </summary>
public class MenuPlayers : MonoBehaviour {

    public List<PlayerInfo> CurrentPlayers = new List<PlayerInfo>();

    private void OnEnable()
    {
        MenuPlayers[] objects = FindObjectsOfType<MenuPlayers>();
        if(objects.Length >= 2)
        {
            Destroy(this.gameObject);   
        }

    }
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public bool AnyoneHasParachute()
    {
        for(int i = 0; i < CurrentPlayers.Count; i++)
        {
            if (CurrentPlayers[i].Hasparachute)
                return true;
        }
        return false;
    }

    public int GetConquestPointSum()
    {
        int ret = 0;
        for(int i = 0; i < CurrentPlayers.Count; i++)
        {
            ret += CurrentPlayers[i].ConquestScore;
        }
        return ret;
    }
}
