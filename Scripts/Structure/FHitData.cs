using UnityEngine;
using System;

[Serializable]
public struct FHitData
{
    [SerializeField]
    private int Damage;
    private AActor From;
    private AActor To;

    
    public int gDamage
    {
        get => Damage;
        set => Damage = value;
    }

    public AActor gFrom
    {
        get => From;
        set => From = value;
    }
    public AActor gTo
    {
        get => To;
        set => To = value;
    }


}
