using System;
using CustomComponent;
using UnityEngine;

public class StatusComponent : ComponentBase
{
    
    [SerializeField]
    private float MaxHp;
    [SerializeField]
    private float CurrentHp;

    [SerializeField]
    private float MaxSkillGauge;
    private float CurrentSkillGauge;

    public Action DeadEvent;


    protected override void Awake()
    {
        CurrentHp = MaxHp;
        CurrentSkillGauge = MaxSkillGauge;

    }
    public void DecreaseHp(float InDamage)
    {
        if (IsDead == true) return;
        CurrentHp -= InDamage;

        if (CurrentHp <=0.0f)
        {
            CurrentHp = 0.0f;
            if (DeadEvent != null)
            {
                DeadEvent();
            }
            
        }
    }

    public void IncreaseHp(float InDamage)
    {
        CurrentHp += InDamage;
        if (CurrentHp >= MaxHp)
        {
            CurrentHp = MaxHp;
        }
    }

    public void DecreaseSkillGauge(float InSkillGauge)
    {
        
        CurrentSkillGauge -= InSkillGauge;
        
        
        if (CurrentSkillGauge <=0.0f)
        {
            CurrentSkillGauge = 0.0f;
        }
    }

    public void IncreaseSkillGauge(float InSkillGauge)
    {
        CurrentSkillGauge += InSkillGauge;
        if (CurrentSkillGauge >= MaxSkillGauge)
        {
            CurrentSkillGauge = MaxSkillGauge;
        }
    }

    public float GetCurrentHp => CurrentHp;
    public float GetMaxHp => MaxHp;

    public bool IsDead => Global.IsNearFloatZeroCheck(GetCurrentHp);

}
