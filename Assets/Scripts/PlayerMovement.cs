using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    Rigidbody rb;
    Vector3 inputDir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        inputDir = new Vector3(h, 0f, v).normalized;
    }

    void FixedUpdate()
    {
        Vector3 targetVelocity = inputDir * moveSpeed;
        Vector3 velocity = targetVelocity;
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }
}
