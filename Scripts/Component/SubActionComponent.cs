using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomComponent;
using UnityEngine;

public class SubActionComponent : ComponentBase
{
    private List<SubActionBase> SubActions = new List<SubActionBase>();
    private SubActionBase UseSubAction;
    private int SubActionIndex = -1;


    public void Initialization()
    {
        for (int i = 0; i < SubActions.Count; i++)
        {
            SubActions[i].Initialization();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        SubActions = GetComponentsInChildren<SubActionBase>().ToList();
        SetEnable(false);
    }

    public void SubAction(int Index, float Horizontal = 0.0f , float Vertical = 0.0f)
    {
        if (SubActions.Count > Index)
        {
            if (SubActions[Index].CanSubAction() == true)
            {
                SubActions[Index].SubAction(Horizontal, Vertical);
                UseSubAction = SubActions[Index];
                SubActionIndex = Index;
                SetEnable(true);
            }
            
        }
    }
    private void AnimEvent()
    {
        if (UseSubAction != null)
        {
            UseSubAction.AnimEvent();
        }
    }

    public void EndSubAction()
    {
        if (GetEnable() == true)
        {
            SetEnable(false);
        }
        
        if (UseSubAction != null)
        {
            UseSubAction.EndSubAction();
            UseSubAction = null;
        }

        SubActionIndex = -1;
    }

    public SubActionBase GetSubAction() => UseSubAction;
    public int gSubActionIndex => SubActionIndex;


}
