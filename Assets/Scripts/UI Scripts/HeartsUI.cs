using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    [Header("Target")]
    public PlayerHealth player;          // playerHealth here

    [Header("Prefabs / Sprites")]
    public Image heartPrefab;            // heart icon prefab
    public Sprite heartFull;
    public Sprite heartHalf;
    public Sprite heartEmpty;

    List<Image> hearts = new List<Image>();

    int lastMax = -1;
    int lastCur = -1;

    void Start()
    {
        RebuildIfNeeded();
        Refresh();
    }

    void Update()
    {
        // update when value changes
        if (player == null) return;

        if (player.maxHealth != lastMax || player.currentHealth != lastCur)
        {
            RebuildIfNeeded();
            Refresh();
        }
    }

    void RebuildIfNeeded()
    {
        if (player == null || heartPrefab == null) return;

        int neededHearts = Mathf.CeilToInt(player.maxHealth / 2f);

        if (neededHearts == hearts.Count) return;

        // Clear old
        for (int i = transform.childCount - 1; i >= 0; i--)
            Destroy(transform.GetChild(i).gameObject);

        hearts.Clear();

        // Create new
        for (int i = 0; i < neededHearts; i++)
        {
            Image h = Instantiate(heartPrefab, transform);
            hearts.Add(h);
        }
    }

    void Refresh()
    {
        if (player == null) return;

        lastMax = player.maxHealth;
        lastCur = player.currentHealth;

        int hp = Mathf.Clamp(player.currentHealth, 0, player.maxHealth);

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartHp = hp - (i * 2);

            if (heartHp >= 2) hearts[i].sprite = heartFull;
            else if (heartHp == 1) hearts[i].sprite = heartHalf;
            else hearts[i].sprite = heartEmpty;
        }
    }
}
