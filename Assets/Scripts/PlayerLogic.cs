using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    // Component references
    public DirectorLogic dirLogic;
    Rigidbody2D rb;
    HealthLogic myHealth;
    public GameObject bullet;
    // Core values
    public Vector2 aimAngle;
    public float aimDistance;
    public int budgetCost;
    public int healthMax;
    float horizontal;
    float vertical;
    public float speed;
    float fireTimer;
    public float fireDelay;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myHealth = GetComponent<HealthLogic>();
        myHealth.hp = healthMax;
    }

    void AimShip()
    {
        // Find location of mouse.
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 myPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        aimAngle = new Vector2(
            mousePosition.x - myPosition.x,
            mousePosition.y - myPosition.y
        );
        // Write down the distance in case we need it
        aimDistance = Vector2.Distance(myPosition, mousePosition);

        // Rotate on the "up" axis to match the proper direction
        transform.up = aimAngle;
    }
    void FireWeapon()
    {
        // Can I fire yet?
        if (fireTimer <= 0)
        {
            GameObject shot = Instantiate(bullet, transform.position, Quaternion.identity);
            shot.transform.up = transform.up;
            fireTimer = fireDelay;
        }
     
    }

    private void Update()
    {
        fireTimer -= Time.deltaTime;
        AimShip();
        if (Input.GetMouseButton(0))
        {
            FireWeapon();
        }


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2 (horizontal, vertical);
        transform.position += new Vector3(dir.x, dir.y, 0) * (speed /10 );
        /*
        rb.velocity = (new Vector2(horizontal, vertical) * speed);
        */
    }
}
