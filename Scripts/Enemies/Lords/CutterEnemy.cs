using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutterEnemy : PawnEnemy
{
    [SerializeField]
    GameObject laser;

    static LinkedList<CutterEnemy> brothers = new LinkedList<CutterEnemy>();
    public static bool boss = false;
    LinkedListNode<CutterEnemy> currentNode;

    static float clock = 0;
    float powerPeriod = 3f;
    float powerAnim = 1f;

    [SerializeField]
    GameObject repellerAnim;
    [SerializeField]
    float repellerRadius = 10f;

    Repeller repeller;

    new protected void Start()
    {
        repeller = new Repeller(repellerAnim, repellerRadius);
        brothers.AddLast(this);
        currentNode = brothers.Last;

        SetGravityVariable(10);
        SetBodyVelocity(-transform.position.normalized);
        IncreaseBodyVelocity(new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)));

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
        Vector3 position = transform.position;
        if (currentNode.Next == null || position.x > 53 || position.x < -53 || position.y > 30 || position.y < -30)
            return;

        Vector3 nextPosition = currentNode.Next.Value.transform.position;
        LinkedListNode<CutterEnemy> checkedNode = currentNode;
        while (nextPosition.x > 53 || nextPosition.x < -53 || nextPosition.y > 30 || nextPosition.y < -30)
        {
            if (checkedNode.Next == null)
                return;

            nextPosition = checkedNode.Next.Value.transform.position;
            checkedNode = checkedNode.Next;
        }

        CutterLaser newInstance = Instantiate(laser, transform.position, Quaternion.identity).GetComponent<CutterLaser>();
        newInstance.SetSpace(GetSpace());
        newInstance.Setup(transform.position, nextPosition);


        if (boss)
        {
            Debug.Log("Repelled");
            repeller.Function(ref objectList, transform.position);
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
