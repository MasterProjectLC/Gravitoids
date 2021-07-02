using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DirectedShield : Shield
{
    UnityEvent Damaged = new UnityEvent();

    public override void DealDamage(int damage)
    {
        Damaged.Invoke();
        return;
    }

    public UnityEvent GetOnDamaged()
    {
        return Damaged;
    }
}
