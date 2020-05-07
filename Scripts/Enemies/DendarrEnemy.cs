using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DendarrEnemy : BossEnemy
{
    bool enraged = false;
    float distanceToReappear = 75f;

    float clock = 0f;
    float cooldown = 4f;
    float speed = 16f;

    float rotation = 90f;
    float deltaRotation = 0f;

    protected bool tail = false;
    int tailSize = 2;
    float tailClock = 0f;
    float tailRefreshRate = 35;
    float tailTimeDiff = 0.3f;
    protected Queue<Vector3> posQueue = new Queue<Vector3>();
    protected Queue<Vector3> dirQueue = new Queue<Vector3>();

    protected GameObject child;

    [SerializeField]
    protected GameObject nextPrefab;

    new protected void Start()
    {

        // Movement Setup
        SetBodyVelocity(new Vector2(0f, -1f) * speed);
        rotation = 270f;

        transform.position = new Vector2(-1, 70);

        // Start tail
        if (!tail)
        {
            SpawnTail(tailSize);
            AudioManager.AM.Play(bossThemeName);
        }

        knockbackImmune = true;
        base.Start();
    }

    protected void SpawnTail(int tailPos)
    {
        if (tailPos <= 0)
            return;

        for (int i = 0; i < tailRefreshRate; i++)
        {
            posQueue.Enqueue(new Vector3(0f, 320f));
            dirQueue.Enqueue(new Vector3(0f, 0f, 270f));
        }

        GameObject newObject = Instantiate(nextPrefab);
        newObject.GetComponent<DendarrEnemy>().SetSpace(GetSpace());
        GetSpace().AddToList(newObject);
        newObject.GetComponent<DendarrTail>().SetParent(this);
        child = newObject;

        if (tailPos != 1)
            newObject.GetComponent<DendarrTail>().SpawnTail(tailPos - 1);
    }

    // Update is called once per frame
    new private void Update()
    {
        if (!GetPlayerShip()) { return; }

        HeadMovement();

        if (tailClock > tailTimeDiff / tailRefreshRate)
        {
            TailMovement();
            tailClock = 0;
        }
        else
            tailClock += Time.deltaTime;

        LoopAround();

        Gravitos();
    }

    void HeadMovement()
    {
        // Change movement
        if (clock > cooldown)
        {
            deltaRotation = Random.Range(-16f, 16f);
            clock = 0f;
        }
        else
            clock += Time.deltaTime;

        // Actual movement
        rotation += deltaRotation * Time.deltaTime;
        Vector2 newDirection = new Vector2(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad));
        SetBodyVelocity(newDirection * speed);

        EnemyRotation(deltaRotation);
    }

    protected void TailMovement()
    {
        if (!child)
            return;

        posQueue.Enqueue(transform.position);
        child.transform.position = posQueue.Dequeue();

        dirQueue.Enqueue(transform.eulerAngles);
        child.transform.eulerAngles = dirQueue.Dequeue();

        child.GetComponent<DendarrTail>().TailMovement();
    }

    protected void LoopAround()
    {
        Vector3 position = transform.position;
        if (position.x > distanceToReappear)
            transform.position = new Vector3(-(distanceToReappear -5f), position.y, position.z);

        else if (position.x < -distanceToReappear)
            transform.position = new Vector3(distanceToReappear - 5f, position.y, position.z);

        if (position.y > distanceToReappear && 300f > position.y)
            transform.position = new Vector3(position.x, -(distanceToReappear - 5f), position.z);

        else if (position.y < -distanceToReappear)
            transform.position = new Vector3(position.x, distanceToReappear - 5f, position.z);
    }


    public override void DealDamage(int damage)
    {
        Debug.Log("Health " + health + ", Damage " + damage);

        health -= damage;

        if (health < 40)
        {
            ChangeColor(new Color(0.99f, 0.6f, 0.15f), false);

            if (!enraged)
            {
                enraged = true;
                speed = 28;
                distanceToReappear = 58f;
                AudioManager.AM.Play("DendarrRoar");
            }
        }
        else
        {
            Color myColor = GetComponent<SpriteRenderer>().color;
            ChangeColor(new Color(myColor.r, myColor.g, myColor.b, myColor.a - (damage * (1f / maxHealth)) ), true);
        }

        if (health <= 0)
        {
            child.GetComponent<DendarrTail>().Explode(10);
            GetSpace().EndExplosion();
            Explode(10);
        }
    }

    public void ChangeColor(Color color, bool explode)
    {
        GetComponent<SpriteRenderer>().color = color;

        if (explode)
            ParticleExplosion(1);

        if (child)
            child.GetComponent<DendarrEnemy>().ChangeColor(color, explode);
    }

    new protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.GetComponent<DendarrEnemy>())
            CollisionCheck(collision.gameObject, GetBodyVelocity());
    }

}

