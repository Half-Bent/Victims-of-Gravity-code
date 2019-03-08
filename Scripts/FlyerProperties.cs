using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FlyerProperties : MonoBehaviour {



    [Header("Homing")]
    public bool homing;
    public float homingSpeed = 25f;
    public float homingTime = 10f;
    private bool tracking = true;
    private Vector3 trackDirection;

    [Header("Movement")]
    public float speed = 8f;
    public bool torque;
    private Vector3 torqueVector;
    public bool noRotation;

    [Header("Spawning - use only with Side Spawning")]
    public bool spawner;
    public GameObject spawnling;
    public float spawnSpeed = 8f;
    public float startTime;
    public float cooldown;
    private bool onCooldown;
    public float endTime;
    private float time;
    public int spawnAmount;
    // if snipe is off, shoot spawnlings around with relative degrees
    // if snipe is on, shoot spawnlings to each player, from nearest to farthest
    public bool snipe;

    [Header("Sound")]
    public float pitchRange;
    public bool playOnce;
    public float soundStartTime;
    public float soundTimeInterval;
    public AudioClip collisionAudioClip;
    public AudioClip[] audioClips;
    private AudioSource audioSource;
    public  AudioMixerGroup SFXGroup;

    [Header("Other")]
    public bool spawned = false;
    public int scorePenalty;
    public enum ObstacleType
    {
        Debris,
        BigTarget,
        Bonus
    };
    public ObstacleType obstacleType;
    public bool sideSpawning;
    private GameObject[] players;
    private Vector3 direction;
    private Rigidbody rb;
    


    // Use this for initialization
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        //audioSource = GetComponent<AudioSource>();
        //audioSource.clip = audioClip;
        // sets target direction as a random player
        direction = (players[Random.Range(0, players.Length)].transform.position - this.transform.position).normalized;
        rb = GetComponent<Rigidbody>();

        if (torque)
        {
            torqueVector = new Vector3(Random.Range(2, 10), Random.Range(2, 10), Random.Range(2, 10));
            rb.AddTorque(torqueVector);
        }

        if(sideSpawning)
            rb.constraints = RigidbodyConstraints.FreezePositionY;

        time = Time.time;

        audioSource = GetComponent<AudioSource>();
        if(audioClips != null)
            Invoke("PlaySounds", soundStartTime);
    }

    public void SetNewDirection(Vector3 euler)
    {
        homing = false;
        this.direction = euler;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (spawned)
            return;

        // handle basic movement
        if (homing)
        {
            if(!sideSpawning)
                rb.velocity = Vector3.up;
            Homing();
        }
        else
        {
            rb.velocity = direction * speed;
            rb.AddForce(direction * speed);
        }
        

        if (spawner)
        {
            Spawner();
        }
    }

    private void Homing()
    {
        if (tracking)
        {
            // tracks closest player's x and z positions
            trackDirection = new Vector3((GetClosestPlayer(players).transform.position.x - this.transform.position.x),
                                         0,
                                         (GetClosestPlayer(players).transform.position.z - this.transform.position.z));
            trackDirection = trackDirection.normalized;

            if (homingTime < 0)
            {
                homingTime -= Time.deltaTime;
                tracking = false;
            }
            else if (!sideSpawning && this.transform.position.y >= players[0].transform.position.y)
            {
                tracking = false;
            }
        }

        homingTime -= Time.deltaTime;

        // when tracking is over, the object will still move to the last tracked position
        if(sideSpawning)
        {
            rb.velocity = trackDirection;
        }
        else
        {
            rb.AddForce(trackDirection * homingSpeed);
        }
        
    }

    // Get the closest player from this GameObject
    private GameObject GetClosestPlayer(GameObject[] players)
    {
        GameObject target = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
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


    private void Spawner()
    {
        if(Time.time > time + startTime && Time.time < time + endTime && !onCooldown)
        {
            StartCoroutine(Spawn());
            onCooldown = true;
            Invoke("SpawnResetCooldown", cooldown);
        }

    }

    private void SpawnResetCooldown()
    {
        onCooldown = false;
    }

    IEnumerator Spawn()
    {
        if (snipe)
            spawnAmount = players.Length;

        for(int i = 0; i < spawnAmount; i++)
        {
            GameObject spawn = Instantiate(spawnling);
            spawn.transform.position = transform.position;
            spawn.transform.rotation = Random.rotation;
            Destroy(spawn, 10f);
            if (snipe)
            {
                spawn.GetComponent<Rigidbody>().velocity = (players[i].transform.position - transform.position).normalized * spawnSpeed;
            }
            else
            {
                Vector3 splashDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (360 / spawnAmount * (i+1) + (270 / spawnAmount))) * 2,
                                                      0,
                                                      Mathf.Sin(Mathf.Deg2Rad * (360 / spawnAmount * (i+1) + (270 / spawnAmount))) * 2);
                
                splashDirection = splashDirection.normalized;
                
                spawn.GetComponent<Rigidbody>().velocity = splashDirection * spawnSpeed;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(collisionAudioClip != null && other.gameObject.tag.Contains("Player"))
        {
            audioSource.clip = collisionAudioClip;
            audioSource.pitch = (Random.Range(1 - pitchRange, 1 + pitchRange));
            audioSource.Play();
        }
    }

    private void PlaySounds()
    {
        if (audioClips != null && audioClips.Length > 0)
        {
            float randomTime = soundTimeInterval * Random.Range(0.5f, 1.5f);
            audioSource.clip = audioClips[Random.Range(0, audioClips.Length - 1)];
            audioSource.pitch = (Random.Range(1 - pitchRange, 1 + pitchRange));
            audioSource.outputAudioMixerGroup = SFXGroup;
            audioSource.Play();

            if (playOnce == false)
            {
                Invoke("PlaySounds", randomTime);
            }
        }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (Vector3.Distance(transform.position, players[0].transform.position) < 0.5f)
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
    }*/
}





