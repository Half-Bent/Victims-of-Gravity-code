using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCameraSolidColor : MonoBehaviour {

    // parachute sprite to deactivate in ending cutscene
    public GameObject parachute;

	// Use this for initialization
	void Start () {
        parachute.GetComponent<SpriteRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Awake()
    {
        Camera.main.clearFlags = CameraClearFlags.Color;
    }
}


