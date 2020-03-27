using UnityEngine;

public abstract class PlayerShip : Ship
{
    [SerializeField]
    protected float speed;
    protected Vector2 targetDirection;

    [SerializeField]
    protected GameObject moveFire;

    [SerializeField]
    protected GameObject[] energyCells;

    [SerializeField]
    protected int maxEnergy = 6;
    protected int energyLevel = 6;
    protected float fuelLevel = 4f;

    [SerializeField]
    protected float chargeCooldown = 8f;
    protected float clock = 0;

    protected float targetRotation = 0f;
    protected float rotationSpeed = 0f;


    // Start is called before the first frame update
    new void Start()
    {
        energyLevel = maxEnergy;
        SetGravityVariable(5);
        base.Start();
    }

    protected void Updater()
    {
        // Player Powers
        if (Input.GetKeyDown(KeyCode.LeftShift))
            ShiftPower();

        if (Input.GetKeyDown(KeyCode.Space))
            SpacePower();

        if (Input.GetKeyDown(KeyCode.LeftControl))
            ControlPower();

        PlayerMovement();

        // Charge
        ChargeCell();

        // Rotation
        PlayerRotation();

        base.Update();
    }

    // Player Powers
    public abstract void ShiftPower();

    public abstract void SpacePower();

    public abstract void ControlPower();


    // Charge
    protected void ChargeCell()
    {
        if (energyLevel < maxEnergy)
        {
            if (clock >= chargeCooldown)
            {
                UseEnergyCell(1);
                clock = 0;
            } else
            {
                clock += Time.deltaTime;
            }
        }
    }


    // Player Movement
    public void PlayerMovement()
    {
        float forcesCount = 0;
        float newRotation = 0f;

        moveFire.SetActive(false);

        // Fuel 
        /*
        if (fuelLevel < 0f)
        {
            if (energyLevel > 0)
            {
                UseEnergyCell(-1);
                fuelLevel = 4f;
            } else
            {
                return;
            }
        } */

        // Accepting Input
        targetDirection = Vector2.zero;
        GoInDirection(KeyCode.W, new Vector2(0f, speed), 0f, ref newRotation, ref forcesCount);
        GoInDirection(KeyCode.A, new Vector2(-speed, 0f), 90f, ref newRotation, ref forcesCount);
        GoInDirection(KeyCode.S, new Vector2(0f, -speed), 180f, ref newRotation, ref forcesCount);
        GoInDirection(KeyCode.D, new Vector2(speed, 0f), 270f, ref newRotation, ref forcesCount);
        DirectionException(KeyCode.W, KeyCode.D, 360f, ref newRotation);


        if (forcesCount != 0)
        {
            // Sound
            if (!AudioManager.AM.GetIsPlaying("ShipMovement"))
                AudioManager.AM.Play("ShipMovement");

            // Use up Fuel
            fuelLevel -= Time.deltaTime;

            // Change Rotation
            if (targetRotation != newRotation / forcesCount)
            {

                targetRotation = newRotation / forcesCount;

                // Choose closer number
                float targetRotationAbove = 360 + targetRotation;
                float targetRotationBelow = -360 + targetRotation;
                bool clockwise = true;

                // Choose rotation direction
                float currentEulerAngle = transform.eulerAngles.z;
                float angleDifference = Mathf.Abs(currentEulerAngle - targetRotation);

                if (Mathf.Abs(currentEulerAngle - targetRotationAbove) < angleDifference)         // rotação positiva
                    clockwise = false;
                else if (Mathf.Abs(currentEulerAngle - targetRotationBelow) < angleDifference)    // rotação negativa
                    clockwise = true;
                else
                {
                    if (transform.eulerAngles.z < targetRotation)
                        clockwise = false;
                    else
                        clockwise = true;
                }

                // Rotate
                if (clockwise)
                    rotationSpeed = -200f;
                else
                    rotationSpeed = 200f;
            }
        }
        else
        {
            AudioManager.AM.Stop("ShipMovement");
        }

        // Loop around
        Vector3 position = transform.position;
        if (position.x > 53)
        {
            transform.position = new Vector3(-53, position.y, position.z);
        }
        else if (position.x < -53)
        {
            transform.position = new Vector3(53, position.y, position.z);
        }

        if (position.y > 30)
        {
            transform.position = new Vector3(position.x, -30, position.z);
        }
        else if (position.y < -30)
        {
            transform.position = new Vector3(position.x, 30, position.z);
        }

    }

    protected void GoInDirection(KeyCode key, Vector2 direction, float targetAngle, ref float newRotation, ref float forcesCount)
    {
        if (Input.GetKey(key))
        {
            moveFire.SetActive(true);
            IncreaseBodyVelocity(direction * Time.deltaTime);
            targetDirection += direction;
            newRotation += targetAngle;
            forcesCount++;
        }
    }

    // Player Rotation
    protected void DirectionException(KeyCode key, KeyCode key2, float targetAngle, ref float newRotation)
    {
        if (Input.GetKey(key) && Input.GetKey(key2))
            newRotation += targetAngle;
    }

    public void PlayerRotation()
    {
        if (rotationSpeed == 0f) { return; }

        // when close enough...
        if (Mathf.Abs(transform.eulerAngles.z - targetRotation) < 4f) {
            transform.eulerAngles = transform.eulerAngles + new Vector3(0f, 0f, targetRotation - transform.eulerAngles.z);
            rotationSpeed = 0f;
        }

        transform.Rotate(new Vector3(0f, 0f, 1f), rotationSpeed * Time.deltaTime, UnityEngine.Space.Self);
    }


    // Misc
    public void SetSpeed(int speed)
    {
        this.speed = speed;
    }

    public void UseEnergyCell(int change)
    {
        int newLevel = energyLevel + change;

        if (newLevel >= 0 && newLevel <= 6)
            energyLevel = newLevel;

        for (int i = energyCells.Length-1; i >= 0; i--)
        {
            if (energyLevel-1 >= i)
                energyCells[i].SetActive(true);
            else
                energyCells[i].SetActive(false);
        }
    }

    public override void DealDamage(int damage)
    {
        health -= damage;
        AdjustColor();
        ParticleExplosion(1);

        if (health <= 0)
        {
            Explode(6);
        }
    }

    public void AdjustColor()
    {
        Color myColor = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(myColor.r, myColor.g, myColor.b, (health / maxHealth));
    }

    new private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack") { return; }

        CollisionCheck(collision.collider.gameObject, GetBodyVelocity());
    }

    new private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack") { return; }

        CollisionCheck(collision.gameObject, GetBodyVelocity());
    }
}
