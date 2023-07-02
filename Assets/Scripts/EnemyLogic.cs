using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticBullshit;
using Random = UnityEngine.Random;

public class EnemyLogic : MonoBehaviour
{
    // Component references
    public GameObject playerTarget;
    public GameObject bullet;
    HealthLogic myHealth;
    Rigidbody2D rb;
    // Core values
    [NonSerialized] public Vector3 dir;
    [NonSerialized] public Vector3 aimDir;
    public int budgetCost;
    public int healthMax;
    public int burstAmmo;
    int myAmmo;
    public float speed;
    public float reloadSpeed;
    float reloadTimer;
    public float burstDelay;
    public float burstDuration;
    public float angle;
    public float rotSpeed;
    int rotDir;
    Form myForm;
    Stance stance;
    enum Stance
    {
        moving,
        firing,
        holding
    }
    
    private void Awake()
    {
        myHealth = GetComponent<HealthLogic>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        //stance = Stance.holding;
        // Flip a coin and make it either -1 or 1;
        int rotDir = (Random.Range(0, 2) * 2) - 1;
        rotSpeed *= rotDir;
        myHealth.hp = healthMax;
        myAmmo = burstAmmo;
        burstDelay = burstAmmo / burstDuration;
        FormUpdate(gameForm);
        DirectorSpawnLogic.OnFormChange += OnFormChange;
    }
    private void OnDisable()
    {
        DirectorSpawnLogic.OnFormChange -= OnFormChange;
    }
    void Start()
    {
        myForm = gameForm;
    }

    // Update is called once per frame
    void Update()
    {
        //burstAmmo - How many times to fire per burst
        //myAmmo - The amount of ammo left in the burst
        //burstDelay - Delay between shots in a burst
        //burstTimer - Timer to count burst delay
        //reloadSpeed - How long to refill ammo
        //reloadTimer - Timer to count reload speed

        reloadTimer += Time.deltaTime;
        if (reloadTimer > burstDelay)
        {
            FireShots(4);
            myAmmo -= 1;
            reloadTimer = 0;
            if (myAmmo <= 0)
            {
                // If it's the last shot of the burst, add reload delay to timer
                myAmmo = burstAmmo;
                reloadTimer -= reloadSpeed;
            }
        }

        FormUpdate(myForm);
        transform.Rotate(rotSpeed * Time.deltaTime * Vector3.forward);
        if(myHealth.myCondition == Condition.dying)
        {
            Destroy(gameObject);
        }
    }
    void FireShots(int amount)
    {
        // Spread shots around 360 degrees
        float spreadIncrement = 360 / amount;
        float angle = transform.eulerAngles.z;
        
        for(int i = 0; i < amount; i++)
        {
            // Adjust angle each shot in the burst to evenly distribute around enemy
            angle += (spreadIncrement * i);
            Quaternion addRot = Quaternion.Euler(0f, 0f, angle);
            Quaternion finalRot = Quaternion.identity * addRot;
            GameObject shot = ObjectPool.instance.GetPooledEBullets();
            shot.transform.SetPositionAndRotation(gameObject.transform.position, finalRot);
            shot.SetActive(true);
        }
    }
    void FormUpdate(Form form)
    {
        if (form == Form.arcade)
        {

        }
        else if (form == Form.open)
        {

        }
        else if (form == Form.side)
        {

        }
        else if (form == Form.classic)
        {

        }
    }
    void OnFormChange(Form form)
    {
        if (form == Form.arcade)
        {

        }
        else if (form == Form.open)
        {

        }
        else if (form == Form.side)
        {

        }
        else if (form == Form.classic)
        {

        }
    }
    void DeathRoll()
        {
            Destroy(gameObject);
        }
}
