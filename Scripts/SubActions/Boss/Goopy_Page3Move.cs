using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Goopy_Page3Move : SubActionBase
{

    private readonly float LEFTMOVE = -1.0f;
    private readonly float RIGHTMOVE = 1.0f;



    [SerializeField]
    private float Speed = 4.0f;

    private Rigidbody2D Rigid2D;
    private Animator Anim;

    private float Horizontal;
    private int SoundHandle;


    protected override void Awake()
    {
        base.Awake();
        Rigid2D = GetOwner.GetComponent<Rigidbody2D>();
        Anim = GetOwner.GetComponent<Animator>();
    }

    public override bool CanSubAction()
    {
        return true;
    }

    protected override void Update()
    {
        Anim.SetFloat("Horizontal" , Horizontal);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (Global.IsNearFloatCheck(Horizontal, RIGHTMOVE))
        {
            Rigid2D.velocity = new Vector2(Speed, 0.0f);
        }
        else if (Global.IsNearFloatCheck(Horizontal, LEFTMOVE))
        {
            Rigid2D.velocity = new Vector2(-Speed, 0.0f);
        }
    }

    public override void SubAction(float horizontal, float vertical)
    {
        base.SubAction(horizontal, vertical);


        Vector2 pos = GetOwner.transform.position;
        RaycastHit2D[] hitLeft = Physics2D.RaycastAll(pos, Vector2.left );
        RaycastHit2D[] hitRight = Physics2D.RaycastAll(pos, Vector2.right);

        float distLeft = 0;
        float distRight = 0;


        foreach (var hitresult in hitLeft)
        {
            if (hitresult.transform.CompareTag("Wall")) distLeft = Mathf.Max(hitresult.distance);
        }

        foreach (var hitresult in hitRight)
        {
            if (hitresult.transform.CompareTag("Wall")) distRight = Mathf.Max(hitresult.distance);
        }

        if (distLeft < distRight) Horizontal = RIGHTMOVE;
        else Horizontal = LEFTMOVE;


        Boss boss = GetOwner as Boss;
        if (boss != null)
        {
            boss.TriggerEnter2D += TriggerEnter2D;
        }


        SoundHandle = SoundManager.gInstance.PlaySound("Page3Move", GetOwner.transform, 1.0f, true);
    }

    public override void EndSubAction()
    {
        base.EndSubAction();
        Rigid2D.velocity = Vector2.zero;

        Boss boss = GetOwner as Boss;
        if (boss != null)
        {
            boss.TriggerEnter2D -= TriggerEnter2D;
        }
        SoundManager.gInstance.StopSound(SoundHandle);
    }

    private void TriggerEnter2D(Collider2D collider)
    {
        if (GetEnable() == false) return;
        if (collider.tag.Equals("Wall"))
        {
            Horizontal *= -1;
        }
    }
}
