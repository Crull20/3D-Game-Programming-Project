using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 100;
    private int currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{name} took {amount} damage. HP: {currentHealth}");

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        // replace with death animation later
        Destroy(gameObject);
    }
}
