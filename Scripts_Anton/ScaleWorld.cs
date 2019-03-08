using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWorld : MonoBehaviour {
    public Vector3 MaxScale = new Vector3(10, 10, 10);
    public float ScaleTime = 5f;

	// Use this for initialization
	void Start () {
        StartCoroutine(Scale());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float GetCurrentProgress()
    {
        return  transform.localScale.magnitude / MaxScale.magnitude;
    }

    IEnumerator Scale()
    {

        Vector3 origin = transform.localScale;

        float time = 0;
        do
        {
            transform.localScale = Vector3.Lerp(origin, MaxScale, time / ScaleTime);
            time += Time.deltaTime;
            yield return null;
        } while (time <= ScaleTime);
        transform.localScale = MaxScale;
    }
}
