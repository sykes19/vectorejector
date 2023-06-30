using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticBullshit;

public class AutoVisionCull : MonoBehaviour
{
    public float offScreenLeniencyRatio;
    Vector2 ob;
    Vector2 pos;
    Transform tf;
    void Awake()
    {
        ob = fieldSize * offScreenLeniencyRatio;
        tf = gameObject.transform;
    }

    void Update()
    {
        ob = fieldSize * offScreenLeniencyRatio;
        // Am I out of bounds?
        if (Mathf.Abs(tf.position.x) > ob.x || Mathf.Abs(tf.position.y) > ob.y)
        {
            //print("Cull activated");
            gameObject.SetActive(false);
        }
    }
}
