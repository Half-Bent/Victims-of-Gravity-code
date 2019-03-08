using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class BigEvents : MonoBehaviour {

    // Just select the main camera
    [Header("Basic Info")]
    public CinemachineVirtualCamera cameraToZoom;
    public int fovChangeValue;
    public float cameraDefaultFov;
    public GameObject BigTargetPrefab;
    public GameObject warningSignParent;
    public float timeToActivate = 2f;
    public float time;
    
    [Header("Movement")]
    public bool randomRotationAtStart;
    public int spawnDegree;
    public bool randomDegree;
    public bool homing;
    public float homingTime;
    public float speed;
    public bool targetCenter; // otherwise target random player
    public bool spawnAdds;
    public GameObject spawnling;
    public float spawnInterval;
    public float spawnSpeed;

    [Header("Warning Sign Sound")]
    public AudioClip audioClip;
    private AudioSource audioSource;

    private GameObject[] players;
    private Rigidbody rb;
    private GameSceneManager manager;

    // Use this for initialization
    void Start ()
    {
        cameraDefaultFov = cameraToZoom.m_Lens.FieldOfView;
        players = GameObject.FindGameObjectsWithTag("Player");
        audioSource = GetComponent<AudioSource>();
        manager = FindObjectOfType<GameSceneManager>();
    }
    
    private bool isActivated = false;
    private bool isDone = false;
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (manager.StopSpawn)
            return;
        // Activate Block
        if(timeToActivate > 0)
        {
            timeToActivate -= Time.deltaTime;
        }
        else if (timeToActivate <= 0 && !isActivated)
        {
            ActivateGameEvent();
            Invoke("EndEvent", time);
        }
        
        if(isActivated)
        {
            // Camera zoom
            CameraZoom(true);

            // Instantiate Big Target
            InstantiateBigTarget();
        }

        if (homing && bigTarget != null)
            Homing();

        if (spawnAdds && bigTarget != null)
            Spawner();

        // Reset camera back to default fov when done
        if (isDone)
        {
            CameraZoom(false);
        }
    }
    


    public void ActivateGameEvent()
    {
        isActivated = true;

        // Initial sound play
        audioSource.clip = audioClip;
        audioSource.Play();

        // Blinking
        warningSignParent.SetActive(true);
        for (int i = 1; i <= 5; i++)
        {
            Invoke("WarningSignBlink", (0.7f * i));
        }
    }


    public void CameraZoom(bool zoomOut)
    {
        if(zoomOut == true && cameraToZoom.m_Lens.FieldOfView < (cameraDefaultFov + fovChangeValue) && !isDone)
        {
            cameraToZoom.m_Lens.FieldOfView += fovChangeValue / 200.0f;
        }
        else if (zoomOut == false && cameraToZoom.m_Lens.FieldOfView > (cameraDefaultFov))
        {
            cameraToZoom.m_Lens.FieldOfView -= fovChangeValue / 200.0f;
        }
    }






    
    public void WarningSignBlink()
    {
        if(warningSignParent.activeSelf)
        {
            warningSignParent.SetActive(false);
        }
        else
        {
            warningSignParent.SetActive(true);
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

    private bool instantiateDone = false;
    private Vector3 direction;
    private GameObject bigTarget;
    public void InstantiateBigTarget()
    {
        if (instantiateDone == false)
        {
            bigTarget = Instantiate(BigTargetPrefab);
            rb = bigTarget.GetComponent<Rigidbody>();

            if (randomDegree)
                spawnDegree = Random.Range(0, 360);

            Vector3 spawnDir = new Vector3(Mathf.Cos((spawnDegree + 90) * Mathf.Deg2Rad), 0, Mathf.Sin((spawnDegree + 90) * Mathf.Deg2Rad)).normalized;
            bigTarget.transform.position = new Vector3(spawnDir.x * 20.0f, 3, spawnDir.z * 20.0f);
            
            direction = (new Vector3(cameraToZoom.transform.position.x, 0, cameraToZoom.transform.position.z) - bigTarget.transform.position).normalized;

            if (!targetCenter)
                direction = (players[Random.Range(0, players.Length)].transform.position - bigTarget.transform.position).normalized;

            rb.velocity = direction * speed;

            if (randomRotationAtStart)
            {
                bigTarget.transform.rotation = Random.rotation;
            }
            else
            {
                bigTarget.transform.rotation = Quaternion.Euler(0, (Mathf.Rad2Deg * Mathf.Atan2(direction.x, direction.z)), 90);
            }
            

            instantiateDone = true;
        }
    }

    

    private bool tracking = true;
    private Vector3 trackDirection;
    private void Homing()
    {
        if (tracking)
        {
            // tracks closest player's x and z positions
            trackDirection = new Vector3((GetClosestPlayer(players).transform.position.x - bigTarget.transform.position.x),
                                         0,
                                         (GetClosestPlayer(players).transform.position.z - bigTarget.transform.position.z));
            trackDirection = trackDirection.normalized;

            if (homingTime < 0)
            {
                homingTime -= Time.deltaTime;
                tracking = false;
            }
        }

        homingTime -= Time.deltaTime;

        // when tracking is over, the object will still move to the last tracked position
        
        rb.velocity = trackDirection;
    }



    // Get the closest player from this GameObject
    private GameObject GetClosestPlayer(GameObject[] players)
    {
        GameObject target = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = bigTarget.transform.position;
        foreach (GameObject potentialTarget in players)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                target = potentialTarget;
            }
        }

        return target;
    }


    private bool onCooldown = false;
    private void Spawner()
    {
        if (!onCooldown)
        {
            StartCoroutine(Spawn());
            onCooldown = true;
            Invoke("SpawnResetCooldown", spawnInterval);
        }

    }

    private void SpawnResetCooldown()
    {
        onCooldown = false;
    }

    IEnumerator Spawn()
    {
        GameObject spawn = Instantiate(spawnling);
        spawn.transform.position = bigTarget.transform.position;
        spawn.transform.rotation = Random.rotation;
        Destroy(spawn, 15f);
        spawn.GetComponent<Rigidbody>().velocity = (players[Random.Range(0, players.Length)].transform.position - spawn.transform.position).normalized * spawnSpeed;
        
        yield return null;
    }

    private void EndEvent()
    {
        Destroy(bigTarget, 0.5f);
        isDone = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with " + other.gameObject.name);
    }
}



