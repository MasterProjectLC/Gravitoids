using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightEnemy : EnemyShip
{
    [SerializeField]
    float targetScale = 3f;

    protected void EmergeAnimation()
    {
        if (transform.localScale.x == targetScale)
            return;

        if (transform.localScale.x < targetScale)
            transform.localScale += new Vector3(5 * Time.deltaTime, 5 * Time.deltaTime, 5 * Time.deltaTime);
        else
            transform.localScale = new Vector3(targetScale, targetScale, targetScale);
    }
}
