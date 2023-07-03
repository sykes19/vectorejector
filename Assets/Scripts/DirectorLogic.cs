using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static StaticBullshit;

public class DirectorLogic : MonoBehaviour
{
    #region INIT
    public delegate void FormChange(Form newForm);
    public static event FormChange OnFormChange;

    int budgetAllowance;            // How much 
    int budgetBonus;                // Extra currency for spawning
    bool spawnCooldown;             // Pause difficulty climb during opening act
    [Tooltip("Target for how much threat should be in play")]
    public int threatPar;
    [Tooltip("Target par to ramp up to during opening act")]
    public int parTargetBase;
    [Tooltip("How long to wait before increasing par toward its goal")]
    public int parIncreaseDelay;
    [Tooltip("Amount to increase initial par toward goal")]
    public int parIncreaseAmount;
    [Tooltip("How often to roll a dice on whether to spawn a wave")]
    public int waveInterval;
    [Tooltip("Seconds until difficulty increases")]
    public int baseDifficultyCrawl;
    [Tooltip("Time until boredom currency increases")]
    public int baseBoredomCrawl;
    public int boredomCrawl;
    public int difficultyCrawl;
    public int threatEnemy;
    public int threatPlayer;
    public int threatTotal;
    public int difficulty = 1;
    public int budget;
    public int boredom;
    public int excitement;
    public int stress;
    // Test


    #endregion
    private void Awake()
    {
        UpdateScreenSize();
    }
    void Start()
    {
        // Give the initial delegated order to change form
        SetForm(Form.arcade);
    }

    // Update is called once per frame
    void Update()
    {
        // Update global variables for screen size
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
