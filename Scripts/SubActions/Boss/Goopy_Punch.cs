using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goopy_Punch : SubActionBase
{

    [SerializeField]
    private float CoolTime = 5.0f;
    private float CurrentCoolTime = 0.0f;


    private Animator Anim;

    public override void Initialization()
    {
        base.Initialization();
        CurrentCoolTime = Time.time + CoolTime;
    }

    protected override void Awake()
    {
        base.Awake();
        CurrentCoolTime = Time.time + CoolTime;

        Anim = GetOwner.GetComponent<Animator>();
    }

    public override bool CanSubAction()
    {
        return Time.time > CurrentCoolTime;
    }

    public override void SubAction(float Horizontal, float Vertical)
    {
        base.SubAction(Horizontal, Vertical);
        Anim.SetTrigger("Punch");

        Goopy goopy = GetOwner as Goopy;

        if (goopy.gPageIndex == 0)
            SoundManager.gInstance.PlaySound("SmallPunch");
        else if (goopy.gPageIndex == 1)
            SoundManager.gInstance.PlaySound("BigPunch");
        else if (goopy.gPageIndex == 2)
            SoundManager.gInstance.PlaySound("tombstoneSplat"); 


    }

    public override void EndSubAction()
    {
        base.EndSubAction();
        CurrentCoolTime = Time.time + CoolTime;
    }
}
