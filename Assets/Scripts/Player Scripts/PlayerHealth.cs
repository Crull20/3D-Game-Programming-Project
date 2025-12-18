using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 10;
    public int currentHealth;
    public int heartCount = 5; // always show 5 hearts

    public bool invincible;
    public float iFrameTime = 0.3f;
    bool canTakeDamage = true;

    [Header("Hearts UI (2 HP per heart)")]
    public Transform heartsParent;
    public Image heartPrefab;
    public Sprite heartFull;
    public Sprite heartHalf;
    public Sprite heartEmpty;

    private readonly List<Image> hearts = new List<Image>();
    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        BuildHearts();
        UpdateHearts();
    }

    public void TakeDamage(int amount, Vector3 hitFrom)
    {
        if (invincible || !canTakeDamage) return;

        currentHealth -= amount;
        Debug.Log($"Player took {amount} damage. HP: {currentHealth}");

        UpdateHearts();

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
    }

    void BuildHearts()
    {
        if (heartsParent == null || heartPrefab == null) return;

        for (int i = heartsParent.childCount - 1; i >= 0; i--)
            Destroy(heartsParent.GetChild(i).gameObject);

        hearts.Clear();

        for (int i = 0; i < heartCount; i++)
        {
            Image img = Instantiate(heartPrefab, heartsParent);
            hearts.Add(img);
        }
    }

    void UpdateHearts()
    {
        if (hearts.Count == 0) return;

        int hp = Mathf.Clamp(currentHealth, 0, maxHealth);

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartHp = hp - (i * 2);

            if (heartHp >= 2) hearts[i].sprite = heartFull;
            else if (heartHp == 1) hearts[i].sprite = heartHalf;
            else hearts[i].sprite = heartEmpty;
        }
    }
}
