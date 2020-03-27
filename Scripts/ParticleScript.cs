using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    float clock = 0;

    // Update is called once per frame
    void Update()
    {
        clock += Time.deltaTime;

        if (clock > 5)
        {
            Destroy(gameObject);
        }
    }
}
