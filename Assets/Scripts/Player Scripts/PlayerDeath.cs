using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] string deathSceneName = "DeathScene";

    PlayerHealth playerHealth;
    bool hasDied = false;

    void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (hasDied) return;

        if (playerHealth.currentHealth <= 0)
        {
            hasDied = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            SceneManager.LoadScene(deathSceneName);
        }
    }
}
