using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeller : MonoBehaviour
{
    [SerializeField]
    protected GameObject repellerAnim;

    [SerializeField]
    private float repellerRadius = 10f;


    public Repeller(GameObject repellerAnim, float repellerRadius)
    {
        this.repellerAnim = repellerAnim;
        this.repellerRadius = repellerRadius;
    }

    public void Function(ref List<GameObject> objectList, Vector3 myPosition)
    {
        Instantiate(repellerAnim, myPosition, Quaternion.identity);
        for (int i = 0; i < objectList.Count; i++)
        {
            Vector3 targetPosition = objectList[i].transform.position;
            float distance = (myPosition - targetPosition).magnitude;

            if (distance < repellerRadius)
            {
                Vector2 direction = (targetPosition - myPosition).normalized;
                objectList[i].GetComponent<SpaceObject>().IncreaseBodyVelocity(direction * (repellerRadius - distance));
            }
        }
    }
}
