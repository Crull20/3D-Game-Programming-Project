using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPickup : MonoBehaviour
{
    public PickupCounter counter; // drag GameManager here
    public string playerTag = "Player";

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;
        if (!other.CompareTag(playerTag)) return;

        collected = true;

        if (counter != null)
            counter.AddPickup(1);

        Destroy(gameObject);
    }
}
