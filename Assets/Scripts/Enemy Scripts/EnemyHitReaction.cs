using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyHitReaction : MonoBehaviour
{
    [Header("Knockback")]
    public float knockbackForce = 8f;
    public float knockbackTime = 0.12f;

    [Header("Hit Flash")]
    public SpriteRenderer spriteRenderer; // enemy sprite
    public Color hitColor = Color.red;
    public float flashTime = 0.1f;

    Rigidbody rb;
    Color originalColor;
    bool isKnockback;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public void ApplyHit(Vector3 fromPosition)
    {
        Vector3 dir = transform.position - fromPosition;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.001f)
            dir = transform.forward;
        dir.Normalize();

        StopAllCoroutines();
        StartCoroutine(KnockbackRoutine(dir));
        StartCoroutine(FlashRoutine());
    }

    IEnumerator KnockbackRoutine(Vector3 dir)
    {
        isKnockback = true;

        Vector3 v = dir * knockbackForce;
        v.y = rb.velocity.y;
        rb.velocity = v;

        yield return new WaitForSeconds(knockbackTime);
        isKnockback = false;
    }

    IEnumerator FlashRoutine()
    {
        if (spriteRenderer == null) yield break;

        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(flashTime);
        spriteRenderer.color = originalColor;
    }

    public bool IsKnockbackActive() => isKnockback;
}