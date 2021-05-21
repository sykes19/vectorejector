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
    public Vector2 aimAngle;
    public float aimDistance;
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

    void AimShip()
    {
        // Find location of mouse.
        Vector2 mouseLocation = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(mouseLocation);
        Vector2 myPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        aimAngle = new Vector2(
            mousePosition.x - myPosition.x,
            mousePosition.y - myPosition.y
        );
        // Write down the distance in case we need it
        aimDistance = Vector2.Distance(myPosition, mousePosition);

        transform.up = aimAngle;
    }

    private void Update()
    {
        AimShip();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        rb.velocity = (new Vector2(horizontal, vertical) * speed);
    }
}
