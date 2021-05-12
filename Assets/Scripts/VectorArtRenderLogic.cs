using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorArtRenderLogic : MonoBehaviour
{
    public VectorArtShape Shape;
    public float MagnitudeOffset = 0; // Higher values push art further away from origin
    public float AngleScale = 1; // Decrease to compensate possible warping caused by MagnitudeOffset
    public float MagnitudeScale = 1; // Higher values increase size of art

    LineRenderer Renderer;
    List<Vector3> OriginPoints = new List<Vector3>();
    Quaternion LastRotation;
    float LastMagnitudeOffset;
    float LastAngleScale;
    float LastMagnitudeScale;

    // Start is called before the first frame update
    void Start()
    {
        Renderer = gameObject.GetComponent<LineRenderer>();
        Renderer.loop = Shape.loop;
        Recalc();
    }

    // Update is called once per frame
    void Update()
    {
        List<Vector3> Points;

        // If the only change since last frame is translation, we can simplify calculations slighly
        if ((transform.rotation == LastRotation)
         && (MagnitudeOffset == LastMagnitudeOffset)
         && (AngleScale == LastAngleScale)
         && (MagnitudeScale == LastMagnitudeScale))
        {
            Points = new List<Vector3>();
            foreach (Vector3 Point in OriginPoints)
            {
                Points.Add(Point + transform.position);
            }
        }
        else
        {
            Points = Recalc();
        }
        Renderer.positionCount = Points.Count;
        Renderer.SetPositions(Points.ToArray());
    }

    List<Vector3> Recalc()
    {
        LastRotation = transform.rotation;
        LastMagnitudeOffset = MagnitudeOffset;
        LastAngleScale = AngleScale;
        LastMagnitudeScale = MagnitudeScale;

        List<Vector3> Points = new List<Vector3>();
        OriginPoints.Clear();

        //Rotate the art based on the current Z Euler angle, negated because + is CCW
        float AngleOffset = -transform.eulerAngles.z; 

        foreach (PolarVector Polar in Shape.points)
        {
            float A = (Polar.Angle * AngleScale + AngleOffset) * Mathf.Deg2Rad;
            float M = Polar.Magnitude * MagnitudeScale + MagnitudeOffset;
            Vector3 OriginPoint = new Vector3(Mathf.Sin(A) * M, Mathf.Cos(A) * M, 0);
            OriginPoints.Add(OriginPoint);
            Points.Add(OriginPoint + transform.position);
        }

        return Points;
    }
}

