using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutShip : PlayerShip
{
    [SerializeField]
    LayerMask mask;

    [SerializeField]
    protected GameObject laser;

    protected float shield = 0f;
    protected float? dash = null;

    new void Update()
    {
        if (dash != null)
        {
            if (dash > 0f)
                dash -= Time.deltaTime;
            else
            {
                dash = null;
                AdjustColor();
                GetComponent<Collider2D>().enabled = true;
                SetBodyVelocity(GetBodyVelocity().normalized * 5f);
            }
        }

        Updater();
    }

    public override void DealDamage(int damage)
    {
        if (shield > 0f) { return; }
        base.DealDamage(damage);
    }

    public override void SpacePower()
    {
        Laser();
    }

    public override void ShiftPower()
    {
        Dash();
    }

    public override void ControlPower()
    {
        Swapper();
    }


    void Laser()
    {
        if (energyLevel <= 0)
            return;

        AudioManager.AM.Play("Laser");
        laser.SetActive(true);
        laser.GetComponent<Laser>().SetAlpha(1);
        UseEnergyCell(-1);
    }

    void Dash()
    {
        if (energyLevel <= 0)
            return;

        AudioManager.AM.Play("Warp2");
        SetBodyVelocity(targetDirection.normalized*50f);
        dash = 0.25f;
        GetComponent<Collider2D>().enabled = false;
        Color myColor = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(myColor.r, myColor.g, myColor.b, myColor.a/4);
        UseEnergyCell(-1);
    }

    void Swapper()
    {
        if (energyLevel <= 0)
            return;
        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        if (!hit) { return; }

        Collider2D other = hit.collider;

        if (other.GetComponent<BossEnemy>()) { return; }

        AudioManager.AM.Play("Ping");
        Vector3 myPosition = transform.position;
        Vector3 otherPosition = other.transform.position;

        other.transform.position = new Vector3(200f, 200f, 200f);
        transform.position = otherPosition;
        other.transform.position = myPosition;

        UseEnergyCell(-1);
    }
}
