using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot_AnimEvent : MonoBehaviour
{

    private SubActionComponent SubAction;
    private void Awake()
    {
        SubAction = GetComponent<SubActionComponent>();
    }

    private void EndBeam()
    {
        Carrot_Beam beam = SubAction.GetSubAction() as Carrot_Beam;
        if (beam.GetEndBeam == false)
        {
            SubAction.EndSubAction();
        }
    }
}
