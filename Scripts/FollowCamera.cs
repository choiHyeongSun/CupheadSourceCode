using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;
    [Range(1.0f, 3.0f), SerializeField]
    private float CameraSpeed = 1.0f; 
    [SerializeField]
    private bool HorizontalOnly = false;

    private void FixedUpdate()
    {
        if (HorizontalOnly == true)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Lerp(transform.position.x, Player.transform.position.x, Time.deltaTime * CameraSpeed);
            transform.position = pos;
        }
        else
        {
            Vector3 pos = transform.position;
            pos = Vector3.Lerp(transform.position, Player.transform.position, Time.deltaTime * CameraSpeed);
            transform.position = new Vector3(pos.x, pos.y , transform.position.z);
        }
    }
}
