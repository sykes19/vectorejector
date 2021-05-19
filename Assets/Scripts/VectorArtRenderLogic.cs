using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorArtRenderLogic : MonoBehaviour
{
    public VectorArtShape shape;
    public float magnitudeOffset = 0; // Higher values push art further away from origin
    public float angleScale = 1; // Decrease to compensate possible warping caused by MagnitudeOffset
    public float magnitudeScale = 1; // Higher values increase size of art

    LineRenderer lineRenderer;
    List<Vector2> polarPoints;
    List<Vector3> originPoints = new List<Vector3>();
    Quaternion lastRotation;
    float lastMagnitudeOffset;
    float lastAngleScale;
    float lastMagnitudeScale;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.loop = shape.loop;
        polarPoints = shape.toPolar();
        Recalc();
    }

    // Update is called once per frame
    void Update()
    {
        List<Vector3> points;

        // If the only change since last frame is translation, we can simplify calculations slighly
        if ((transform.rotation == lastRotation)
         && (magnitudeOffset == lastMagnitudeOffset)
         && (angleScale == lastAngleScale)
         && (magnitudeScale == lastMagnitudeScale))
        {
            points = new List<Vector3>();
            foreach (Vector3 Point in originPoints)
            {
                points.Add(Point + transform.position);
            }
        }
        else
        {
            points = Recalc();
        }
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    // Recalculates rotation
    List<Vector3> Recalc()
    {
        lastRotation = transform.rotation;
        lastMagnitudeOffset = magnitudeOffset;
        lastAngleScale = angleScale;
        lastMagnitudeScale = magnitudeScale;

        List<Vector3> points = new List<Vector3>();
        originPoints.Clear();

        //Rotate the art based on the current Z Euler angle, negated because + is CCW
        float angleOffset = -transform.eulerAngles.z; 

        foreach (Vector2 polar in polarPoints)
        {
            float A = (polar.x * angleScale + angleOffset) * Mathf.Deg2Rad;
            float M = polar.y * magnitudeScale + magnitudeOffset;
            Vector3 originPoint = new Vector3(Mathf.Sin(A) * M, Mathf.Cos(A) * M, 0);
            originPoints.Add(originPoint);
            points.Add(originPoint + transform.position);
        }

        return points;
    }
}

