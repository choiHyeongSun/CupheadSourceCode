using System;
using System.Collections;
using System.Collections.Generic;
using CustomComponent;
using UnityEngine;
using UnityEngine.InputSystem;

public class APlayer : AActor, IHitEvent
{
    public struct SkillStatus
    {
        private float SpecialGauge;

        public float gSpecialGauge
        {
            get => SpecialGauge;
            set => SpecialGauge = value;
        }
    }


    private MovementComponent Movement;
    private ParryComponent Parry;
    private DashComponent Dash;
    private JumpComponent Jump;
    private DirectionComponent Direction;
    private ShootComponent Shoot;
    private HitComponent Hit;
    private SubActionComponent SubAction;
    private StatusComponent Status;


    private bool PauseShoot;
    private bool PauseAim;
    private bool PauseDuck;
    private bool CanDash = true;
    private bool IsDescend = false;
    private bool IsDescendCheck = false;

    private float Horizontal;
    private float Vertical;

    private Vector2 PrevBoxSize;
    private BoxCollider2D Box2D;

    private BoxCollider2D DescendCollision;
    private Animator Anim;

    private SkillStatus Skillstatus;


    private Dictionary<int, Vector2> OnCollisionNormals = new Dictionary<int, Vector2>();
    protected override void Awake()
    {
        base.Awake();

        Movement = GetComponent<MovementComponent>();
        Direction = GetComponent<DirectionComponent>();
        Jump = GetComponent<JumpComponent>();
        Dash = GetComponent<DashComponent>();
        Shoot = GetComponent<ShootComponent>();
        Parry = GetComponent<ParryComponent>();
        Hit = GetComponent<HitComponent>();
        SubAction = GetComponent<SubActionComponent>();
        Box2D = GetComponent<BoxCollider2D>();
        Status = GetComponent<StatusComponent>();
        Anim = GetComponent<Animator>();

        PrevBoxSize = Box2D.size;
        if (GetGameManager.gIsGameProduction == true)
        {
            Anim.SetTrigger("Intro");
            gIsIntro = true;
        }

        Status.DeadEvent = StatusDeadEvent;
    }

    protected override void Update()
    {
        if (gIsIntro == true || GetGameManager.gIsGameProduction == true || Status.IsDead == true) return;
        base.Update();
        Debug.Log(Horizontal + " " + Vertical);

        if (IsCanShoot() == true) Shoot.ShootDirection(Horizontal, Vertical);
        StopComponent();
        ReplayComponent();
    }

    protected override void FixedUpdate()
    {
        IsDescendCheck = false;
        if (IsDescend == true) StartCoroutine(DescendCoroutine());
    }

    protected override void LateUpdate()
    {
        if (Global.IsNearFloatZeroCheck(Box2D.size.y - PrevBoxSize.y) == false && Jump.enabled == false)
        {
            float diffY = (PrevBoxSize.y - Box2D.size.y) * 0.5f;

            transform.position -= Vector3.up * diffY;
            PrevBoxSize = Box2D.size;
            return;
        }

        FallingCheck();
    }


    public void OnMove(InputAction.CallbackContext callback)
    {
        Vector2 value = callback.ReadValue<Vector2>();
        Horizontal = value.x;
        Vertical = value.y;
    }

    public void OnShoot(InputAction.CallbackContext callback)
    {
        if (gIsIntro == true || GetGameManager.gIsGameProduction == true || Status.IsDead == true) return;
        if (callback.performed == true)
        {
            if (IsCanShoot() == true) Shoot.Shoot();
            else PauseShoot = true;
        }

        else if (callback.canceled == true)
        {
            Shoot.StopShoot();
            PauseShoot = false;
        }
    }

    public void OnSubAction(InputAction.CallbackContext callback)
    {
        if (gIsIntro == true || GetGameManager.gIsGameProduction == true || Status.IsDead == true) return;
        if (callback.performed == true)
        {
            if (Input.GetKeyDown(KeyCode.V) == true)
            {
                if (SubAction.GetEnable() == false && IsCanSubAction() == true)
                {
                    SubAction.SubAction(0, Horizontal, Vertical);
                }
            }

        }
    }

    public void OnDash(InputAction.CallbackContext callback)
    {
        if (gIsIntro == true || GetGameManager.gIsGameProduction == true || Status.IsDead == true) return;
        if (callback.performed == true)
        {
            if (IsCanDash() == true && CanDash == true)
            {
                CanDash = false;
                Dash.Dash(Horizontal);
            }
        }

    }

    public void OnDucking(InputAction.CallbackContext callback)
    {
        if (gIsIntro == true || GetGameManager.gIsGameProduction == true || Status.IsDead == true) return;
        if (callback.performed == true)
        {
            if (IsCanAim() == true) Direction.Duck();
            else PauseDuck = true;
        }
        else if (callback.canceled == true)
        {
            if (Direction.GetEnable() == true && Direction.GetDuck() == true) Direction.StopDuck();
            PauseDuck = false;
        }
    }

    public void OnAim(InputAction.CallbackContext callback)
    {
        if (gIsIntro == true || GetGameManager.gIsGameProduction == true || Status.IsDead == true) return;
        if (callback.performed == true)
        {
            if (IsCanAim() == true) Direction.Aim();
            else PauseAim = true;
        }
        else if (callback.canceled == true)
        {
            if (Direction.GetEnable() == true && Direction.GetAim() == true)
                Direction.StopAim();
            PauseAim = false;
        }
    }

    public void OnJump(InputAction.CallbackContext callback)
    {
        if (gIsIntro == true || GetGameManager.gIsGameProduction == true || Status.IsDead == true) return;
        if (GetEntryChangeMap == true) return;

        if (callback.performed == true)
        {
            if (IsCanJump() == true)
            {
                if (DescendCollision != null && Direction.GetDuck() == true) { IsDescend = true; return; }
                if (Jump.GetEnable() == true) Parry.EnableParry();
                else Jump.Jump();
            }
        }
        else if (callback.canceled == true)
        {
            Jump.StopJump();
        }
    }


    private void StopComponent()
    {
        if (Global.IsPreventInputKey == true || gIsIntro == true) return;

        if (IsCanMove() == true)
        {
            Movement.Movement(Horizontal);
        }
        else
        {
            Movement.Movement(0.0f);
        }

        if (Direction.GetEnable() == true)
        {
            if (IsCanAim() == false)
            {

                if (Direction.GetDuck() == true) PauseDuck = true;
                if (Direction.GetAim() == true) PauseAim = true;
                Direction.Stop();

            }
            else
            {
                Direction.LookAt(Horizontal, Vertical);
            }
        }

        if (Shoot.GetEnable() == true)
        {
            if (IsCanShoot() == false)
            {
                Shoot.StopShoot();
                PauseShoot = true;
            }
            Direction.LookAt(Horizontal, Vertical);
        }

        if (SubAction.GetEnable() == true)
        {
            if (IsCanSubAction() == false)
            {
                SubAction.EndSubAction();
            }
        }
    }

    private void ReplayComponent()
    {
        if (PauseDuck == true)
        {
            if (IsCanAim() == true)
            {
                Direction.Duck();
                PauseDuck = false;
            }

        }
        if (PauseAim == true)
        {
            if (IsCanAim() == true)
            {
                Direction.Aim();
                PauseAim = false;
            }
        }
        if (PauseShoot == true)
        {
            if (IsCanShoot() == true)
            {
                Shoot.Shoot();
                PauseShoot = false;
            }
        }

    }
    private void FallingCheck()
    {
        if (gIsIntro == true) return;
        if (IsCanJump() == true && Jump.GetEnable() == false)
        {
            foreach (var collision in OnCollisionNormals)
            {

                float contactAngle = Mathf.Atan2(collision.Value.y, collision.Value.x);
                //
                if (Global.IsNearFloatCheck(contactAngle * Mathf.Rad2Deg, 90.0f, 10.0f))
                {
                    CanDash = true;
                    return;
                }
            }
            Jump.Falling();
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 result = Vector3.zero;
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (collision.GetContact(i).normal.y > result.y)
                result = collision.GetContact(i).normal;
        }
        if (OnCollisionNormals.ContainsKey(collision.gameObject.GetHashCode()) == false)
            OnCollisionNormals.Add(collision.gameObject.GetHashCode(), result);

        if (collision.gameObject.CompareTag("Descend") == true && IsDescendCheck == false)
        {
            DescendCollision = collision.collider as BoxCollider2D;
        }
        else
        {
            DescendCollision = null;
            IsDescendCheck = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Vector3 result = Vector3.zero;
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (collision.GetContact(i).normal.y > result.y)
                result = collision.GetContact(i).normal;
        }
        if (OnCollisionNormals.ContainsKey(collision.gameObject.GetHashCode()) == false)
            OnCollisionNormals[collision.gameObject.GetHashCode()] = result;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (Direction.GetDuck() == true) return;

        if (OnCollisionNormals.ContainsKey(collision.gameObject.GetHashCode()) == true)
            OnCollisionNormals.Remove(collision.gameObject.GetHashCode());
    }



    public void HitEvent(FHitData HitData)
    {
        if (Hit.GetHitImmune == false && Status.IsDead == false)
        {
            if (Dash.GetEnable() == true)
            {
                Dash.EndDash();
            }
            Hit.Hit();
            Status.DecreaseHp(HitData.gDamage);
            SoundManager.gInstance.PlaySound("PlayerHit", transform);
        }
    }

    private void StatusDeadEvent()
    {
        Anim.SetTrigger("Deadth");
        SoundManager.gInstance.PlaySound("PlayerDeadth");
    }

    public IEnumerator DescendCoroutine()
    {

        Physics2D.IgnoreCollision(DescendCollision, Box2D, true);
        OnCollisionNormals.Clear();
        IsDescend = false;

        while (true)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(Box2D.bounds.center, Box2D.bounds.size, transform.rotation.eulerAngles.z);

            bool check = false;
            foreach (var colldier in colliders)
            {
                if (colldier == DescendCollision)
                {
                    check = true;
                    break;
                }
            }

            if (check == false)
            {
                Physics2D.IgnoreCollision(DescendCollision, Box2D, false);
                DescendCollision = null;
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void DeadthEvent()
    {
        gameObject.SetActive(false);
    }

    public float gSkillGauge => Skillstatus.gSpecialGauge;
    public void IncreaseSkillGauge(float Guage)
    {
        float maxGauge = 5.0f;
        Skillstatus.gSpecialGauge += Guage;
        if (Skillstatus.gSpecialGauge > maxGauge)
        {
            Skillstatus.gSpecialGauge = maxGauge;
        }
    }

    public void DecreaseSkillGuage(float Guage)
    {
        float minGauge = 0.0f;
        Skillstatus.gSpecialGauge -= Guage;
        if (Skillstatus.gSpecialGauge < minGauge)
        {
            Skillstatus.gSpecialGauge -= minGauge;
        }
    }



}
