using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CustomComponent
{
    public class JumpComponent : ComponentBase
    {
        [SerializeField]
        private int MaxJumpCount = 1;

        [SerializeField]
        private float JumpForce = 10.0f;
        [SerializeField]
        private float KeyPressTime = 0.1f;

        [SerializeField]
        private Vector2 JumpBoxSize;
        [SerializeField]
        private bool DrawDebug;

        [SerializeField]
        private GameObject JumpDustEffect;


        private Rigidbody2D Rigid2D;
        private Animator Anim;


        private int CurrentJumpCount;

        private Vector2 BoxSize;
        private BoxCollider2D Box2D;

        Coroutine JumpCoroutine;

        protected override void Awake()
        {
            base.Awake();
            Rigid2D = GetComponent<Rigidbody2D>();
            Anim = GetComponent<Animator>();
            Box2D = GetComponent<BoxCollider2D>();
            BoxSize = Box2D.size;
            SetEnable(false);
        }


        public void Jump()
        {
            if (CurrentJumpCount < MaxJumpCount)
            {
                SetEnable(true);
                DustEffect();

                CurrentJumpCount++;
                JumpCoroutine = StartCoroutine(Jumping(KeyPressTime));

                SoundManager.gInstance.PlaySound("PlayerJump", GetOwner.transform, 0.1f);


                Anim.SetTrigger("JumpEnter");
                Anim.SetBool("Jump", true);
                if (Global.IsNearFloatZeroCheck(JumpBoxSize.magnitude) == false)
                {
                    Box2D.size = JumpBoxSize;
                }


            }


        }

        public void Falling()
        {
            SetEnable(true);
            Anim.SetTrigger("JumpEnter");
            Anim.SetBool("Jump", true);


            if (Global.IsNearFloatZeroCheck(JumpBoxSize.magnitude) == false)
            {
                Box2D.size = JumpBoxSize;
            }
        }

        public void StopJump()
        {
            if (JumpCoroutine != null)
            {
                StopCoroutine(JumpCoroutine);
            }

        }

        protected IEnumerator Jumping(float duration)
        {
            float time = Time.time + duration;
            Vector2 velocity = Vector2.zero;
            while (time >= Time.time)
            {
                yield return new WaitForFixedUpdate();
                velocity = Vector2.up * JumpForce;
                Rigid2D.velocity = new Vector2(Rigid2D.velocity.x, velocity.y);
            }
            velocity = Vector2.up * JumpForce;
            Rigid2D.velocity = new Vector2(Rigid2D.velocity.x, velocity.y);

        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (GetEnable() == false) return;
            for (int i = 0; i < other.contactCount; i++)
            {
                float contactAngle = Mathf.Atan2(other.GetContact(i).normal.y, other.GetContact(i).normal.x);
                if (Global.IsNearFloatCheck(contactAngle * Mathf.Rad2Deg, 90.0f, 10.0f))
                {
                    SetGround();
                    DustEffect();
                    SoundManager.gInstance.PlaySound("LendGround", GetOwner.transform);
                    return;
                }
            }



        }

        public void SetGround()
        {
            SetEnable(false);
            Anim.SetBool("Jump", false);
            Box2D.size = BoxSize;
            CurrentJumpCount = 0;
            StopJump();
        }

        private void DustEffect()
        {
            if (JumpDustEffect != null)
            {
                Vector3 dustSpawnPos = transform.position + Vector3.down * BoxSize.y * 0.1f;
                Instantiate(JumpDustEffect, dustSpawnPos, Quaternion.identity);
            }
        }

        public int GetMaxJumpCount => MaxJumpCount;
        public void SetMaxJumpCount(int value) => MaxJumpCount = value;



    }
}
