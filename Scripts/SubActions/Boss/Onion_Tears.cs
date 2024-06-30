using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class Onion_Tears : SubActionBase
{

    [SerializeField]
    private GameObject TearBulletPrefab;
    [SerializeField]
    private GameObject TearParryBulletPrefab;

    [SerializeField]
    private float[] Durations;

    [SerializeField]
    private List<Vector2> SpawnTearTimes;

    [SerializeField]
    private BoxCollider2D LeftSpawner;
    [SerializeField]
    private BoxCollider2D RightSpawner;

    [SerializeField]
    private FHitData HitData;

    [Range(0.0f, 100.0f), SerializeField]
    private float ParryPercentage;

    private Animator Anim;

    private Coroutine TearCoroutine;
    private int Index;

    private int SoundHandle;

    protected override void Awake()
    {
        base.Awake();
        Anim = GetOwner.GetComponent<Animator>();
    }
    public override bool CanSubAction()
    {
        return true;
    }

    public void TearStart()
    {
        if (TearCoroutine == null)
        {
            TearCoroutine = StartCoroutine(SpawnTears());
            Index = (Index + 1) % Durations.Length;
            SoundHandle = SoundManager.gInstance.PlaySound("OnionCrying", GetOwner.transform, 1.0f, true);

        }

    }

    public void TearEnd()
    {
        if (TearCoroutine != null)
        {
            StopCoroutine(TearCoroutine);
            TearCoroutine = null;
            SoundManager.gInstance.StopSound(SoundHandle);
        }
    }


    public override void SubAction(float Horizontal, float Vertical)
    {
        base.SubAction(Horizontal, Vertical);
        Anim.SetBool("SubAction", true);
    }



    private IEnumerator SpawnTears()
    {
        float time = Time.time + Durations[Index];
        while (time > Time.time)
        {
            float randomSpawnTime = Random.Range(SpawnTearTimes[Index].x, SpawnTearTimes[Index].y);
            yield return new WaitForSeconds(randomSpawnTime);


            float percent = ParryPercentage * 0.01f;
            float random = Random.Range(0, 1.0f);

            int IsRight = Random.Range(1, 100);


            Vector2 minPosition = IsRight < 50.0f ? LeftSpawner.bounds.min : RightSpawner.bounds.min;
            Vector2 maxPosition = IsRight < 50.0f ? LeftSpawner.bounds.max : RightSpawner.bounds.max;

            Vector2 spawnPosition = new Vector2(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y));


            GameObject obj;
            if (random < percent)
            {
                obj = Instantiate(TearParryBulletPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                obj = Instantiate(TearBulletPrefab, spawnPosition, Quaternion.identity);
            }

            Bullet bullet = obj.GetComponent<Bullet>();
            bullet.SetOwner(GetOwner);
            bullet.SetHitData(HitData);

        }
        Anim.SetTrigger("EndSubAction");
    }
}
