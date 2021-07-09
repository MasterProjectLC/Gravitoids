using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosslaserEnemy : KnightEnemy
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
        MoveTowardsPlayer();

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
