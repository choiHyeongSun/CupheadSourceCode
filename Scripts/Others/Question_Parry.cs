using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question_Parry : MonoBehaviour
{
    [SerializeField]
    private bool IsParry;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals("Parry") == false || IsParry == false) return;

        ParryCollision parryCollision = collision.gameObject.GetComponent<ParryCollision>();
        AActor toActor = parryCollision.GetOwner;
        ParryComponent parry = toActor.GetActorComponent<ParryComponent>();

        if (parry != null && IsParry == true)
        {
            if (parry.GetParry == true)
            {
                parry.Parry(transform.position);
                Destroy(gameObject);
                return;
            }
        }
    }
}
