using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    public GameObject bullet_K;
    public GameObject bullet_E;
    public GameObject bullet_X;
    GameObject bType;
    enum wTypes {N,E,K,X,EE,KK,XX,EK,EX,KX};
    wTypes weapon;
    int damage;
    int shots;
    float spread;
    float offset;
    float spin;
    float speed;

    PlayerLogic player;
    // Start is called before the first frame update
    void Awake()
    {   
        weapon = wTypes.N;
        ChangeWeapon();
        player = GetComponent<PlayerLogic>();
    }

    void Update()
    {
        // Temp weapon changing
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            weapon = wTypes.K;
            ChangeWeapon();
            Debug.Log("Kinetic");
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            weapon = wTypes.E;
            ChangeWeapon();
            Debug.Log("Energy");
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            weapon = wTypes.X;
            ChangeWeapon();
            Debug.Log("Explosive");
        }
    }

    // Update is called once per frame
    public void Fire(int slot)
    {
        if (slot == 1)
        {
            // Determine the angle each shot will take given spread, and total shots
            float spreadPerShot = (spread * 2) / (Mathf.Max(1, (shots - 1)));
            // If even amount of shots, split them evenly
            float nextSpread = 0;
            for(int shotsLeft = shots; shotsLeft > 0; shotsLeft -= 1)
            {   
                // Change shot angle by increment decided earlier
                float shotAngle = spread - nextSpread;
                GameObject shot = Instantiate(bType, gameObject.transform.position, gameObject.transform.rotation);
                BulletLogic shotLogic = shot.GetComponent<BulletLogic>();
                // Pass values to bullet instance
                shotLogic.speed = speed;
                shotLogic.damage = damage;
                shotLogic.spin = spin;
                shotLogic.shotAngle = shotAngle;
                shotLogic.direction = player.aimAngle;
                // Increment shot angle for next
                nextSpread += spreadPerShot;
            }
        }
    }

    void ChangeWeapon()
    {
        switch(weapon)
        {
        case wTypes.N:
            // Default
            damage = 8;
            shots = 1;
            spread = 0;
            speed = 1;
            spin = 0;
            bType = bullet_K;

            break;
        case wTypes.E:
            // Energy
            damage = 10;
            shots = 1;
            spread = 0;
            speed = 3f;
            spin = 0;
            bType = bullet_E;

            break;
        case wTypes.K:
            // Kinetic
            damage = 8;
            shots = 3;
            spread = 10;
            speed = 1;
            spin = 0;
            bType = bullet_K;

            break;
        case wTypes.X:
            // Explosive
            damage = 20;
            shots = 1;
            spread = 0;
            speed = 0.7f;
            spin = 0;
            bType = bullet_X;

            break;
        case wTypes.EE:
            // Mass Driver
            break;
        case wTypes.KK:
            // Spread
            break;
        case wTypes.XX:
            // Bomb
            break;
        case wTypes.EK:
            // Mag-wave
            break;
        case wTypes.KX:
            // Gauss
            break;
        case wTypes.EX:
            // Plasma
            break;
        }
    }
}
