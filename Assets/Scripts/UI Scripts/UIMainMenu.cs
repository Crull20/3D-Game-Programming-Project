using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIMainMenu : MonoBehaviour
{
    Button PlayButton;
    Button QuitButton;
    VisualElement ButtonContainer;
    VisualElement GameStartFade;

    public float buttonFade = 0.5f;
    public float hold = 0.5f;
    public float shotFadeIn = 0.5f;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        PlayButton = root.Q<Button>("PlayButton");
        QuitButton = root.Q<Button>("QuitButton");
        ButtonContainer = root.Q<VisualElement>("ButtonContainer");
        GameStartFade = root.Q<VisualElement>("GameStartFade");

        PlayButton.clicked += () => StartCoroutine(PlaySequence());
        QuitButton.clicked += QuitGame;
    }

    private IEnumerator PlaySequence()
    {
        yield return Fade(ButtonContainer, 1f, 0f, buttonFade);   // A
        yield return new WaitForSeconds(hold);                    // B
        yield return Fade(GameStartFade, 0f, 1f, shotFadeIn);          // C

        // Now you're fully covered by the screenshot, so the cut is hidden:
        SceneManager.LoadScene("Level1");
    }

    private IEnumerator Fade(VisualElement el, float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            el.style.opacity = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        el.style.opacity = to;
    }


    public void QuitGame()
    {
        //exits the game to desktop
        Application.Quit();

        //FOR IN UNITY LAUNCHER, just exits the "game" mode becasue the other quit code only works on .exe game files.
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
