using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class WinScreenUI : MonoBehaviour
{
    [SerializeField] private string Level1Name = "Level1";

    [Header("Optional: if you want to show/hide the whole overlay by name")]
    [SerializeField] private string overlayRootName = "VisualElement"; // or set to the container element name you use

    private UIDocument _doc;
    private VisualElement _root;

    private Button _retryButton;
    private Button _quitButton;

    private void Awake()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        _doc = GetComponent<UIDocument>();
        _root = _doc.rootVisualElement;

        // These names must match your UXML element "name" fields:
        _retryButton = _root.Q<Button>("RetryButton");
        _quitButton = _root.Q<Button>("QuitButton");

        if (_retryButton == null || _quitButton == null)
        {
            Debug.LogError("DeathScreenUI: One or more buttons not found. Check the UXML 'name' fields: RetryButton, MainMenuButton, QuitButton.");
            return;
        }

        _retryButton.clicked += OnRetryClicked;
        _quitButton.clicked += OnQuitClicked;
    }

    private void OnDestroy()
    {
        if (_retryButton != null) _retryButton.clicked -= OnRetryClicked;
        if (_quitButton != null) _quitButton.clicked -= OnQuitClicked;
    }

    private void OnRetryClicked()
    {
        SceneManager.LoadScene(Level1Name);
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
