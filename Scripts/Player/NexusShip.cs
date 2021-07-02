using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NexusShip : PlayerShip
{
    [SerializeField]
    private DirectedShield directedShield;
    private UnityAction shieldAction;

    [SerializeField]
    private float shieldCooldown = 5f;
    protected float shield = 0f;


    new void Start()
    {
        shieldAction += OnShieldDamage;
        directedShield.GetOnDamaged().AddListener(shieldAction);
        directedShield.SetSpace(GetSpace());
        base.Start();
    }


    new void Update()
    {
        Updater();

        if (shield > 0f)
        {
            shield -= Time.deltaTime;
            if (shield <= 0f)
                directedShield.gameObject.SetActive(true);
        }

        ShieldRotation();
    }

    public override void SpacePower()
    {
        
        Debug.Log("test");
    }

    public override void ShiftPower()
    {
        
    }

    public override void ControlPower()
    {
        Swapper();
    }


    void Swapper()
    {
        if (energyLevel <= 0)
            return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        if (!hit) { return; }

        Debug.Log("HIT");
        Collider2D other = hit.collider;

        if (other.GetComponent<BossEnemy>() || !other.GetComponent<SpaceObject>()) { return; }

        Vector3 myVelocity = GetBodyVelocity();
        Vector3 otherVelocity = other.GetComponent<SpaceObject>().GetBodyVelocity();

        SetBodyVelocity(otherVelocity);
        other.GetComponent<SpaceObject>().SetBodyVelocity(myVelocity);

        UseEnergyCell(-1);
    }


    public void ShieldRotation()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        directedShield.transform.parent.LookAt(new Vector3(mousePos.x, mousePos.y, -100f), Vector3.back);
    }

    private void OnShieldDamage()
    {
        directedShield.gameObject.SetActive(false);
        shield = shieldCooldown;
    }

    new private void OnCollisionEnter2D(Collision2D collision)
    {
        if (shield >= shieldCooldown - 0.1f)
            return;

        base.OnCollisionEnter2D(collision);
    }
}
