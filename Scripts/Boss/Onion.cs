using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Onion : Boss, IHitEvent
{
    [SerializeField]
    private SpriteRenderer Renderer;
    [SerializeField]
    private FHitData HitData;

    private SubActionComponent SubAction;
    private StatusComponent Status;

    private Coroutine HitCoroutine;
    private BoxCollider2D Box2D;
    private Animator Anim;

    private Dictionary<int, IHitEvent> HitEvents = new Dictionary<int, IHitEvent>();

    protected override void Awake()
    {
        base.Awake();
        SubAction = GetComponent<SubActionComponent>();
        Status = GetComponent<StatusComponent>();
        Box2D = GetComponent<BoxCollider2D>();
        Anim = GetComponent<Animator>();
        Status.DeadEvent = DeadEvent;
        if (Renderer != null)
        {
            Renderer.sharedMaterial = Material.Instantiate(Renderer.sharedMaterial);
        }
        SoundManager.gInstance.PlaySound("OnionRiseGround", transform);

    }

    protected override void Update()
    {
        if (Status.IsDead == true || gIsIntro == true || GetGameManager.gIsGameProduction == true) return;
        base.Update();
        
    }

    protected override void LateUpdate()
    {
        if (Status.IsDead == false)
        {
            foreach (var hitevent in HitEvents)
            {
                hitevent.Value.HitEvent(HitData);
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

        if (IsCanSubAction() == true)
        {
            SubAction.SubAction(0);
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
        StartCoroutine(SpawnBossExploreCoroutine(Box2D));
        SoundManager.gInstance.PlaySound("OnionDie", transform);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        ITeamGenerator to = collision.gameObject.GetComponent<ITeamGenerator>();
        ITeamGenerator from = this;
        ETeamCheckResult result = Global.TeamChecker(to, from);
        if (result == ETeamCheckResult.Same) return;

        AActor toActor = collision.gameObject.GetComponent<AActor>();
        AActor fromActor = this;
        if (toActor == null) return;

        HitData.gTo = toActor;
        HitData.gFrom = fromActor;

        IHitEvent hitEvent = toActor.GetComponent<IHitEvent>();
        HitComponent hit = toActor.GetComponent<HitComponent>();

        if (hitEvent != null && HitEvents.ContainsKey(toActor.GetHashCode()) == false)
        {
            HitEvents.Add(toActor.GetHashCode(), hitEvent);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        AActor toActor = collision.gameObject.GetComponent<AActor>();
        if (toActor is IHitEvent == true)
        {
            HitEvents.Remove(toActor.GetHashCode());
        }
    }


}
