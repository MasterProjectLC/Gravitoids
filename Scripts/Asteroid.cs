using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : SpaceObject
{
    private float clickAcceleration = 0;
    private float additionalMass = 0;
    private float baseMass = 1;

    [SerializeField]
    private SpriteRenderer white;
    [SerializeField]
    private SpriteRenderer texture;

    // Start is called before the first frame update
    void Start()
    {
    }

    new private void Update()
    {
        AsteroidRotation();

        if (GetComponent<SpriteRenderer>().color.g < 1f)
        {
            Color thisColor = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(thisColor.r, thisColor.g + Time.deltaTime, thisColor.b + Time.deltaTime);
        }

        base.Update();
    }

    public void AsteroidRotation()
    {
        float newRotation = 10f * GetBodyVelocity().magnitude * Mathf.Clamp(GetBodyVelocity().x, -1f, 1f);

        transform.Rotate(new Vector3(0f, 0f, 1f), newRotation * Time.deltaTime, UnityEngine.Space.Self);
    }

    public void SetScale(int newScale)
    {
        int newBaseMass = (int)Mathf.Pow(newScale, 2f);

        SetAdditionalMass(additionalMass * newBaseMass / baseMass);
        SetBaseMass(newBaseMass);
        SetMass(baseMass + additionalMass);
        scale = newScale;

        float scaler = newScale * 3;
        transform.localScale = new Vector3(scaler, scaler, 1);
    }

    // Changing Mass

    public void AddMass(float signal)
    {
        clickAcceleration += Time.deltaTime * signal;
        float newAdditionalMass = additionalMass + (clickAcceleration * baseMass);

        float maxMultiplier = 20f;

        // Stop at Max Change
        if (newAdditionalMass >= maxMultiplier * baseMass || newAdditionalMass < 0) { return; }

        // Change Speed with Momentum
        if (GetBodyVelocity().magnitude - (clickAcceleration * (1/ maxMultiplier)) > 0f)
            IncreaseBodyVelocity(-GetBodyVelocity().normalized * clickAcceleration * (1/ maxMultiplier));
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 0.3f, 0.3f, 1);
            SetBodyVelocity(Vector2.zero);
            return;
        }

        additionalMass = newAdditionalMass;
        SetMass(baseMass + additionalMass);

        UpdateColor(additionalMass/baseMass);
    }

    public void UpdateColor(float newColor)
    {
        if (newColor >= 0)
        {
            white.color = new Color(1, 1, 1, newColor * 0.1f);
            texture.color = new Color(1, 1, 1, (newColor * 0.1f));
        }
    }

    public void ResetClickAcceleration()
    {
        clickAcceleration = 0;
    }

    // Receiving Damage

    override public void DealDamage(int damage)
    {
        float priorScale = scale;

        if (damage < 0) { damage = 0; }

        SetScale((int)GetScale() - damage);

        if (scale <= 0)
        {
            Explode(priorScale);
        }
    }

    // SETTER/GETTER ------------------------------------

    private void SetAdditionalMass(float newMass)
    {
        additionalMass = newMass;
    }

    protected void SetBaseMass(float newMass)
    {
        baseMass = newMass;
    }

    protected float GetBaseMass()
    {
        return baseMass;
    }


    // EVENTS --------------------------------------------

    void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            AddMass(1);
        }
        if (Input.GetMouseButton(1))
            AddMass(-1);
        if (Input.GetMouseButton(2))
            Debug.Log("Middle click on this object");

        if (Input.GetMouseButtonUp(0))
            ResetClickAcceleration();
        if (Input.GetMouseButtonUp(1))
            ResetClickAcceleration();
    }

    new private void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionCheck(collision.collider.gameObject, GetBodyVelocity());
    }

    new private void OnTriggerEnter2D(Collider2D collision)
    {
        CollisionCheck(collision.gameObject, GetBodyVelocity());
    }
}
