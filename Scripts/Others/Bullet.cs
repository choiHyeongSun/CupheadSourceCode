using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : AActor
{
    [SerializeField]
    private float Speed;
    [SerializeField]
    private bool IsParry;
    [SerializeField]
    private GameObject BulletDead;



    private Camera Cam;

    private Vector2 Direction = Vector2.right;
    private Rigidbody2D Rigid;
    private BoxCollider2D Box;
    private AActor Owner;
    private FHitData HitData;

    private Sprite ParrySprite;
    private SpriteRenderer Renderer;
    private AActor Target;



    public Action HitIncreaseAction;

    protected override void Awake()
    {
        base.Awake();
        Rigid = GetComponent<Rigidbody2D>();
        Box = GetComponent<BoxCollider2D>();
        Renderer = GetComponent<SpriteRenderer>();

        if (IsParry == true && ParrySprite != null)
        {
            Renderer.sprite = ParrySprite;
        }

        if (Cam == null)
        {
            Cam = Camera.main;
        }
    }

    protected override void Update()
    {
        float2 velocity = Rigid.velocity;
        float3 position = transform.position;

        if (Box != null)
        {
            position = Box.bounds.center;

            if (velocity.x < 0.0f) position.x = Box.bounds.max.x;
            else position.x = Box.bounds.min.x;

            if (velocity.y < 0.0f) position.y = Box.bounds.max.y;
            else position.y = Box.bounds.min.y;
        }

        Vector3 point = Cam.WorldToViewportPoint(position);
        if (point.x < 0.0f || point.y < 0.0f)
        {
            Destroy(gameObject);
        }
        else if (point.x > 1.0f || point.y > 1.0f)
        {
            Destroy(gameObject);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Parry") == true) return;

        ITeamGenerator to = collision.gameObject.GetComponent<ITeamGenerator>();
        ITeamGenerator from = Owner;
        ETeamCheckResult result = Global.TeamChecker(to, from);
        if (result == ETeamCheckResult.Same) return;

        AActor toActor = collision.gameObject.GetComponent<AActor>();
        AActor fromActor = Owner;
        if (toActor == null) return;

        HitData.gTo = toActor;
        HitData.gFrom = fromActor;

        IHitEvent hitEvent = toActor.GetComponent<IHitEvent>();
        HitComponent hit = toActor.GetComponent<HitComponent>();
        StatusComponent status = toActor.GetComponent<StatusComponent>();
        ParryComponent parry = toActor.GetActorComponent<ParryComponent>();

        if (hit != null)
        {
            if (hit.GetHitImmune == true || status.IsDead == true) return;
        }

        if (hitEvent != null)
        {
            
            if (IsParry == true)
            {
                if (parry.GetParry == true)
                {
                    parry.Parry(transform.position);
                    TriggerEnter(collision);
                    return;
                }
            }

            hitEvent.HitEvent(HitData);
            TriggerEnter(collision);

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals("Parry") == false) return;
        
        ParryCollision parryCollision = collision.gameObject.GetComponent<ParryCollision>();
        AActor toActor = parryCollision.GetOwner;
        ParryComponent parry = toActor.GetActorComponent<ParryComponent>();
        
        if (parry != null && IsParry == true)
        {
            
            if (parry.GetParry == true)
            {
                parry.Parry(transform.position);
                TriggerEnter(collision);
                return;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TriggerExit(collision);
    }


    protected virtual void TriggerEnter(Collider2D collision)
    {
        if (GetPeashotDeath != null)
        {
            Instantiate(GetPeashotDeath, transform.position, Quaternion.identity);
        }

        if (HitIncreaseAction != null) HitIncreaseAction();
        Destroy(gameObject);
    }

    protected virtual void TriggerExit(Collider2D collision)
    {
    }

    public void SetDirection(Vector2 InValue)
    {
        Direction = InValue;
        Rigid.velocity = Direction * Speed;
    }



    public Vector2 gDirection => Direction;
    public FHitData GetHitData => HitData;
    protected AActor GetOwner => Owner;
    protected AActor GetTarget => Target;
    protected Rigidbody2D GetRigidbody2D => Rigid;
    protected BoxCollider2D GetBox2D => Box;
    protected Renderer GetRenderer => Renderer;


    protected float GetSpeed => Speed;
    public void SetOwner(AActor InValue)
    {
        Owner = InValue;
        SetTeamID(InValue.GetTeamID());

    }
        
    public void SetTarget(AActor InValue) => Target = InValue;
    public void SetCamera(Camera InCam) => Cam = InCam;

    public void SetHitData(FHitData InValue) => HitData = InValue;
    public GameObject GetPeashotDeath => BulletDead;



}

