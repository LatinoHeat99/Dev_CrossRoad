using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMove : MonoBehaviour
{
    public float Speed { get; private set; } = 4.0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveTo());
    }

    IEnumerator MoveTo()
    {
        if (Camera.main.WorldToViewportPoint(transform.position).x > 1.0f)
            Destroy(gameObject, 2.0f);
            
        transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward, Time.deltaTime * Speed);
        yield return null;
        StartCoroutine(MoveTo());
    }

    public void setSpeed(float _speed)
    {
        Speed += _speed * 0.5f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            Debug.Log("Stupid ass hole~~~");
    }
}
