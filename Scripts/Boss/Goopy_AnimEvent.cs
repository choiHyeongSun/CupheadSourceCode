using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gGoopy_AnimEvent : MonoBehaviour
{
    [SerializeField]
    private Vector2 ColliderOffset = new Vector2(-0.53f, 0.64f);
    [SerializeField]
    private float ColliderRadius = 1.7f;
    [SerializeField]
    private GameObject Page3Object;
    [SerializeField]
    private GameObject Page2Object;



    private SubActionComponent SubAction;
    private CircleCollider2D Circle2D;
    private BoxCollider2D Box2D;




    private void Awake()
    {
        SubAction = GetComponent<SubActionComponent>();
        Circle2D = GetComponent<CircleCollider2D>();
        Box2D = GetComponent<BoxCollider2D>();
    }

    private void JumpEvent()
    {
        if (SubAction.GetSubAction() != null)
        {
            Goopy_Jump jump = SubAction.GetSubAction() as Goopy_Jump;
            if (jump != null)
            {
                jump.JumpEvent();
            }
        }
    }
    private void Page2Collider()
    {
        Circle2D.radius = ColliderRadius;
        Circle2D.offset = ColliderOffset;

        APlayer player = FindObjectOfType<APlayer>();
        float dist = player.transform.position.x - transform.position.x;
        if (dist > 0.0f)
        {
            transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
    }

    private void Page3Intro()
    {
        Circle2D.enabled = false;
        Box2D.enabled = true;

        Page3Object.SetActive(true);
        Page2Object.SetActive(false);
    }

    private void PlaySound(string ClipName)
    {
        SoundManager.gInstance.PlaySound(ClipName, transform);
    }


}
