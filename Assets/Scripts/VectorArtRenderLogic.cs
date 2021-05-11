using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct VectorArtRenderComponent
{
    public VectorArtScriptableObject VectorArt;
    public PolarVector Offset; // Modifying Offset.Angle will change the relative direction this art. Increasing Offset.Magnitude will push it further away from origin.
    public PolarVector Scale; // Modifying Scale.Magnitude will resize the art. We may need to decrease Scale.Angle to compensate for increased Offset.Magnitude.
    public Vector3 OriginOffset;
}

public class VectorArtRenderLogic : MonoBehaviour
{
    public VectorArtRenderComponent[] Components;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
