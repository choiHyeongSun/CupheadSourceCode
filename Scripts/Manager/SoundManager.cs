using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{

    private static SoundManager Instance;

    [System.Serializable]
    internal struct AudioClipNode
    {
        [SerializeField]
        private String ClipName;
        [SerializeField]
        private AudioClip Clip;

        public String gClipName => ClipName;
        public AudioClip gClip => Clip;
    }
    
    [SerializeField]
    private GameObject SoundPrefabs;
    [SerializeField]
    private List<AudioClipNode> SoundClips;



    private Dictionary<String, AudioClip> Clips = new Dictionary<string, AudioClip>();
    private Dictionary<int, SoundObject> UseSound = new Dictionary<int, SoundObject>();
    private Queue<SoundObject> SoundQue = new Queue<SoundObject>();


    




    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    private void Start()
    {
        foreach (var clipNode in SoundClips)
        {
            Clips.Add(clipNode.gClipName, clipNode.gClip);

            if (clipNode.gClipName.Equals("BGM"))
            {
                PlaySound("BGM", transform, 0.3f, true);
            }


        }


    }



    public int PlaySound(String ClipName, Transform Parent, Action StopAciton ,float Volume = 1.0f, bool IsLoop = false)
    {
        if (Clips.ContainsKey(ClipName) == false) return 0;

        int num = PlaySound(ClipName, Parent, Volume, IsLoop);
        UseSound[num].SoundStopCallbackEvent += StopAciton;
        return UseSound[num].gameObject.GetHashCode();
    }

    public int PlaySound(String ClipName, Transform Parent = null, float Volume = 1.0f, bool IsLoop = false)
    {
        if (Clips.ContainsKey(ClipName) == false) return 0;

        if (SoundQue.Count == 0)
        {
            GenerateSoundObject();
        }
        
        SoundObject obj = SoundQue.Dequeue();
        obj.Play(Clips[ClipName], Parent, Volume, IsLoop);
        UseSound.Add(obj.gameObject.GetHashCode(), obj);
        return obj.gameObject.GetHashCode();
    }


    public void StopSound(int HashCode)
    {
        UseSound[HashCode].Stop();
    }

    private void GenerateSoundObject()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(SoundPrefabs, transform);
            SoundObject sound = obj.GetComponent<SoundObject>();
            sound.SoundStopEvent = StopSoundEvent;
            SoundQue.Enqueue(sound);
        }
    }

    private void StopSoundEvent(int HashCode ,  SoundObject SoundSource)
    {
        UseSound[HashCode].transform.parent = transform;
        SoundQue.Enqueue(UseSound[HashCode]);
        UseSound.Remove(HashCode);

    }


    public static SoundManager gInstance => Instance;


}
