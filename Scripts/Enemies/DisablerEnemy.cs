using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablerEnemy : KnightEnemy
{
    float cooldown = 2f;
    float clock = 0f;

    [SerializeField]
    SpriteRenderer cooldownSprite;

    [SerializeField]
    GameObject[] laserObjects;

    new private void Start()
    {
        AudioManager.AM.Play("CrossDeath");
        base.Start();
    }

    // Update is called once per frame
    new private void Update()
    {
        if (!GetPlayerShip()) { return; }

        EmergeAnimation();

        SetGravityVariable(5);

        IncreaseBodyVelocity((GetPlayerShip().transform.position - transform.position) * Time.deltaTime * 0.01f);

        EnemyRotation(20f);
        CooldownClock();
        MorraNegoNey();

        Gravitos();
    }

    private void CooldownClock()
    {
        if (clock > cooldown)
        {
            for (int i = 0; i < laserObjects.Length; i++)
            {
                AudioManager.AM.Play("Laser");
                laserObjects[i].SetActive(true);
                laserObjects[i].GetComponent<Laser>().SetAlpha(1f);
            }
            clock = 0f;
        } else
        {
            clock += Time.deltaTime;
            cooldownSprite.color = new Color(cooldownSprite.color.r, cooldownSprite.color.g, cooldownSprite.color.b, clock* (1/cooldown));
        }

    }

    private void MorraNegoNey()
    {
        // Destroy if reaches end of screen
        Vector3 position = transform.position;
        if (position.x > 53 || position.x < -53 || position.y > 30 || position.y < -30)
        {
            AudioManager.AM.Play("CrossDeath");
            DestroyObject();
        }
    }

    public override void DealDamage(int damage)
    {
        health -= damage;
        Color myColor = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(myColor.r, myColor.g, myColor.b, myColor.a - (damage * (1f / maxHealth)));
        ParticleExplosion(1);

        if (health <= 0)
        {
            Explode(5);
        }
    }

    new private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack") { return; }

        CollisionCheck(collision.gameObject, GetBodyVelocity());
    }

    new private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack") { return; }

        CollisionCheck(collision.collider.gameObject, GetBodyVelocity());
    }
}
