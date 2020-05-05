using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : SpaceObject
{
    BossEnemy boss;
    bool activated = true;
    int fading = 2;
    float alpha = 1f;

    [SerializeField]
    SpriteRenderer redLight;

    private void Start()
    {
        scale = 10;
        knockbackImmune = true;
    }

    new private void Update()
    {
        if (!activated) { return; }

        SetColor(alpha);
        alpha += Time.deltaTime * fading;

        if (alpha <= 0 && fading == -2) { fading = 2; }
        else if (alpha >= 1f && fading == 2) { fading = -2; }

        base.Update();
    }

    public override void DealDamage(int damage)
    {
        if (!activated) { return; }

        boss.WeakPointStatus(-1);
        activated = false;
        SetColor(0f);
    }

    private void SetColor(float alpha)
    {
        redLight.color = new Color(redLight.color.r, redLight.color.g, redLight.color.b, alpha);
    }

    public void SetBoss(BossEnemy boss)
    {
        this.boss = boss;
    }

    public void SetActivated(bool activated)
    {
        this.activated = activated;
    }


}
