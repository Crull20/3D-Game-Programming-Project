using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEventRelay : MonoBehaviour
{
    private EnemyMeleeAttack melee;

    void Awake()
    {
        // find the attack script on the root
        melee = GetComponentInParent<EnemyMeleeAttack>();
    }

    // must match exactly
    public void DoHitNow() => melee?.DoHitNow();
    public void EndAttack() => melee?.EndAttack();
}