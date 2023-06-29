using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticBullshit;

public class PlayerLogic : MonoBehaviour
{
    #region INIT
    // Component references
    public DirectorLogic dirLogic;
    public DirectorSpawnLogic spawnLogic;
    Rigidbody2D rb;
    HealthLogic myHealth;
    public GameObject bullet;
    public GameObject leftWing;
    public GameObject rightWing;
    // Core values
    public Vector3 aimAngle;
    public float aimDistance;
    public int budgetCost;
    public int healthMax;
    float horizontal;
    float vertical;
    public float speed;
    float fireTimer;
    public float fireDelay;
    Vector3 forwardVector;
    #endregion
    #region Ship Form Init
    // Ship transformation values
    public float formChangeDuration = 0.7f;
    private float formChangeTimer;
    private Vector3 leftWingStartScale;
    private Vector3 rightWingStartScale;
    private Vector3 leftWingTargetScale;
    private Vector3 rightWingTargetScale;
    private Vector3 leftWingStartPos;
    private Vector3 leftWingTargetPos;
    private Vector3 rightWingStartPos;
    private Vector3 rightWingTargetPos;
    private Quaternion leftWingStartRot;
    private Quaternion leftWingTargetRot;
    private Quaternion rightWingStartRot;
    private Quaternion rightWingTargetRot;
    private Vector3 Lpos;
    private Vector3 Rpos;
    private Vector3 Lscl;
    private Vector3 Rscl;
    private float Lrot;
    private float Rrot;
    private float maxTilt = 0.4f;
    private float tiltSpeed = 10f;
    private bool isTransforming;
    Transform Lt;
    Transform Rt;
    #endregion
    public Form myForm;
    void Awake()
    {
        Lt = leftWing.transform;
        Rt = rightWing.transform;
        rb = GetComponent<Rigidbody2D>();
        myHealth = GetComponent<HealthLogic>();
        myHealth.hp = healthMax;

    }

    private void Start()
    {
        if (spawnLogic == null)
        {
            myForm = Form.arcade;
            print("Could not find spawnLogic, overriding!");
            spawnLogic = GameObject.Find("DirectorObj").GetComponent<DirectorSpawnLogic>();
        }
        myForm = spawnLogic.gameForm;
        FormChange(myForm);
    }

    void Update()
    {
        // Mandatory visual and input updates
        FormUpdate(myForm);
        fireTimer += Time.deltaTime;
        transform.up = aimAngle;
        if (Input.GetMouseButton(0))
            FireWeapon();

        #region DEBUG
        // ***DEBUG***
        // Forcefully change ship form

        //***END DEBUG***
        #endregion
    }
    void FixedUpdate()
    {
        if (myForm != spawnLogic.gameForm)
            FormChange(spawnLogic.gameForm);
        // Movement code
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(horizontal, vertical);
        dir.Normalize();
        transform.position += new Vector3(dir.x, dir.y, 0) * (speed / 10); // Magic number for precision? :x
    }

    // This is a one-time order to make all necessary adjustments to change forms
    public void FormChange(Form form)
    {
        Lrot = 0f;  // Zero this shit out to start fresh
        Rrot = 0f;
        Lpos = Vector3.zero;
        Rpos = Vector3.zero;
        Lscl = Vector3.zero;
        Rscl = Vector3.zero;

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
                    Lpos = new Vector3(0, 0, 0);
                    Lrot = 0;
                    Lscl = new Vector3(0.6f, 1, 1);
                    Rpos = new Vector3(0, 0, 0);
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
        // Convert desired rotation float into useable quaternion
        Quaternion desiredLrot = Quaternion.Euler(0f, 0f, Lrot);
        Quaternion desiredRrot = Quaternion.Euler(0f, 0f, Rrot);

        // Set beginning and end values for FormUpdate to process each frame
        leftWingStartPos = Lt.localPosition;
        leftWingTargetPos = Lpos;
        leftWingStartScale = Lt.localScale;
        leftWingTargetScale = Lscl;
        leftWingStartRot = Lt.localRotation;
        leftWingTargetRot = desiredLrot;

        rightWingStartPos = Rt.localPosition;
        rightWingTargetPos = Rpos;
        rightWingStartScale = Rt.localScale;
        rightWingTargetScale = Rscl;
        rightWingStartRot = Rt.localRotation;
        rightWingTargetRot = desiredRrot;

        formChangeTimer = 0f;   // Reset transformation timer
    }

    // This runs every frame to make form-specific checks
    void FormUpdate(Form myForm)
    {
        // Update visual transformation if necessary
        if (formChangeTimer < formChangeDuration)
        {
            isTransforming = true;
            formChangeTimer += Time.deltaTime;
            float t = formChangeTimer / formChangeDuration;
            Lt.localPosition = Vector3.Lerp(leftWingStartPos, leftWingTargetPos, t);
            Rt.localPosition = Vector3.Lerp(rightWingStartPos, rightWingTargetPos, t);
            Lt.localScale = Vector3.Lerp(leftWingStartScale, leftWingTargetScale, t);
            Rt.localScale = Vector3.Lerp(rightWingStartScale, rightWingTargetScale, t);
            Lt.localRotation = Quaternion.Slerp(leftWingStartRot, leftWingTargetRot, t);
            Rt.localRotation = Quaternion.Slerp(rightWingStartRot, rightWingTargetRot, t);
        }
        else
            isTransforming = false;

        // Restrict or allow aiming based on form, and do form-specific bullshit
        if (myForm == Form.open || myForm == Form.arcade)
            AimShip();
        else if (myForm == Form.classic)
            aimAngle = new Vector2(0, 1);
        else if (myForm == Form.side)
        {
            aimAngle = new Vector2(1, 0);
            // Tilt wings by sliding on X axis based on vertical input held
            if (!isTransforming && Mathf.Abs(vertical) >= 0.2)
            {
                // Make the equation work for both up and down movement
                float targetX = (maxTilt * Mathf.Sign(vertical));
                // Take into account potential differences in default pos for L and R wings
                float targetXL = leftWingTargetPos.x + targetX;
                float targetXR = rightWingTargetPos.x + targetX;
                Vector3 targetPositionL = new Vector3(targetXL, Lt.localPosition.y, Lt.localPosition.z);
                Vector3 targetPositionR = new Vector3(-targetXR, Rt.localPosition.y, Rt.localPosition.z);
                Lt.localPosition = Vector3.Lerp(Lt.localPosition, targetPositionL, Time.deltaTime * tiltSpeed);
                Rt.localPosition = Vector3.Lerp(Rt.localPosition, targetPositionR, Time.deltaTime * tiltSpeed);
            }
            else    // Reset to neutral when nothing is being held
            {
                Lt.localPosition = Vector3.Lerp(Lt.localPosition, leftWingTargetPos, Time.deltaTime * tiltSpeed);
                Rt.localPosition = Vector3.Lerp(Rt.localPosition, rightWingTargetPos, Time.deltaTime * tiltSpeed);
            }
        }
    }
    void AimShip()
    {
        // Find location of mouse.
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 myPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        aimAngle = new Vector3(
            mousePosition.x - myPosition.x,
            mousePosition.y - myPosition.y
        );
        // Write down the distance in case we need it
        aimDistance = Vector2.Distance(myPosition, mousePosition);
    }
    void FireWeapon()
    {
        // Can I fire yet?
        if (fireTimer >= fireDelay)
        {
            // Offset the shot in front of me a lil bit
            float distance = 0.6f;
            forwardVector = transform.up;
            Vector3 offset = forwardVector * distance;
            Vector3 shotSpawn = transform.position + offset;
            GameObject shot = Instantiate(bullet, shotSpawn, Quaternion.identity);
            shot.transform.up = transform.up;
            fireTimer = 0;
        }  
    }
}
