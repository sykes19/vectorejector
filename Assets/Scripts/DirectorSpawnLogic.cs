using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectorSpawnLogic : MonoBehaviour
{
    public GameObject asteroidObj;
    private Vector3 mouseLocation;
    private Vector3 mousePosition;
    private Vector2 fieldSize;

    private Vector3 spawn;

    private float spawnInterval = 1;
    private float timer = 0;


    // Start is called before the first frame update
    void Start()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        // Detect screen size and bind them to valuble coordinates
        fieldSize.y = Camera.main.orthographicSize;
        fieldSize.x = fieldSize.y * screenAspect;
        print("W "+fieldSize.x);
        print("H "+fieldSize.y);


    }

    void Update()
    {
        if (timer >= spawnInterval)
        {
            // Determine a valid spawn location
            spawn = new Vector3(fieldSize.x / 0.9f, Random.Range(fieldSize.y, -fieldSize.y), 0);
            Instantiate(asteroidObj, spawn, Quaternion.identity);
            timer = 0;
        }
        timer += Time.deltaTime;


        // This is just a region. Don't be scared.
        #region DEBUG SHIT
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

        if (Input.GetKeyDown(KeyCode.F1))
        {
            // Raycast directly downward based on mouse and receive reference to object hit
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider.gameObject.CompareTag("Enemy"))
                hit.collider.GetComponent<HealthLogic>().dBuffer += 1000;
            
        }
        ////////// END DEBUG SHIT ////////////
        #endregion 
    }
}
