using UnityEngine;
using System.Collections.Generic;

public abstract class SpaceObject : MonoBehaviour
{
    private Space space;
    protected List<GameObject> objectList = new List<GameObject>();

    [SerializeField]
    private GameObject explosion;

    [SerializeField]
    protected float mass = 1;

    protected float scale = 1;

    protected float gravityVariable = 1;

    private Vector2 movement = new Vector2(0, 0);

    private Vector2 bodyVelocity = new Vector2(0, 0);

    protected bool knockbackImmune = false;

    private string[] collisionSounds = { "Collision1", "Collision2" };

    // Update is called once per frame
    protected void Update()
    {
        Gravitos();
    }

    // GRAVITY FAILS
    protected void Gravitos()
    {
        float gravitationalConstant = space.GetGravitationalConstant();

        for (int i = 0; i < objectList.Count; i++)
        {
            if (objectList[i] == gameObject)
                continue;

            // Calcular módulo
            float distance = (transform.position - objectList[i].transform.position).magnitude;
            if (distance > 60 || distance < 1) { continue; }

            float accModule = space.GetGravitationalConstant() * objectList[i].GetComponent<SpaceObject>().GetMass() / Mathf.Pow(distance, 2);

            // Calcular direcao
            Vector2 direction = -(transform.position - objectList[i].transform.position).normalized;

            IncreaseBodyVelocity(direction * accModule * gravityVariable);
        }

        transform.position += (Vector3)bodyVelocity * Time.deltaTime;
    }

    public bool CollisionCheck(GameObject otherObject, Vector2 newBodyVelocity) {

        // Setup
        SpaceObject otherSpaceObject = otherObject.GetComponent<SpaceObject>();
        Asteroid otherAsteroid = otherSpaceObject as Asteroid;

        float otherDamage;
        Vector2 otherVelocity = otherSpaceObject.GetBodyVelocity();

        otherDamage = otherSpaceObject.GetScale();

        if ((int)GetScale() >= otherDamage && otherDamage > 0)
        {
            // Sound
            float volume = (scale < 10) ? scale/10 : 1f;
            AudioManager.AM.Play(collisionSounds[Random.Range(0, collisionSounds.Length)], volume);

            // Push + Damage other object
            otherSpaceObject.IncreaseBodyVelocity((newBodyVelocity) * GetScale() / 5);
            otherSpaceObject.DealDamage((int)GetScale());

            // Push + Damage ourselves
            if (otherDamage > 0 && !knockbackImmune)
            {
                IncreaseBodyVelocity(otherVelocity * otherDamage / 5);
            }
            DealDamage((int)otherDamage);
            return true;
        }
        return false;
    }

    public void Explode(float explosionSize)
    {
        ParticleExplosion(explosionSize);
        DestroyObject();
    }

    public void ParticleExplosion(float explosionSize)
    {
        GameObject instance = Instantiate(explosion, transform.position, Quaternion.identity);
        instance.GetComponent<ParticleSystem>().startSize = explosionSize * 0.1f;
    }

    public void DestroyObject()
    {
        GetSpace().EraseObject(this.gameObject);
        Destroy(gameObject);
    }

    // SETTER/GETTER ----------------------------------

    public void IncreaseBodyVelocity(Vector2 increase)
    {
        bodyVelocity += increase;
    }

    public Vector2 GetBodyVelocity()
    {
        return bodyVelocity;
    }

    public void SetBodyVelocity(Vector2 newBodyVelocity)
    {
        bodyVelocity = newBodyVelocity;
    }

    public void SetSpace(Space newSpace)
    {
        space = newSpace;
    }

    protected Space GetSpace()
    {
        return space;
    }

    public void SetObjectList(List<GameObject> newList)
    {
        objectList = newList;
    }

    public float GetScale()
    {
        return scale;
    }

    public float GetMass()
    {
        return mass;
    }

    public void SetMass(float newMass)
    {
        mass = newMass;
    }

    protected void SetGravityVariable(float newGV)
    {
        gravityVariable = newGV;
    }

    public abstract void DealDamage(int damage);

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionCheck(collision.collider.gameObject, bodyVelocity);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Sanity " + collision.gameObject.tag);
        CollisionCheck(collision.gameObject, bodyVelocity);
    }
}
