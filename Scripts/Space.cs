using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Space : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer background;

    [SerializeField]
    private GameObject credits;

    [SerializeField]
    private int smallCount;

    [SerializeField]
    private int mediumCount;

    [SerializeField]
    private int bigCount;

    [SerializeField]
    private int giantCount;

    [SerializeField]
    private int gargantuanCount;

    [SerializeField]
    private float speederWave = 5;

    [SerializeField]
    private float waveCooldown = 4;
    private float wave = 0;
    private float clock = 0;
    private float deathClock = 0;

    [SerializeField]
    private GameObject[] asteroidPresets;

    [SerializeField]
    private GameObject playerShip;
    private GameObject playerShipInstance;
    private GameObject bossShipInstance;

    [SerializeField]
    private GameObject speederShip;

    [SerializeField]
    private GameObject disablerShip;

    [SerializeField]
    private GameObject bossShip;

    [SerializeField]
    private Transform[] spawnLocations;

    private List<GameObject> objectList = new List<GameObject>();

    bool gameEnd = false;
    bool backgrounding = true;


    public void StartGame()
    {
        if (!backgrounding)
        {
            SpawnPlayerShip();
            AudioManager.AM.Play("GameTheme");
        } else
        {
            // Spawn asteroids
            SpawnAsteroids(1, smallCount);
            SpawnAsteroids(2, mediumCount);
            SpawnAsteroids(3, bigCount);
            SpawnAsteroids(5, giantCount);
            SpawnAsteroids(8, gargantuanCount);
        }
    }

    private void Update()
    {
        if (backgrounding) { return; }

        WaveControl();

        // When Player Destroyed...
        if (!playerShipInstance)
        {
            AudioManager.AM.StopAll();
            if (deathClock > 1)
                ResetGame();
            else
                deathClock += Time.deltaTime;
        }

        SpaceReveal();
    }

    public void WaveControl()
    {
        if (gameEnd) { return; }

        if (clock > waveCooldown)
        {

            int asteroidWaveCount = 2;
            if (objectList.Count < 16)
                asteroidWaveCount = 5;

            // Asteroid Spawn
            for (int i = 0; i < asteroidWaveCount; i++)
                SpawnAsteroid(Random.Range(2, 8), spawnLocations[Random.Range(0, spawnLocations.Length)].position, true);

            // Speeder Spawn
            SpawnSpeeder(spawnLocations[Random.Range(0, spawnLocations.Length)].position);

            // Disabler Spawn
            if (wave % 4 == 0 && wave != 0 && playerShipInstance)
            {
                Vector3 spawnPosition;
                do {
                    spawnPosition = new Vector3(Random.Range(-43, 43), Random.Range(-20, 20), playerShipInstance.transform.position.z);
                } while ((playerShipInstance.transform.position - spawnPosition).magnitude < 15f ||
                (bossShipInstance && (bossShipInstance.transform.position - spawnPosition).magnitude < 32f));

                SpawnDisabler(spawnPosition);
            }

            // Boss Spawn
            if (wave == 16)
            {
                AudioManager.AM.Stop("GameTheme");
                AudioManager.AM.Play("BossTheme");
                SpawnBoss(new Vector2(-70, -1));
                Debug.Log("spawn boss");
            }

            // Wave Control
            wave++;
            clock = 0;
        }
        else
        {
            clock += Time.deltaTime;
        }
    }

    public void SpawnAsteroids(int scale, int count)
    {
        // Spawn space objects
        for (int i = 0; i < count; i++)
        {
            Vector2 spawnPosition;

            do
            {
                spawnPosition = new Vector2(Random.Range(-50, 50), Random.Range(-30, 30));
            } while ((spawnPosition.x > -8 && spawnPosition.x < 8) && (spawnPosition.y > -7 && spawnPosition.y < 7));

            SpawnAsteroid(scale, spawnPosition, false);
        }
    }

    public void SpawnAsteroid(int scale, Vector2 spawnLocation, bool gameActive)
    {
        GameObject newObject = Instantiate(asteroidPresets[Random.Range(0, asteroidPresets.Length)], spawnLocation, Quaternion.identity, this.transform);
        newObject.GetComponent<Asteroid>().SetSpace(this);
        newObject.GetComponent<Asteroid>().SetScale(scale);
        newObject.GetComponent<Asteroid>().SetMass(1);

        if (gameActive)
            newObject.GetComponent<Asteroid>().SetBodyVelocity(-spawnLocation * Random.Range(0.01f, 0.1f));
        else
            newObject.GetComponent<Asteroid>().SetBodyVelocity(new Vector2(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f)));

        objectList.Add(newObject);
        newObject.GetComponent<Asteroid>().SetObjectList(objectList);
    }

    public void SpawnSpeeder(Vector2 spawnLocation)
    {
        GameObject newObject = Instantiate(speederShip, spawnLocation, Quaternion.identity, this.transform);
        newObject.GetComponent<SpeederEnemy>().SetSpace(this);
        newObject.GetComponent<SpeederEnemy>().SetMass(1);

        if (playerShipInstance)
            newObject.GetComponent<SpeederEnemy>().SetPlayerShip(playerShipInstance.GetComponent<PlayerShip>());

        //newObject.GetComponent<SpeederEnemy>().SetBodyVelocity(-spawnLocation * 0.1f);

        objectList.Add(newObject);
        newObject.GetComponent<SpeederEnemy>().SetObjectList(objectList);
    }

    public void SpawnDisabler(Vector2 spawnLocation)
    {
        GameObject newObject = Instantiate(disablerShip, spawnLocation, Quaternion.identity, this.transform);
        newObject.GetComponent<DisablerEnemy>().SetSpace(this);
        newObject.GetComponent<DisablerEnemy>().SetScale(6);
        newObject.GetComponent<DisablerEnemy>().SetMass(1);

        if (playerShipInstance)
            newObject.GetComponent<DisablerEnemy>().SetPlayerShip(playerShipInstance.GetComponent<PlayerShip>());

        objectList.Add(newObject);
        newObject.GetComponent<DisablerEnemy>().SetObjectList(objectList);
    }

    public void SpawnBoss(Vector2 spawnLocation)
    {
        GameObject newObject = Instantiate(bossShip, spawnLocation, Quaternion.identity, this.transform);
        newObject.GetComponent<BossEnemy>().SetSpace(this);
        newObject.GetComponent<BossEnemy>().SetScale(10);
        newObject.GetComponent<BossEnemy>().SetMass(4);
        bossShipInstance = newObject;

        if (playerShipInstance)
            newObject.GetComponent<BossEnemy>().SetPlayerShip(playerShipInstance.GetComponent<PlayerShip>());

        newObject.GetComponent<BossEnemy>().SetBodyVelocity(new Vector2(1f, 0f));
        objectList.Add(newObject);
        newObject.GetComponent<BossEnemy>().SetObjectList(objectList);
    }

    public void SpawnPlayerShip()
    {
        // Spawn space objects
        Vector2 spawnPosition = new Vector2(0f, 0f);

        GameObject newObject = Instantiate(playerShip, spawnPosition, Quaternion.identity, this.transform);
        newObject.GetComponent<PlayerShip>().SetSpace(this);
        playerShipInstance = newObject;

        objectList.Add(newObject);
        newObject.GetComponent<PlayerShip>().SetObjectList(objectList);
    }

    public void EndExplosion()
    {
        gameEnd = true;
        AudioManager.AM.Play("BossDeath");
        AudioManager.AM.Stop("BossTheme");
        AudioManager.AM.Play("VictoryTheme");

        for (int i = objectList.Count - 1; i >= 0; i--)
        {
            if (!objectList[i].GetComponent<PlayerShip>())
                objectList[i].GetComponent<SpaceObject>().Explode(2);
        }
    }

    public void ResetGame()
    {
        AudioManager.AM.StopAll();
        Messenger.MS.resetting = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SpaceReveal()
    {
        if (!gameEnd) { return; }

        credits.transform.position += new Vector3(0f, 15f*Time.deltaTime, 0f);

        if (background.color.r < 1f)
            background.color = new Color(background.color.r + Time.deltaTime, background.color.g + Time.deltaTime, background.color.b + Time.deltaTime, background.color.a);
    }

    public float GetGravitationalConstant()
    {
        return 6.67f * Mathf.Pow(10, -3);
    }

    public void EraseObject(GameObject erased)
    {
        objectList.Remove(erased);
    }

    public void SetBackgrounding(bool back)
    {
        backgrounding = back;
    }

}
