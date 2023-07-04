using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StaticBullshit : MonoBehaviour
{
    public static Vector2 fieldSize;
    public static Form gameForm;

    public enum Form
    {
        open,
        side,
        classic,
        arcade
    }
    public enum Condition
    {
        alive,
        dying,
        dead
    }
    public enum State
    {
        free,
        stunned
    }
}
