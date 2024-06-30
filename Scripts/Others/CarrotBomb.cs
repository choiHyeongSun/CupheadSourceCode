using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class CarrotBomb : Bullet, IHitEvent
{
    [SerializeField]
    private float RotateSpeed = 1.0f;
    private Rigidbody2D Rigid2D;
    private StatusComponent Status;

    private Coroutine HitCoroutine;
    protected override void Awake()
    {
        base.Awake();
        Rigid2D = GetComponent<Rigidbody2D>();
        Status = GetComponent<StatusComponent>();
        Status.DeadEvent = DeadthEvent;

        if (GetRenderer != null)
        {
            GetRenderer.sharedMaterial = Material.Instantiate(GetRenderer.sharedMaterial);
        }
    }

    protected override void Update()
    {
        float2 velocity = Rigid2D.velocity;
        float3 position = transform.position;

        if (GetBox2D != null)
        {
            position = GetBox2D.bounds.center;

            if (velocity.x < 0.0f) position.x = GetBox2D.bounds.max.x;
            else position.x = GetBox2D.bounds.min.x;

            if (velocity.y < 0.0f) position.y = GetBox2D.bounds.max.y;
            else position.y = GetBox2D.bounds.min.y;
        }

        Vector3 point = Camera.main.WorldToViewportPoint(position);
        if (point.x < 0.0f || point.y < 0.2f)
        {
            DeadthEvent();
        }
        else if (point.x > 1.0f || point.y > 2.0f)
        {
            DeadthEvent();
        }
    }

    protected override void FixedUpdate()
    {
        if (GetTarget != null)
        {
            Vector3 dir = (GetTarget.transform.position - transform.position).normalized;
            Quaternion rotate = Global.LookAtDirection(dir) * Quaternion.Euler(0.0f, 0.0f ,90.0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, RotateSpeed);

            Rigid2D.velocity = -transform.up * GetSpeed;
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
        GetRenderer.sharedMaterial.SetFloat("_Hit", 0.5f);
        yield return new WaitForSeconds(0.05f);
        GetRenderer.sharedMaterial.SetFloat("_Hit", 0.0f);
        HitCoroutine = null;
    }


    private void DeadthEvent()
    {
        if (GetPeashotDeath != null)
        {
            Instantiate(GetPeashotDeath, transform.position, Quaternion.identity);
        }
        SoundManager.gInstance.PlaySound("CarrotBombDead");
        Destroy(gameObject);
    }

    protected override void TriggerEnter(Collider2D collision)
    { 
        base.TriggerEnter(collision);
        SoundManager.gInstance.PlaySound("CarrotBombDead");
    }
}
