using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 15f;
    public float sprintMultiplier = 1.5f;

    public float groundCheckDistance = 1.1f;
    public LayerMask groundMask;
    public float groundCheckRadius = 0.25f;

    Rigidbody rb;
    Transform cam;
    Vector3 inputDir;

    [Header("Dash")]
    public float dashSpeed = 18f;
    public float dashDuration = 0.12f;
    public float dashCooldown = 0.6f;

    private bool isDashing;
    private float dashTimeLeft;
    private float dashCooldownLeft;
    private Vector3 dashDir;

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
        camForward.Normalize();

        Vector3 camRight = cam.right;
        camRight.y = 0f;
        camRight.Normalize();

        inputDir = (camForward * v + camRight * h).normalized;

        if (dashCooldownLeft > 0f)
            dashCooldownLeft -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && dashCooldownLeft <= 0f && !isDashing && IsGrounded())
        {
            // dash in direction moving, if standing still, dash forward
            Vector3 desired = transform.TransformDirection(inputDir);
            desired.y = 0f;

            dashDir = (inputDir.sqrMagnitude > 0.001f) ? inputDir : transform.forward;

            isDashing = true;
            dashTimeLeft = dashDuration;
            dashCooldownLeft = dashCooldown;
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

        if (isDashing)
        {
            //Dash velocity (keep gravity)
            Vector3 v = dashDir * dashSpeed;
            v.y = rb.velocity.y;
            rb.velocity = v;

            dashTimeLeft -= Time.fixedDeltaTime;
            if (dashTimeLeft <= 0f)
                isDashing = false;

            return;                 // skip normal movement while dashing
        }

        // move toward direction based on input
        Vector3 worldMove = transform.TransformDirection(inputDir) * currentSpeed;
        worldMove.y = rb.velocity.y; // keep current vertical velocity
        rb.velocity = worldMove;

    }

    bool IsGrounded()
    {
        return Physics.SphereCast(
            transform.position,
            groundCheckRadius,
            Vector3.down,
            out _,
            groundCheckDistance,
            groundMask,
            QueryTriggerInteraction.Ignore
        );
    }

}