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

    // The list of points is always a Vector2 array, even if the points are polar. Use the following methods to fetch the points properly.
    public List<VectorP> getPolar() //Returns a list of the points in polar coodinates, regardless of what type it's stored as
    {
        List<VectorP> polarPoints = new List<VectorP>();
        if (type == VectorArtType.polar)
        {
            foreach (Vector2 point in points)
            {
                polarPoints.Add(new VectorP(point.x, point.y));
            }
        }
        else
        {
            foreach (Vector2 point in points)
            {
                polarPoints.Add(point.ToVectorP());
            }
        }
        return polarPoints;
    }

    public List<Vector2> getCartesian() //Returns a list of the points in cartesian coodinates, regardless of what type it's stored as
    {
        if (type == VectorArtType.cartesian)
        {
            return new List<Vector2>(points); // Just convert the array to a list
        }
        List<Vector2> cartesianPoints = new List<Vector2>();
        foreach (Vector2 point in points)
        {
            cartesianPoints.Add(new VectorP(point.x, point.y).ToVector2());
        }
        return cartesianPoints;
    }
}