using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyShip : Ship
{
    private PlayerShip playerShip;

    // Update is called once per frame
    new private void Update()
    {
        base.Update();
    }

    public void EnemyRotation(float rotationSpeed)
    {
        transform.Rotate(new Vector3(0f, 0f, 1f), rotationSpeed * Time.deltaTime, UnityEngine.Space.Self);
    }

    public void SetPlayerShip(PlayerShip newShip)
    {
        playerShip = newShip;
    }

    public PlayerShip GetPlayerShip()
    {
        return playerShip;
    }

    public override void DealDamage(int damage)
    {
        health -= damage;
        Color myColor = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(myColor.r, myColor.g, myColor.b, myColor.a - (damage * (1f / maxHealth)));
        ParticleExplosion(1);

        if (health <= 0)
        {
            Explode(6);
        }
    }
}
