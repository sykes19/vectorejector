using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthLogic : MonoBehaviour
{
    public int dBuffer;
    public int health;
    
    // Your condition is whether or not you are alive or dead, or flagged for death cleanup.
    public enum Condition
    {
        alive,
        dying,
        dead
    };

    // Your state is whether you can take action or move around.
    public enum State
    {
        free,
        stunned,
    }
    public Condition myCondition;
    public State myState;

    void Awake()
    {
        myCondition = Condition.alive;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (dBuffer > 0)
        {
            health -= dBuffer;
            dBuffer = 0;
            myCondition = Condition.dying;
        }
    }
}
