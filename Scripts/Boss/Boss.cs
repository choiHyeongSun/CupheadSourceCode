using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : AActor
{
    [SerializeField]
    private GameObject BossExploreEffect;
    [SerializeField]
    private float Duration = 0.6f;

    protected IEnumerator SpawnBossExploreCoroutine(BoxCollider2D Box2D)
    {
        while (true)
        {
            Vector2 min = Box2D.bounds.min;
            Vector2 max = Box2D.bounds.max;
            Vector2 random = Global.RandomVector2(new Vector2(min.x, max.x), new Vector2(min.y, max.y));
            Instantiate(BossExploreEffect, new Vector3(random.x, random.y), Quaternion.identity);
            yield return new WaitForSeconds(Duration);
        }
    }

    protected IEnumerator SpawnBossExploreCoroutine(CircleCollider2D Box2D)
    {
        while (true)
        {
            Vector2 min = Box2D.bounds.min;
            Vector2 max = Box2D.bounds.max;
            Vector2 random = Global.RandomVector2(new Vector2(min.x, max.x), new Vector2(min.y, max.y));
            Instantiate(BossExploreEffect, new Vector3(random.x, random.y), Quaternion.identity);
            yield return new WaitForSeconds(Duration);
        }
    }


    public Action<Collider2D> TriggerEnter2D;
    public Action<Collider2D> TriggerExit2D;


}
