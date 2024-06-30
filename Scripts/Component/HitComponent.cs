using System.Collections;
using System.Collections.Generic;
using CustomComponent;
using UnityEngine;

public class HitComponent : ComponentBase
{
    [SerializeField]
    private float HitImmnuneTime = 1.0f;
    [SerializeField]
    private float Duration = 0.1f;
    [SerializeField]
    private SpriteRenderer Renderer;

    private Rigidbody2D Rigid2D;
    private Animator Anim;
    private bool IsHitImmune;
    

    private float GravityScale = 0.0f;
    


    protected override void Awake()
    {
        base.Awake();
        Anim = GetComponent<Animator>();
        Rigid2D = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();

        GravityScale = Rigid2D.gravityScale;
        SetEnable(false);

    }

    public void Hit()
    {
        Anim.SetTrigger("Hitted");
        SetEnable(true);
        if (Global.IsNearFloatZeroCheck(HitImmnuneTime) == false)
        {
            StartCoroutine(HitImmune());
        }

        Rigid2D.gravityScale = 0.0f;
        


    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Rigid2D.velocity = Vector3.zero;
    }

    private void EndHit()
    {
        SetEnable(false);
        Rigid2D.gravityScale = GravityScale;
    }

    private IEnumerator HitImmune()
    {
        Color color = Renderer.color;
        IsHitImmune = true;

        Coroutine coroutine = StartCoroutine(HitSpriteEffect());
        yield return new WaitForSeconds(HitImmnuneTime);
        StopCoroutine(coroutine);
        Renderer.color = color;
        IsHitImmune = false;
    }

    private IEnumerator HitSpriteEffect()
    {
        Color color = Renderer.color;
        while (true)
        {
            color.a = 0.0f;
            Renderer.color = color;
            yield return new WaitForSeconds(Duration * 0.5f);
            color.a = 1.0f;
            Renderer.color = color;
            yield return new WaitForSeconds(Duration * 0.5f);
        }
        
    }

    public bool GetHitImmune => IsHitImmune;
}
