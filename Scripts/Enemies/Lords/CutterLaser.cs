using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutterLaser : SpaceObject
{
    float clock = 0f;
    float powerAnim = 0.5f;

    LineRenderer line;
    EdgeCollider2D edgeCollider;

    public void Setup(Vector2 myPosition, Vector2 nextPosition)
    {
        knockbackImpaired = true;
        knockbackImmune = true;

        line = GetComponent<LineRenderer>();
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
            line.startColor = new Color(0.7332588f, 0.4801976f, 0.9339623f, powerAnim - clock);
            line.endColor = new Color(0.7332588f, 0.4801976f, 0.9339623f, powerAnim - clock);
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