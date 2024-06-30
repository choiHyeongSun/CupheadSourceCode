using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot_Beam : SubActionBase
{

    [SerializeField]
    private float BeamCoolTime = 3.0f;

    [SerializeField]
    private float BeamTime = 1.0f;

    [SerializeField]
    private float MaxBeamCount = 3;

    [SerializeField]
    private GameObject BeamPrefab;

    [SerializeField]
    private GameObject BeamIntroPrefab;

    [SerializeField]
    private FHitData HitData;


    private bool IsEndBeam;
    private float CurrentCoolTime = 0.0f;
    private Animator Anim;
    private Coroutine BeamCoroutine;
    private GameObject BeamIntroEffect;
    private int SoundHandle;
    public override void Initialization()
    {
        CurrentCoolTime = Time.time + BeamCoolTime;
    }
    protected override void Awake()
    {
        base.Awake();
        CurrentCoolTime = Time.time + BeamCoolTime;
        Anim = GetOwner.GetComponent<Animator>();
    }

    public override bool CanSubAction()
    {
        return CurrentCoolTime < Time.time;
    }

    public override void SubAction(float Horizontal, float Vertical)
    {
        Anim.SetBool("SubAction", true);
        
    }

    public override void EndSubAction()
    {
        base.EndSubAction();
        if (BeamCoroutine != null)
        {
            StopCoroutine(BeamCoroutine);
            IsEndBeam = false;
            BeamCoroutine = null;
        }
        Anim.SetBool("SubAction", false);
        CurrentCoolTime = Time.time + BeamCoolTime;

    }

    public override void AnimEvent()
    {
        base.AnimEvent();
        if (BeamCoroutine == null)
        {

            BeamCoroutine = StartCoroutine(Beam());
        }
            
    }

    private IEnumerator Beam()
    {
        const int BeamCount = 4;
        const float BeamDelay = 0.2f;
        IsEndBeam = true;

        for (int i = 0; i < MaxBeamCount; i++)
        {
            BeamIntroEffect = Instantiate(BeamIntroPrefab, GetShootTrans.position, Quaternion.identity);
            SoundHandle = SoundManager.gInstance.PlaySound("MindMeldLoop", GetOwner.transform, 1.0f, true);
            yield return new WaitUntil(() => BeamIntroEffect == null);

            SoundManager.gInstance.StopSound(SoundHandle);
            SoundManager.gInstance.PlaySound("MindMeldBeam", GetOwner.transform);

            APlayer player = FindObjectOfType<APlayer>();
            Vector3 dir = (player.transform.position - GetShootTrans.position).normalized;

            for (int beam = 0; beam < BeamCount; beam++)
            {
                if (BeamPrefab != null)
                {
                    GameObject obj = Instantiate(BeamPrefab, GetShootTrans.position, Global.LookAtDirection(dir));
                    Bullet bullet = obj.GetComponent<Bullet>();

                    bullet.SetOwner(GetOwner);
                    bullet.SetDirection(dir);
                    bullet.SetHitData(HitData);
                }
                else
                {
                    yield break;
                }
                yield return new WaitForSeconds(BeamDelay);
            }
            yield return new WaitForSeconds(BeamTime);
        }

        IsEndBeam = false;
    }

    public bool GetEndBeam => IsEndBeam;
}
