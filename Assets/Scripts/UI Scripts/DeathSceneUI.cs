using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DeathScreenUI : MonoBehaviour
{
    [Header("Scene name to load for Main Menu")]
    [SerializeField] private string mainMenuSceneName = "UI & Main Menu";
    [SerializeField] private string Level1Name = "Level1";

    [Header("Optional: if you want to show/hide the whole overlay by name")]
    [SerializeField] private string overlayRootName = "VisualElement"; // or set to the container element name you use

    private UIDocument _doc;
    private VisualElement _root;

    private Button _retryButton;
    private Button _mainMenuButton;
    private Button _quitButton;

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        _root = _doc.rootVisualElement;

        // These names must match your UXML element "name" fields:
        _retryButton = _root.Q<Button>("RetryButton");
        _mainMenuButton = _root.Q<Button>("MainMenuButton");
        _quitButton = _root.Q<Button>("QuitButton");

        if (_retryButton == null || _mainMenuButton == null || _quitButton == null)
        {
            Debug.LogError("DeathScreenUI: One or more buttons not found. Check the UXML 'name' fields: RetryButton, MainMenuButton, QuitButton.");
            return;
        }

        _retryButton.clicked += OnRetryClicked;
        _mainMenuButton.clicked += OnMainMenuClicked;
        _quitButton.clicked += OnQuitClicked;
    }

    private void OnDestroy()
    {
        if (_retryButton != null) _retryButton.clicked -= OnRetryClicked;
        if (_mainMenuButton != null) _mainMenuButton.clicked -= OnMainMenuClicked;
        if (_quitButton != null) _quitButton.clicked -= OnQuitClicked;
    }

    private void OnRetryClicked()
    {
        SceneManager.LoadScene(Level1Name);
    }

    private void OnMainMenuClicked()
    {
        if (string.IsNullOrWhiteSpace(mainMenuSceneName))
        {
            Debug.LogError("DeathScreenUI: mainMenuSceneName is empty.");
            return;
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void OnQuitClicked()
    {
        Application.Quit();

#if UNITY_EDITOR
        // So Quit works in the editor too:
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // Optional helpers if you want to toggle the overlay on death:
    public void ShowOverlay(bool show)
    {
        var overlay = _root.Q<VisualElement>(overlayRootName) ?? _root;
        overlay.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;

        if (show) _retryButton?.Focus();
    }
}
