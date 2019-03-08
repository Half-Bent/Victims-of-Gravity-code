using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour {

    public List<ScenePrefabInfo> prefabObjects;
    public GameObject player;
    public float height = 4;
    public float radius;
    public float spawnTimer;
    public Transform CameraPos;

    private float timer = 0;
    private GameSceneManager manager;

	// Use this for initialization
	void Start ()
    {
        manager = FindObjectOfType<GameSceneManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!manager.StopSpawn)
        {
            timer += Time.deltaTime;

            if (timer >= spawnTimer)
            {
                GameObject test = Instantiate(prefabObjects[Random.Range(0, prefabObjects.Count - 1)].Prefab);

                if (!test.GetComponent<FlyerProperties>().sideSpawning)
                {
                    test.transform.position = Utils.RandomPointInCircle(transform.position, radius);
                }
                else
                {
                    test.transform.position = Utils.RandomPointInCircle(new Vector3(transform.position.x,
                                                                                    height,
                                                                                    transform.position.z), radius);
                }

                if (!test.GetComponent<FlyerProperties>().noRotation)
                    test.transform.rotation = Random.rotation;

                Destroy(test, 40f);
                timer = 0;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(enabled)
            Gizmos.DrawWireSphere(transform.position, radius);
    }
}