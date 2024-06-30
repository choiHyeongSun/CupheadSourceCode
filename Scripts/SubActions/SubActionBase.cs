using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SubActionBase : MonoBehaviour
{
    [SerializeField]
    private Transform ShotTrans;

    private AActor Owner;
    

    protected virtual void Awake()
    { 
        Owner = gameObject.GetComponentInParent<AActor>();
        SetEnable(false);
        
    }

    public virtual void Initialization() {}
    

    protected virtual void Start() { }
    protected virtual void OnEnable() { }
    protected virtual void OnDisable() { }
    protected virtual void Update() { }
    protected virtual void FixedUpdate() { }
    protected virtual void LateUpdate() { }


    public abstract bool CanSubAction();

    public virtual void SubAction(float Horizontal, float Vertical)
    {
        SetEnable(true);
    }

    public virtual void EndSubAction()
    {
        SetEnable(false);
    }


    public virtual void AnimEvent(){}
    


    public AActor GetOwner => Owner;
    protected Transform GetShootTrans => ShotTrans;
    public void SetEnable(bool IsEnable) => enabled = IsEnable;
    public bool GetEnable() => enabled;

    

}
