using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ship : SpaceObject
{
    [SerializeField]
    protected int health = 3;
    protected int maxHealth = 3;

    // Start is called before the first frame update
    protected void Start()
    {
        maxHealth = health;
    }

    public void SetScale(int newScale)
    {
        scale = newScale;
    }
}
