using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnExpansion : MonoBehaviour
{
    private Vector3 defaultScale;
    [Tooltip("Default value is 1")]
    public float expansionSpeed = 1;
    private Transform tf;
    bool isExpanding = true;

    void Awake()
    {
        defaultScale = gameObject.transform.localScale;
        tf = gameObject.transform;

    }

    private void OnEnable()
    {
        tf.localScale = Vector3.zero;
        isExpanding = true;
    }

    void Update()
    {
        // Expand until desired size is reached and then wait.
        if (isExpanding)
        {
            if (tf.localScale.magnitude < defaultScale.magnitude)
                tf.localScale += (defaultScale / 10) * (Time.deltaTime * (expansionSpeed * 60));
            else if (tf.localScale.magnitude != defaultScale.magnitude)
            {
                tf.localScale = defaultScale;
                isExpanding = false;
            }
        }
        
    }
}
