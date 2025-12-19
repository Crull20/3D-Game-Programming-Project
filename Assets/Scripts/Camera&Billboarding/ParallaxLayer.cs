using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public Transform cameraTransform;
    [Range(0f, 1f)]
    public float parallaxStrength = 0.1f;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void LateUpdate()
    {
        if (!cameraTransform) return;

        var cam = cameraTransform.position;

        transform.position = new Vector3(
            startPos.x + cam.x * parallaxStrength,
            startPos.y + cam.y * parallaxStrength,
            startPos.z
        );
    }
}
