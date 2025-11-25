using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 15f;

    Rigidbody rb;
    Transform cam;
    Vector3 inputDir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        inputDir = new Vector3(h, 0f, v).normalized;

        Vector3 camForward = cam.forward;
        camForward.y = 0f;
        if (camForward.sqrMagnitude > 0.01f) {
            Quaternion targetRot = Quaternion.LookRotation(camForward);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    void FixedUpdate()
    {
        Vector3 worldMove = transform.TransformDirection(inputDir) * moveSpeed;
        worldMove.y = rb.velocity.y;
        rb.velocity = worldMove;
    }
}
