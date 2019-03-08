using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameIntro : MonoBehaviour {

    // Need clouds
    public CinemachineVirtualCamera virtualCameraDefault;
    public float fovChange;
    public float fovDefault;
    public GameObject[] Players;
    public float playerDepth;
    public Image introFade;
    public float FadeOutDuration = 0.7f;
    public float FadeInDuration = 1f;

    // For activating UI
    public GameObject uiGroup;
    public GameObject[] playerScoreUI;
    
    // Use this for initialization
    void Start ()
    {
        PlayerController.DisableControl();
        Invoke("FadeOut", 0.5f);

        IntroCutscene();
    }

    private void IntroCutscene()
    {
        Camera.main.enabled = true;
        StopAllCoroutines();
        FadeOut();
        
        virtualCameraDefault.m_Lens.FieldOfView += fovChange;

        PlayerController.DisableControl();
        for (int i = 0; i < Players.Length; i++)
        {
            // Positions Players near Main Camera in circle
            Players[i].transform.position = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (360 / Players.Length * (i + 1) + (270 / Players.Length))) * 2 + Camera.main.transform.position.x,
                                                        Camera.main.transform.position.y + 1,
                                                        Mathf.Sin(Mathf.Deg2Rad * (360 / Players.Length * (i + 1) + (270 / Players.Length))) * 2 + Camera.main.transform.position.z - 0.25f);
        }

        Invoke("ActivateIntro", 0.6f);
        
    }

    private void ActivateIntro()
    {
        StartCoroutine(CameraZoomIn());
        StartCoroutine(MovePlayers());
        //StartCoroutine(CameraShake(virtualCameraDefault, new Vector3(0.4f, 0.4f, 0.4f), 0.6f, 0.5f));
        Invoke("EnablePlayerControls", 3f);
    }
    
    private void FadeOut()
    {
        introFade.canvasRenderer.SetAlpha(1.0f);
        introFade.CrossFadeAlpha(0.0f, FadeOutDuration, false);
    }

    private void FadeIn()
    {
        introFade.canvasRenderer.SetAlpha(0.0f);
        introFade.CrossFadeAlpha(1.0f, FadeInDuration, false);
    }

    IEnumerator CameraZoomIn()
    {
        float dif = Mathf.Abs(virtualCameraDefault.m_Lens.FieldOfView - fovDefault);
        while(dif > 0.05)
        {
            virtualCameraDefault.m_Lens.FieldOfView = Mathf.Lerp(virtualCameraDefault.m_Lens.FieldOfView, fovDefault, 0.1f);
            dif = Mathf.Abs(virtualCameraDefault.m_Lens.FieldOfView - fovDefault);
            yield return null;
        }
    }

    IEnumerator MovePlayers()
    {
        while(Players[0].transform.position.y >= playerDepth)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].transform.position = new Vector3(Players[i].transform.position.x,
                                                        Mathf.Lerp(Players[i].transform.position.y, playerDepth, 0.7f * Time.deltaTime),
                                                        Players[i].transform.position.z);
               
            }
            yield return null;
        }
    }

    private IEnumerator CameraShake(CinemachineVirtualCamera cam, Vector3 magnitude, float duration, float wavelength)
    {
        Vector3 startPos = cam.transform.localPosition;
        float endTime = Time.time + duration;
        float currentX = 0;

        while (Time.time < endTime)
        {
            Vector3 shakeAmount = new Vector3(
                Mathf.PerlinNoise(currentX, 5) - .5f,
                0,
                Mathf.PerlinNoise(currentX, 19) - .5f
            );

            cam.transform.localPosition = Vector3.Scale(magnitude, shakeAmount) + startPos;
            currentX += wavelength;
            yield return null;
        }
        cam.transform.localPosition = startPos;
    }

    private void DisablePlayerControls()
    {
        PlayerController.DisableControl();
    }

    private void EnablePlayerControls()
    {
        PlayerController.EnableControl();
        uiGroup.SetActive(true);

        for(int i = 0; i < FindObjectOfType<MenuPlayers>().CurrentPlayers.Count; i++)
        {

            playerScoreUI[(int)FindObjectOfType<MenuPlayers>().CurrentPlayers[i].Playerid - 1].SetActive(true);
            Debug.Log("activated " + i);
        }
    }
}
