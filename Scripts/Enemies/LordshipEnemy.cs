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

    [SerializeField]
    GameObject spatialAnchor;

    [SerializeField]
    Transform core;


    new private void Start()
    {
        AudioManager.AM.Play(bossThemeName);

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

        if (GetBodyVelocity().magnitude > 5f && velocityDiff > 1f)
        {
            SetBodyVelocity(Vector2.zero);
            Instantiate(spatialAnchor, transform.position, transform.rotation);
        }

        IncreaseBodyVelocity(directionToPlayer * Time.deltaTime * Mathf.Max(0f, velocityDiff/4f));
    }

    public void TurretRotation()
    {
        if (GetPlayerShip())
            transform.LookAt(new Vector3(GetPlayerShip().transform.position.x, GetPlayerShip().transform.position.y, -1000f), Vector3.forward);
    }
}

