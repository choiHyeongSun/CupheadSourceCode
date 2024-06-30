using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Carrot : Boss, IHitEvent
{
    [SerializeField]
    private SpriteRenderer Renderer;
    [SerializeField]
    private BoxCollider2D CarrotBombSpawner;
    [SerializeField]
    private GameObject CarrotBombPrefab;
    [SerializeField]
    private FHitData HitData;


    private SubActionComponent SubAction;
    private StatusComponent Status;

    private Coroutine HitCoroutine;
    private Animator Anim;

    private Coroutine CreateBomdCoroutine;
    private BoxCollider2D Box2D;



    protected override void Awake()
    {
        base.Awake();
        SubAction = GetComponent<SubActionComponent>();
        Status = GetComponent<StatusComponent>();
        Anim = GetComponent<Animator>();
        Box2D = GetComponent<BoxCollider2D>();

        Status.DeadEvent = DeadEvent;
        if (Renderer != null)
        {
            Renderer.sharedMaterial = Material.Instantiate(Renderer.sharedMaterial);
        }

        SoundManager.gInstance.PlaySound("CarrotRise", transform);
    }

    protected override void Update()
    {
        if (Status.IsDead == true || gIsIntro == true || GetGameManager.gIsGameProduction == true) return;

        base.Update();
        if (IsCanSubAction() == true)
        {
            SubAction.SubAction(0);
        }
        

        if (SubAction.GetEnable() == false)
        {
            if (CreateBomdCoroutine == null)
                CreateBomdCoroutine = StartCoroutine(CreateBomb());
        }
        else
        {
            if (CreateBomdCoroutine != null)
            {
                StopCoroutine(CreateBomdCoroutine);
                CreateBomdCoroutine = null;
            }
        }
    }

    public void HitEvent(FHitData To)
    {
        if (Status.IsDead == true) return;
        Status.DecreaseHp(To.gDamage);
        if (HitCoroutine == null)
        {
            HitCoroutine = StartCoroutine(DecreaseHpEvent());
        }
    }

    private IEnumerator DecreaseHpEvent()
    {
        Renderer.sharedMaterial.SetFloat("_Hit", 0.5f);
        yield return new WaitForSeconds(0.05f);
        Renderer.sharedMaterial.SetFloat("_Hit", 0.0f);
        HitCoroutine = null;
    }

    private void DeadEvent()
    {

        if (SubAction.GetEnable() == true)
        {
            SubAction.EndSubAction();
        }

        if (Anim != null)
        {
            Anim.SetTrigger("Deadth");
        }

        if (CreateBomdCoroutine != null)
        {
            StopCoroutine(CreateBomdCoroutine);
            CreateBomdCoroutine = null;
        }
        SoundManager.gInstance.PlaySound("CarrotDie", transform);
        StartCoroutine(SpawnBossExploreCoroutine(Box2D));

    }

    private IEnumerator CreateBomb()
    {
        Vector2 carrotSpawnTime = new Vector2(1.0f, 1.5f);
        AActor target = FindObjectOfType<APlayer>();
        while (true)
        {
            float spawnTime = Random.Range(carrotSpawnTime.x, carrotSpawnTime.y);
            yield return new WaitForSeconds(spawnTime);

            float spawnX = Random.Range(CarrotBombSpawner.bounds.min.x, CarrotBombSpawner.bounds.max.x);
            float spawnY = Random.Range(CarrotBombSpawner.bounds.min.y, CarrotBombSpawner.bounds.max.y);

            if (CarrotBombPrefab != null)
            {
                GameObject obj = Instantiate(CarrotBombPrefab, new Vector3(spawnX, spawnY), Quaternion.identity);
                Bullet bullet = obj.GetComponent<Bullet>();
                bullet.SetOwner(this);
                bullet.SetTarget(target);
                bullet.SetHitData(HitData);
            }


        }
    }
}
