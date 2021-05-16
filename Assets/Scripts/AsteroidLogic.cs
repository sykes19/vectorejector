using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidLogic : MonoBehaviour
{
    Rigidbody2D rb;
    Renderer rend;
    Vector2 direction;
    HealthLogic myHealth;
    public float speed;
    public float angle;
    public float deathRadius;
    public float deathStrength;
    public int budgetCost;
    public int healthMax;
    private bool seen;

    // Bounce code or something
    //static Vector2 MirrorX = new Vector2(-1f, 1f);
    //static Vector2 MirrorY = new Vector2(1f, -1f);

    void Awake()
    {
        seen = false;
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<Renderer>();
        // No collisions for now. Mommy is mad.
        GetComponent<Collider2D>().isTrigger = true;

        myHealth = GetComponent<HealthLogic>();
        myHealth.health = healthMax;

        // Set a bunch of initial values. These can be overridden during the Start and OnAwake phases.
        // Eventually, direction won't be true random, but dependant on the perspective type we're in.
        // Speed will also be factored in dynamically, but for now, randomized for testing.

        angle = Random.Range(0,359);                        // Random direction
        speed = speed * Random.Range(0.5f,2.5f);            // Random speed
        float dirx = Mathf.Sin(angle * Mathf.Deg2Rad) * speed;
        float diry = Mathf.Cos(angle * Mathf.Deg2Rad) * speed;
        direction = new Vector2(dirx, diry);

        rb.velocity = direction;                            
        rb.angularVelocity = Random.Range(-50,50);          // Random spin
    }

    void FixedUpdate()
    {
        // If I've been seen before, destroy self off screen
        if (rend.isVisible == false && seen == true)
            myHealth.myCondition = HealthLogic.Condition.dying;
        // If I'm being seen for the first time, make note
        else if (rend.isVisible == true && seen == false)
            seen = true;

        // Check if it's time to die
        if (myHealth.myCondition == HealthLogic.Condition.dying)
        {
            //Explode();
            // I don't want to explode anymore. Saving the code for later.
            DeathRoll();
        }
    }

    private void Update()
    {
        
    }

    private void DeathRoll()
    {
        myHealth.myCondition = HealthLogic.Condition.dead;
        Destroy(gameObject);
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
                // Decrease strength based on distance to edge of radius, linear
                float pushStrength = deathStrength * (1 - (distance / deathRadius));

                // Grab an angle to the target, normalize it, and multiply it by strength
                Vector2 targetPush = hit.transform.position - transform.position;
                targetPush = targetPush.normalized * pushStrength;

                // Add force to target based on normalized vectory * strength
                targetRB.AddForce(targetPush, ForceMode2D.Impulse);
            }

        }
    }
}
