using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parachute : MonoBehaviour {

    // not in use

    public float speed = 8f;
    public bool torque;

    private Rigidbody rb;
    private Vector3 direction;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        direction = (new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z) - transform.position).normalized;
    }

    private void Awake()
    {
        rb.AddTorque(new Vector3(Random.Range(7, 10), Random.Range(7, 10), Random.Range(7, 10)));
    }

    // Update is called once per frame
    void Update () {
        rb.velocity = direction * speed;
    }
}
