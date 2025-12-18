using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimDriver : MonoBehaviour
{
    [Header("References")]
    public Animator animator;        // EnemyObject Animator

    [Header("Tuning")]
    public float moveThreshold = 0.05f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (animator == null || rb == null) return;

        // Only consider ground-plane movement 
        Vector3 v = rb.velocity;
        v.y = 0f;

        bool isMoving = v.sqrMagnitude > (moveThreshold * moveThreshold);
        animator.SetBool("IsMoving", isMoving);
    }

    public void PlayAttack()
    {
        if (animator == null) return;
        animator.SetTrigger("Attack");
    }
}
