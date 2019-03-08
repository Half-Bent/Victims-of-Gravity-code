using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour {

    public float speed = 10.0f;

    private Rigidbody2D rb2d;
    private BoxCollider2D bgCollider;
    private float bgHorizontalLength;


	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(-speed, 0.0f);

        bgCollider = GetComponent<BoxCollider2D>();
        bgHorizontalLength = bgCollider.size.x;
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.x < -bgHorizontalLength)
            Reposition();
	}

    private void Reposition()
    {
        Vector2 offset = new Vector2(bgHorizontalLength * 2f, 0);
        transform.position = (Vector2)transform.position + offset;
    }
}