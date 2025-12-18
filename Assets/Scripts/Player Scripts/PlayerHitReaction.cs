using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerHitReaction : MonoBehaviour
{
    [Header("Knockback")]
    public float knockbackForce = 10f;
    public float knockbackTime = 0.12f;

    [Header("Red hit flash")]
    public SpriteRenderer spriteRenderer; // drag your player sprite renderer here
    public Color hitColor = Color.red;
    public float flashTime = 0.12f;

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
        // direction away from enemy (horizontal only)
        Vector3 dir = transform.position - fromPosition;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) dir = transform.forward;
        dir.Normalize();

        StopAllCoroutines();
        StartCoroutine(KnockbackRoutine(dir));
        StartCoroutine(FlashRoutine());
    }

    IEnumerator KnockbackRoutine(Vector3 dir)
    {
        isKnockback = true;

        // Set velocity so it works even with your movement script overwriting rb.velocity
        Vector3 v = dir * knockbackForce;
        v.y = rb.velocity.y; // keep gravity/vertical
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

    // Let PlayerMovement read this so it doesn't overwrite knockback
    public bool IsKnockbackActive() => isKnockback;
}