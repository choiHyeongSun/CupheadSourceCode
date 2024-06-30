using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEffect : MonoBehaviour
{

   
    private void EffectEnd()
    {
        Destroy(gameObject);
    }

    private void AnimEvent()
    {
        if (AnimEventCallback != null)
        {
            AnimEventCallback();
        }
    }

    public Action AnimEventCallback;

}
