using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using UnityEngine;
using static StaticBullshit;

public class DirectorSpawnLogic : MonoBehaviour
{
    #region INIT
    public Form gameForm;
    // Prefab references
    public GameObject asteroidObj;
    public GameObject playerObj;
    public GameObject starObj;
    public DirectorLogic dirLogic;
    PlayerLogic pLogic;
    // Spawning related
    Vector3 mouseLocation;
    Vector3 mousePosition;
    Vector3 spawnH;
    Vector3 spawnV;
    public int starCount;
    public int bakedStars;
    Vector2 starSpawnLoc;
    // Currency related
    public float astInterval;
    float astTimer;
    public float starInterval;
    public float starIntervalAdjusted;
    float starTimer;


    #endregion

    void Awake()
    {
        UpdateScreenSize();
        gameForm = Form.arcade;
        // Failsafe spawn timer
        if (astInterval == 0)
            astInterval = 1;
        dirLogic = GetComponent<DirectorLogic>();



        SpawnPlayer();
        // Spawn with a bunch of baked stars
        for (int i = 0; i < bakedStars; i++)
        {
            SpawnStar(true);
        }

    }
    void Update()
    {
        // Update global variables for screen size
        UpdateScreenSize();
        // Asteroid spawn timer
        if (astTimer >= astInterval)
        {
            SpawnAsteroid(40);
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

        // Force form changes
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetForm(Form.arcade);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetForm(Form.classic);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SetForm(Form.open);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            SetForm(Form.side);
        ////////// END DEBUG SHIT ////////////
        #endregion 
    }
    void UpdateScreenSize()
    {
        // Detect screen size and bind them to global variables
        float screenAspect = (float)Screen.width / (float)Screen.height;
        fieldSize.y = Camera.main.orthographicSize;
        fieldSize.x = fieldSize.y * screenAspect;
    }
    void SetForm(Form form)
    {
        gameForm = form;
        pLogic.FormChange(form);
    }
    void SpawnPlayer()
    {
        GameObject player = Instantiate(playerObj, new Vector2(0,0), Quaternion.identity);
        pLogic = player.GetComponent<PlayerLogic>();
        pLogic.FormChange(gameForm);
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
        GameObject star = Instantiate(starObj, starSpawnLoc, Quaternion.identity);
        star.GetComponent<StarLogic>().spawnLogic = this; // Here's my card...
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
            astLogic.dirLogic = dirLogic;

            // Find random point in a circle in the center of screen
            int circleSize = 10;
            Vector3 centerScreen = 
                new Vector2(this.transform.position.x, this.transform.position.y) +
                (Random.insideUnitCircle * circleSize);
            // Set asteroid direction toward chosen point in space
            Vector2 targetDir = centerScreen - ast.transform.position;
            astLogic.direction = targetDir;

            //
            Debug.DrawLine(ast.transform.position, targetDir, Color.green, 1.5f);
            //

            // Subtract that asteroid's value from the for loop condition
            // Some asteroids might have unique values; this is future proofing
            i -= astLogic.budgetCost;
        }
    }
}
