using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinnerDeclare : MonoBehaviour
{
    private MenuPlayers Players;

    // Start is called before the first frame update
    void Start()
    {
        Players = FindObjectOfType<MenuPlayers>();
        List<PlayerInfo> currentPlayers = Players.CurrentPlayers;
        int index = -1;
        int lastScore = -1;
        for(int i = 0; i < currentPlayers.Count; i++)
        {
            if(currentPlayers[i].ConquestScore > lastScore)
            {
                index = i;
                lastScore = currentPlayers[i].ConquestScore;
            }
        }

        GetComponent<TextMeshProUGUI>().text = "Winner is: " + currentPlayers[index].Playerid.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
