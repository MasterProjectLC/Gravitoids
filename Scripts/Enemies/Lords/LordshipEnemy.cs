using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LordshipEnemy : BossEnemy
{
    LoopAround loopAround;

    float clock = 0;
    const float powerPeriod = 4f;
    const float powerAnim = 1f;
    const float powerForce = 10f;

    float speedLimit = 15f;
    float acceleration = 1f;

    bool enraged = false;

    [SerializeField]
    GameObject spatialAnchor;

    [SerializeField]
    GameObject repellerAnim;

    Repeller repeller;

    [SerializeField]
    Transform core;


    new private void Start()
    {
        repeller = new Repeller(repellerAnim, 30f);
        AudioManager.AM.Play(bossThemeName);
        SetGravityVariable(3);

        // Movement Setup
        loopAround = new LoopAround(75f);
        transform.position = new Vector2(70, 1);
        CutterEnemy.boss = true;

        base.Start();
    }

    private void OnDestroy()
    {
        CutterEnemy.boss = false;
    }

    // Update is called once per frame
    new private void Update()
    {
        Movement();
        TurretRotation();
        transform.position = loopAround.Function(transform.position);

        core.Rotate(new Vector3(0f, 0f, 1f), 10f * Time.deltaTime, UnityEngine.Space.Self);

        Gravitos();
    }


    protected void Movement()
    {
        if (!GetPlayerShip())
            return;

        Vector2 directionToPlayer = (GetPlayerShip().transform.position - transform.position);
        float velocityDiff = (GetBodyVelocity().normalized - directionToPlayer.normalized).magnitude;

        if (GetBodyVelocity().magnitude > speedLimit && velocityDiff > 1f)
        {
            SetBodyVelocity(Vector2.zero);
            Instantiate(spatialAnchor, transform.position, transform.rotation);
        }

        IncreaseBodyVelocity(directionToPlayer * Time.deltaTime * Mathf.Max(0f, acceleration*velocityDiff / 4f));
    }


    public override void DealDamage(int damage)
    {
        Debug.Log("Health " + health + ", Damage " + damage);

        health -= damage;
        Color myColor = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(myColor.r, myColor.g, myColor.b, myColor.a - (damage * (1f / maxHealth)));
        ParticleExplosion(1);

        if (health < 40 && !enraged)
        {
            enraged = true;
            speedLimit = 5f;
            acceleration = 2f;
            AudioManager.AM.Play("LordshipActivated");
            EnragedAnimation();
        }

        if (health <= 0)
        {
            GetSpace().EndExplosion();
            Explode(10);
        }
    }

    IEnumerator EnragedAnimation()
    {
        yield return new WaitForSeconds(1);

        core.gameObject.SetActive(true);
        repeller.Function(ref objectList, transform.position);
    }

    public void TurretRotation()
    {
        if (GetPlayerShip())
            transform.LookAt(new Vector3(GetPlayerShip().transform.position.x, GetPlayerShip().transform.position.y, -1000f), Vector3.forward);
    }
}

