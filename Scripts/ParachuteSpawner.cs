using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParachuteSpawner : MonoBehaviour {

    public float timeToSpawn = 10f;
    public float speed = 1.5f;
    public GameObject parachute;

    private bool isActivated;
    private Rigidbody rb;
    private Vector3 direction;
    public Vector3 Direction
    {
        set { direction = value; }
    }

	// Use this for initialization
	void Start ()
    {
        


    }
	
	// Update is called once per frame
	void Update ()
    {
        if (timeToSpawn > 0)
        {
            timeToSpawn -= Time.deltaTime;
        }
        else if (timeToSpawn <= 0 && !isActivated)
        {
            isActivated = true;

            GameObject p = Instantiate(parachute);
            rb = p.GetComponent<Rigidbody>();

            int spawnDegree = Random.Range(0, 360);
            Vector3 spawnDir = new Vector3(Mathf.Cos((spawnDegree + 90) * Mathf.Deg2Rad), 0, Mathf.Sin((spawnDegree + 90) * Mathf.Deg2Rad)).normalized;
            p.transform.position = new Vector3(spawnDir.x * 16.0f, 4, spawnDir.z * 16.0f);
            direction = (new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z) - p.transform.position).normalized;

            rb.AddTorque(new Vector3(Random.Range(10, 15), Random.Range(10, 15), Random.Range(10, 15)));
        }

        if(isActivated)
            rb.velocity = direction * speed;
    }
}
