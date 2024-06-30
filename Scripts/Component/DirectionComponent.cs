using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using CustomComponent;
using Unity.Mathematics;
using UnityEngine;

public class DirectionComponent : ComponentBase
{
    private Rigidbody2D Rigid2D;
    private BoxCollider2D Box;
    private Animator Anim;
    

    private Vector2 BoxSize;

    private bool IsDuck;
    private bool IsAim;
    protected override void Awake()
    {
        base.Awake();
        Rigid2D = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        Box = GetComponent<BoxCollider2D>();

        BoxSize = Box.size;
        SetEnable(false);
    }

    public void VelocityLookAt(float InHorizontal)
    {
        if (InHorizontal > 0.0f)
        {
            GetOwner.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        else if (InHorizontal < 0.0f)
        {
            GetOwner.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
    }

    public void LookAt(float InHorizontal, float InVertical)
    {
        Vector2 dir = new Vector2(math.abs(InHorizontal), InVertical).normalized;
        VelocityLookAt(InHorizontal);
        Anim.SetFloat("DirectionX", dir.x);
        Anim.SetFloat("DirectionY", dir.y);
    }

    public void Aim()
    {
        SetEnable(true);
        Rigid2D.velocity = Vector2.zero;
        Anim.SetBool("Aim", true);
        IsAim = true;

        if (IsDuck == true)
        {
            Anim.SetBool("Duck", false);
            Box.size = BoxSize;
        }

    }

    public void StopAim()
    {
        Anim.SetBool("Aim", false);
        IsAim = false;

        if (IsDuck == false)
        {
            SetEnable(false);
        }
        else
        {
            Anim.SetBool("Duck", true);
            Box.size = new Vector2(BoxSize.x , BoxSize.y * 0.5f);
        }
    }

    public void Duck()
    {
        if (IsAim == false)
        {
            SetEnable(true);
            Anim.SetBool("Duck", true);
            Rigid2D.velocity = Vector2.zero;            
            Box.size = new Vector2(BoxSize.x , BoxSize.y * 0.5f);
        }

        IsDuck = true;
    }
    public void StopDuck()
    {
        
        Anim.SetBool("Duck", false);
        IsDuck = false;

        if (IsAim == false)
        {
            SetEnable(false);
            Box.size = BoxSize;
        }
    }

    public void Stop()
    {
        Anim.SetBool("Duck", false);
        Anim.SetBool("Aim", false);

        IsDuck = false;
        IsAim = false;

        SetEnable(false);
    }

    public bool GetDuck() => IsDuck;
    public bool GetAim() => IsAim;
}
