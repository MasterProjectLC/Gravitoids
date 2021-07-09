using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OphzekEnemy : PawnEnemy
{
    float clock = 0;
    float animPeriod = 1.75f;

    int currentFrame = 0;
    [SerializeField]
    Sprite[] animationFrames;

    SpriteRenderer spriteRenderer;

    string[] soundList = { "OphzekReproduce1", "OphzekReproduce2" };


    new private void Start()
    {
        SetGravityVariable(10);
        spriteRenderer = GetComponent<SpriteRenderer>();
        base.Start();
    }

    // Update is called once per frame
    new private void Update()
    {
        if (!GetPlayerShip()) { return; }

        Reproduce();
        TurretRotation();

        Gravitos();
    }

    public void Reproduce()
    {
        if (clock > animPeriod)
        {
            currentFrame++;

            if (currentFrame >= animationFrames.Length)
                currentFrame = 0;

            spriteRenderer.sprite = animationFrames[currentFrame];

            if (currentFrame == animationFrames.Length-1)
            {
                Vector3 toPlayer = (GetPlayerShip().transform.position - transform.position).normalized * 5f;
                GameObject newObject = GetSpace().SpawnEnemy(transform.position + toPlayer, Space.enemyShipTypes.Pawn);
                newObject.GetComponent<OphzekEnemy>().SetBodyVelocity(toPlayer);

                if (transform.position.x > -60f && transform.position.x < 60f && transform.position.y > -60f && transform.position.y < 60f)
                AudioManager.AM.Play(soundList[Random.Range(0, soundList.Length)]);
            }

            clock = 0;
        }
        else
        {
            clock += Time.deltaTime;
        }
    }

    public void TurretRotation()
    {
        transform.LookAt(new Vector3(GetPlayerShip().transform.position.x, GetPlayerShip().transform.position.y, -1000f), Vector3.forward);
    }

    public override void DealDamage(int damage)
    {
        Explode(1);
    }

    public void ActivateEye()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
