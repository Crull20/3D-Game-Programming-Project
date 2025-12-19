using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatRotate : MonoBehaviour
{
    public float floatAmplitude = 0.15f;
    public float floatSpeed = 2f;
    public float rotateSpeed = 60f;

    private Vector3 startPos;


    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Float up/down
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = startPos + new Vector3(0f, yOffset, 0f);

        // Rotate around Y
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f, Space.World);
    }



}
