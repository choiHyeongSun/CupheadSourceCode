using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColliderHit : MonoBehaviour
{

    [SerializeField]
    private FHitData HitData;

    private StatusComponent Status;
    private Dictionary<int, IHitEvent> HitEvents = new Dictionary<int, IHitEvent>();
    private AActor Owner;

    private void Awake()
    {
        Owner = GetComponentInParent<AActor>();
        Status = Owner.GetActorComponent<StatusComponent>();
    }


    private void Update()
    {
        if (Status != null)
        {
            if (Status.IsDead == false)
            {
                foreach (var hitevent in HitEvents)
                {
                    hitevent.Value.HitEvent(HitData);
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

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
