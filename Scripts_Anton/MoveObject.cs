using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour {
    public Vector3 Direction;
    public float speed = 15f;
    public SpawnObject playerRef;
    private Vector3 direction;
    private Rigidbody body;
    private bool move = true;

	// Use this for initialization
	void Start () {
        direction = Direction;
        body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        body.velocity = direction;
        body.AddForce(direction * speed);

        //transform.position += direction * speed * Time.deltaTime;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (Vector3.Distance(transform.position, playerRef.player.transform.position) < 0.5f)
        {
            if (Random.value > 0.5)
            {
                direction += Vector3.right;
                direction.Normalize();
            }
            else
            {
                direction += Vector3.left;
                direction.Normalize();
            }
        }
    }


}
