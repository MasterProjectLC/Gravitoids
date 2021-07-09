using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopAround : MonoBehaviour
{
    float distanceToReappear = 75f;

    public LoopAround(float distanceToReappear)
    {
        this.distanceToReappear = distanceToReappear;
    }

    public Vector3 Function(Vector3 position)
    {
        Vector3 retorno = position;

        if (position.x > distanceToReappear)
            retorno = new Vector3(-(distanceToReappear - 5f), position.y, position.z);

        else if (position.x < -distanceToReappear)
            retorno = new Vector3(distanceToReappear - 5f, position.y, position.z);

        if (position.y > distanceToReappear && 300f > position.y)
            retorno = new Vector3(position.x, -(distanceToReappear - 5f), position.z);

        else if (position.y < -distanceToReappear)
            retorno = new Vector3(position.x, distanceToReappear - 5f, position.z);

        return retorno;
    }

    public void SetDistance(float distance)
    {
        distanceToReappear = distance;
    }
}
