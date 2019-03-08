using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour {

    public List<ScenePrefabInfo> SpawnablePrefabs;
    public List<ScenePrefabInfo> MoonPrefabs;
    public List<ScenePrefabInfo> MarsPrefabs;
    public List<ScenePrefabInfo> SunPrefabs;
    public bool StopSpawn = false;
    public bool StopScore = false;
    public PlayerInfo.Planets CurrentMap
    {
        get; private set;
    }
    public int progress
    {
        get { return MapProgress; }
    }
    [SerializeField]
    private int MapProgress = 0;
    [SerializeField]
    private int NextSceneIndex = 1;
    private void OnEnable()
        
    {
        SceneManager.sceneLoaded += SceneWasLoaded;
    }

    private void SceneWasLoaded(Scene scene, LoadSceneMode mode)
    {
        StopScore = false;
        if (scene.name != "Menu")
        {
            switch (scene.buildIndex)
            {
                case 2:
                    {
                        RemoveParachuteOnStart();
                        CurrentMap = PlayerInfo.Planets.Earth;
                        NextSceneIndex = 3;
                        SpawnObject[] spawners = FindObjectsOfType<SpawnObject>();
                        for (int i = 0; i < spawners.Length; i++)
                        {
                            spawners[i].prefabObjects = SpawnablePrefabs;
                        }
                        break;
                    }
                case 3:
                    {
                        RemoveParachuteOnStart();
                        CurrentMap = PlayerInfo.Planets.Moon;
                        NextSceneIndex = 4;
                        SpawnObject[] spawners = FindObjectsOfType<SpawnObject>();
                        for (int i = 0; i < spawners.Length; i++)
                        {
                            spawners[i].prefabObjects = MoonPrefabs;
                        }
                        break;
                    }
                case 4:
                    {
                        RemoveParachuteOnStart();
                        CurrentMap = PlayerInfo.Planets.Mars;
                        NextSceneIndex = 5;
                        SpawnObject[] spawners = FindObjectsOfType<SpawnObject>();
                        for (int i = 0; i < spawners.Length; i++)
                        {
                            spawners[i].prefabObjects = MarsPrefabs;
                        }
                        break;
                    }
                case 5:
                    {
                        RemoveParachuteOnStart();
                        CurrentMap = PlayerInfo.Planets.Sun;
                        NextSceneIndex = 0;
                        SpawnObject[] spawners = FindObjectsOfType<SpawnObject>();
                        for (int i = 0; i < spawners.Length; i++)
                        {
                            spawners[i].prefabObjects = SunPrefabs;
                        }
                        break;
                    }

            }
            MapProgress++;
        }
        else if(scene.buildIndex == 0)
        {
            MapProgress = 0;
        }
    }

    private void RemoveParachuteOnStart()
    {
        PlayerInfo player = FindObjectOfType<MenuPlayers>().CurrentPlayers.Find(x => x.Hasparachute);

        if(player != null)
        {
            player.Hasparachute = false;
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(NextSceneIndex);
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneWasLoaded;
    }

}

[System.Serializable]
public class ScenePrefabInfo
{
    public enum SpawnInfo : byte
    {
        Before = 0x1,
        After,
        BeforeAndAfter
    }
    public GameObject Prefab;
    public SpawnInfo spawnInfo;
    [Range(0, 100)]
    public float SpawnRate = 5f;
    
}