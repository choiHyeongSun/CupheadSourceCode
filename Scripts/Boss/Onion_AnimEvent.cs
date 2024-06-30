using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onion_AnimEvent : MonoBehaviour
{
    private SubActionComponent SubAction;
    private void Awake()
    {
        SubAction = GetComponent<SubActionComponent>();
    }

    private void TearStart()
    {
        if (SubAction.GetSubAction() != null)
        {
            Onion_Tears tears = SubAction.GetSubAction() as Onion_Tears;
            if (tears != null)
            {
                tears.TearStart();
            }
        }
    }

    private void TearEnd()
    {
        if (SubAction.GetSubAction() != null)
        {
            Onion_Tears tears = SubAction.GetSubAction() as Onion_Tears;
            if (tears != null)
            {
                tears.TearEnd();
            }
        }
    }

    private void DeadthEvent()
    {
        Destroy(gameObject);
    }

}
