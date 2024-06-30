using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField]
    private Vector2 Speed = new Vector2(1, 1.2f);
    [SerializeField]
    private Transform StartTrans;
    [SerializeField]
    private Transform EndTrans;

    private float CurrentSpeed;

    private void Awake()
    {
        CurrentSpeed = Random.Range(Speed.x, Speed.y);
    }
    private void Update()
    {
        transform.Translate(-Vector3.right * Time.deltaTime * CurrentSpeed);
        if (EndTrans.position.x > transform.position.x)
        {
            CurrentSpeed = Random.Range(Speed.x, Speed.y);
            Vector3 pos = transform.position;
            pos.x = StartTrans.position.x;
            transform.position = pos;
        }
        
    }
}
