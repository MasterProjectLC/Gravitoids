using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : SpaceObject
{
    private void Start()
    {
        scale = 10;
        knockbackImmune = true;
    }

    public override void DealDamage(int damage)
    {
        return;
    }
}
