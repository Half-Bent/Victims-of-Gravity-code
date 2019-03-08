using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudAnimatFix : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<CloudsToy>().MaxWidthCloud = GetComponent<CloudsToy>().MaxWidthCloud + 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
