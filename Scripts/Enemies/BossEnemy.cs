using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : EnemyShip
{
    [SerializeField]
    protected string bossThemeName;

    public string GetBossTheme()
    {
        return bossThemeName;
    }
}

