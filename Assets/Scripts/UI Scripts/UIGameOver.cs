using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("Drag these from your Hierarchy")]
    public GameObject Background;      // your death screen panel
    public GameObject Crosshair;       // optional
    public GameObject PickupCounter;   // optional
    public GameObject GameOverText;

    [Header("Scene Names")]
    public string MainMenuSceneName = "MainMenu";

    private bool isGameOver = false;


    public void DebugClick()
    {
        Debug.Log("Button click received!");
    }



    void Start()
    {
        if (Background != null) Background.SetActive(false);
    }

    void Update()
    {
        // TEMP: press L to test death screen
        if (!isGameOver && Input.GetKeyDown(KeyCode.L))
        {
            ShowGameOver();
        }
    }

    public void ShowGameOver()
    {
        isGameOver = true;

        if (Background != null) Background.SetActive(true);
        if (Crosshair != null) Crosshair.SetActive(false);
        if (GameOverText != null) GameOverText.SetActive(false);

        // Pause
        Time.timeScale = 0f;

        // Unlock cursor so you can click buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // --- BUTTON FUNCTIONS ---

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenuSceneName);
    }

    public void Quit()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

