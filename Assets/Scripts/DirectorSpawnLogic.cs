using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerLogic;

public class DirectorSpawnLogic : MonoBehaviour
{
    #region INIT
    // Prefab references
    public GameObject asteroidObj;
    public GameObject playerObj;
    public DirectorLogic dirLogic;
    PlayerLogic pLogic;
    // Spawning related
    private Vector3 mouseLocation;
    private Vector3 mousePosition;
    private Vector2 fieldSize;
    private Vector3 spawnH;
    private Vector3 spawnV;
    // Currency related
    private float spawnInterval;
    private float timer;

    #endregion

    void Awake()
    {
        // Failsafe spawn timer
        if (spawnInterval == 0)
            spawnInterval = 1;
        dirLogic = GetComponent<DirectorLogic>();

        // Detect screen size and bind them to valuble coordinates
        float screenAspect = (float)Screen.width / (float)Screen.height;
        fieldSize.y = Camera.main.orthographicSize;
        fieldSize.x = fieldSize.y * screenAspect;

        SpawnPlayer();

    }
    void Update()
    {
        if (timer >= spawnInterval)
        {
            // Spend 40 budget coins (budgies) on asteroids on a set interval.
            SpawnAsteroid(40);
            timer -= spawnInterval;
        }
        timer += Time.deltaTime;



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
            pLogic.FormChange(Form.arcade);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            pLogic.FormChange(Form.classic);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            pLogic.FormChange(Form.open);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            pLogic.FormChange(Form.side);
        ////////// END DEBUG SHIT ////////////
        #endregion 
    }
    void SpawnPlayer()
    {
        GameObject player = Instantiate(playerObj, new Vector2(0,0), Quaternion.identity);
        pLogic = player.GetComponent<PlayerLogic>();
    }

    // This method spawns asteroids until the budget you give it runs empty
    // They spawn off screen, and are sent toward the center with random offsets
    void SpawnAsteroid(int budgetLimit)
    {
        // Spawn asteroids until your budgetLimit empties
        for(int i = budgetLimit; i > 0;)
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
