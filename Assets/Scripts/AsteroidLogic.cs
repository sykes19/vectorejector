using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticBullshit;

public class AsteroidLogic : MonoBehaviour
{
    #region INIT
    // Component references
    public DirectorLogic dirLogic;
    Rigidbody2D rb;
    // Core values
    public Vector2 direction;
    HealthLogic myHealth;
    public int budgetCost;
    public int healthMax;
    // Movement related
    public float speed;
    public float angle;
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

        // No physics for now. Just triggers.
        GetComponent<Collider2D>().isTrigger = true;

        // Attach to health script, and tell it how beeg boi I am
        myHealth = GetComponent<HealthLogic>();
        myHealth.hp = healthMax;

        // Set initial values in case nothing else gives me any orders.
        speed *= Random.Range(0.5f,2.5f);                   // Random speed
        rb.angularVelocity = Random.Range(-50,50);          // Random spin

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

        rb.velocity = direction.normalized * speed;
        directionOld = direction;
    }

    void FixedUpdate()
    {
        // Check if outside sources tell me to move elsewhere
        if (directionOld != direction)
            UpdateMovement();

        // Check if it's time to die
        if (myHealth.myCondition == HealthLogic.Condition.dying)
        {
            DeathRoll();
        }
    }

    private void Update()
    {
        
    }

    private void DeathRoll()
    {
        myHealth.myCondition = HealthLogic.Condition.dead;
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
            Rigidbody2D targetRB = hit.GetComponent<Rigidbody2D>();
            if (targetRB != null)
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
