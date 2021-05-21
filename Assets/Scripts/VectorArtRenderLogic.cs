using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorArtRenderLogic : MonoBehaviour
{
    public VectorArtShape shape;
    public float magnitudeOffset = 0; // Higher values push art further away from origin
    public float angleScale = 1; // Set to -1 to mirror the art. Otherwise, decrease to compensate possible warping caused by MagnitudeOffset
    public float magnitudeScale = 1; // Higher values increase size of art (without making the lines thicker)

    LineRenderer lineRenderer;
    List<VectorP> polarPoints; // persistent list of the art in polar coords
    List<Vector3> originPoints = new List<Vector3>(); // does not include current translation, and persistes between updates
    Quaternion lastRotation;
    float lastMagnitudeOffset;
    float lastAngleScale;
    float lastMagnitudeScale;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.loop = shape.loop;
        polarPoints = shape.getPolar();
        Recalc();
    }

    // Update is called once per frame
    void Update()
    {
        List<Vector3> renderPoints;

        // If the only change since last frame is translation, we can simplify calculations slighly
        if ((transform.rotation == lastRotation)
         && (magnitudeOffset == lastMagnitudeOffset)
         && (angleScale == lastAngleScale)
         && (magnitudeScale == lastMagnitudeScale))
        {
            renderPoints = new List<Vector3>();
            foreach (Vector3 Point in originPoints)
            {
                renderPoints.Add(Point + transform.position);
            }
        }
        else
        {
            renderPoints = Recalc();
        }
        lineRenderer.positionCount = renderPoints.Count;
        lineRenderer.SetPositions(renderPoints.ToArray());
    }

    // Recalculates rotation and scaling
    List<Vector3> Recalc()
    {
        lastRotation = transform.rotation;
        lastMagnitudeOffset = magnitudeOffset;
        lastAngleScale = angleScale;
        lastMagnitudeScale = magnitudeScale;

        // newPoints includes current translation. It gets returned to become renderPoints.
        List<Vector3> newPoints = new List<Vector3>();

        // originPoints does not include current translation, and persistes between updates
        originPoints.Clear();

        //Rotate the art based on the current Z Euler angle, negated because + is CCW
        float angleOffset = -transform.eulerAngles.z; 

        foreach (VectorP polar in polarPoints)
        {
            Vector3 originPoint = new VectorP((polar.angle * angleScale) + angleOffset, (polar.magnitude * magnitudeScale) + magnitudeOffset).ToVector3();
            originPoints.Add(originPoint);
            newPoints.Add(originPoint + transform.position);
        }

        return newPoints;
    }
}

