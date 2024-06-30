using UnityEngine;

namespace CustomComponent
{
    public class MovementComponent : ComponentBase
    {
        [SerializeField]
        private float Speed = 5.0f;

        private Vector2 Velocity;
        private Vector2 Direction;

        private Rigidbody2D Rigid2D;
        private Animator Anim;
        private BoxCollider2D Box2D;
        private bool Is8WayMovenet = false;

        protected override void Awake()
        {
            base.Awake();
            Rigid2D = GetComponent<Rigidbody2D>();
            Anim = GetComponent<Animator>();
            Box2D = GetComponent<BoxCollider2D>();
        }

        public void LookAt(float InHorizontal)
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


        public void Movement(float InHorizontal, float InVertical)
        {
            Vector2 dir = new Vector2(InHorizontal, InVertical);
            Direction = dir.normalized;
            Velocity = dir.normalized * Speed;
            
            if (Global.IsNearFloatZeroCheck(dir.magnitude) == false)
            {
                Anim.SetBool("Run", true);
                Anim.SetFloat("DirectionX", Mathf.Abs(Direction.x));
                Anim.SetFloat("DirectionY", Direction.y);
                LookAt(InHorizontal);
                SetEnable(true);
            }
            else
            {
                Rigid2D.velocity = Vector2.zero;
                SetEnable(false);
            }
        }

        public void Movement(float InHorizontal)
        {
            Vector2 dir = new Vector2(InHorizontal, 0.0f);
            Direction = dir.normalized;
            Velocity = dir.normalized * Speed;
            Velocity.y = Rigid2D.velocity.y;

            if (Global.IsNearFloatZeroCheck(InHorizontal) == false)
            {
                Anim.SetBool("Run", true);
                LookAt(InHorizontal);
                SetEnable(true);
            }
            else
            {
                SubActionComponent component = GetOwner.GetActorComponent<SubActionComponent>();
                if (component != null)
                {
                    SubActionBase subAction = component.GetSubAction();
                    if (subAction != null)
                    {
                        return;
                    }
                }
                Rigid2D.velocity = new Vector2(0.0f, Velocity.y);
                SetEnable(false);
            }

        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();


            Collider2D[] colliders;
            bool isContactWall = false;
            Vector2 pos = GetOwner.transform.position + new Vector3(Direction.x, Direction.y, 0.0f) * 0.5f;
            colliders = Physics2D.OverlapBoxAll(pos, Box2D.size, transform.rotation.z);

            Global.DrawDebugSquare(pos, Box2D.size, Color.black);

            foreach (var collider in colliders)
            {
                if (collider.tag.Equals("Wall"))
                {
                    isContactWall = true;
                }
            }


            if (Is8WayMovenet == false)
            {
                if (isContactWall == true)
                {
                    Velocity = new Vector2(0.0f, Rigid2D.velocity.y);
                }
                Velocity = new Vector2(Velocity.x, Rigid2D.velocity.y);
            }
            else
            {
                if (isContactWall == false)
                {
                    //Velocity = Vector2.zero;
                }
            }
            Rigid2D.velocity = Velocity;
            Velocity = Vector2.zero;    
        }

        protected override void LateUpdate()
        {
            Anim.SetBool("Run", false);
        }

        public void Set8WayMovenet(bool IsEnable) => Is8WayMovenet = IsEnable;
    }
}
