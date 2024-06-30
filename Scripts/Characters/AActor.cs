using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomComponent;
using Unity.IO.LowLevel.Unsafe;
using static UnityEngine.Rendering.DebugUI;

public class AActor : MonoBehaviour, ITeamGenerator
{
    [SerializeField]
    private int TeamID;

    [SerializeField]
    private bool IsIntro = true;

    [SerializeField]
    private Camera MainCamera;

    private bool EntryChangeMapCollider;

     

    private List<ComponentBase> Components = new List<ComponentBase>();
    private GameManager GameManager;
    public virtual void Initialization()
    {
    }

    public T GetActorComponent<T>() where T : ComponentBase
    {
        foreach (var component in Components)
        {
            if (component is T == true)
            {
                return component as T;
            }
        }
        return null;
    }

    protected virtual void Awake()
    {
        ComponentBase[] components = gameObject.GetComponents<ComponentBase>();
        foreach (ComponentBase component in components)
        {
            Components.Add(component);
        }

        if (MainCamera == null)
        {
            MainCamera = Camera.main;
        }

        GameManager = FindObjectOfType<GameManager>();
    }

    protected virtual void Start() {}
    protected virtual void OnEnable() {}
    protected virtual void OnDisable() {}
    protected virtual void Update() {}
    protected virtual void FixedUpdate() {}
    protected virtual void LateUpdate() { }


    public bool IsCanMove()
    {
        ComponentBase[] components = gameObject.GetComponents<ComponentBase>();

        foreach (ComponentBase component in components)
        {
            if (component.GetCanMove == false && component.enabled == true)
                return false;
        }
        return true;
    }

    public bool IsCanAim()
    {
        ComponentBase[] components = gameObject.GetComponents<ComponentBase>();

        foreach (ComponentBase component in components)
        {
            if (component.GetCanAim == false && component.enabled == true)
                return false;
        }
        return true;
    }

    public bool IsCanShoot()
    {
        ComponentBase[] components = gameObject.GetComponents<ComponentBase>();

        foreach (ComponentBase component in components)
        {
            if (component.GetCanShoot == false && component.enabled == true)
                return false;
        }
        
        return true;
    }

    public bool IsCanJump()
    {
        ComponentBase[] components = gameObject.GetComponents<ComponentBase>();

        foreach (ComponentBase component in components)
        {
            if (component.GetCanJump == false && component.enabled == true)
                return false;
        }

        return true;
    }

    public bool IsCanSubAction()
    {
        ComponentBase[] components = gameObject.GetComponents<ComponentBase>();

        foreach (ComponentBase component in components)
        {
            if (component.GetCanSubAction == false && component.enabled == true)
                return false;
        }

        return true;
    }
    public bool IsCanDash()
    {
        ComponentBase[] components = gameObject.GetComponents<ComponentBase>();

        foreach (ComponentBase component in components)
        {
            if (component.GetCanDash == false && component.enabled == true)
                return false;
        }

        return true;
    }

    private void StartIntro()
    {
        IsIntro = true;
    }

    private void EndIntro()
    {
        IsIntro = false;
        Initialization();
    }

    public int GetTeamID() => TeamID;
    public void SetTeamID(int Value) => TeamID = Value;

    public bool gIsIntro
    {
        get => IsIntro;
        set => IsIntro = value;
    } 


    protected GameManager GetGameManager => GameManager;
    public Camera gCamera => MainCamera;

    protected bool GetEntryChangeMap => EntryChangeMapCollider;
    public void SetEntryChangeMap(bool Value) => EntryChangeMapCollider = Value;

}
