using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AltitudeMeter : MonoBehaviour {

    public GameObject altitudeMeter;
    public Slider slider;
    public GameObject endingGroup;
    public GameObject GameProgress;
    private TimelineController tlc;

    private void Awake()
    {
        slider.value = 1;
        tlc = endingGroup.GetComponent<TimelineController>();
    }

    private void Start()
    {
    }


    // Update is called once per frame
    void Update () {

        // tähän kokonaisprogress kunhan selvitetään cloudtransitionit
        slider.value = 1 - GameProgress.GetComponent<GameProgress>().ProgressGlobal;
        if(slider.value <= 0)
        {
            // aktivoidaan lopetus
            endingGroup.SetActive(true);
        }
	}
}