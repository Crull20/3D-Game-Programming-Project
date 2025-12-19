using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [Header("References")]
    public Transform attackPoint;
    public Transform followTarget;   // <-- drag your FollowTarget here
    public Camera mainCam;           // optional fallback

    [Header("Attack Settings")]
    public float attackRadius = 1.2f;
    public float attackRangeForward = 1.2f;
    public int damage = 25;
    public float attackCooldown = 0.4f;

    [Header("VFX")]
    public GameObject slashVfxPrefab;

    [Header("VFX Placement")]
    public float forwardOffset = 1.2f;

    public LayerMask enemyLayers;

    private bool canAttack = true;
    private bool flipNext;
    private readonly HashSet<Collider> hitThisSwing = new HashSet<Collider>();

    void Awake()
    {
        if (mainCam == null) mainCam = Camera.main;
    }

    void Update()
    {
        if (canAttack && Input.GetMouseButtonDown(0))
        {
            flipNext = !flipNext;
            StartCoroutine(AttackRoutine(flipNext));
        }
    }

    IEnumerator AttackRoutine(bool flipX)
    {
        canAttack = false;
        hitThisSwing.Clear();

        Vector3 dir = GetAttackDir();

        SpawnSlash(dir, flipX);

        AudioManager.I?.Play2D(AudioManager.I.playerSlash, 0.9f);

        yield return new WaitForSeconds(0.15f);
        DoDamage(dir);

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    Vector3 GetAttackDir()
    {
        // 1) Prefer FollowTarget direction (what you asked for)
        if (followTarget != null && attackPoint != null)
        {
            Vector3 d = followTarget.position - attackPoint.position;
            d.y = 0f;
            if (d.sqrMagnitude > 0.0001f) return d.normalized;
        }

        // 2) Fallback: camera forward projected on ground
        if (mainCam != null)
        {
            Vector3 d = mainCam.transform.forward;
            d.y = 0f;
            if (d.sqrMagnitude > 0.0001f) return d.normalized;
        }

        return Vector3.forward;
    }

    void SpawnSlash(Vector3 dir, bool flipX)
    {
        if (slashVfxPrefab == null || attackPoint == null) return;

        Vector3 pos = attackPoint.position + dir * forwardOffset;
        Quaternion rot = Quaternion.LookRotation(dir, Vector3.up) * Quaternion.Euler(45f, 0f, 150f);

        GameObject slash = Instantiate(slashVfxPrefab, pos, rot);

        var sr = slash.GetComponentInChildren<SpriteRenderer>();
        // if (sr != null) sr.flipX = flipX;
    }

    void DoDamage(Vector3 dir)
    {
        if (attackPoint == null) return;

        Vector3 center = attackPoint.position + dir * attackRangeForward;
        Collider[] hits = Physics.OverlapSphere(center, attackRadius, enemyLayers, QueryTriggerInteraction.Ignore);

        foreach (Collider c in hits)
        {
            if (hitThisSwing.Contains(c)) continue;
            hitThisSwing.Add(c);

            IDamageable dmg = c.GetComponentInParent<IDamageable>();
            if (dmg != null) dmg.TakeDamage(damage, transform.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Vector3 dir = Vector3.forward;
        if (followTarget != null)
        {
            dir = followTarget.position - attackPoint.position;
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.0001f) dir = Vector3.forward;
            else dir.Normalize();
        }

        Vector3 center = attackPoint.position + dir * attackRangeForward;
        Gizmos.DrawWireSphere(center, attackRadius);
    }
}