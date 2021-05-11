using UnityEngine;
using System;

// Defines and enables creation of custom data assets.
// https://docs.unity3d.com/Manual/class-ScriptableObject.html

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/VectorArtScriptableObject", order = 1)]
public class VectorArtScriptableObject : ScriptableObject
{
    public PolarVector[] points; // Polar coodinate system
    public bool loop; // Last point connects back to the first point
}