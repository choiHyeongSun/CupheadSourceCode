using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParryCollision : MonoBehaviour
{
    private AActor Owner;

    private void Awake()
    {
        Transform parent = transform.parent;
        while (parent.parent != null)
        {
            parent = parent.parent;
        }

        Owner = parent.gameObject.GetComponent<AActor>();
    }


    private void OnTriggerStay2D(Collider2D collider)
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {

    }

    private void OnTriggerExit2D(Collider2D collider)
    {

    }

    public AActor GetOwner=> Owner;



}
