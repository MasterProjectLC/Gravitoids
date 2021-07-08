using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightEnemy : EnemyShip
{
    [SerializeField]
    float targetScale = 3f;

    new protected void Start()
    {
        SetGravityVariable(5);
        base.Start();
    }

    protected void MoveTowardsPlayer()
    {
        if (GetPlayerShip())
            IncreaseBodyVelocity((GetPlayerShip().transform.position - transform.position) * Time.deltaTime * 0.01f);
    }

    protected void EmergeAnimation()
    {
        if (transform.localScale.x == targetScale)
            return;

        if (transform.localScale.x < targetScale)
            transform.localScale += new Vector3(5 * Time.deltaTime, 5 * Time.deltaTime, 5 * Time.deltaTime);
        else
            transform.localScale = new Vector3(targetScale, targetScale, targetScale);
    }

    protected void MorraNegoNey()
    {
        // Destroy if reaches end of screen
        Vector3 position = transform.position;
        if (position.x > 53 || position.x < -53 || position.y > 30 || position.y < -30)
        {
            AudioManager.AM.Play("CrossDeath");
            DestroyObject();
        }
    }
}
