using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticBullshit;

public class AutoVisionCull : MonoBehaviour
{
    bool seen;
    float failsafeTimer;
    // Start is called before the first frame update
    void Awake()
    {

    }
    private void OnBecameInvisible()
    {
        if (seen)
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameVisible()
    {
        seen = true;
    }
    // Update is called once per frame
    void Update()
    {
        // If spawned but never shown to camera, force the value after a time limit
        failsafeTimer += Time.deltaTime;
        if (failsafeTimer > neverSeenTimeLimit)
            seen = true;
    }
}
