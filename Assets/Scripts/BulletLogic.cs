using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    public int speed;
    public int damage;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            HealthLogic otherhp = other.GetComponent<HealthLogic>();
            otherhp.dBuffer += damage;
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {

    }
}