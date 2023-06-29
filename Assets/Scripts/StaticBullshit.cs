using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StaticBullshit : MonoBehaviour
{
    public static Vector2 fieldSize;
    public static float neverSeenTimeLimit = 4;

    public enum Form
    {
        open,
        side,
        classic,
        arcade
    }
}
