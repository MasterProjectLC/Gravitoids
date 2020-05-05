using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : EnemyShip
{
    [SerializeField]
    float shieldCooldown = 20f;

    float cooldown = 8f;

    float clock = 0f;

    [SerializeField]
    GameObject[] cooldownSprites;

    GameObject[] shieldGenerators;

    [SerializeField]
    Shield shield;

    [SerializeField]
    WeakPoint[] weakPoints;
    int weakPointCount = 0;

    new private void Start()
    {
        shield.SetSpace(GetSpace());
        SetBodyVelocity(new Vector2(1f, 0f));

        for (int i = 0; i < weakPoints.Length; i++)
        {
            weakPoints[i].SetSpace(GetSpace());
            weakPoints[i].SetBoss(this);
        }
        weakPointCount = weakPoints.Length;

        knockbackImmune = true;
        base.Start();
    }

    // Update is called once per frame
    new private void Update()
    {
        if (!GetPlayerShip()) { return; }

        EnemyRotation(5f);
        CooldownClock();
        LoopAround();

        Gravitos();
    }

    private void CooldownClock()
    {
        if (clock > cooldown)
        {
            if (!shield.gameObject.activeInHierarchy)
            {
                AudioManager.AM.Play("BossShield");
                WeakPointStatus(weakPoints.Length);
                for (int i = 0; i < weakPoints.Length; i++)
                    weakPoints[i].SetActivated(true);
                clock = 0f;
            }
        }
        else
        {
            clock += Time.deltaTime;
            for (int i = 0; i < cooldownSprites.Length; i++)
            {
                if (i <= cooldownSprites.Length * clock / cooldown)
                    cooldownSprites[i].SetActive(true);
                else
                    cooldownSprites[i].SetActive(false);
            }
        }

    }

    private void LoopAround()
    {
        Vector3 position = transform.position;
        if (position.x > 75)
            transform.position = new Vector3(-75, position.y, position.z);
    }

    public void WeakPointStatus(int change)
    {
        weakPointCount += change;

        if (weakPointCount <= 0)
        {
            shield.gameObject.SetActive(false);
            cooldown = shieldCooldown;
            clock = 0f;
        } else
        {
            shield.gameObject.SetActive(true);
        }
    }


    public override void DealDamage(int damage)
    {
        Debug.Log("Health " + health + ", Damage " + damage);

        health -= damage;
        Color myColor = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(myColor.r, myColor.g, myColor.b, myColor.a - (damage * (1f / maxHealth)));
        ParticleExplosion(1);

        if (health <= 0)
        {
            GetSpace().EndExplosion();
            Explode(10);
        }
    }

    new private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Shield" || (collision.GetComponent<Laser>() && !shield.gameObject.activeInHierarchy))
        {
            CollisionCheck(collision.gameObject, GetBodyVelocity());
        }
    }


}

