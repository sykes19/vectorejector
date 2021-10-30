using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    public GameObject bullet_K;
    PlayerLogic player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerLogic>();
    }

    // Update is called once per frame
    public void Fire(int slot)
    {
        if (slot == 1)
        {
            GameObject shot = Instantiate(bullet_K, gameObject.transform.position, gameObject.transform.rotation);
            BulletLogic shotLogic = shot.GetComponent<BulletLogic>();
            shotLogic.direction = player.aimAngle;
        }
    }
}
