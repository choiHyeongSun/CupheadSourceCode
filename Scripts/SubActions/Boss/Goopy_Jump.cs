using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goopy_Jump : SubActionBase
{
    [SerializeField]
    private float[] JumpForce;
    [SerializeField]
    private float Gravity = 7.0f;

    private Rigidbody2D Rigid2D;
    private Animator Anim;

    private bool DirCheck = false;
    private bool JumpCheck = true;


    public override void Initialization()
    {
        base.Initialization();
        DirCheck = false;
        SetEnable(false);

    }

    protected override void Awake()
    {
        base.Awake();
        Rigid2D = GetOwner.GetComponent<Rigidbody2D>();
        Anim = GetOwner.GetComponent<Animator>();
    }
    

    public override bool CanSubAction()
    {
        return JumpCheck;
    }

    public void JumpEvent()
    {
        int index = Random.Range(0, JumpForce.Length);
        float offset = 0.3f;
        if (Global.IsNearFloatZeroCheck(GetOwner.transform.rotation.eulerAngles.y))
        {
            DirCheck = false;
        }
        else if (Global.IsNearFloatCheck(GetOwner.transform.rotation.eulerAngles.y, 180.0f))
        {
            DirCheck = true;
        }

        if (DirCheck == true)
        {
            Rigid2D.velocity = (Vector2.right * offset + Vector2.up).normalized * JumpForce[index];
        }
        else
        {
            Rigid2D.velocity = (-Vector2.right * offset + Vector2.up).normalized * JumpForce[index];
        }

        Rigid2D.gravityScale = Gravity;
    }

    public void LookAt()
    {
        if (Global.IsNearFloatZeroCheck(GetOwner.transform.rotation.eulerAngles.y))
        {
            GetOwner.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
        else if (Global.IsNearFloatCheck(GetOwner.transform.rotation.eulerAngles.y, 180.0f))
        {
            GetOwner.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
    }

    public override void SubAction(float Horizontal, float Vertical)
    {
        base.SubAction(Horizontal, Vertical);
        Anim.SetTrigger("Jump");
        JumpCheck = false;

        Boss boss = GetOwner as Boss;
        if (boss != null)
        {
            boss.TriggerEnter2D += TriggerEnter2D;
        }

        Goopy goopy = GetOwner as Goopy;

        if (goopy.gPageIndex == 0)
            SoundManager.gInstance.PlaySound("SmallJump");
        if (goopy.gPageIndex == 1)
            SoundManager.gInstance.PlaySound("BigJump");
    }

    public override void EndSubAction()
    {
        base.EndSubAction();
        Rigid2D.velocity = Vector3.zero;
        Rigid2D.gravityScale = 0.0f;

        Boss boss = GetOwner as Boss;
        if (boss != null)
        {
            boss.TriggerEnter2D -= TriggerEnter2D;
        }
    }

    private void TriggerEnter2D(Collider2D collider)
    {
        if (collider.tag.Equals("Wall"))
        {
            Vector3 velocity = Rigid2D.velocity;
            velocity.x = -velocity.x;
            Rigid2D.velocity = velocity;
            LookAt();
        }
        else if (collider.tag.Equals("Ground"))
        {
            JumpCheck = true;
            Goopy goopy = GetOwner as Goopy;

            if (goopy.gPageIndex == 0)
                SoundManager.gInstance.PlaySound("SmallLend");
            if (goopy.gPageIndex == 1)
                SoundManager.gInstance.PlaySound("BigLend");
        }
    }



    public bool gJumpCheck => JumpCheck;


}
