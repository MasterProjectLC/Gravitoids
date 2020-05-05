using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DendarrTail : DendarrEnemy
{
    DendarrEnemy parent;

    [SerializeField]
    SpriteRenderer greenLight = null;
    int fading = 2;
    float alpha = 1f;

    new protected void Start()
    {
        tail = true;
        base.Start();
        SetBodyVelocity(Vector2.zero);
    }

    new private void Update()
    {
        if (greenLight != null)
        {
            SetColor(alpha);
            alpha += Time.deltaTime * fading;

            if (alpha <= 0 && fading == -2) { fading = 2; }
            else if (alpha >= 1f && fading == 2) { fading = -2; }
        }

        LoopAround();
    }

    private void SetColor(float alpha)
    {
        greenLight.color = new Color(greenLight.color.r, greenLight.color.g, greenLight.color.b, alpha);
    }

    public override void DealDamage(int damage)
    {
        if (greenLight != null)
            damage *= 5;
        parent.DealDamage(damage);
    }

    public void SetParent(DendarrEnemy parent)
    {
        this.parent = parent;
    }
}

