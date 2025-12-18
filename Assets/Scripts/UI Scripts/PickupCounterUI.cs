using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickupCounter : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI counterText;

    [Header("Goal")]
    public int totalPickups = 5;

    private int current = 0;

    void Start()
    {
        UpdateUI();
    }

    public void AddPickup(int amount = 1)
    {
        current += amount;
        if (current > totalPickups) current = totalPickups;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (counterText != null)
            counterText.text = $"Map Peices {current}/{totalPickups}";
    }
}
