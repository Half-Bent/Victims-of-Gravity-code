using System.Collections;
using System.Collections.Generic;
using XboxCtrlrInput;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScoreScreen : MonoBehaviour {

    public GameObject endingCanvas;

    public Image background;

    // Text for Score Names
    public Text surviveText;
    public Text bigTargetText;
    public Text debrisText;
    public Text parachuteText;
    public Text finalScoreText;

    // Text for Score values
    public Text surviveValue;
    public Text bigTargetValue;
    public Text debrisValue;
    public Text parachuteValue;
    public Text finalScoreValue;

    // Score values
    public int STBValue = 0;
    public int BTPValue = -800;
    public int DPValue = -1250;
    public int PBValue = 500;
    private int finalValue;
    [SerializeField]
    private bool shallWeContinue = false;

    private void Update()
    {
        if(Utils.AnyButtonAnyControll() || Input.anyKey && shallWeContinue)
        {
            Debug.Log("Pressed to continue");
            SceneManager.LoadScene(0);
        }
    }

    private void Start()
    {
        //STBValue = ScoreUpdate.SurviveScore();

        finalValue = STBValue +
                     BTPValue +
                     DPValue +
                     PBValue;

        endingCanvas.SetActive(true);
        FadeInBackground();
        FadeInSurvive();
        Invoke("FadeInDebris", 0.2f);
        Invoke("FadeInBigTarget", 0.4f);
        Invoke("FadeInParachute", 0.6f);
        Invoke("FadeInFinal", 0.8f);
    }
    
    private void FadeInSurvive()
    {
        surviveValue.text = STBValue.ToString();
        surviveText.text = "Survive Bonus:";
        surviveText.canvasRenderer.SetAlpha(0.01f);
        surviveValue.canvasRenderer.SetAlpha(0.01f);
        surviveText.CrossFadeAlpha(1.0f, 0.5f, false);
        surviveValue.CrossFadeAlpha(1.0f, 0.5f, false);
    }

    private void FadeInBigTarget()
    {
        bigTargetValue.text = BTPValue.ToString();
        bigTargetText.text = "Big Target Penalty:";
        bigTargetText.canvasRenderer.SetAlpha(0.01f);
        bigTargetValue.canvasRenderer.SetAlpha(0.01f);
        bigTargetText.CrossFadeAlpha(1.0f, 0.5f, false);
        bigTargetValue.CrossFadeAlpha(1.0f, 0.5f, false);
    }

    private void FadeInDebris()
    {
        debrisValue.text = DPValue.ToString();
        debrisText.text = "Debris Penalty:";
        debrisText.canvasRenderer.SetAlpha(0.01f);
        debrisValue.canvasRenderer.SetAlpha(0.01f);
        debrisText.CrossFadeAlpha(1.0f, 0.5f, false);
        debrisValue.CrossFadeAlpha(1.0f, 0.5f, false);
    }

    private void FadeInParachute()
    {
        parachuteValue.text = PBValue.ToString();
        parachuteText.text = "Parachute Bonus:";
        parachuteText.canvasRenderer.SetAlpha(0.01f);
        parachuteValue.canvasRenderer.SetAlpha(0.01f);
        parachuteText.CrossFadeAlpha(1.0f, 0.5f, false);
        parachuteValue.CrossFadeAlpha(1.0f, 0.5f, false);
    }
    
    private void FadeInFinal()
    {
        finalScoreValue.text = finalValue.ToString();
        finalScoreText.text = "Final Score:";
        finalScoreText.canvasRenderer.SetAlpha(0.01f);
        finalScoreValue.canvasRenderer.SetAlpha(0.01f);
        finalScoreText.CrossFadeAlpha(1.0f, 0.5f, false);
        finalScoreValue.CrossFadeAlpha(1.0f, 0.5f, false);
        shallWeContinue = true;
    }

    private void FadeInBackground()
    {
        background.canvasRenderer.SetAlpha(0.01f);
        background.CrossFadeAlpha(0.7f, 0.5f, false);
    }
}

