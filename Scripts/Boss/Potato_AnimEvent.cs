using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potato_AnimEvent : MonoBehaviour
{
    private SubActionComponent SubAction;


    private void Awake()
    {
        SubAction = GetComponent<SubActionComponent>();
    }

    private void ShootAnimEvent()
    {
        Potato_Shoot shoot = SubAction.GetSubAction() as Potato_Shoot;
        if (shoot != null && shoot.IsShootEnd)
        {
            SubAction.EndSubAction();
        }
        
    }

    private void DeadthEvent()
    {
        Destroy(gameObject);
    }

    
    
}
