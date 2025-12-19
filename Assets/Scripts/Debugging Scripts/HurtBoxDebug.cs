using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBoxDebug : MonoBehaviour
{
    void Update()
    {
        var hits = Physics.OverlapSphere(transform.position, 0.25f, ~0, QueryTriggerInteraction.Collide);
        bool foundSelf = false;
        foreach (var h in hits)
            if (h.transform == transform) foundSelf = true;

        if (!foundSelf)
            Debug.LogWarning("[HurtboxDebug] Hurtbox collider is NOT being detected by physics overlap!");
    }
}
