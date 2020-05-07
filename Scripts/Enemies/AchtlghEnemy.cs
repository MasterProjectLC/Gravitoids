using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchtlghEnemy : KnightEnemy
{
    float cooldown = 2.5f;
    float powerRadius = 24f;
    float clock = 0f;

    float pissedClock = -0.5f;

    [SerializeField]
    Sprite regularSprite;

    [SerializeField]
    Sprite pissedSprite;

    SpriteRenderer spriteRenderer;
    [SerializeField]
    GameObject otherLight;

    string[] soundList = { "Achtlgh1", "Achtlgh2", "Achtlgh3" };

    new private void Start()
    {
        AudioManager.AM.Play("CrossDeath");

        spriteRenderer = GetComponent<SpriteRenderer>();
        base.Start();
    }

    // Update is called once per frame
    new private void Update()
    {
        if (!GetPlayerShip()) { return; }

        EmergeAnimation();
        AchtAnimation();

        SetGravityVariable(5);

        IncreaseBodyVelocity((GetPlayerShip().transform.position - transform.position) * Time.deltaTime * 0.01f);

        CooldownClock();
        MorraNegoNey();

        Gravitos();
    }

    private void AchtAnimation()
    {
        if (pissedClock > -0.5f)
        {
            pissedClock -= Time.deltaTime;

            if (pissedClock < 0f)
            {
                spriteRenderer.sprite = regularSprite;
                otherLight.SetActive(false);
            }
        }
    }

    private void CooldownClock()
    {
        if (clock > cooldown)
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                Vector3 ophzekPosition = objectList[i].transform.position;

                if (objectList[i].GetComponent<OphzekEnemy>() && (transform.position - ophzekPosition).magnitude < powerRadius)
                {
                    Vector3 toPlayer = (GetPlayerShip().transform.position - ophzekPosition).normalized;
                    objectList[i].GetComponent<OphzekEnemy>().SetBodyVelocity(toPlayer * 8f);
                    objectList[i].GetComponent<OphzekEnemy>().ActivateEye();
                }
            }
            pissedClock = 0.1f;
            spriteRenderer.sprite = pissedSprite;
            otherLight.SetActive(true);

            AudioManager.AM.Play(soundList[Random.Range(0, soundList.Length)]);

            clock = 0f;

        } else
        {
            clock += Time.deltaTime;
            //cooldownSprite.color = new Color(cooldownSprite.color.r, cooldownSprite.color.g, cooldownSprite.color.b, clock* (1/cooldown));
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
