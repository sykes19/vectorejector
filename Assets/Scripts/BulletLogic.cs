using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    public Rigidbody2D rb;
    public Collider2D hitbox;
    SpriteRenderer rend;
    public int damage;
    public float speed;
    float defaultSpeed;
    public float spin;
    public float shotAngle;
    public Vector3 direction;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<Collider2D>();
        rend = GetComponent<SpriteRenderer>();
        //direction = transform.forward;
    }

    void Start()
    {
        // Temp values
        if (damage == 0) {damage = 10;}
        defaultSpeed = 1000f;
        direction = RotateZ(direction, shotAngle);
    }

    void FixedUpdate()
    {
        rb.transform.Rotate(0,0,spin);
        rb.velocity = direction * (defaultSpeed * speed) * Time.fixedDeltaTime;
        if (rend.isVisible == false) {Destroy(gameObject);}
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            HealthLogic eHP = other.gameObject.GetComponent<HealthLogic>();
            eHP.dBuffer += damage;
            Destroy(gameObject);
        }

    }
    public Vector3 RotateZ(Vector3 v, float degrees)
    {
        float angle = degrees * Mathf.Deg2Rad;

        float sin = Mathf.Sin( angle );
        float cos = Mathf.Cos( angle );
       
        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (cos * ty) + (sin * tx);

        return v;
    }
}
