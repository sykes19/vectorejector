using UnityEngine;
using System;

// Defines and enables creation of the VectorArtShape, which contains a single plot of polar coordinates
// https://docs.unity3d.com/Manual/class-ScriptableObject.html

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/VectorArtShape", order = 1)]
public class VectorArtShape : ScriptableObject
{
    public PolarVector[] points; // Polar coodinate system
    public bool loop; // Last point connects back to the first point
}