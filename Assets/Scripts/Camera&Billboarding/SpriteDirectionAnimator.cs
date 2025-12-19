using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpriteDirectionAnimator : MonoBehaviour
{
    public Rigidbody rb;                    // assign player's rigidbody in inspector
    public float idleThreshold = 0.1f;      // speed below which we consider player idle

    private Animator anim;
    private Transform cam;

    private string currentState;

    private enum Facing { Front, Back, Left, Right }
    private Facing lastFacing = Facing.Back; // default when game starts

    // deadzone to counter jitters
    [Header("Facing")]
    public float sideBias = 1.25f; // >1 means needs to be 25% stronger to count left/right

    [Header("Roll")]
    private bool rollLocked;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }
    }

    private void Update()
    {

        if (rollLocked) return;

        if (rb == null) return;

        // grab camera if it appears later
        if (cam == null && Camera.main != null)
            cam = Camera.main.transform;

        if (cam == null) return;

        // horizontal velocity only
        Vector3 vel = rb.velocity;
        vel.y = 0f;
        float speed = vel.magnitude;

        bool moving = speed > idleThreshold;
        Facing facing = lastFacing;

        if (moving)
        {
            // camera relative basis vectors
            Vector3 camForward = cam.forward;
            camForward.y = 0f;
            camForward.Normalize();

            Vector3 camRight = cam.right;
            camRight.y = 0f;
            camRight.Normalize();

            float forwardAmount = Vector3.Dot(vel, camForward);
            float rightAmount = Vector3.Dot(vel, camRight);

            // decide whether movement is more left/right or forward/back
            if (Mathf.Abs(rightAmount) > Mathf.Abs(forwardAmount) * sideBias)
            {
                // mostly horizontal relative to camera
                facing = rightAmount > 0f ? Facing.Right : Facing.Left;
            }
            else
            {
                // Mostly forward/back relative to camera
                // forwardAmount > 0 = moving away from camera -> show back
                facing = forwardAmount > 0f ? Facing.Back : Facing.Front;
            }

            lastFacing = facing;
        }

        string nextState = GetStateName(facing, moving);

        if (nextState != currentState)
        {
            anim.Play(nextState);
            currentState = nextState;
        }
    }

    private string GetStateName(Facing facing, bool moving)
    {
        switch (facing)
        {
            case Facing.Front: return moving ? "WalkFront" : "IdleFront";
            case Facing.Back: return moving ? "WalkBack" : "IdleBack";
            case Facing.Left: return moving ? "WalkLeft" : "IdleLeft";
            case Facing.Right: return moving ? "WalkRight" : "IdleRight";
        }

        // fallback
        return "IdleBack";
    }

    public void PlayRoll(Vector3 worldDir)
    {
        if (cam == null && Camera.main != null) cam = Camera.main.transform;
        if (cam == null) return;

        // decide facing from roll direction
        Vector3 d = worldDir;
        d.y = 0f;
        if (d.sqrMagnitude < 0.0001f) d = transform.forward;
        d.Normalize();

        Vector3 camForward = cam.forward; camForward.y = 0f; camForward.Normalize();
        Vector3 camRight = cam.right; camRight.y = 0f; camRight.Normalize();

        float forwardAmount = Vector3.Dot(d, camForward);
        float rightAmount = Vector3.Dot(d, camRight);

        float absF = Mathf.Abs(forwardAmount);
        float absR = Mathf.Abs(rightAmount);

        Facing facing;
        // choose lef/right if more sideways than for/back
        if (absR > absF * sideBias)
            facing = rightAmount > 0f ? Facing.Right : Facing.Left;
        else
            facing = forwardAmount > 0f ? Facing.Back : Facing.Front;

        lastFacing = facing;

        string rollState = facing switch
        {
            Facing.Front => "RollFront",
            Facing.Back => "RollBack",
            Facing.Left => "RollLeft",
            Facing.Right => "RollRight",
            _ => "RollBack"
        };

        rollLocked = true;
        anim.Play(rollState);
        currentState = rollState;

        Debug.Log("PlayRoll: " + rollState);
    }

    public void EndRoll()
    {
        rollLocked = false;
    }

}
