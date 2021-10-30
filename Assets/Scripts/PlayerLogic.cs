using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    // Component references
    // Director creates us, and gives us this reference
    public DirectorLogic dirLogic;
    WeaponLogic weapon;
    Rigidbody2D rb;
    Renderer rend;
    HealthLogic myHealth;
    // Core values
    public Vector2 aimAngle;
    public float aimDistance;

    public int budgetCost;
    public int healthMax;

    public float speed;
    public Vector2 impulseCache;
    private Vector2 impulse;
    public float impulseDecayRate;
    private Vector2 impulseDecay;
    private Vector2 targetVelocity;
    private float minSpeed;
    private Vector2 input;
    

    // Start is called before the first frame update
    void Awake()
    {
        speed = 400f;

        // Decay 10% of total impulse per second
        impulseDecayRate = 1.0f;

        weapon = GetComponent<WeaponLogic>();
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        // DEBUG
        if (Input.GetKeyDown(KeyCode.E))
        {
            impulseCache += aimAngle * (aimDistance * 0.2f);
        }
        // END DEBUG

        // Input capture. Applies to FixedUpdate()
        input = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        AimShip();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            weapon.Fire(1);
        }
    }
    void FixedUpdate()
    {
        ImpulseUpdate();
        MoveShip(input);
    }

#region Moving
    void ImpulseUpdate()
    {
        float speedCutoff = 0.1f;
        // Add new forces to the current impulse amount
        impulse += impulseCache;
        // If we've received new forces, calculate a new decay rate
        // And set minimum speed at 10% of current total impulse
        if (impulseCache != Vector2.zero)
        {
            minSpeed = (Mathf.Abs(impulse.x) + Mathf.Abs(impulse.y)) * speedCutoff;
            impulseDecay = impulse * impulseDecayRate;
        }
        // Get absolute speed values of impulse and decay
        float currentSpeed = Mathf.Abs(impulse.x) + Mathf.Abs(impulse.y);
        // Decay impulse by percentage (impulseDecay) every second
        if (currentSpeed > minSpeed)
        {
            impulse -= impulseDecay * Time.fixedDeltaTime;
        }
        else {impulse = Vector2.zero;}
        // Clear out impulse cache
        impulseCache = Vector2.zero;
    }
    void MoveShip(Vector2 input)
    {
         // If moving diagonally, reduce velocity down to 71% on each axis
        if (Mathf.Abs(input.x) == 1f && Mathf.Abs(input.y) == 1f)
                {targetVelocity = input * 0.71f;}
        else    {targetVelocity = input;}
        // Add impulse velocity after all normal movement.
        targetVelocity = (targetVelocity * (speed)) * Time.fixedDeltaTime;
        rb.velocity = targetVelocity + impulse;
    }
#endregion
#region Aiming
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
        aimAngle = aimAngle.normalized;
    }
#endregion
    // Update is called once per frame
}
