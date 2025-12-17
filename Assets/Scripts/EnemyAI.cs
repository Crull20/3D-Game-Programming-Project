using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 3f;
    public float stopDistance = 1.5f;
    public float turnSpeed = 12f;   // higher = snappier

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // stop physics torque causing spin
    }

    void FixedUpdate()
    {
        if (target == null) return;

        // Move on ground plane (XZ)
        Vector3 toTarget = target.position - transform.position;
        toTarget.y = 0f;

        float dist = toTarget.magnitude;
        if (dist <= stopDistance)
        {
            // stop horizontal movement
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            return;
        }

        Vector3 dir = toTarget / dist; // normalized

        // velocity on BOTH x and z
        rb.velocity = new Vector3(dir.x * moveSpeed, rb.velocity.y, dir.z * moveSpeed);

        // smooth rotate toward dir (prevents spin/jitter)
        if (dir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
            Quaternion newRot = Quaternion.Slerp(rb.rotation, targetRot, turnSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(newRot);
        }
    }
}
