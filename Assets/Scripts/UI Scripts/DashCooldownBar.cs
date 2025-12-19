using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashCooldownBar : MonoBehaviour
{
    public PlayerMovement1 player;   // player here
    public Image fillImage;          // drag DashBarFill

    void Awake()
    {
        if (fillImage == null) fillImage = GetComponent<Image>();
    }

    void Update()
    {
        if (player == null || fillImage == null) return;

        // 0 when you JUST dashed, 1 when ready again
        float t = player.GetDashCooldownFill01();
        fillImage.fillAmount = t;
    }
}
