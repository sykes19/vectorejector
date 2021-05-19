using UnityEngine;
using System.Collections.Generic;
using System;

// Defines and enables creation of the VectorArtShape, which contains a single plot of polar coordinates
// https://docs.unity3d.com/Manual/class-ScriptableObject.html

public enum VectorArtType { polar, cartesian}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/VectorArtShape", order = 1)]
public class VectorArtShape : ScriptableObject
{
    public VectorArtType type; // Polar or Cartesian
    public Vector2[] points; // If polar, x is Angle and y is Magnitude
    public bool loop; // Last point connects back to the first point
    public List<Vector2> toPolar() //Returns a list of the points in polar coodinates
    {
        if (type == VectorArtType.polar) { return new List<Vector2>(points); }
        List<Vector2> polars = new List<Vector2>();
        Vector2 up = new Vector2(0, 1);
        foreach (Vector2 point in points)
        {
            float A = Vector2.SignedAngle(up, point);
            float M = point.magnitude;
            polars.Add(new Vector2(A, M));
        }
        return polars;
    }
}