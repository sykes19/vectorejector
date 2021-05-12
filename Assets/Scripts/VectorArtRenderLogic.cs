using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VectorArtRenderComponent
{
    public VectorArtShape Shape;
    public PolarVector Offset; // Modifying Offset.Angle will change the relative direction this art. Increasing Offset.Magnitude will push it further away from origin.
    public PolarVector Scale; // Modifying Scale.Magnitude will resize the art. We may need to decrease Scale.Angle to compensate for increased Offset.Magnitude.
    public Vector3 OriginOffset;
    internal LineRenderer Renderer;
}

public class VectorArtRenderLogic : MonoBehaviour
{
    public VectorArtRenderComponent[] Components;

    // Start is called before the first frame update
    void Start()
    {
        foreach (VectorArtRenderComponent component in Components)
        {
            component.Renderer = gameObject.AddComponent<LineRenderer>();
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Redraw()
    {
        Vector3[] temp = new Vector3[4];
        temp[0] = new Vector3(1, 1, 0);
        temp[0] = new Vector3(-1, 1, 0);
        temp[0] = new Vector3(-1, -1, 0);
        temp[0] = new Vector3(1, -1, 0);

        foreach (VectorArtRenderComponent component in Components)
        {
            component.Renderer.SetPositions(temp);
        }
    }
}

