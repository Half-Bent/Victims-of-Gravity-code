using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour {

    public float FadeTime = 3f;
    public float StartFadeTime = 10f;
    public SpriteRenderer TargetSpriteRenderer;

    private float startTime;

	// Use this for initialization
	void Start () {
        StartCoroutine(Fade());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartFade()
    {
        startTime = Time.time;
    }

    IEnumerator Fade()
    {
        float time = 0;
        yield return new WaitForSeconds(2);

        while (time < 1)
        {
            TargetSpriteRenderer.color = new Color(TargetSpriteRenderer.color.r, TargetSpriteRenderer.color.g, TargetSpriteRenderer.color.b, Mathf.Lerp(TargetSpriteRenderer.color.a, 1, time));
            time += Time.deltaTime * (1f / 1.5f);
            yield return null;
        }
        yield return new WaitForSeconds(FadeTime);

        time = 0;
        while (time < 1)
        {
            TargetSpriteRenderer.color = new Color(TargetSpriteRenderer.color.r, TargetSpriteRenderer.color.g, TargetSpriteRenderer.color.b, Mathf.Lerp(TargetSpriteRenderer.color.r, 0, time));
            time += Time.deltaTime * (1f / 1.5f);
            yield return null;
        }


    }

    

}
