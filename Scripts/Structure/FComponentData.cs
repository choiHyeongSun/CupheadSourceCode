using UnityEngine;
using System;


[Serializable]
public struct FComponentData
{
    [SerializeField]
    private bool CanMove;
    [SerializeField]
    private bool CanAim;
    [SerializeField]
    private bool CanShoot;
    [SerializeField]
    private bool CanJump;
    [SerializeField]
    private bool CanDash;
    [SerializeField]
    private bool CanSubAction;

    public bool gCanMove => CanMove;
    public bool gCanAim => CanAim;
    public bool gCanShoot => CanShoot;
    public bool gCanJump => CanJump;
    public bool gCanDash => CanDash;
    public bool gCanSubAction => CanSubAction;

}