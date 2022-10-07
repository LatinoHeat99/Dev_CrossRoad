using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class SpawnerData
{
    [Serializable]
    public struct Data
    {
        public int StepNumber;
        public Vector2 SpawnTimeMinMax;
        public float SpeedRate;
    }
    public List<Data> datas;
}
public class StageManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObj;
    public int StageNumber;
    [SerializeField]
    private string DataName = "spawnerData";
    private string Extention = ".json";
    [SerializeField]
    private SpawnerData spawnData;
    public int MaxSpawners;
    private List<EnemySpawner> Spawners = new List<EnemySpawner>();
    [SerializeField]
    private GameObject prefabSpawner;
    public int lastSpawnerStep { get; private set; }

    private static StageManager instance;
    public static StageManager Instance { get { return instance; } }
    private Player player;
    public int currStepNumber { get { return player.ThroughCount; } }
    private Queue<SpawnerData.Data> StageQue = new Queue<SpawnerData.Data>();
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(instance);
    }
    private void Start()
    {
        player = playerObj.GetComponent<Player>();
        NextStage();

        while (MaxSpawners > Spawners.Count)
        {
            GameObject obj = Instantiate<GameObject>(prefabSpawner);
            EnemySpawner spawner = obj.GetComponent<EnemySpawner>();
            //spawner.initSpwaner(spawnData.datas[Spawners.Count]);
            SpawnerData.Data data = StageQue.Dequeue();
            spawner.initSpwanerData(data);
            Spawners.Add(spawner);
            lastSpawnerStep = data.StepNumber;
        }
    }

    public SpawnerData.Data? GetData()
    {
        //if(lastSpawnerStep > (spawnData.datas.Count - 1))
        //    return null;
        //return spawnData.datas[lastSpawnerStep]; 
        if (StageQue.Count > 0)
        {
            SpawnerData.Data data = StageQue.Dequeue();
            lastSpawnerStep = data.StepNumber;
            return data;
        }
        return null;
    }

    public void NextStage()
    {
        StageNumber++;
        string path = Path.Combine(Application.dataPath, DataName + StageNumber.ToString() + Extention);
        string fromJsonData = File.ReadAllText(path);
        Debug.Log(fromJsonData);
        spawnData = JsonUtility.FromJson<SpawnerData>(fromJsonData);

        StageQue = new Queue<SpawnerData.Data>(spawnData.datas);

        for(int i = 0; i < Spawners.Count; i++)
        {
            //Spawners[i].initSpwaner(spawnData.datas[i]);
            //lastSpawnerStep = spawnData.datas[i].StepNumber;        
            Spawners[i].initSpwanerData(StageQue.Dequeue());
            Spawners[i].initSpwaner();
            lastSpawnerStep = StageQue.Dequeue().StepNumber;
        }

        player.initPlayer();
    }


    [ContextMenu("To json")]
    void SaveToJson()
    {
        string toJsonData = JsonUtility.ToJson(spawnData, true);
        Debug.Log(toJsonData);
        string path = Path.Combine(Application.dataPath, DataName + StageNumber.ToString() + Extention);
        Debug.Log(path);
        File.WriteAllText(path, toJsonData);
    }

    [ContextMenu("From json")]
    void LoadFromJson()
    {
        string path = Path.Combine(Application.dataPath, DataName + StageNumber.ToString() + Extention);
        string fromJsonData = File.ReadAllText(path); 
        Debug.Log(fromJsonData);
        spawnData = JsonUtility.FromJson<SpawnerData>(fromJsonData);
    }
}