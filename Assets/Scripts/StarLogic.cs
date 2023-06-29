using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticBullshit;

public class StarLogic : MonoBehaviour
{
    public DirectorSpawnLogic spawnLogic;
    public Vector3 dir;
    public float speed;
    float scale;
    public float scaleBase;
    public float scaleRatio;
    public float minSpeed;
    public float maxSpeed;
    public float speedMulti;
    float newSpeed;
    float starInterval;
    float aspectRatioMod;
    Form myForm;

    private void Awake()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        scale = scaleBase * (speed * scaleRatio);
        transform.localScale = transform.localScale * scale;
    }

    void Start()
    {
        if (spawnLogic == null)
        {
            //spawnLogic = GameObject.Find("DirectorObj").GetComponent<DirectorSpawnLogic>();
            print("EMERGENCY: Star FAILED to find Director!!!");
        }
        myForm = spawnLogic.gameForm;
        spawnLogic.starCount++;
        starInterval = spawnLogic.starInterval;
        FormUpdate(myForm);
    }

    // Set star speed multiplier, spawn rate, and direction based on form change
    void FormUpdate(Form form)
    {
        if (form == Form.arcade)
        {
            // Slow speed, northbound
            speedMulti = 0.8f;
            aspectRatioMod = (fieldSize.y / fieldSize.x);
            dir = new Vector3(0, 1);
        }
        else if (form == Form.open)
        {
            //Not implemented.
            speedMulti = 0.2f;
            aspectRatioMod = (fieldSize.y / fieldSize.x);
            dir = new Vector3(0, 1);
        }
        else if (form == Form.side)
        {
            //Fast speed, westbound. Adjust for spawning on shorter screen size
            speedMulti = 3f;
            aspectRatioMod = 1;
            dir = new Vector3(-1, 0);
        }
        else if (form == Form.classic)
        {
            //Medium speed, southbound
            speedMulti = 1.5f;
            aspectRatioMod = (fieldSize.y / fieldSize.x);
            dir = new Vector3(0, -1);
        }
        else
            print("ERROR: Star failed to retrieve Form!");
        myForm = form;
        // Adjust spawn rate based on speed and length of spawn plane
        spawnLogic.starIntervalAdjusted = (starInterval / speedMulti) * aspectRatioMod;
    }
    private void Update()
    {
        newSpeed = (speed * Time.deltaTime) *  speedMulti;
        transform.position += dir * newSpeed;
    }

    void FixedUpdate()
    {
        if (myForm != spawnLogic.gameForm)
            FormUpdate(spawnLogic.gameForm);
    }

    private void OnDestroy()
    {
        spawnLogic.starCount--;
    }
}
