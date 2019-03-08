using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdate : MonoBehaviour {

    public Text PlayerScore;
    public GameObject parachuteIndicatorParent;
    public Sprite mult1;
    public Sprite mult2;
    public Sprite mult3;
    public Sprite mult4;
    public static int logDifficulty = 4;
    private  int scoreMultiplier;
    private float t;
    private int surviveScore;
    private PlayerController currentPlayer;
    private int PlayerScore_;

    private Image p1_sr;

	// Use this for initialization
	void Start ()
    {
        t = logDifficulty;
        p1_sr = PlayerScore.GetComponentInChildren<Image>();
        currentPlayer = GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        
        if (!PlayerController.isControlDisabled)
        {
            t += Time.deltaTime;
            scoreMultiplier = Mathf.RoundToInt(Mathf.Log(t, logDifficulty));
            surviveScore += Mathf.RoundToInt(t / 11 * scoreMultiplier);// - 
            PlayerScore_ = surviveScore - FindObjectOfType<MenuPlayers>().CurrentPlayers.Find(s => s.Playerid == currentPlayer.CurrentController).CountScore();

            if (FindObjectOfType<MenuPlayers>().CurrentPlayers.Find(s => s.Playerid == currentPlayer.CurrentController).Hasparachute)
            {
                parachuteIndicatorParent.SetActive(true);
            }
            else
            {
                parachuteIndicatorParent.SetActive(false);
            }
                

            PlayerScore.text = PlayerScore_.ToString();

            // Changing multiplier icon
            switch (scoreMultiplier)
            {
                case 1:
                    p1_sr.sprite = mult1;
                    break;
                case 2:
                    p1_sr.sprite = mult2;
                    break;
                case 3:
                    p1_sr.sprite = mult3;
                    break;
                case 4:
                    p1_sr.sprite = mult4;
                    break;
                default:
                    p1_sr.sprite = mult1;
                    break;
            }
        }
    }



    public void Reset()
    {
        t = logDifficulty;
        Debug.Log("score multiplier reseted");
    }

    public int SurviveScore()
    {
        return surviveScore;
    }
}


