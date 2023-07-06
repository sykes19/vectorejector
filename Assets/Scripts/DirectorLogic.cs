using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static StaticBullshit;

public class DirectorLogic : MonoBehaviour
{
    public static DirectorLogic Instance { get; private set; }

    #region INIT
    public delegate void FormChange(Form newForm);
    public static event FormChange OnFormChange;


    [Tooltip("Target for how much threat should be in play")]
    public float threatPar;
    [Tooltip("Target par to ramp up to during opening act")]
    public int parTargetBase;
    [Tooltip("How long to wait before increasing par toward its goal")]
    public float parIncreaseDelay;
    [Tooltip("Amount to increase initial par toward goal")]
    public float parIncreaseAmount;
    [Tooltip("How many seconds to wait to decide on whether to spawn a wave")]
    public float waveInterval;
    [Tooltip("Seconds before difficulty increases at normal stress levels")]
    public float difficultyCrawl;
    [Tooltip("Amount to increment difficulty every increment")]
    public float difficultyIncreaseAmount;
    [Tooltip("Time until boredom currency increases")]
    public float boredomCrawl;
    public int threatEnemy;
    public int threatPlayer;
    public int threatTotal;
    public float difficulty = 1;
    public int budget;
    public float boredom;
    public float excitement;
    public float stress;
    public float perSecond;

    float waveTimer;
    int budgetAllowance;            // How much I can spend to spawn this wave
    int budgetBonus;                // Extra currency for spawning
    bool pauseTimers;               // Disable difficulty scaling during gameplay (for spawn, form change, death..)
    private float spawnTimer;


    #endregion
    private void Awake()
    {
        // Singleton sanity check
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        UpdateScreenSize();
    }
    void Start()
    {
        // Give the initial delegated order to change form
        SetForm(Form.arcade);
        pauseTimers = true;
        spawnTimer = 5;
        waveTimer = waveInterval;
    }

    // Update is called once per frame
    void Update()
    {

        spawnTimer -= Time.deltaTime;
        perSecond = (60 * Time.deltaTime);

        if (spawnTimer < 0)
        {   // Begin raising difficulty
            pauseTimers = false;
            waveTimer -= Time.deltaTime;
        }
        CurrencyUpdate();

        if (waveTimer < 0)
        {
            WaveSpawn();
            waveTimer = waveInterval;
        }

        UpdateScreenSize();
        #region DEBUG SHIT
        // Force form changes
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetForm(Form.arcade);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetForm(Form.classic);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SetForm(Form.open);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            SetForm(Form.side);
        #endregion
    }
    void WaveSpawn()
    {
        //if (budget < threatPar)
        if (Random.Range(1, 101) <= excitement / 3) // chance to spawn wave capped at 33% on a clear map
        {

        }
    }
        

    /// <summary>
    /// CurrencyUpdate runs once per frame and is in charge of keeping track
    /// of the emotion values and currencies the Director has. It doesn't invoke
    /// or return anything, it's merely the logic behind how the values evolve.
    /// 
    /// Although the Director decides when to spawn things and what currency to use,
    /// the SpawnLogic script is what spawns shit, after receiving an allowance.
    /// </summary>
    void CurrencyUpdate()
    {
        // If we are in active gameplay...
        if (pauseTimers == false)
        {
            // Increase threat par as the difficulty rises
            threatPar = parTargetBase * (1 + ((difficulty - 1) / 2)); // magic bullshit, makes it good

            // Passive increase in difficulty and boredom, scaled by stress ratio
            float amount = difficultyIncreaseAmount / stress;
            difficulty += amount * (Time.deltaTime / difficultyCrawl);
            boredom += (boredomCrawl * perSecond) / stress;
        }
        else
        {
            // If par has been reset, papidly increase par to target amount.
            if (threatPar < parTargetBase)
                threatPar += parIncreaseAmount * perSecond;
        }

        threatTotal = threatEnemy - threatPlayer; // Combine enemy+player threat
        if (threatTotal < 10)
        {
            //print("Warning!! Threat fell below 0! Failsafe activating");
            threatTotal = 10; // Sanity check so threat doesn't go negative
        }

        // The difference between par and total threat gives us a budget to spawn with
        budget = Mathf.RoundToInt(threatPar) - threatTotal;

        // Excitement is a percentage of (budget/par), used during wave spawning
        excitement = (budget / threatPar) * 100;

        // 0.5 stress means it's too easy, 1.5 stress means things are too hard
        stress = threatTotal / threatPar;
        if (stress < 0.1)
            stress = 0.1f; // Clamp stress to 10%
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
        gameForm = form;
        OnFormChange?.Invoke(gameForm);
    }


}
