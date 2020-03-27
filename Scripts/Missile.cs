using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : SpaceObject
{
    private void Start()
    {
        scale = 0.25f;
    }

    public override void DealDamage(int damage)
    {
        Explode(1);
    }

    new private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.GetComponent<PlayerShip>() && tag == "PlayerAttack") { return; }

        CollisionCheck(collision.collider.gameObject, GetBodyVelocity());
    }

    new private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerShip>() && tag == "PlayerAttack") { return; }

        CollisionCheck(collision.gameObject, GetBodyVelocity());
    }
}
