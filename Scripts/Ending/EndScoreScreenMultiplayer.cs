using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScoreScreenMultiplayer : MonoBehaviour
{
    public GameObject endingCanvasSingle;
    public GameObject endingCanvasMulti;

    public Image background;
    public GameObject backgroundHolder;
    public GameObject winnerSpriteHolder;

    // Text for Score Names
    [Header("Singleplayer")]
    public Text surviveText_single;
    public Text bigTargetText_single;
    public Text debrisText_single;
    public Text parachuteText_single;
    public Text finalScoreText_single;
    // Text for Score values
    public Text surviveValue_single;
    public Text bigTargetValue_single;
    public Text debrisValue_single;
    public Text parachuteValue_single;
    public Text finalScoreValue_single;

    // for multiplayer
    [Header("Multiplayer")]
    public Text[] surviveText;
    public Text[] bigTargetText;
    public Text[] debrisText;
    public Text[] parachuteText;
    public Text[] finalScoreText;
    // Text for Score values
    public Text[] surviveValue;
    public Text[] bigTargetValue;
    public Text[] debrisValue;
    public Text[] parachuteValue;
    public Text[] finalScoreValue;

    public Image[] pLogo;
    public Sprite[] pSprite;

    [SerializeField]
    private bool shallWeContinue = false;
    private List<int> playerScores = new List<int>();
    
    public int STBValue = 0;
    public int BTPValue = 0;
    public int DPValue = 0;
    public int PBValue = 0;
    private int finalValue;
    
    // for testing - delete when done with score
    /*
    private int tempP1Score = 1020;
    private int tempP2Score = 3204;
    private int tempP3Score = 980;
    private int tempP4Score = 3203;*/
    // -----------------------------------------

    private void Update()
    {
        if (Utils.AnyButtonAnyControll() || Input.anyKey && shallWeContinue)
        {
            Debug.Log("Pressed to continue");
            /*if(SceneManager.GetActiveScene().buildIndex != 2)
            {
                // load next scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                // returning back to menu
                SceneManager.LoadScene(0);
            }*/
            SceneManager.LoadScene(1);
            
        }
    }

    private void Start()
    {
        /*
        STBValue = ScoreUpdate.SurviveScore();
        finalValue = STBValue +
                     BTPValue +
                     DPValue +
                     PBValue;
        */

        // getting scores to the list and sort them from highest to lowest
        for (int i = 0; i < FindObjectOfType<MenuPlayers>().CurrentPlayers.Count; i++)
        {
            //   get all current players final scores
            if (FindObjectOfType<MenuPlayers>().CurrentPlayers[i].Hasparachute)
            {
                playerScores.Add(FindObjectOfType<MenuPlayers>().CurrentPlayers[i].CountScore() + 5000);
                FindObjectOfType<MenuPlayers>().CurrentPlayers[i].WonPlanets |= FindObjectOfType<GameSceneManager>().CurrentMap;
                Debug.Log(FindObjectOfType<MenuPlayers>().CurrentPlayers[i].WonPlanets.ToString());

            }
            else
            {
                playerScores.Add(FindObjectOfType<MenuPlayers>().CurrentPlayers[i].CountScore());
            }
        }


        // for testing - delete when done with score
        /*playerScores.Add(tempP1Score);
        playerScores.Add(tempP2Score);
        playerScores.Add(tempP3Score);
        playerScores.Add(tempP4Score);*/
        // -----------------------------------------

        playerScores.Sort();
        playerScores.Reverse();

        // vaihda pelaajien määräksi
        /*if (playerScores.Count > 1)
        {*/
            endingCanvasMulti.SetActive(true);
            /*
        }
        else
        {
            endingCanvasSingle.SetActive(true);
        }*/

        Debug.Log("CURRENT PLAYERS" + FindObjectOfType<MenuPlayers>().CurrentPlayers.Count);

        FadeInBackground();
        FadeInSurvive();
        Invoke("FadeInDebris", 0.2f);
        Invoke("FadeInBigTarget", 0.4f);
        Invoke("FadeInParachute", 0.6f);
        Invoke("FadeInFinal", 0.8f);
    }

    private void FadeInSurvive()
    {
        // vaihda pelaajien määräksi
        //if (playerScores.Count > 1) // use multiplayer text elements
        //{
            for (int i = 0; i < FindObjectOfType<MenuPlayers>().CurrentPlayers.Count; i++)
            {
                List<ScoreInformation> debris = FindObjectOfType<MenuPlayers>().CurrentPlayers[i].Scores.FindAll(s => s.obsType == FlyerProperties.ObstacleType.Debris);
                List<ScoreInformation> bigTarget = FindObjectOfType<MenuPlayers>().CurrentPlayers[i].Scores.FindAll(s => s.obsType == FlyerProperties.ObstacleType.BigTarget);
                int total = FindObjectOfType<MenuPlayers>().CurrentPlayers[i].CountScore();

                int debrisSum = 0;
                for(int d = 0; d < debris.Count; d++)
                {
                    debrisSum += debris[d].Score;
                }

                int bigTargetSum = 0;
                for (int b = 0; b < bigTarget.Count; b++)
                {
                    bigTargetSum += bigTarget[b].Score;
                }

                surviveValue[i].text = (total + debrisSum + bigTargetSum).ToString(); // player's survive score
                surviveText[i].text = "Survive Bonus:";
                surviveText[i].canvasRenderer.SetAlpha(0.01f);
                surviveValue[i].canvasRenderer.SetAlpha(0.01f);
                surviveText[i].CrossFadeAlpha(1.0f, 0.5f, false);
                surviveValue[i].CrossFadeAlpha(1.0f, 0.5f, false);

            }
        /*}
        else // this is singleplayer, use singleplayer text elements only
        {
            List<ScoreInformation> debris = FindObjectOfType<MenuPlayers>().CurrentPlayers[0].Scores.FindAll(s => s.obsType == FlyerProperties.ObstacleType.Debris);
            List<ScoreInformation> bigTarget = FindObjectOfType<MenuPlayers>().CurrentPlayers[0].Scores.FindAll(s => s.obsType == FlyerProperties.ObstacleType.BigTarget);
            int total = FindObjectOfType<MenuPlayers>().CurrentPlayers[0].CountScore();

            int debrisSum = 0;
            for (int d = 0; d < debris.Count; d++)
            {
                debrisSum += debris[d].Score;
            }

            int bigTargetSum = 0;
            for (int b = 0; b < bigTarget.Count; b++)
            {
                bigTargetSum += bigTarget[b].Score;
            }

            surviveValue_single.text = (total + debrisSum + bigTargetSum).ToString(); // player's survive score
            surviveText_single.text = "Survive Bonus:";
            surviveText_single.canvasRenderer.SetAlpha(0.01f);
            surviveValue_single.canvasRenderer.SetAlpha(0.01f);
            surviveText_single.CrossFadeAlpha(1.0f, 0.5f, false);
            surviveValue_single.CrossFadeAlpha(1.0f, 0.5f, false);
        }*/

    }

    private void FadeInBigTarget()
    {
        // vaihda pelaajien määräksi
        //if (playerScores.Count > 1) // use multiplayer text elements
        //{
            for (int i = 0; i < FindObjectOfType<MenuPlayers>().CurrentPlayers.Count; i++)
            {
            List<ScoreInformation> bigTarget = FindObjectOfType<MenuPlayers>().CurrentPlayers[i].Scores.FindAll(s => s.obsType == FlyerProperties.ObstacleType.BigTarget);
                
                int bigTargetSum = 0;
                for (int b = 0; b < bigTarget.Count; b++)
                {
                    bigTargetSum += bigTarget[b].Score;
                }
                
                bigTargetValue[i].text = "-" + bigTargetSum; // player's big target penalty score
                bigTargetText[i].text = "Big Target Penalty:";
                bigTargetText[i].canvasRenderer.SetAlpha(0.01f);
                bigTargetValue[i].canvasRenderer.SetAlpha(0.01f);
                bigTargetText[i].CrossFadeAlpha(1.0f, 0.5f, false);
                bigTargetValue[i].CrossFadeAlpha(1.0f, 0.5f, false);
            }

        /*}
        else // this is singleplayer, use singleplayer text elements only
        {
            List<ScoreInformation> bigTarget = FindObjectOfType<MenuPlayers>().CurrentPlayers[0].Scores.FindAll(s => s.obsType == FlyerProperties.ObstacleType.BigTarget);

            int bigTargetSum = 0;
            for (int b = 0; b < bigTarget.Count; b++)
            {
                bigTargetSum += bigTarget[b].Score;
            }
            
            bigTargetValue_single.text = "-" + bigTargetSum;
            bigTargetText_single.text = "Big Target Penalty:";
            bigTargetText_single.canvasRenderer.SetAlpha(0.01f);
            bigTargetValue_single.canvasRenderer.SetAlpha(0.01f);
            bigTargetText_single.CrossFadeAlpha(1.0f, 0.5f, false);
            bigTargetValue_single.CrossFadeAlpha(1.0f, 0.5f, false);
        }*/

    }

    private void FadeInDebris()
    {
        // vaihda pelaajien määräksi
        //if (playerScores.Count > 1) // use multiplayer text elements
        //{
            for (int i = 0; i < FindObjectOfType<MenuPlayers>().CurrentPlayers.Count; i++)
            {
            List<ScoreInformation> debris = FindObjectOfType<MenuPlayers>().CurrentPlayers[i].Scores.FindAll(s => s.obsType == FlyerProperties.ObstacleType.Debris);

                int debrisSum = 0;
                for (int d = 0; d < debris.Count; d++)
                {
                    debrisSum += debris[d].Score;
                }

                debrisValue[i].text = "-" + debrisSum; // player's debris penalty score
                debrisText[i].text = "Debris Penalty:";
                debrisText[i].canvasRenderer.SetAlpha(0.01f);
                debrisValue[i].canvasRenderer.SetAlpha(0.01f);
                debrisText[i].CrossFadeAlpha(1.0f, 0.5f, false);
                debrisValue[i].CrossFadeAlpha(1.0f, 0.5f, false);
            }

        /*}
        else // this is singleplayer, use singleplayer text elements only
        {
            List<ScoreInformation> debris = FindObjectOfType<MenuPlayers>().CurrentPlayers[0].Scores.FindAll(s => s.obsType == FlyerProperties.ObstacleType.Debris);

            int debrisSum = 0;
            for (int d = 0; d < debris.Count; d++)
            {
                debrisSum += debris[d].Score;
            }

            debrisValue_single.text = "-" + debrisSum;
            debrisText_single.text = "Debris Penalty:";
            debrisText_single.canvasRenderer.SetAlpha(0.01f);
            debrisValue_single.canvasRenderer.SetAlpha(0.01f);
            debrisText_single.CrossFadeAlpha(1.0f, 0.5f, false);
            debrisValue_single.CrossFadeAlpha(1.0f, 0.5f, false);
        }*/

    }

    private void FadeInParachute()
    {
        // vaihda pelaajien määräksi
        //if (playerScores.Count > 1) // use multiplayer text elements
        //{
            for (int i = 0; i < FindObjectOfType<MenuPlayers>().CurrentPlayers.Count; i++)
            {
            if (FindObjectOfType<MenuPlayers>().CurrentPlayers[i].Hasparachute)
                {
                    parachuteValue[i].text = "5000"; // player's parachute bonus score
                    FindObjectOfType<MenuPlayers>().CurrentPlayers[i].ConquestScore += 1;

                }
            else
                {
                    parachuteValue[i].text = "Retired"; // player's parachute bonus score
                }
                    
                parachuteText[i].text = "Parachute Bonus:";
                parachuteText[i].canvasRenderer.SetAlpha(0.01f);
                parachuteValue[i].canvasRenderer.SetAlpha(0.01f);
                parachuteText[i].CrossFadeAlpha(1.0f, 0.5f, false);
                parachuteValue[i].CrossFadeAlpha(1.0f, 0.5f, false);
            }

        /*}
        else // this is singleplayer, use singleplayer text elements only
        {
            if (FindObjectOfType<MenuPlayers>().CurrentPlayers[0].Hasparachute)
            {
                parachuteValue_single.text = "5000"; // player's parachute bonus score
            }
            else
            {
                parachuteValue_single.text = "--"; // player's parachute bonus score
            }
            
            parachuteText_single.text = "Parachute Bonus:";
            parachuteText_single.canvasRenderer.SetAlpha(0.01f);
            parachuteValue_single.canvasRenderer.SetAlpha(0.01f);
            parachuteText_single.CrossFadeAlpha(1.0f, 0.5f, false);
            parachuteValue_single.CrossFadeAlpha(1.0f, 0.5f, false);
        }*/

    }

    private void FadeInFinal()
    {
        // vaihda pelaajien määräksi
        //if (playerScores.Count > 1) // use multiplayer text elements
       // {
            for (int i = 0; i < FindObjectOfType<MenuPlayers>().CurrentPlayers.Count; i++)
            {
            finalScoreValue[i].text = playerScores[i].ToString(); // player's final score
                finalScoreText[i].text = "Final Score:";
                finalScoreText[i].canvasRenderer.SetAlpha(0.01f);
                finalScoreValue[i].canvasRenderer.SetAlpha(0.01f);
                finalScoreText[i].CrossFadeAlpha(1.0f, 0.5f, false);
                finalScoreValue[i].CrossFadeAlpha(1.0f, 0.5f, false);

                pLogo[i].sprite = pSprite[i];

                // determine, who has this score rank and set sprite to corresponding player
               /* if (tempP1Score == playerScores[i])
                    pLogo[i].sprite = pSprite[0];

                if (tempP2Score == playerScores[i])
                    pLogo[i].sprite = pSprite[1];

                if (tempP3Score == playerScores[i])
                    pLogo[i].sprite = pSprite[2];

                if (tempP4Score == playerScores[i])
                    pLogo[i].sprite = pSprite[3];*/
            }

       /* }
        else // this is singleplayer, use singleplayer text elements only
        {
            finalScoreValue_single.text = playerScores[0].ToString();
            finalScoreText_single.text = "Final Score:";
            finalScoreText_single.canvasRenderer.SetAlpha(0.01f);
            finalScoreValue_single.canvasRenderer.SetAlpha(0.01f);
            finalScoreText_single.CrossFadeAlpha(1.0f, 0.5f, false);
            finalScoreValue_single.CrossFadeAlpha(1.0f, 0.5f, false);
        }*/

        Invoke("DeclareWinner", 1.0f);
    }

    private void FadeInBackground()
    {
        Debug.Log("SCORE SCREEN FADED");
        backgroundHolder.SetActive(true);
        background.canvasRenderer.SetAlpha(0.01f);
        background.CrossFadeAlpha(0.7f, 0.5f, false);
    }

    public void DeclareWinner()
    {
        winnerSpriteHolder.SetActive(true);
        Debug.Log("YOU CAN NOW CONTINUE");
        shallWeContinue = true;
    }
}




