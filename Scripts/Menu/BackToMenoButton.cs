using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenoButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Escape) && !SceneManager.GetActiveScene().name.Contains("Menu"))
            SceneManager.LoadScene(0);
	}
}
