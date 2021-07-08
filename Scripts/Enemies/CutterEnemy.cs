using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutterEnemy : PawnEnemy
{
    [SerializeField]
    GameObject laser;

    LineRenderer line;
    EdgeCollider2D edgeCollider;

    static LinkedList<CutterEnemy> brothers = new LinkedList<CutterEnemy>();
    LinkedListNode<CutterEnemy> currentNode;

    static float clock = 0;
    float powerPeriod = 3f;
    float powerAnim = 1f;
    Color purple;

    new protected void Start()
    {
        brothers.AddLast(this);
        currentNode = brothers.Last;

        SetGravityVariable(10);
        line = laser.GetComponent<LineRenderer>();
        edgeCollider = laser.GetComponent<EdgeCollider2D>();

        purple = new Color();
        ColorUtility.TryParseHtmlString("3C0076", out purple);
        line.startColor = purple;
        line.endColor = purple;
        base.Start();
    }

    private void OnDestroy()
    {
        brothers.Remove(currentNode);
    }

    // Update is called once per frame
    new private void Update()
    {
        if (!GetPlayerShip()) { return; }

        Clock();
        Gravitos();
    }

    public void Clock()
    {
        if (clock < powerAnim)
        {
            line.startColor = new Color(purple.r, purple.g, purple.b, powerAnim - clock);
            line.endColor = new Color(purple.r, purple.g, purple.b, powerAnim - clock);
        } else if (line.enabled)
        {
            line.enabled = false;
            //edgeCollider.enabled = false;
        }

        if (currentNode != brothers.First)
            return;

        if (clock > powerPeriod)
        {
            foreach (CutterEnemy cutter in brothers)
                cutter.Laser();
            clock = 0;
        }
        else
            clock += Time.deltaTime;
    }

    public void Laser()
    {
        if (currentNode.Next == null)
            return;
        Vector3 nextPosition = currentNode.Next.Value.transform.position;

        line.enabled = true;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, nextPosition);

        edgeCollider.enabled = true;
        Vector2[] edges = new Vector2[2];
        edges[0] = Vector2.zero;
        edges[1] = nextPosition - transform.position;
        edgeCollider.points = edges;
        Debug.Log(edgeCollider.points[1]);
        Debug.Log(edgeCollider.points[1]);
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
