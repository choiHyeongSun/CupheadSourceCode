using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Goopy : Boss, IHitEvent
{
    [SerializeField]
    private SpriteRenderer PageOneOrTwoRenderer;
    [SerializeField]
    private SpriteRenderer PageThreeRenderer;
    [SerializeField]
    private GameObject JumpDust;
    [SerializeField]
    private FHitData HitData;


    private SubActionComponent SubAction;
    private StatusComponent Status;


    private Coroutine HitCoroutine;
    private Animator Anim;

    private CircleCollider2D Circle2D;
    private Dictionary<int, IHitEvent> HitEvents = new Dictionary<int, IHitEvent>();

    private int PageIndex = 0;


    public override void Initialization()
    {
        base.Initialization();
        SubAction.Initialization();
    }

    protected override void Awake()
    {
        base.Awake();
        SubAction = GetComponent<SubActionComponent>();
        Status = GetComponent<StatusComponent>();
        Anim = GetComponent<Animator>();
        Circle2D = GetComponent<CircleCollider2D>();

        if (PageOneOrTwoRenderer != null)
        {
            PageOneOrTwoRenderer.sharedMaterial = Material.Instantiate(PageOneOrTwoRenderer.sharedMaterial);
        }

        if (PageThreeRenderer != null)
        {
            PageThreeRenderer.sharedMaterial = Material.Instantiate(PageThreeRenderer.sharedMaterial);
        }

        Status.DeadEvent = DeadEvent;
    }
    protected override void Update()
    {
        if (Status.IsDead == true) return;

        if (PageIndex < 2)
        {
            foreach (var hitevent in HitEvents)
            {
                hitevent.Value.HitEvent(HitData);
            }
        }

        if (gIsIntro == true || GetGameManager.gIsGameProduction == true) return;

        if (PageIndex < 2)
        {
            PageOneAndTwo();
        }
        else
        {
            if (IsCanSubAction() == true)
            {
                SubAction.SubAction(2);
            }
            else
            {

                if (SubAction.GetSubAction() is Goopy_Punch == false)
                {
                    SubActionBase action = SubAction.GetSubAction();

                    SubAction.SubAction(1);
                    if (SubAction.GetSubAction() != action && action != null)
                    {
                        action.EndSubAction();

                    }
                }
                
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
        if (PageIndex == 2)
        {
            PageThreeRenderer.sharedMaterial.SetFloat("_Hit", 0.5f);
        }
        else
        {
            PageOneOrTwoRenderer.sharedMaterial.SetFloat("_Hit", 0.5f);
        }
        
        yield return new WaitForSeconds(0.05f);

        if (PageIndex == 2)
        {
            PageThreeRenderer.sharedMaterial.SetFloat("_Hit", 0.0f);
        }
        else
        {
            PageOneOrTwoRenderer.sharedMaterial.SetFloat("_Hit", 0.0f);
        }
        
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
        StartCoroutine(SpawnBossExploreCoroutine(Circle2D));
        SoundManager.gInstance.PlaySound("BigDeath", transform);
    }

    private void EndSubAction()
    {
        if (SubAction.GetSubAction() != null)
        {
            SubAction.EndSubAction();
        }
    }

    


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (TriggerEnter2D != null)
        {
            TriggerEnter2D(collision);
        }

        if (PageIndex == 2) return;
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
        if (TriggerExit2D != null)
        {
            TriggerExit2D(collision);
        }

        AActor toActor = collision.gameObject.GetComponent<AActor>();
        if (toActor is IHitEvent == true)
        {
            HitEvents.Remove(toActor.GetHashCode());
        }
    }


    private void PageOneAndTwo()
    {
        base.Update();

        if (IsCanSubAction() == true)
        {
            SubAction.SubAction(1);
            if (SubAction.GetSubAction() == null)
            {
                SubAction.SubAction(0);
                if (PageIndex == 1)
                {
                    Vector3 offset = PageOneOrTwoRenderer.bounds.center + Vector3.down * 1.0f;
                    Instantiate(JumpDust, offset, Quaternion.identity);
                }
            }
        }

        Goopy_Jump jump = SubAction.GetSubAction() as Goopy_Jump;

        if (jump != null)
        {
            if (jump.gJumpCheck == true)
            {
                SubAction.EndSubAction();
            }
        }
        if (IsCanSubAction())
        {
            if (PageIndex == 0 && Status.GetCurrentHp / Status.GetMaxHp < 0.8f && Status.GetCurrentHp / Status.GetMaxHp > 0.2f)
            {
                PageIndex++;
                Anim.SetInteger("Page", PageIndex);
                return;
            }
            if (PageIndex == 1 && Status.GetCurrentHp / Status.GetMaxHp < 0.4f)
            {
                PageIndex++;
                Anim.SetInteger("Page", PageIndex);
                PageOneOrTwoRenderer.sharedMaterial.SetFloat("_Hit", 0.0f);
                return;
            }
        }
     
    }

    public int gPageIndex => PageIndex;
}
