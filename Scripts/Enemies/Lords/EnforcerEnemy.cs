using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforcerEnemy : KnightEnemy
{
    float clock = 0;
    const float powerPeriod = 4f;
    const float powerAnim = 1f;
    const float powerForce = 10f;
    Color purple;

    [SerializeField]
    GameObject tractor;

    LineRenderer line;

    new private void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        purple = new Color();
        ColorUtility.TryParseHtmlString("3C0076", out purple);
        line.startColor = purple;
        line.endColor = Color.clear;
        base.Start();
    }

    // Update is called once per frame
    new private void Update()
    {
        EmergeAnimation();
        MoveTowardsPlayer();

        Enforce();
        TurretRotation();
        MorraNegoNey();

        Gravitos();
    }


    public void Enforce()
    {
        if (clock > powerPeriod)
        {
            if (!GetPlayerShip())
                return;
            Vector3 direction = (GetPlayerShip().transform.position - tractor.transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(tractor.transform.position, direction);

            if (!hit) { return; }
            Collider2D other = hit.collider;

            if (other.GetComponent<EnemyShip>()) { return; }
            line.SetPosition(0, tractor.transform.position);
            line.SetPosition(1, hit.point);
            line.startColor = purple;

            other.GetComponent<SpaceObject>().IncreaseBodyVelocity(-direction * powerForce);

            clock = 0;
        }
        else
        {
            if (clock < powerAnim)
                line.startColor = new Color(purple.r, purple.g, purple.b, powerAnim - clock);
            clock += Time.deltaTime;
        }
    }


    public void TurretRotation()
    {
        if (GetPlayerShip())
            transform.LookAt(new Vector3(GetPlayerShip().transform.position.x, GetPlayerShip().transform.position.y, -100f), Vector3.forward);
    }
}

