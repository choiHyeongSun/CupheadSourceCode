using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Potato_Shoot : SubActionBase
{

    [SerializeField]
    private GameObject BulletPrefab;
    [SerializeField]
    private GameObject ParryBulletPrefab;

    [SerializeField]
    private GameObject DustPrefab;

    [SerializeField]
    private float Delay = 3;
    [SerializeField]
    private int MaxShoot = 3;
    [SerializeField]
    private float[] ShootAnimSpeed;

    [SerializeField]
    private FHitData HitData;

    private float CurrentDelay = 0;
    private int CurrentShoot = 0;
    private Animator Anim;


    public override void Initialization()
    {
        CurrentDelay = Time.time + Delay;
    }
    protected override void Awake()
    {
        base.Awake();
        Anim = GetOwner.GetComponent<Animator>();
        CurrentDelay = Time.time + Delay;

    }

    public override bool CanSubAction()
    {
        if (GetEnable() == true)
            return false;
        return CurrentDelay < Time.time;
    }

    public override void AnimEvent()
    {
        base.AnimEvent();


        CurrentShoot++;
        GameObject bulletObject = null;

        if (BulletPrefab == null)
        {
            CurrentShoot = MaxShoot;
            return;
        }

        if (CurrentShoot != MaxShoot)
        {
            bulletObject = Instantiate(BulletPrefab, GetShootTrans.position, Quaternion.identity);
            SoundManager.gInstance.PlaySound("PotatoSpit", GetOwner.transform);
        }
        else
        {
            if (ParryBulletPrefab == null) return;
            bulletObject = Instantiate(ParryBulletPrefab, GetShootTrans.position, Quaternion.identity);

            if (DustPrefab == null) return;
            Instantiate(DustPrefab, GetShootTrans.position, Quaternion.identity);

            SoundManager.gInstance.PlaySound("PotatoSpit", GetOwner.transform);


        }

        Bullet bullet = bulletObject.GetComponent<Bullet>();

        if (bullet == null)
        {
            Destroy(bullet);
            return;
        }

        bullet.SetDirection(Vector2.left);
        bullet.SetOwner(GetOwner);
        bullet.SetHitData(HitData);

        if (DustPrefab != null)
        {
            Instantiate(DustPrefab, GetShootTrans.position, Quaternion.identity);
        }
    }

    public override void SubAction(float Horizontal, float Vertical)
    {
        base.SubAction(Horizontal, Vertical);
        StatusComponent status = GetOwner.GetComponent<StatusComponent>();
        int animSpeedIndex = 0;

        if (status != null)
        {
            int size = (ShootAnimSpeed.Length - 1);
            animSpeedIndex = size - Mathf.RoundToInt(status.GetCurrentHp / status.GetMaxHp) * size;

        }

        Anim.SetBool("SubAction", true);
        Anim.speed = ShootAnimSpeed[animSpeedIndex];
        CurrentShoot = 0;
    }

    public override void EndSubAction()
    {
        base.EndSubAction();
        CurrentDelay = Time.time + Delay;
        Anim.SetBool("SubAction", false);
        Anim.speed = 1.0f;
    }


    public bool IsShootEnd => CurrentShoot == MaxShoot;
}
