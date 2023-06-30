using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticBullshit;

public class BulletLogic : MonoBehaviour
{
    Rigidbody2D rb;
    public int speed;
    public int damage;
    public bool friendly;
    string target;

    // All public values need set in the prefab.
    // If the player didn't shoot the shot, it's not friendly
    // transform.up needs to be adjusted on instantiation

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (friendly)
            target = "Enemy";
        else
            target = "Player";
        rb.velocity = transform.up.normalized * speed;
    }
    void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(target))
        {   
            // Save these values for a future particle system
            Vector2 hitSpeed = other.relativeVelocity;
            Vector2 hitLoc = other.GetContact(0).point;

            HealthLogic otherhp = other.gameObject.GetComponent<HealthLogic>();
            if (otherhp == null)
            {
                print("Bullet can't find HealthLogic! Terminating!");
                gameObject.SetActive(false);
            }
            else
            {
                otherhp.dBuffer += damage;
                gameObject.SetActive(false);
            } 
        }
    }
}