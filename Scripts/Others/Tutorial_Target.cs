using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Target : AActor , IHitEvent
{
    [SerializeField]
    private GameObject DeadEffect;
    private StatusComponent Status;
    private Transform Parent;

    private Coroutine HitCoroutine;
    private List<SpriteRenderer> Renderers = new List<SpriteRenderer>();


    protected override void Awake()
    {
        base.Awake();
        Parent = transform.parent;
        Renderers.Add(GetComponent<SpriteRenderer>());
        Renderers.Add(GetComponentInParent<SpriteRenderer>());
        Status = GetComponent<StatusComponent>();

        foreach (var render in Renderers)
        {
            if (render != null)
            {
                render.sharedMaterial = Material.Instantiate(render.sharedMaterial);
            }
        }

        Status.DeadEvent = DeadEvent;
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

    private void DeadEvent()
    {
        Instantiate(DeadEffect, transform.position, Quaternion.identity);
        Destroy(Parent.gameObject);
    }

    private IEnumerator DecreaseHpEvent()
    {
        foreach (var render in Renderers)
        {
            render.sharedMaterial.SetFloat("_Hit", 0.5f);
        }
        
        yield return new WaitForSeconds(0.05f);

        foreach (var render in Renderers)
        {
            render.sharedMaterial.SetFloat("_Hit", 0.0f);
        }
        HitCoroutine = null;
    }
}
