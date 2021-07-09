using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : SpaceObject
{
    float alpha = 1f;

    // Start is called before the first frame update
    void Start()
    {
        knockbackImpaired = true;
    }

    // Update is called once per frame
    new void Update()
    {
        if (!GetComponent<SpriteRenderer>())
            return;

        if (alpha <= 0f) { gameObject.SetActive(false); }

        Color color = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, alpha);

        alpha -= Time.deltaTime * 4f;
    }

    public override void DealDamage(int damage)
    {
        return;
    }

    public void SetAlpha(float neww)
    {
        alpha = neww;
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
