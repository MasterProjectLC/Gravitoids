using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NexusShip : PlayerShip
{
    [SerializeField]
    private DirectedShield directedShield;
    private UnityAction shieldAction;

    [SerializeField]
    protected GameObject repellerAnim;

    [SerializeField]
    protected GameObject vortexAnim;

    [SerializeField]
    private float shieldCooldown = 5f;
    protected float shield = 0f;

    [SerializeField]
    private float repellerRadius = 10f;

    [SerializeField]
    private float vortexRadius = 10f;


    new void Start()
    {
        shieldAction += OnShieldDamage;
        directedShield.GetOnDamaged().AddListener(shieldAction);
        directedShield.SetSpace(GetSpace());
        base.Start();
    }


    new void Update()
    {
        Updater();

        if (shield > 0f)
        {
            shield -= Time.deltaTime;
            if (shield <= 0f)
            {
                directedShield.gameObject.SetActive(true);
                AudioManager.AM.Play("Shield");
            }
        }

        ShieldRotation();
    }

    public override void SpacePower()
    {
        Repeller();
    }

    public override void ShiftPower()
    {
        Vortex();
    }

    public override void ControlPower()
    {
        Swapper();
    }



    void Vortex()
    {
        if (energyLevel <= 0)
            return;

        AudioManager.AM.Play("DeepWarp");
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mousePos3D = new Vector3(mousePos.x, mousePos.y);

        Instantiate(vortexAnim, mousePos3D, transform.rotation);
        for (int i = 0; i < objectList.Count; i++)
        {
            Vector3 targetPosition = objectList[i].transform.position;
            float distance = (mousePos3D - targetPosition).magnitude;

            if (distance < vortexRadius)
            {
                Vector2 direction = (mousePos3D - targetPosition).normalized;
                objectList[i].GetComponent<SpaceObject>().IncreaseBodyVelocity(direction * (vortexRadius - distance));
            }
        }

        UseEnergyCell(-1);
    } 


    void Repeller()
    {
        if (energyLevel <= 0)
            return;

        AudioManager.AM.Play("Repeller");
        Instantiate(repellerAnim, transform.position, transform.rotation);
        for (int i = 0; i < objectList.Count; i++)
        {
            Vector3 targetPosition = objectList[i].transform.position;
            float distance = (transform.position - targetPosition).magnitude;

            if (distance < repellerRadius)
            {
                Vector2 direction = (targetPosition - transform.position).normalized;
                objectList[i].GetComponent<SpaceObject>().IncreaseBodyVelocity(direction*(repellerRadius-distance));
            }
        }

        UseEnergyCell(-1);
    }


    void Swapper()
    {
        if (energyLevel <= 0)
            return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        if (!hit) { return; }

        Debug.Log("HIT");
        Collider2D other = hit.collider;

        if (other.GetComponent<BossEnemy>() || !other.GetComponent<SpaceObject>()) { return; }

        AudioManager.AM.Play("Ping");
        Vector3 myVelocity = GetBodyVelocity();
        Vector3 otherVelocity = other.GetComponent<SpaceObject>().GetBodyVelocity();

        SetBodyVelocity(otherVelocity);
        other.GetComponent<SpaceObject>().SetBodyVelocity(myVelocity);

        UseEnergyCell(-1);
    }


    public void ShieldRotation()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        directedShield.transform.parent.LookAt(new Vector3(mousePos.x, mousePos.y, -100f), Vector3.back);
    }

    private void OnShieldDamage()
    {
        directedShield.gameObject.SetActive(false);
        shield = shieldCooldown;
    }

    new private void OnCollisionEnter2D(Collision2D collision)
    {
        if (shield >= shieldCooldown - 0.1f)
            return;

        base.OnCollisionEnter2D(collision);
    }
}
