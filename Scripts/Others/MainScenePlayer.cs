using System.Collections;
using System.Collections.Generic;
using CustomComponent;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainScenePlayer : AActor
{
    private float Horizontal;
    private float Vertical;

    private MovementComponent Move;
    protected override void Awake()
    {
        base.Awake();
        Move = GetComponent<MovementComponent>();
        Move.Set8WayMovenet(true);
    }

    public void OnMove(InputAction.CallbackContext callback)
    {
        Vector2 value = callback.ReadValue<Vector2>();
        Horizontal = value.x;
        Vertical = value.y;
    }
    protected override void Update()
    {
        if (Global.IsPreventInputKey == true) return;
        Move.Movement(Horizontal, Vertical);
    }
}
