using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    #region INIT
    // Component references
    public DirectorLogic dirLogic;
    Rigidbody2D rb;
    HealthLogic myHealth;
    public GameObject bullet;
    public GameObject leftWing;
    public GameObject rightWing;
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

    #endregion
    public enum Form
    {
        open,
        side,
        classic,
        arcade
    }
    public Form myForm;

    void Awake()
    {
        myForm = Form.arcade;
        rb = GetComponent<Rigidbody2D>();
        myHealth = GetComponent<HealthLogic>();
        myHealth.hp = healthMax;
    }

    void Update()
    {
        fireTimer -= Time.deltaTime;
        // Determine how the ship aims based on form
        if (myForm == Form.open || myForm == Form.arcade)
            AimShip();
        else if (myForm == Form.classic)
            aimAngle = new Vector2 (0, 1);
        else if (myForm == Form.side)
            aimAngle = new Vector2 (1, 0);
        // Face ship toward proper angle
        transform.up = aimAngle;

        if (Input.GetMouseButton(0))
        {
            FireWeapon();
        }

        #region DEBUG
        // ***DEBUG***
        // Forcefully change ship form

        //***END DEBUG***
        #endregion
    }
    void FixedUpdate()
    {
        // Movement code
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(horizontal, vertical);
        transform.position += new Vector3(dir.x, dir.y, 0) * (speed / 10);
    }

    public void FormChange(Form form)
    {
        float Lrot = 0f;
        float Rrot = 0f;
        Vector3 Lpos = Vector3.zero;
        Vector3 Rpos = Vector3.zero;
        Vector3 Lscl = Vector3.zero;
        Vector3 Rscl = Vector3.zero;

        // Prepare the values needed to transform the ship's looks based on the form
        switch (form)
        {
            case Form.open:
                {
                    Lpos = new Vector3(-1f, -0.5f, 0);
                    Lrot = 171f;
                    Lscl = Vector3.one;
                    Rpos = new Vector3(1f, -0.5f, 0);
                    Rrot = 189f;
                    Rscl = Vector3.one;
                    myForm = Form.open;
                    break;
                }
            case Form.side:
                {
                    Lpos = new Vector3(0, 0, -5);
                    Lrot = 0;
                    Lscl = new Vector3(0.6f, 1, 1);
                    Rpos = new Vector3(0, 0, 5);
                    Rrot = 0;
                    Rscl = new Vector3(0.6f, 1, 1);
                    myForm = Form.side;
                    break;
                }
            case Form.classic:
                {
                    Lpos = new Vector3(-1f, 0, 0);
                    Lrot = -12.45f;
                    Lscl = Vector3.one;
                    Rpos = new Vector3(1f, 0, 0);
                    Rrot = 12.45f;
                    Rscl = Vector3.one;
                    myForm = Form.classic;
                    break;
                }
            case Form.arcade:
                {
                    Lpos = new Vector3(-1f, 0, 0);
                    Lrot = -12.45f;
                    Lscl = Vector3.one;
                    Rpos = new Vector3(1f, 0, 0);
                    Rrot = 12.45f;
                    Rscl = Vector3.one;
                    myForm = Form.arcade;
                    break;
                }
        }
        // Adjust wings to match form
        leftWing.transform.localPosition = Lpos;
        leftWing.transform.localScale = Lscl;
        Quaternion desiredLrot = Quaternion.Euler(0f, 0f, Lrot);
        leftWing.transform.localRotation = desiredLrot;

        rightWing.transform.localPosition = Rpos;
        rightWing.transform.localScale = Rscl;
        Quaternion desiredRrot = Quaternion.Euler(0f, 0f, Rrot);
        rightWing.transform.localRotation = desiredRrot;
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




}
