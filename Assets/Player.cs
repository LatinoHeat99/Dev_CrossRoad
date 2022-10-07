using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string Name;// { get; private set; } = "Unknown";
    public int[] Indexs;
    public float Speed;// { get; private set; } = 5.0f;
    
    //public PlayerData(string _name, int[] _idx, float _speed)
    //{
    //    Name = _name;
    //    Indexs = _idx;
    //    Speed = _speed;
    //}
}

public class Player : StateMachine
{
    [HideInInspector]
    public Idle idleState;
    [HideInInspector]
    public Move moveState;

    [SerializeField]
    PlayerData playerData;

    private int throughCount = 0;
    public int ThroughCount { get { return throughCount; } }
    private void Awake()
    {
        idleState = new Idle(this);
        moveState = new Move(this);
    }

    public void initPlayer()
    {
        throughCount = 0;
        transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    [ContextMenu("To json")]
    void SaveToJson()
    {
        string toJsonData = JsonUtility.ToJson(playerData, true);
        Debug.Log(toJsonData);
        string path = Path.Combine(Application.dataPath, "playerData.json");
        Debug.Log(path);
        File.WriteAllText(path, toJsonData);
    }

    [ContextMenu("From json")]
    void LoadFromJson()
    {
        string path = Path.Combine(Application.dataPath, "playerData.json");
        string fromJsonData = File.ReadAllText(path);
        Debug.Log(fromJsonData);
        playerData = JsonUtility.FromJson<PlayerData>(fromJsonData);
    }

    protected override BaseState GetInitState()
    {
        return idleState;
    }

    public float GetSpeed()
    {
        return playerData.Speed;
    }

    public void addThroughCount()
    {
        throughCount++;
        
        Debug.Log("throughCount " + throughCount);
    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        movePos = transform.position + transform.forward * 2.5f;
    //        StartCoroutine(Move());
    //        Debug.Log("Update " + transform.position);
    //    }
    //}
    //IEnumerator Move()
    //{
    //    Vector3 velocity = Vector3.zero;
        
    //    while(!(Vector3.Distance(transform.position, movePos) < Mathf.Epsilon))
    //    {
    //        //transform.position = Vector3.SmoothDamp(transform.position, movePos, ref velocity, 0.5f);
    //        transform.position = Vector3.MoveTowards(transform.position, movePos, Time.deltaTime * 5.0f);
            
    //        Debug.Log("Move " + transform.position);
    //        yield return null;
    //    }
    //}
}
