using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticBullshit;
using Random = UnityEngine.Random;

public class PlayerLogic : MonoBehaviour
{
    #region INIT
    public delegate void PlayerDeath();
    public static event PlayerDeath OnPlayerDeath;
    // Component references
    Rigidbody2D rb;
    HealthLogic myHealth;
    public GameObject bullet;
    public GameObject leftWing;
    public GameObject rightWing;
    // Core values
    public Vector3 aimAngle;
    [NonSerialized] public float aimDistance;
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
    public float formChangeDuration;
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
    private Vector3 defaultScale;
    private Transform shipTf;
    Transform Lt;
    Transform Rt;
    #endregion

    void Awake()
    {   
        defaultScale = gameObject.transform.localScale;
        shipTf = gameObject.transform;
        shipTf.localScale = Vector3.zero;
        Lt = leftWing.transform;
        Rt = rightWing.transform;
        rb = GetComponent<Rigidbody2D>();
        myHealth = GetComponent<HealthLogic>();
    }
    // Subscribe to the form change event
    private void OnEnable()
    {
        OnFormChange(gameForm);
        myHealth.hp = healthMax;
        myHealth.myCondition = Condition.alive;
        DirectorSpawnLogic.OnFormChange += OnFormChange;
    }
    private void OnDisable()
    {
        DirectorSpawnLogic.OnFormChange -= OnFormChange;   
    }

    private void Start()
    {
        
    }

    void Update()
    {
        // Expand ship until it's desired size
        if(shipTf.localScale.magnitude < defaultScale.magnitude)
            shipTf.localScale += (defaultScale / 10) * (Time.deltaTime * 60);
        else if (shipTf.localScale.magnitude != defaultScale.magnitude)
            shipTf.localScale = defaultScale;

        if (myHealth.myCondition == Condition.dying)
        {
            // Announce I've died
            OnPlayerDeath?.Invoke();
            Destroy(gameObject);
        }
        // Mandatory visual and input updates
        FormUpdate(gameForm);
        fireTimer += Time.deltaTime;

        if (Input.GetMouseButton(0))
            FireWeapon();

        // Movement code
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector2 dir = new(horizontal, vertical);
        dir.Normalize();
        transform.position += ((60 * Time.deltaTime) * (speed / 10)) * new Vector3(dir.x, dir.y, 0); // Magic number for precision? :x
    }
    void FixedUpdate()
    {

    }

    // This is a one-time order to make all necessary adjustments to change forms
    void OnFormChange(Form form)
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
        {
            // Find location of mouse.
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 myPosition = new(gameObject.transform.position.x, gameObject.transform.position.y);
            aimAngle = mousePosition - myPosition;
            // Write down the distance in case we need it
            aimDistance = Vector2.Distance(myPosition, mousePosition);
        }
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
                Vector3 targetPositionL = new(targetXL, Lt.localPosition.y, Lt.localPosition.z);
                Vector3 targetPositionR = new(-targetXR, Rt.localPosition.y, Rt.localPosition.z);
                Lt.localPosition = Vector3.Lerp(Lt.localPosition, targetPositionL, Time.deltaTime * tiltSpeed);
                Rt.localPosition = Vector3.Lerp(Rt.localPosition, targetPositionR, Time.deltaTime * tiltSpeed);
            }
            else    // Reset to neutral when nothing is being held
            {
                Lt.localPosition = Vector3.Lerp(Lt.localPosition, leftWingTargetPos, Time.deltaTime * tiltSpeed);
                Rt.localPosition = Vector3.Lerp(Rt.localPosition, rightWingTargetPos, Time.deltaTime * tiltSpeed);
            }
        }
        // Rotation code, it needs to take an adjusted aimAngle from FormUpdate
        float angle = (Mathf.Atan2(aimAngle.y, aimAngle.x) * Mathf.Rad2Deg) - 90;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
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
            GameObject shot = ObjectPool.instance.GetPooledPBullets();
            if (shot != null )
            {
                shot.transform.SetPositionAndRotation(shotSpawn, Quaternion.identity);
                shot.transform.up = transform.up;
                shot.SetActive(true);
            }
            fireTimer = 0;
        }  
    }
}
