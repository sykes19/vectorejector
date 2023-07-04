using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticBullshit;

public class DirectorSpawnLogic : MonoBehaviour
{
    #region INIT

    // Prefab references
    public GameObject asteroidObj;
    public GameObject playerObj;
    public GameObject starObj;
    public GameObject enemyObj;
    public DirectorLogic dirLogic;
    // Spawning related
    GameObject player;
    Vector3 mouseLocation;
    Vector3 mousePosition;
    Vector3 spawnH;
    Vector3 spawnV;
    Vector2 starSpawnLoc;
    public int starCount;
    public int bakedStars;
    public float starSpeedMultiplier;
    public float playerRespawnTimer;
    bool playerAlive;
    // Timer related
    public float astInterval;
    float astTimer;
    public float starInterval;
    public float starIntervalAdjusted;
    float starTimer;


    #endregion

    void Awake()
    {
        // Failsafe spawn timer
        if (astInterval == 0)
            astInterval = 1;
        dirLogic = GetComponent<DirectorLogic>();
        SpawnPlayer();
    }
    private void OnEnable()
    {
        DirectorLogic.OnFormChange += OnFormChange;
        PlayerLogic.OnPlayerDeath += OnPlayerDeath;
    }
    private void OnDisable()
    {
        DirectorLogic.OnFormChange -= OnFormChange;
        PlayerLogic.OnPlayerDeath -= OnPlayerDeath;
    }
    private void Start()
    {
        // Spawn with a bunch of baked stars
        for (int i = 0; i < bakedStars; i++)
        {
            SpawnStar(true);
        }
    }
    void Update()
    {

        // Asteroid spawn timer
        if (astTimer >= astInterval)
        {
            //SpawnAsteroid(40);
            astTimer -= astInterval;
        }
        // Star spawn timer
        if (starTimer >= starIntervalAdjusted)
        {
            SpawnStar(false);
            starTimer -= starIntervalAdjusted;
        }

        astTimer += Time.deltaTime;
        starTimer += Time.deltaTime;
        playerRespawnTimer -= Time.deltaTime;
        if (!playerAlive && playerRespawnTimer < 0)
        {
            SpawnPlayer();
        }
        #region DEBUG
        ///////// BEGIN DEBUG SHIT //////////////
        // Capture mouse location, and then convert that to the proper value based on the camera space
        mouseLocation = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        mousePosition = Camera.main.ScreenToWorldPoint(mouseLocation);
        mousePosition.z = 0;

        if (Input.GetKeyDown(KeyCode.F4))
        {
            // Spawn asteroid on mouse, for debug purposes
            Instantiate(asteroidObj, mousePosition, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            // Spawn asteroid on mouse, for debug purposes
            GameObject e = Instantiate(enemyObj, mousePosition, Quaternion.identity);
            e.GetComponent<EnemyLogic>().playerTarget = player;
        }
        ////////// END DEBUG SHIT ////////////
        #endregion 
    }

    void OnFormChange(Form form)
    {
        float aspectRatioMod = 1;
        if (form == Form.arcade)
        {
            starSpeedMultiplier = 0.8f;
            aspectRatioMod = (fieldSize.y / fieldSize.x);
        }
        else if (form == Form.open)
        {
            starSpeedMultiplier = 0.2f;
            aspectRatioMod = (fieldSize.y / fieldSize.x);
        }
        else if (form == Form.side)
        {
            starSpeedMultiplier = 3f;
            aspectRatioMod = 1;
        }
        else if (form == Form.classic)
        {
            starSpeedMultiplier = 1.5f;
            aspectRatioMod = (fieldSize.y / fieldSize.x);
        }
        starIntervalAdjusted = (starInterval / starSpeedMultiplier) * aspectRatioMod;
    }
    void SpawnPlayer()
    {
        player = Instantiate(playerObj, new Vector2(0,0), Quaternion.identity);
        playerAlive = true;
    }
    void SpawnStar(bool visible)
    {
        /* This method spawns a star either visibly in a random spot on the screen, or
        off of the edge of the screen */
        float randX = Random.Range(-fieldSize.x, fieldSize.x);
        float randY = Random.Range(-fieldSize.y, fieldSize.y);
        float buffer = 1.05f;
        // If spawning a baked star, ignore edge spawn logic and do true random
        if (visible)
            starSpawnLoc = new Vector2(randX, randY);
        else
        {
            if (gameForm == Form.arcade)
            {
                // Spawn from south
                starSpawnLoc = new Vector2(randX, -fieldSize.y * buffer);
            }
            else if (gameForm == Form.open)
            {
                // Spawn from south??
                starSpawnLoc = new Vector2(randX, -fieldSize.y * buffer);
            }
            else if (gameForm == Form.side)
            {
                // Spawn from east
                starSpawnLoc = new Vector2(fieldSize.x * buffer, randY);
            }
            else if (gameForm == Form.classic)
            {
                // Spawn from north
                starSpawnLoc = new Vector2(randX, fieldSize.y * buffer);
            }
        }
        //GameObject star = Instantiate(starObj, starSpawnLoc, Quaternion.identity);
        // Spawn object from star pool;
        GameObject star = ObjectPool.instance.GetPooledStars();
        if (star != null)
        {
            star.transform.SetPositionAndRotation(starSpawnLoc, Quaternion.identity);
            star.GetComponent<StarLogic>().spawnLogic = this; // Here's my card...
            star.SetActive(true);
        }
        else
            print("Holy shit, we're out of stars!!");
    }
    void SpawnAsteroid(int budgetLimit)
    {
        // This method spawns asteroids until the budget you give it runs empty
        // They spawn off screen, and are sent toward the center with random offsets
        // Spawn asteroids until your budgetLimit empties
        for (int i = budgetLimit; i > 0;)
        {
            // Set the boundaries for spawning and assign all possibilities to array
            spawnH = new Vector3(fieldSize.x / 0.95f, Random.Range(fieldSize.y, -fieldSize.y), 0);
            spawnV = new Vector3(Random.Range(fieldSize.x, -fieldSize.x), fieldSize.y / 0.95f, 0);
            Vector3[] spawnLocs = { spawnH, spawnV, -spawnH, -spawnV };

            // Spawn asteroid in random spawn position, and pass my ID
            GameObject ast = Instantiate(asteroidObj, spawnLocs[Random.Range(0,spawnLocs.Length)], Quaternion.identity);
            AsteroidLogic astLogic = ast.GetComponent<AsteroidLogic>();

            // Find random point in a circle in the center of screen
            int circleSize = 10;
            Vector3 centerScreen = 
                new Vector2(this.transform.position.x, this.transform.position.y) +
                (Random.insideUnitCircle * circleSize);
            // Set asteroid direction toward chosen point in space
            Vector2 targetDir = centerScreen - ast.transform.position;
            astLogic.dir = targetDir;

            //
            Debug.DrawLine(ast.transform.position, targetDir, Color.green, 1.5f);
            //

            // Subtract that asteroid's value from the for loop condition
            // Some asteroids might have unique values; this is future proofing
            i -= astLogic.budgetCost;
        }
    }
    void OnPlayerDeath()
    {
        playerAlive = false;
        playerRespawnTimer = 1f;
    }
}
