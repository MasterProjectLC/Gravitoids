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
        MoveTowardsPlayer();

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
}
