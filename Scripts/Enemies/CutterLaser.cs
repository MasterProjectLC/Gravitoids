using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutterLaser : SpaceObject
{
    float clock = 0f;
    float powerPeriod = 3f;
    float powerAnim = 0.75f;

    LineRenderer line;
    EdgeCollider2D edgeCollider;
    Color purple;

    public void Setup(Vector2 myPosition, Vector2 nextPosition)
    {
        knockbackImpaired = true;
        knockbackImmune = true;

        purple = new Color();
        ColorUtility.TryParseHtmlString("3C0076", out purple);
        line = GetComponent<LineRenderer>();
        line.startColor = purple;
        line.endColor = purple;
        line.SetPosition(0, myPosition);
        line.SetPosition(1, nextPosition);

        edgeCollider = GetComponent<EdgeCollider2D>();
        Vector2[] edges = new Vector2[2];
        edges[0] = Vector2.zero;
        edges[1] = nextPosition - myPosition;
        edgeCollider.points = edges;
    }


    // Update is called once per frame
    new void Update()
    {
        if (clock < powerAnim)
        {
            line.startColor = new Color(purple.r, purple.g, purple.b, powerAnim - clock);
            line.endColor = new Color(purple.r, purple.g, purple.b, powerAnim - clock);
        }
        else
            DestroyObject();
            
        clock += Time.deltaTime;
    }

    public override void DealDamage(int damage)
    {
        return;
    }


    new private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.GetComponent<CutterEnemy>()) { return; }
        CollisionCheck(collision.collider.gameObject, GetBodyVelocity());
    }

    new private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CutterEnemy>()) { return; }
        CollisionCheck(collision.gameObject, GetBodyVelocity());
    }
}