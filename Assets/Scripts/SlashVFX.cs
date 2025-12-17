using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashVFX : MonoBehaviour
{
    public float lifetime = 0.35f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
