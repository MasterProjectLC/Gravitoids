using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorShip : PlayerShip
{
    [SerializeField]
    protected GameObject turret;

    [SerializeField]
    protected Transform turretSpawn;

    [SerializeField]
    protected GameObject missile;

    [SerializeField]
    protected GameObject spatialAnchor;

    [SerializeField]
    protected GameObject shieldSprite;

    protected float shield = 0f;

    new void Update()
    {
        Updater();

        if (shield > 0f)
        {
            Color color = shieldSprite.GetComponent<SpriteRenderer>().color;
            color = new Color(color.r, color.g, color.b, shield);
            shieldSprite.GetComponent<SpriteRenderer>().color = color;
            shield -= Time.deltaTime * 2f;

        }
        else
        {
            shieldSprite.SetActive(false);
        }

        TurretRotation();
    }

    public override void DealDamage(int damage)
    {
        if (shield > 0f) { return; }
        base.DealDamage(damage);
    }

    public override void SpacePower()
    {
        Missile();
        Debug.Log("test");
    }

    public override void ShiftPower()
    {
        PlayerShield();
    }

    public override void ControlPower()
    {
        SpatialAnchor();
    }

    private void Missile()
    {
        // Send missile
        if (energyLevel > 0)
        {
            AudioManager.AM.Play("MissileShot");
            GameObject instance = Instantiate(missile, turretSpawn.position, turretSpawn.rotation);
            instance.GetComponent<SpaceObject>().SetSpace(GetSpace());
            instance.GetComponent<SpaceObject>().SetBodyVelocity((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * 100f);
            IncreaseBodyVelocity(-(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized);
            UseEnergyCell(-1);
        }
    }

    private void PlayerShield()
    {
        // SHIFT - Shield
        if (energyLevel > 0)
        {
            AudioManager.AM.Play("Shield");
            shield = 1f;
            shieldSprite.SetActive(true);
            UseEnergyCell(-1);
        }
    }

    private void SpatialAnchor()
    {
        // CTRL - Spatial Anchor
        if (energyLevel > 0)
        {
            AudioManager.AM.Play("SpatialAnchor");
            SetBodyVelocity(new Vector2(0f, 0f));
            Instantiate(spatialAnchor, transform.position, transform.rotation);
            UseEnergyCell(-1);
        }
    }

    public void TurretRotation()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        turret.transform.LookAt(new Vector3(mousePos.x, mousePos.y, -100f), Vector3.back);
    }
}
