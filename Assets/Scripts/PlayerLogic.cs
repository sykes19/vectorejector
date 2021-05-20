using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    // Component references
    public DirectorLogic dirLogic;
    Rigidbody2D rb;
    Renderer rend;
    // Core values
    public Vector2 direction;
    HealthLogic myHealth;
    public int budgetCost;
    public int healthMax;
    float horizontal;
    float vertical;
    public float speed;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        rb.velocity = (new Vector2(horizontal, vertical) * speed);
    }
}
