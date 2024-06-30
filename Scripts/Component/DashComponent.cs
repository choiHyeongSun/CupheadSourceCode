using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CustomComponent
{
    public class DashComponent : ComponentBase
    {
        [SerializeField]
        private GameObject DashEffect;
        [SerializeField]
        private float Speed = 10.0f;
        [SerializeField]
        private bool DrawDebug;

        private BoxCollider2D Box2D;
        private Rigidbody2D Rigid2D;
        private Animator Anim;
        
        private Vector2 Velocity;
        private Vector2 Direction;
        
        private float GravityScale;


        protected override void Awake()
        {
            base.Awake();
            Rigid2D = GetComponent<Rigidbody2D>();
            Anim = GetComponent<Animator>();
            Box2D = GetComponent<BoxCollider2D>();

            GravityScale = Rigid2D.gravityScale;
            SetEnable(false);
        }

        public void Dash(float InHorizontal)
        {
            if (Global.IsNearFloatZeroCheck(InHorizontal) == true)
            {
                return;
            }
            Direction = new Vector2(InHorizontal , 0.0f);

            Velocity = Direction.normalized * Speed;
            Rigid2D.gravityScale = 0.0f;

            Anim.SetTrigger("Dash");
            SetEnable(true);

            if (DashEffect != null)
            {
                Vector3 pos = GetOwner.transform.position ;
                Instantiate(DashEffect, pos, Quaternion.Euler(0.0f, 40.0f, 0.0f));
            }

            SoundManager.gInstance.PlaySound("PlayerDash", GetOwner.transform);
        }

        protected override void FixedUpdate()
        {
            Vector2 pos = GetOwner.transform.position + new Vector3(Direction.x, Direction.y, 0.0f) * 0.5f;
            Collider2D colliders = Physics2D.OverlapBox(pos, Box2D.size, transform.rotation.z , ~(1 << 7 | 1 << 6 | 1 << 3));
            Rigid2D.velocity = Velocity;

            if (colliders != null)
            {
                Rigid2D.velocity = Vector2.zero;
            }
            
        }       

        public void EndDash()
        {
            Rigid2D.gravityScale = GravityScale;
            Rigid2D.velocity = Vector2.zero;
            Velocity = Vector2.zero;
            SetEnable(false);
        }
    }
}