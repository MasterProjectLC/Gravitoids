using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OphzekEnemy : EnemyShip
{
    float clock = 0;
    float animPeriod = 1.75f;

    int currentFrame = 0;
    [SerializeField]
    Sprite[] animationFrames;

    SpriteRenderer spriteRenderer;

    new private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        base.Start();
    }

    // Update is called once per frame
    new private void Update()
    {
        SetGravityVariable(10);

        if (!GetPlayerShip()) { return; }

        //IncreaseBodyVelocity((GetPlayerShip().transform.position - transform.position).normalized * Time.deltaTime * 4f);
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
        transform.LookAt(new Vector3(GetPlayerShip().transform.position.x, GetPlayerShip().transform.position.y, -100f), Vector3.forward);
    }

    public override void DealDamage(int damage)
    {
        Explode(1);
    }
}
