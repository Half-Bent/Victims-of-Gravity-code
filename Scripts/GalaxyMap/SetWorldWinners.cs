using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWorldWinners : MonoBehaviour
{
    [System.Serializable]
    public class FlagSprites
    {
        public GameObject WinFlag;
        public bool HasPlayed;
    }
    public AnimationClip animationClip;
    public GameObject winnerObject;
    [Header(header: "Flag information")]
    public FlagSprites earthsprite;
    public FlagSprites MoonSprite;
    public FlagSprites MarsSprite;
    public FlagSprites SunSprite;

    PlayerInfo.Planets usedPlanet = PlayerInfo.Planets.Earth;
    bool CanContinue = false;
    PlayerInfo s;
    // Start is called before the first frame update
    void Start()
    {
        s = FindObjectOfType<MenuPlayers>().CurrentPlayers.Find(x => x.UseKeyboard);

        usedPlanet = FindObjectOfType<GameSceneManager>().CurrentMap;
        MenuPlayers Players = FindObjectOfType<MenuPlayers>();

        for(int i = 0; i < Players.CurrentPlayers.Count; i++)
        {
            if((Players.CurrentPlayers[i].WonPlanets & PlayerInfo.Planets.Earth) == PlayerInfo.Planets.Earth)
            {
                earthsprite.WinFlag.GetComponent<SpriteRenderer>().color = Players.CurrentPlayers[i].GetColor();
                earthsprite.HasPlayed = true;
            }
            if ((Players.CurrentPlayers[i].WonPlanets & PlayerInfo.Planets.Mars) == PlayerInfo.Planets.Mars)
            {
                MarsSprite.WinFlag.GetComponent<SpriteRenderer>().color = Players.CurrentPlayers[i].GetColor();
                MarsSprite.HasPlayed = true;

            }
            if ((Players.CurrentPlayers[i].WonPlanets & PlayerInfo.Planets.Moon) == PlayerInfo.Planets.Moon)
            {
                MoonSprite.WinFlag.GetComponent<SpriteRenderer>().color = Players.CurrentPlayers[i].GetColor();
                MoonSprite.HasPlayed = true;

            }
            if ((Players.CurrentPlayers[i].WonPlanets & PlayerInfo.Planets.Sun) == PlayerInfo.Planets.Sun)
            {
                SunSprite.WinFlag.GetComponent<SpriteRenderer>().color = Players.CurrentPlayers[i].GetColor();
                SunSprite.HasPlayed = true;

            }
        }

        StartCoroutine(InvokeAnimationFlag());

    }

    // Update is called once per frame
    void Update()
    {
        if (CanContinue)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                FindObjectOfType<GameSceneManager>().LoadNextScene();
            }
            if (Utils.AnyButtonAnyControll())
            {
                FindObjectOfType<GameSceneManager>().LoadNextScene();
            }
        }
    }

    IEnumerator InvokeAnimationFlag()
    {
        yield return new WaitForSeconds(0.3f);
        if (earthsprite.HasPlayed)
        {
            earthsprite.WinFlag.GetComponent<Animator>().SetTrigger("TurnTrigger");
        }
        yield return new WaitForSeconds(animationClip.length);

        if(MoonSprite.HasPlayed)
        {

            MoonSprite.WinFlag.GetComponent<Animator>().SetTrigger("TurnTrigger");
            yield return new WaitForSeconds(animationClip.length);
        }
        if(MarsSprite.HasPlayed)
        {
            MarsSprite.WinFlag.GetComponent<Animator>().SetTrigger("TurnTrigger");
            yield return new WaitForSeconds(animationClip.length);
        }
        if(SunSprite.HasPlayed)
        {
            SunSprite.WinFlag.GetComponent<Animator>().SetTrigger("TurnTrigger");
            yield return new WaitForSeconds(animationClip.length);
        }
        if(FindObjectOfType<MenuPlayers>().GetConquestPointSum() == 4)
        {
            winnerObject.SetActive(true);
        }
        CanContinue = true;
    }
}
