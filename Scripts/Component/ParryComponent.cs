using System;
using System.Collections;
using System.Collections.Generic;
using CustomComponent;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class ParryComponent : ComponentBase
{
    [SerializeField]
    private float ParryVelocity;
    [SerializeField]
    private GameObject ParryEffect;
    [SerializeField]
    private GameObject ParryAround;

    private bool IsParry;
    private bool IsParrying;
    
    private Animator Anim;
    private Rigidbody2D Rigid2D;

    private Vector3 Velocity;

    protected override void Awake()
    {
        base.Awake();
        Anim = GetComponent<Animator>();
        Rigid2D = GetComponent<Rigidbody2D>();
        SetEnable(false);
    }
    protected override void Update()
    {
        base.Update();
        if (GetOwner.GetComponent<JumpComponent>().GetEnable() == false)
        {
            StopParry();
        }
    }
    public void EnableParry()
    {
        IsParry = true;
        Anim.SetBool("Parrying", true);
        SetEnable(true);
    }

    public void StopParry()
    {
        IsParry = false;
        Anim.SetBool("Parrying", false);
        SetEnable(false);
    }

    public void Parry(Vector3 EffectPos)
    {
        if (IsParry == true && GetOwner.GetComponent<JumpComponent>().GetEnable() == true)
        {
            Velocity = Vector3.up * ParryVelocity;
            StartCoroutine(Global.SlowMotion(0.15f, 0));

            if (ParryEffect != null)
            {
                Instantiate(ParryEffect, EffectPos, Quaternion.identity);
            }

            if (ParryAround != null)
            {
                ParryAround.SetActive(true);
            }


            
            string[] parrySound = new[] { "Parry1", "Parry2" };
            int index = Random.Range(0, 1);
            SoundManager.gInstance.PlaySound(parrySound[index], GetOwner.transform);

            APlayer player = GetOwner as APlayer;
            if (player == null) return;
            player.IncreaseSkillGauge(1.0f);


        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Global.IsNearFloatZeroCheck(Velocity.magnitude) == false)
        {
            Rigid2D.velocity = Velocity;
            Velocity = Vector3.zero;
            Anim.SetTrigger("Parry");
            StopParry();
        }
    }


    public bool GetParry => IsParry;
    public bool GetParrying => IsParrying;
}
