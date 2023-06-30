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
    public float speed;
    public float angle;
    public float rotSpeed;
    Form myForm;
    
    private void Awake()
    {
        myHealth = GetComponent<HealthLogic>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
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
        FormUpdate(myForm);
        transform.Rotate(rotSpeed * Time.deltaTime * Vector3.up);

        if(myHealth.myCondition == Condition.dying)
        {

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
