using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticBullshit;
using Random = UnityEngine.Random;

public class AsteroidLogic : MonoBehaviour
{
    #region INIT
    // Component references
    public DirectorLogic dirLogic;
    Rigidbody2D rb;
    // Core values
    HealthLogic myHealth;
    public int budgetCost;
    public int healthMax;
    // Movement related
    [NonSerialized] public Vector2 dir;
    [NonSerialized] public float speed;
    [NonSerialized] public float angle;
    private Vector2 directionOld;
    // Explosion code, currently disabled
    public float deathRadius;
    public float deathStrength;

    #endregion

    void Awake()
    {
        // Sanity check to prevent infinite loops
        if (budgetCost == 0)
            budgetCost = 20;

        rb = GetComponent<Rigidbody2D>();

        // Attach to health script, and tell it how beeg boi I am
        myHealth = GetComponent<HealthLogic>();
        myHealth.hp = healthMax;

        // Set initial values in case nothing else gives me any orders.
        speed *= Random.Range(0.5f,2.5f);                   // Random speed
        rb.angularVelocity = Random.Range(-50,50);          // Random spin
        dir = Random.insideUnitCircle.normalized;           // Random direction
    }

    private void Start()
    {
        UpdateMovement();
    }

    // Method to reset movement if needed
    private void UpdateMovement()
    {
        // This code was for polar -> cartesian conversion, but I removed the polar parts
        // For now...
        //float dirx = Mathf.Sin(angle * Mathf.Deg2Rad) * speed;
        //float diry = Mathf.Cos(angle * Mathf.Deg2Rad) * speed;
        //direction = new Vector2(dirx, diry);

        rb.velocity = dir.normalized * speed;
        directionOld = dir;
    }

    private void Update()
    {
        // Check if outside sources tell me to move elsewhere
        if (directionOld != dir)
            UpdateMovement();

        // Check if it's time to die
        if (myHealth.myCondition == Condition.dying)
        {
            DeathRoll();
        }
    }

    private void DeathRoll()
    {
        myHealth.myCondition = Condition.dead;
        //Explode();
        // *** I like the explosion code, but not on death
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // On death, subtract my budget value from on-screen budget
        
        // THIS CANNOT STAY, IT MUST BE FIXED. FINISH DirectorLogic PLEASE

        //dirLogic.budget -= budgetValue;
    }

    private void Explode()
    {
        // Knockback AoE
        // Create a circular effect, and capture the IDs of every nearby collider
        // Apply a force to each collider in range and knock it back
        Vector2 explosionPos = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, deathRadius);
        foreach (Collider2D hit in colliders)
        {
            if (hit.TryGetComponent<Rigidbody2D>(out var targetRB))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                // Decrease strength based on distance to edge of radius
                float pushStrength = deathStrength * (0.7f - (distance / deathRadius));
                // Clamp strength at 20% of max
                float minStr = deathStrength * 0.2f;
                if (pushStrength < minStr)
                    pushStrength = minStr;

                // Determine the direction the target needs pushed
                Vector2 targetPush = hit.transform.position - transform.position;
                targetPush = targetPush.normalized * pushStrength;

                // Add force to target based on normalized vector * strength
                targetRB.AddForce(targetPush, ForceMode2D.Impulse);
            }

        }
    }
}
