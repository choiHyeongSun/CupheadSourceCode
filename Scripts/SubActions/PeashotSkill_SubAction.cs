using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeashotSkill_SubAction : SubActionBase
{

    [SerializeField]
    private GameObject Bullet;
    [SerializeField]
    private FHitData HitData;

    [SerializeField]
    private GameObject SparkleEffectPrefab;

    private Animator Anim;
    private Rigidbody2D Rigid2D;
    private BoxCollider2D Box;

    private Vector2 Direction;
    private Vector2 BoxSize;
    private float GravityScale;

    private GameObject BulletObject;

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

    protected override void Awake()
    {
        base.Awake();
        Anim = GetOwner.GetComponent<Animator>();
        Rigid2D = GetOwner.GetComponent<Rigidbody2D>();
        Box = GetOwner.GetComponent<BoxCollider2D>();

        BoxSize = Box.size;
        GravityScale = Rigid2D.gravityScale;
    }

    public override bool CanSubAction()
    {
        APlayer player = GetOwner as APlayer;
        if (player != null)
        {
            return player.gSkillGauge >= 1.0f;
        }

        return false;

    }


    public override void SubAction(float Horizontal, float Vertical)
    {
        base.SubAction(Horizontal, Vertical);
        Direction = new Vector2(Horizontal, Vertical).normalized;

        if (Global.IsNearFloatZeroCheck(Direction.magnitude))
        {
            Direction = GetOwner.transform.right;
        }

        VelocityLookAt(Horizontal);
        Anim.SetTrigger("SkillShot");
        Anim.SetFloat("DirectionY", Direction.y);
        Anim.SetFloat("DirectionX", Mathf.Abs(Direction.x));


        Rigid2D.velocity = Vector2.zero;

        Rigid2D.gravityScale = 0.0f;
        Vector2 halfSizeDiff = (BoxSize - Box.size) * 0.5f;
        Box.size = BoxSize;
        GetOwner.transform.position -= new Vector3(halfSizeDiff.x, halfSizeDiff.y, 0.0f);
        SoundManager.gInstance.PlaySound("PeashootImpact01", GetOwner.transform);

        APlayer player = GetOwner as APlayer;
        if (player != null)
        {
            player.DecreaseSkillGuage(1.0f);
        }
    }



    public override void AnimEvent()
    {
        base.AnimEvent();

        if (BulletObject != null) return;
        Rigid2D.velocity = -Direction;
        Quaternion rot = Global.LookAtDirection(Direction);
        BulletObject = Instantiate(Bullet, GetShootTrans.position, rot);
        Bullet bullet = BulletObject.GetComponent<Bullet>();

        if (bullet == null)
        {
            Destroy(bullet);
            return;
        }

        bullet.SetDirection(Direction);
        bullet.SetOwner(GetOwner);
        bullet.SetHitData(HitData);
        bullet.SetCamera(GetOwner.gCamera);
        if (SparkleEffectPrefab != null)
        {
            Instantiate(SparkleEffectPrefab, GetOwner.transform.position, rot);
        }
    }


    public override void EndSubAction()
    {
        base.EndSubAction();
        Rigid2D.gravityScale = GravityScale;
        BulletObject = null;
    }
}
