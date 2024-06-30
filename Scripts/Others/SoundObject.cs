using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Transform = UnityEngine.Transform;


[RequireComponent(typeof(AudioSource))]
public class SoundObject : MonoBehaviour
{
    private AudioSource Audio;

    public Action<int, SoundObject> SoundStopEvent;
    public Action SoundStopCallbackEvent;

    private bool IsPlaying;


    private void Awake()
    {
        Audio = GetComponent<AudioSource>();
    }

    public void Play(AudioClip Clip, Transform Parent, float Volume = 1.0f,bool IsLoop = false)
    {
        Audio.clip = Clip;
        Audio.loop = IsLoop;
        Audio.volume = Volume;
        Audio.Play();

        if (Parent != null)
        {
            transform.parent = Parent;
            transform.localPosition = Vector3.zero;
            transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        }
        IsPlaying = true;

    }



    public void Stop()
    {
        Audio.Stop();
    }

    public void Update()
    {
        if (Audio.isPlaying == false && IsPlaying == true)
        {
            if (SoundStopEvent != null)
            {
                SoundStopEvent(gameObject.GetHashCode() , this);
            }

            if (SoundStopCallbackEvent != null)
            {
                SoundStopCallbackEvent();
                SoundStopCallbackEvent = null;
            }

            IsPlaying = false;
        }
    }

    public bool gIsPlaying => IsPlaying;
}
