using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : SpaceObject
{
    float clock = 0f;
    float framePeriod = 0.05f;
    int currentFrame = 0;

    [SerializeField]
    Sprite[] animationFrames;

    Vector2 push;

    // Start is called before the first frame update
    void Start()
    {
        knockbackImmune = true;
    }

    // Update is called once per frame
    new void Update()
    {
        if (clock > framePeriod)
        {
            UpdateFrame();
            clock = 0;
        } else
        {
            clock += Time.deltaTime;
        }

        base.Update();
    }

    new protected void Gravitos()
    {

    }

    public void Setup(int direction)
    {
        float angle = 0f;

        switch (direction)
        {
            default:
                angle = 0f;
                push = new Vector2(0f, 1f);
                break;

            case 1:
                angle = 90f;
                push = new Vector2(-1f, 0f);
                break;

            case 2:
                angle = 180f;
                push = new Vector2(0f, -1f);
                break;

            case 3:
                angle = 270f;
                push = new Vector2(1f, 0f);
                break;
        }

        transform.eulerAngles = new Vector3(0f, 0f, angle);
    }

    private void UpdateFrame()
    {
        currentFrame++;

        if (currentFrame < animationFrames.Length)
        {
            GetComponent<SpriteRenderer>().sprite = animationFrames[currentFrame];
            if (currentFrame == 7)
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
            else if (currentFrame == 10)
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
            Destroy(gameObject);
    }

    public override void DealDamage(int damage)
    {
    }
}
