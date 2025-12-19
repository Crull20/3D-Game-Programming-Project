using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsProbe : MonoBehaviour
{
    void Update()
    {
        // Probe around the hurtbox itself
        var hits = Physics.OverlapSphere(transform.position, 0.5f, ~0, QueryTriggerInteraction.Collide);

        Debug.Log($"[PhysicsProbe] at {transform.position} hits={hits.Length}");

        foreach (var h in hits)
            Debug.Log($"   -> {h.name} layer={LayerMask.LayerToName(h.gameObject.layer)} trigger={h.isTrigger} enabled={h.enabled}");
    }
}
