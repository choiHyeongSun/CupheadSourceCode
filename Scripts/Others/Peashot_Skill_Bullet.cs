using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peashot_Skill_Bullet : Bullet
{

    [SerializeField]
    private int MaxCount = 4;

    private List<IHitEvent> HitEvents = new List<IHitEvent>();

    private Coroutine HitCoroutine;
    private Vector3 Velocity;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void TriggerEnter(Collider2D collision)
    {
        IHitEvent hitEvent = collision.gameObject.GetComponent<IHitEvent>();
        if (hitEvent != null)
        {
            HitEvents.Add(hitEvent);
        }

        if (HitCoroutine == null)
        {
            StartCoroutine(HitEvent());
        }
    }
    protected override void TriggerExit(Collider2D collision)
    {
        IHitEvent hitEvent = collision.gameObject.GetComponent<IHitEvent>();
        if (hitEvent != null)
        {
            HitEvents.Remove(hitEvent);
        }

        if (HitEvents.Count == 0)
        {
            if (HitCoroutine != null)
            {
                StopCoroutine(HitCoroutine);
                GetRigidbody2D.velocity = Velocity;

            }

            HitCoroutine = null;
        }
    }

    private IEnumerator HitEvent()
    {
        Velocity = GetRigidbody2D.velocity;

        while (true)
        {
            MaxCount--;
            GetRigidbody2D.velocity = Vector2.zero;
            SoundManager.gInstance.PlaySound("PeashootImpact02", GetOwner.transform);
            yield return new WaitForSeconds(0.05f);
            GetRigidbody2D.velocity = Velocity;
            yield return new WaitForSeconds(0.05f);

            for (int i = 0; i < HitEvents.Count; i++)
            {
                HitEvents[i].HitEvent(GetHitData);
            }

            if (MaxCount == 0)
            {
                if (GetPeashotDeath != null)
                {
                    Instantiate(GetPeashotDeath, transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
                break;
            }
        }
    }

    private void OnDestroy()
    {
        if (HitCoroutine != null)
        {
            StopCoroutine(HitCoroutine);
 
        }
    }
}
