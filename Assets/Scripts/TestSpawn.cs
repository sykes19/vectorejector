using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawn : MonoBehaviour
{
    public GameObject asteroid;
    float timer;
    private Vector2 fieldSize;
    // Start is called before the first frame update
    void Start()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        fieldSize.y = Camera.main.orthographicSize;
        fieldSize.x = fieldSize.y * screenAspect;
        //Now the Vector2 "fieldSize" represents your total game field
    }

    // Update is called once per frame
    
    void Update()
    {
        //Count the seconds that pass
        timer += Time.deltaTime;

        // When 2 seconds have passed...
        if (timer > 2)
        {
            timer = 0;
            Vector2 randLocation = new Vector2(Random.Range(-fieldSize.x, fieldSize.x), Random.Range(-fieldSize.y, fieldSize.y));
            Instantiate(asteroid, randLocation, Quaternion.identity);

        }
    }
}
