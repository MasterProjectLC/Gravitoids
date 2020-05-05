using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeederEnemy : EnemyShip
{


    // Update is called once per frame
    new private void Update()
    {
        SetGravityVariable(16);

        if (!GetPlayerShip()) { return; }

        IncreaseBodyVelocity((GetPlayerShip().transform.position - transform.position).normalized * Time.deltaTime * 4f);
        TurretRotation();

        Gravitos();
    }

    public void TurretRotation()
    {
        transform.LookAt(new Vector3(GetPlayerShip().transform.position.x, GetPlayerShip().transform.position.y, -100f), Vector3.forward);
    }

    public override void DealDamage(int damage)
    {
        Explode(1);
    }
}
