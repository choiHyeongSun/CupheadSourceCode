
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomComponent;



public class ShootComponent : ComponentBase
{

    [SerializeField]
    private GameObject BulletPrefab;
    [SerializeField]
    private float ShootLoopTime = 0.1f;
    [SerializeField]
    private Transform ShootPosition;
    [SerializeField]
    private GameObject SpawnBulletEffect;
    [SerializeField]
    private GameObject ShootingEffect;
    [SerializeField]
    private GameObject JumpShootEffect;
    [SerializeField]
    private FHitData HitData;

    [SerializeField]
    private float IncreaseSpecialGauge = 0.1f;

    private Animator Anim;
    private bool IsShoot;
    private Vector2 Direction;
    private GameObject ShootingEffectObject;
    private float CurrentShootLoopTime = 0;
    private int ShootSoundHandle;



    protected override void Awake()
    {
        base.Awake();
        Anim = GetComponent<Animator>();

        SetEnable(false);
    }

    protected override void Update()
    {
        base.Update();
        if (GetOwner.GetComponent<JumpComponent>() != null && ShootingEffectObject != null)
        {
            if (GetOwner.GetComponent<JumpComponent>().GetEnable() == true)
            {
                if (ShootingEffectObject.activeInHierarchy == true)
                {
                    ShootingEffectObject.SetActive(false);
                }
            }
            else
            {
                if (ShootingEffectObject.activeInHierarchy == false)
                    ShootingEffectObject.SetActive(true);
            }
        }
    }

    public void Shoot()
    {
        Anim.SetBool("Shoot", true);
        IsShoot = true;
        StartCoroutine(Shooting());
        SetEnable(true);

        if (ShootingEffect != null)
        {
            ShootingEffectObject = Instantiate(ShootingEffect);

            if (ShootPosition != null)
                ShootingEffectObject.transform.SetParent(ShootPosition);
            else
                ShootingEffectObject.transform.SetParent(GetOwner.transform);

            ShootingEffectObject.transform.localPosition = Vector3.zero;
            ShootSoundHandle = SoundManager.gInstance.PlaySound("PlayerShoot", GetOwner.transform, 1.0f, true);

        }

    }

    public void StopShoot()
    {
        if (GetEnable() == false) return; 
        Anim.SetBool("Shoot", false);
        Direction = Vector2.zero;
        IsShoot = false;
        SetEnable(false);

        SoundManager.gInstance.StopSound(ShootSoundHandle);
        if (ShootingEffectObject != null)
        {
            Destroy(ShootingEffectObject);
        }
    }

    public void ShootDirection(float Horizontal, float Vertical)
    {
        Direction = new Vector2(Horizontal, Vertical).normalized;
        if (Global.IsNearFloatZeroCheck(Direction.magnitude) == true || Anim.GetBool("Duck") == true)
        {
            Direction = GetOwner.transform.right;
        }

        Anim.SetFloat("DirectionY", Direction.y);
        Anim.SetFloat("DirectionX", Mathf.Abs(Direction.x));
    }

    private IEnumerator Shooting()
    {
        bool isSwitch = true;
        while (IsShoot)
        {

            if (CurrentShootLoopTime < Time.time)
            {
                Vector3 spawnPos = transform.position;

                if (ShootPosition != null)
                {
                    spawnPos = ShootPosition.position;

                    if (GetOwner.GetComponent<JumpComponent>() != null)
                    {
                        if (GetOwner.GetComponent<JumpComponent>().GetEnable() == true)
                        {
                            spawnPos = new Vector3(Direction.x, Direction.y, 0.0f) + transform.position;
                            if (JumpShootEffect != null)
                                Instantiate(JumpShootEffect, spawnPos, Quaternion.identity);
                        }
                    }
                }

                if (SpawnBulletEffect != null)
                {
                    Instantiate(SpawnBulletEffect, spawnPos, Quaternion.identity);
                }


                Vector3 dir3D = new Vector3(Direction.x, Direction.y, 0.0f);
                Vector3 offset = Vector3.Cross(dir3D, Vector3.forward).normalized  * 0.1f;

                if (isSwitch == true) spawnPos += offset;
                else spawnPos -= offset;


                GameObject obj = Instantiate(BulletPrefab, spawnPos, Global.LookAtDirection(Direction));
                Bullet bullet = obj.GetComponent<Bullet>();

                if (bullet == null)
                {
                    Destroy(obj);
                }

                bullet.SetHitData(HitData);
                bullet.SetOwner(GetOwner);
                bullet.SetDirection(Direction);
                bullet.SetCamera(GetOwner.gCamera);
                bullet.HitIncreaseAction = BulletHitEvent;
                CurrentShootLoopTime = Time.time + ShootLoopTime;

                isSwitch = !isSwitch;
            }

            yield return null;
        }
    }

    private void BulletHitEvent()
    {
       
        APlayer player = GetOwner as APlayer;
        if (player == null) return;
        player.IncreaseSkillGauge(IncreaseSpecialGauge);
        
        
    }

    public void ShootDirection(Vector2 Direction) => ShootDirection(Direction.x, Direction.y);
}
