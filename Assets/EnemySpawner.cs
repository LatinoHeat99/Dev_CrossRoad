using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;



public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private int stepNumber;
    [SerializeField]
    private float SpawnTimeMin;
    [SerializeField]
    private float SpawnTimeMax;
    public GameObject[] Enemys;

    private Vector3 direction = Vector3.right;
    private Ray passRay;
    private SpawnerData.Data? data;

    private void Awake()
    {
        passRay = new Ray();
    }
    void Start()
    {
        initSpwaner();

        //StartCoroutine(MoveSpwaner());
    }

    public void initSpwanerData(SpawnerData.Data _data)
    {
        data = _data;
        Debug.Log("SeptNumber : " + data.Value.StepNumber.ToString());
        //setPosition();
        //setRay();

        //if (data.SpawnTimeMinMax.y <= Mathf.Epsilon)
        //    StartCoroutine(Regular_Spawn(data.SpawnTimeMinMax.x));
        //else
        //    StartCoroutine(Random_Spawn());
    }
    public void initSpwaner()
    {
        setPosition();
        setRay();

        StopAllCoroutines();
        if (data.Value.SpawnTimeMinMax.y <= Mathf.Epsilon)
            StartCoroutine(Regular_Spawn(data.Value.SpawnTimeMinMax.x));
        else
            StartCoroutine(Random_Spawn());
    }
    //private void initSpwaner()
    //{
    //    setPosition();
    //    setRay();

    //    StopAllCoroutines();
    //    if (UnityEngine.Random.Range(0, 2) > 0)
    //        StartCoroutine(Regular_Spawn(UnityEngine.Random.Range(SpawnTimeMin, SpawnTimeMax)));
    //    else
    //        StartCoroutine(Random_Spawn());
    //}

    private void setPosition()
    {
        Vector3 pos = transform.position;
        pos.z = data.Value.StepNumber * 5.0f;
        transform.position = pos;
    }

    private void setRay()
    {
        passRay.origin = transform.position;
        passRay.direction = direction;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(passRay.origin, passRay.direction * 100.0f, Color.red);
    }

    private void FixedUpdate()
    {
        if (!data.HasValue)
            return;

        if (StageManager.Instance.lastSpawnerStep == StageManager.Instance.currStepNumber)
        {
            Debug.Log("Next Stage!!!!" + StageManager.Instance.lastSpawnerStep + StageManager.Instance.currStepNumber);
            StageManager.Instance.NextStage();
            return;
        }

        if (data.Value.StepNumber > StageManager.Instance.currStepNumber)
        {
            RaycastHit hit;
            if (Physics.Raycast(passRay, out hit))
            {
                //Debug.Log(hit.collider.name);
                if (hit.collider != null && hit.collider.CompareTag("Player"))
                    Debug.Log(data.Value.StepNumber + " through " + gameObject.name);
            }
        }
        
        if(data.Value.StepNumber < (StageManager.Instance.currStepNumber - 1))
        {
            data = StageManager.Instance?.GetData();
            
            //stepNumber += 10;
            if(data.HasValue)
                initSpwaner();
        }
    }
    private GameObject CreateObj(int _idx)
    {
        GameObject enemy = Enemys[_idx];
        GameObject go = Instantiate(enemy, transform.position, enemy.transform.rotation);
        return go;
    }
    private GameObject CreateObj()
    {
        int idx = UnityEngine.Random.Range(0, Enemys.Length);
        
        return CreateObj(idx);
    }
    IEnumerator Regular_Spawn(float _spawnTime)
    {
        GameObject go = CreateObj();
        go.GetComponent<EnemyMove>().setSpeed(_spawnTime * data.Value.SpeedRate);
        yield return new WaitForSeconds(_spawnTime);
        StartCoroutine(Regular_Spawn(_spawnTime));
    }
    IEnumerator Random_Spawn()
    {
        GameObject go = CreateObj();
        float spawnTime = UnityEngine.Random.Range(data.Value.SpawnTimeMinMax.x, data.Value.SpawnTimeMinMax.y);
        go.GetComponent<EnemyMove>().setSpeed(spawnTime * data.Value.SpeedRate);
        yield return new WaitForSeconds(spawnTime);
        StartCoroutine(Random_Spawn());
    }
    //IEnumerator MoveSpwaner()
    //{
    //    Vector3 ViewportPos = Camera.main.WorldToViewportPoint(transform.position);
    //    if (ViewportPos.x > 1.0f)
    //        direction *= -1.0f;
    //    else if( ViewportPos.x < 0.0f)
    //        direction *= -1.0f;
    //    transform.position = Vector3.Lerp(transform.position, transform.position + direction, Time.deltaTime * 2.0f);
    //    yield return null;
    //    StartCoroutine(MoveSpwaner());
    //}
}