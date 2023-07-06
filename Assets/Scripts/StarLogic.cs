using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticBullshit;
using Random = UnityEngine.Random;

public class StarLogic : MonoBehaviour
{
    public DirectorSpawnLogic spawnLogic;
    public Vector3 dir;
    public float speed;
    [Tooltip("The original size of the star")]
    public float scaleBase;
    [Tooltip("High ratio = slow stars are smaller, fast stars are bigger")]
    public float scaleRatio;
    [Tooltip("Scale Factor is what % of speed to consider in using to change scale")]
    public float scaleFactor;
    public float minSpeed;
    public float maxSpeed;
    public float speedMulti;
    float scale;
    float newSpeed;

    private void Awake()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        // Scaled Speed is what percentage of speed should be taken into the equation
        float scaledSpeed = speed * scaleFactor;
        // Scale Ratio is how much speed should affect the scale of the star
        scaleRatio = 1.0f + scaledSpeed * scaleRatio; 
        scale = scaleBase * scaleRatio;
        transform.localScale *= scale;
    }
    private void OnEnable()
    {
        DirectorLogic.OnFormChange += FormUpdate;;
        FormUpdate(gameForm);
    }
    private void OnDisable()
    {
        DirectorLogic.OnFormChange -= FormUpdate;

    }

    void Start()
    {

    }

    // Set star speed multiplier, spawn rate, and direction based on form change
    void FormUpdate(Form form)
    {
        if (form == Form.arcade)
        {
            // Slow speed, northbound
            speedMulti = 0.8f;
            dir = new Vector3(0, 1);
        }
        else if (form == Form.open)
        {
            //Not implemented.
            speedMulti = 0.2f;
            dir = new Vector3(0, 1);
        }
        else if (form == Form.side)
        {
            //Fast speed, westbound. Adjust for spawning on shorter screen size
            speedMulti = 3f;
            dir = new Vector3(-1, 0);
        }
        else if (form == Form.classic)
        {
            //Medium speed, southbound
            speedMulti = 1.5f;
            dir = new Vector3(0, -1);
        }
        else
            print("ERROR: Star failed to retrieve Form!");
    }
    private void Update()
    {
        newSpeed = (speed * Time.deltaTime) *  speedMulti;
        transform.position += dir * newSpeed;
    }

    void FixedUpdate()
    {

    }
}
