using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 15f;
    public float sprintMultiplier = 1.5f;

    public float jumpForce = 5f;
    public float groundCheckDistance = 1.1f;
    public LayerMask groundMask;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    Rigidbody rb;
    Transform cam;
    Vector3 inputDir;
    private bool jumpRequested;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;

        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        inputDir = new Vector3(h, 0f, v).normalized;

        Vector3 camForward = cam.forward;
        camForward.y = 0f;
        if (camForward.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(camForward);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime
            );
        }

        // Handle jump input (space)
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        // Sprint if shift is held
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed *= sprintMultiplier;
        }

        // move toward direction based on input
        Vector3 worldMove = transform.TransformDirection(inputDir) * currentSpeed;
        worldMove.y = rb.velocity.y; // keep current vertical velocity
        rb.velocity = worldMove;

        // apply jump
        if (jumpRequested)
        {
            jumpRequested = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        ApplyBetterGravity();
    }

    void ApplyBetterGravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        // Optional: if going up but jump key is released, apply extra gravity for short hop
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    bool IsGrounded()
    {
        // Simple ground check using a downward SphereCast
        // Adjust groundCheckDistance and radius if needed

        /*
        float radius = 0.2f;
        return Physics.SphereCast(
            transform.position,
            radius,
            Vector3.down,
            out RaycastHit hit,
            groundCheckDistance,
            groundMask
        );
        */

        return true;
    }
}