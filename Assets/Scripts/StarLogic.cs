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
    public float scaleBase;
    public float scaleRatio;
    public float minSpeed;
    public float maxSpeed;
    public float speedMulti;
    float scale;
    float newSpeed;

    private void Awake()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        scale = scaleBase * (speed * scaleRatio);
        transform.localScale = Vector3.one * scale;
    }
    private void OnEnable()
    {
        DirectorSpawnLogic.OnFormChange += FormUpdate;;
        FormUpdate(gameForm);
    }
    private void OnDisable()
    {
        DirectorSpawnLogic.OnFormChange -= FormUpdate;

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
