using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    EnemyAI ai;

    [SerializeField] private float attackRadius = 1.5f;
    [SerializeField] private int damage = 2;
    [SerializeField] private float attackCooldown = 1.0f;

    private float nextAttackTime;
    private bool isAttacking;

    public LayerMask playerLayer;

    private void Awake()
    {
        ai = GetComponent<EnemyAI>();
        nextAttackTime = 0f;
    }

    void Update()
    {
        if (ai == null || ai.target == null) return;              // uses EnemyAI target :contentReference[oaicite:1]{index=1}
        if (isAttacking) return;
        if (Time.time < nextAttackTime) return;

        float dist = Vector3.Distance(transform.position, ai.target.position);
        if (dist > attackRadius) return;

        // Start attack: play animation, lock state, cooldown starts now (or later, your choice)
        isAttacking = true;
        nextAttackTime = Time.time + attackCooldown;

        // Trigger the animation (via your EnemyAnimDriver from earlier)
        var anim = GetComponent<EnemyAnimDriver>();
        anim?.PlayAttack();
    }

    // Called by an Animation Event at the exact frame you want the hit to occur
    public void DoHitNow()
    {
        if (ai == null || ai.target == null) return;

        // flat distance check (optional but nice)
        Vector3 a = transform.position; a.y = 0f;
        Vector3 b = ai.target.position; b.y = 0f;
        if (Vector3.Distance(a, b) > attackRadius) return;

        Vector3 center = ai.target.position;

        Collider[] hits = Physics.OverlapSphere(
            center,
            attackRadius,
            playerLayer,
            QueryTriggerInteraction.Collide
        );

        foreach (var c in hits)
        {
            IDamageable dmg = c.GetComponentInParent<IDamageable>();
            if (dmg == null) continue;

            dmg.TakeDamage(damage);
            return; // only hit once
        }
    }

    // Called by an Animation Event near the end of the attack animation
    public void EndAttack()
    {
        isAttacking = false;
    }
}
