using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using static StaticBullshit;
using Random = UnityEngine.Random;

public class AsteroidLogic : MonoBehaviour
{
    #region INIT
    // Component references
    Rigidbody2D rb;
    HealthLogic myHealth;
    PolygonCollider2D col;
    SpriteRenderer rend;
    public Sprite triangle;
    public Sprite square;
    public Sprite hex;
    // Core values
    public int size;
    public int budgetCost;
    public int healthMax;
    Vector3 baseScale;
    // Movement related
    [NonSerialized] public Vector2 dir;
    public float speed;
    [NonSerialized] public float angle;
    private Vector2 directionOld;
    // Explosion code, currently disabled
    public float deathRadius;
    public float deathStrength;

    #endregion

    void Awake()
    {
        col = GetComponent<PolygonCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        myHealth = GetComponent<HealthLogic>();

        baseScale = gameObject.transform.localScale;
    }
    private void OnEnable()
    {
        SetShape(size);
        DirectorLogic.Instance.threatEnemy += budgetCost;
        // Set initial values in case nothing else gives me any orders.
        speed = (speed / 10) * Random.Range(0.5f, 2.5f);                   // Random speed
        rb.angularVelocity = Random.Range(-100, 100);          // Random spin
        dir = Random.insideUnitCircle.normalized;            // Random direction
    }
    private void OnDisable()
    {
        DirectorLogic.Instance.threatEnemy -= budgetCost;
    }
    private void Start()
    {
        UpdateMovement();
    }

    // Method to reset movement if needed


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
    private void UpdateMovement()
    {
        rb.velocity = dir.normalized * speed;
        directionOld = dir;
    }
    private void DeathRoll()
    {
        myHealth.myCondition = Condition.dead;
        //Explode();
        // *** I like the explosion code, but not on death
        gameObject.SetActive(false);
    }

    void SetShape(int s)
    {
        float sizeMultiplier = 1;
        int choice = s;
        // If size is 0, choose an asteroid at random.
        if (s == 0)
            choice = Random.Range(1, 4);
            
        // Assign changes based on what size asteroid is being spawned
        if (choice == 1)
        {
            sizeMultiplier = 1f;
            rend.sprite = square;
        }
        if (choice == 2)
        {
            sizeMultiplier = 0.5f;
            rend.sprite = triangle;
        }
        if (choice == 3)
        {
            sizeMultiplier = 2f;
            rend.sprite = hex;
        }

        // Apply health and scale multipliers
        gameObject.transform.localScale = baseScale * sizeMultiplier;
        myHealth.hp = Mathf.RoundToInt(healthMax * sizeMultiplier);

        // Reset and re-apply the collider to match the new sprite
        col.pathCount = 0;
        col.pathCount = rend.sprite.GetPhysicsShapeCount();
        
        List<Vector2> path = new();
        for (int i = 0; i < col.pathCount; i++)
        {
            path.Clear();
            rend.sprite.GetPhysicsShape(i, path);
            col.SetPath(i, path.ToArray());
        }
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
