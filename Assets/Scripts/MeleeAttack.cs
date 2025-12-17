using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public Transform attackPoint;

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

        SpawnSlash(flipX);

        yield return new WaitForSeconds(0.15f);
        DoDamage();

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void SpawnSlash(bool flipX)
    {
        if (slashVfxPrefab == null || attackPoint == null) return;

        Vector3 pos = attackPoint.position + attackPoint.forward * forwardOffset;
        Quaternion rot = Quaternion.LookRotation(attackPoint.forward, Vector3.up);

        GameObject slash = Instantiate(slashVfxPrefab, pos, rot);

        var sr = slash.GetComponentInChildren<SpriteRenderer>();
        if (sr != null) sr.flipX = flipX;
    }

    public void DoDamage()
    {
        if (attackPoint == null) return;

        Vector3 center = attackPoint.position + attackPoint.forward * attackRangeForward;
        Collider[] hits = Physics.OverlapSphere(center, attackRadius, enemyLayers, QueryTriggerInteraction.Ignore);

        foreach (Collider c in hits)
        {
            if (hitThisSwing.Contains(c)) continue;
            hitThisSwing.Add(c);

            IDamageable dmg = c.GetComponentInParent<IDamageable>();
            if (dmg != null) dmg.TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Vector3 center = attackPoint.position + attackPoint.forward * attackRangeForward;
        Gizmos.DrawWireSphere(center, attackRadius);
    }
}

public interface IDamageable
{
    void TakeDamage(int amount);
}
