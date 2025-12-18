using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 100;
    public int currentHealth;

    public bool invincible;
    public float iFrameTime = 0.3f;

    bool canTakeDamage = true;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount, Vector3 hitFrom)
    {
        if (invincible || !canTakeDamage) return;

        currentHealth -= amount;
        Debug.Log($"Player took {amount} damage. HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        // invincibility frames so player doesn't die instantly
        StartCoroutine(DamageCooldown());
    }

    System.Collections.IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(iFrameTime);
        canTakeDamage = true;
    }

    void Die()
    {
        Debug.Log("Player died!");
        // TODO: play animation, disable movement, reload scene, etc.
        // Example:
        // GetComponent<PlayerMovement1>().enabled = false;
    }
}
