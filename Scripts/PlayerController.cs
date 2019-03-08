using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerController : MonoBehaviour {

    [Header("Came")]
    [Range(0, 0.5f)]
    public float CameraCournerPaddingMin = 0.1f;
    [Range(0.5f, 1.0f)]
    public float CameraCournerPaddingMax = 0.9f;

    public float speed = 5;
    public int tiltDegree = 15;
    public float SpinRadius = 1.5f;
    public float Cooldown = 4;
    public float force = 4;
    public float pushTime = 2;
    public XboxController CurrentController = XboxController.First;
    public static bool isControlDisabled = false;
    public bool controllerTest = true;
    public PlayerInfo.PlayerColors PrefabColor;
    

    private Rigidbody rb;

    private static XboxController LastController = XboxController.Any;

    private static bool HasControllerID = false;
    private static List<PlayerController> Players = new List<PlayerController>();

    private float cooldownTimer = 0;
    private bool IsPush = false;
    private Vector3 PushDirection;
    private Animator anim;
    private AudioSource audioSource;
    public AudioClip spinSound;
    public AudioClip[] hurtSound;
    [SerializeField]
    private bool UseKeyboard;
    private static bool CanPickParachute = true;

    private void OnEnable()
    {
        Players.Add(this);

    }


    // Use this for initialization
    void Start () {
       
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        DamageTextController.Initialize();
        MenuPlayers s = FindObjectOfType<MenuPlayers>();
        
        try
        {
            CurrentController = s.CurrentPlayers.Find(p => p.CurrentColor == PrefabColor).Playerid;
            Debug.Log(CurrentController);

            UseKeyboard = s.CurrentPlayers.Find(p => p.CurrentColor == PrefabColor).UseKeyboard;
            
        }catch(System.Exception ex)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per fram
    void Update()
    {

        float xAxis = 0;
        float yAxis = 0;
        if (!UseKeyboard)
        {
            xAxis = XCI.GetAxis(XboxAxis.LeftStickX, CurrentController);
            yAxis = XCI.GetAxis(XboxAxis.LeftStickY, CurrentController);
        }
        else
        {
            xAxis = Input.GetAxis("Horizontal");
            yAxis = Input.GetAxis("Vertical");
        }
        if (!isControlDisabled )
        {
            if (!IsPush)
            {
                Movement(xAxis, yAxis);
                TiltRotation(xAxis, yAxis);
                Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
                pos.x = Mathf.Clamp(pos.x, CameraCournerPaddingMin, CameraCournerPaddingMax);
                pos.y = Mathf.Clamp(pos.y, CameraCournerPaddingMin, CameraCournerPaddingMax);
                Vector3 newxz = Camera.main.ViewportToWorldPoint(pos); ;
                transform.position = new Vector3(newxz.x, transform.position.y, newxz.z);
            }
        }
        if (IsPush)
        {
            Push();
        }
    }

    private void FixedUpdate()
    {
        
        if ((XCI.GetButtonDown(XboxButton.A, CurrentController) || Input.GetKeyDown(KeyCode.Space)) && cooldownTimer <= 0 && !IsPush && !isControlDisabled)
        {
            
            if(!anim.GetCurrentAnimatorStateInfo(0).IsName("SpinAnimation"))
            {
                anim.Play("SpinAnimation");
                audioSource.clip = spinSound;
                audioSource.pitch = Random.Range(0.85f, 1.25f);
                audioSource.Play();
            }

            Collider[] hits = Physics.OverlapSphere(transform.position, SpinRadius);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].gameObject.tag.Contains("Player") && hits[i].gameObject.name != gameObject.name)
                {
                    Vector3 dir = (hits[i].transform.position - transform.position).normalized;
                    //hits[i].GetComponent<Rigidbody>().AddForce(dir * force);
                    hits[i].gameObject.GetComponent<PlayerController>().EnablePush(dir);
                    Debug.Log(hits[i].name + dir);
                    cooldownTimer = Cooldown;
                }
                if(hits[i].gameObject.tag.Contains("ObstacleObj") && hits[i].gameObject.name != gameObject.name)
                {
                    Vector3 dir = (hits[i].transform.position - transform.position).normalized;
                    hits[i].GetComponent<FlyerProperties>().SetNewDirection(dir);
                    cooldownTimer = Cooldown;
                }
            }
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private Vector3 originPos;

    public void EnablePush(Vector3 Direction)
    {
        PushDirection = new Vector3(Direction.x, 0, Direction.z);
        IsPush = true;
        originPos = transform.position;
    }


    private float tim = 0;
    private bool gotParachute = false;
    private Transform ParachuteTransform;
    private void Push()
    {
        tim += Time.deltaTime;
        if (tim > pushTime)
        {
            IsPush = false;
            tim = 0;
        }
        else
        {
            transform.position = Vector3.Lerp(originPos, originPos + PushDirection * 2f, tim / pushTime);

            if (gotParachute)
            {
                gotParachute = false;
                ParachuteTransform.localPosition = new Vector3(0, 0, 0);
                ParachuteTransform.parent = null;
                FindObjectOfType<MenuPlayers>().CurrentPlayers.Find(s => s.Playerid == CurrentController).Hasparachute = false;
                ParachuteTransform.gameObject.GetComponent<Renderer>().enabled = true;
                Debug.Log(ParachuteTransform.gameObject.GetComponent<Renderer>().enabled);
                Debug.Log(ParachuteTransform.position);
                FindObjectOfType<ParachuteSpawner>().Direction = new Vector3(Mathf.Cos((Random.Range(0, 360) + 90) * Mathf.Deg2Rad), 0, Mathf.Sin((Random.Range(0, 360) + 90) * Mathf.Deg2Rad)).normalized;
                CanPickParachute = false;
                Invoke("CanPick", 1);
            }
        }
    }

    private void CanPick()
    {
        CanPickParachute = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Contains("parachute") && !FindObjectOfType<MenuPlayers>().AnyoneHasParachute() && CanPickParachute)
        {
            gotParachute = true;
            other.transform.SetParent(transform);
            other.transform.localPosition = new Vector3(0, 0, 0);
            ParachuteTransform = other.gameObject.transform;
            ParachuteTransform.gameObject.GetComponent<Renderer>().enabled = false;
            ParachuteTransform.GetChild(0).gameObject.SetActive(false);
            FindObjectOfType<MenuPlayers>().CurrentPlayers.Find(s => s.Playerid == CurrentController).Hasparachute = true;

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, SpinRadius);
    }


    private bool hitCooldown = false;
    private void OnTriggerStay(Collider collider)
    {
        //Debug.Log("Player collided with something " + collision.gameObject.name);
        if (collider.gameObject.tag.Contains("Player") || collider.gameObject.name.Contains("parachute"))
            return;

        if (hitCooldown == false)
        {
            hitCooldown = true;
            audioSource.clip = hurtSound[Random.Range(0, hurtSound.Length - 1)];
            audioSource.pitch = Random.Range(0.85f, 1.25f);
            audioSource.Play();

            Invoke("AllowPlayerToGetHit", 2.0f);
            GetComponent<ScoreUpdate>().Reset();
            ScoreInformation info = new ScoreInformation();

            FlyerProperties s = collider.gameObject.GetComponent<FlyerProperties>();

            info.obsType = s.obstacleType;
            info.Score = s.scorePenalty;
            //Debug.Log(info);
            //Debug.Log("Hi im player and i hit something");
            FindObjectOfType<MenuPlayers>().CurrentPlayers.Find(ss => ss.Playerid == CurrentController).AddScore(info);

            DamageTextController.CreateDamageText(s.scorePenalty.ToString(), transform);
            StartCoroutine(ShakePlayer());

        }
    }

    private void AllowPlayerToGetHit()
    {
        hitCooldown = false;
    }

    private IEnumerator ShakePlayer()
    {
        // shake player when collides with obstacle
        
        Vector3 startPos = transform.localPosition;
        float endTime = Time.time + 0.3f;
        float currentX = 0;

        while (Time.time < endTime)
        {
            Vector3 shakeAmount = new Vector3(
                Mathf.PerlinNoise(currentX, 2) - .5f,
                0,
                Mathf.PerlinNoise(currentX, 3) - .5f
            );

            Vector3 magnitude = new Vector3(1.1f, 0, 1.1f);
            transform.localPosition = Vector3.Scale(magnitude, shakeAmount) + startPos;
            currentX += 0.99f;
            yield return null;
        }
        transform.localPosition = startPos;
    }

    private void Movement(float x, float y)
    {
        Vector3 movement = new Vector3(x, 0, y) * speed * Time.deltaTime;
        rb.AddRelativeForce(movement * 2000);
    }

    private void TiltRotation(float x, float y)
    {
        Vector3 rotation = new Vector3(tiltDegree * y, 0, tiltDegree * -x);
        rb.transform.eulerAngles = rotation;
    }

    public static void DisableControl()
    {
        isControlDisabled = true;
        
    }

    public static void EnableControl()
    {
        isControlDisabled = false;
    }

    private void OnDisable()
    {
        for(int i = 0; i < Players.Count; i++)
        {
            if (Players[i] == this)
                Players.RemoveAt(i);
        }
    }
}




