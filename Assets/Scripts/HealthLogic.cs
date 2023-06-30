using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticBullshit;

public class HealthLogic : MonoBehaviour
{
    public int dBuffer;
    public int hp;

    // Your condition is whether or not you are alive or dead, or flagged for death cleanup.
    // Your state is whether you can take action or move around.


    public Condition myCondition;
    public State myState;

    void OnEnable()
    {
        myCondition = Condition.alive;
    }
    void OnDisable()
    {
        myCondition = Condition.dead;
    }

    void Update()
    {
        if (myCondition == Condition.alive)
        {
            if (dBuffer > 0)
            {
                hp -= dBuffer;
                dBuffer = 0;
            }
            if (hp <= 0)
            {
                myCondition = Condition.dying;
            }
        }
    }
}
