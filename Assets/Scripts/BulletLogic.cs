using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    public Rigidbody2D rb;
    public Collider2D hitbox;
    public int damage;
    public float speed;
    public Vector2 direction;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<Collider2D>();
    }

    void Start()
    {
        if (damage == 0) {damage = 10;}
        if (speed == 0) {speed = 1000f;}
        Debug.Log(direction +"and "+ speed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = direction * speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            HealthLogic eHP = other.gameObject.GetComponent<HealthLogic>();
            eHP.dBuffer += damage;
            Destroy(gameObject);
        }

    }
}