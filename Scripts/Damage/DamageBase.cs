using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damage
{   

    public abstract class DamageBase : MonoBehaviour
    {
        protected abstract void SendDamage(AActor InFromDamage, AActor InToDamage, float Damage);
    }
}
