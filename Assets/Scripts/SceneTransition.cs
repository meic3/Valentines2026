using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;

    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private Color fadeColor = Color.black;

    private bool isFading = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Set up fade image
            if (fadeImage != null)
            {
                fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0);
                fadeImage.raycastTarget = false; // Don't block clicks when transparent
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Fade in when new scene loads
        StartCoroutine(FadeIn());
    }

    // Load scene with fade transition
    public void LoadSceneWithFade(string sceneName, string spawnID = "")
    {
        if (!isFading)
        {
            StartCoroutine(FadeOutAndLoadScene(sceneName, spawnID));
        }
    }

    // Fade out, then load scene
    IEnumerator FadeOutAndLoadScene(string sceneName, string spawnID)
    {
        isFading = true;

        // Set spawn ID if provided
        if (!string.IsNullOrEmpty(spawnID) && GameManager.instance != null)
        {
            GameManager.instance.nextSpawnID = spawnID;
        }

        // Fade out
        yield return StartCoroutine(Fade(0, 1));

        // Load scene
        SceneManager.LoadScene(sceneName);

        // Note: FadeIn will be called automatically by OnSceneLoaded
        isFading = false;
    }

    // Fade in (from black to clear)
    IEnumerator FadeIn()
    {
        if (fadeImage == null)
            yield break;

        yield return StartCoroutine(Fade(1, 0));
    }

    // Fade out (from clear to black)
    IEnumerator FadeOut()
    {
        if (fadeImage == null)
            yield break;

        yield return StartCoroutine(Fade(0, 1));
    }

    // Core fade function
    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        if (fadeImage == null)
            yield break;

        float elapsedTime = 0f;
        Color color = fadeColor;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            color.a = alpha;
            fadeImage.color = color;

            // Block raycasts when fading
            fadeImage.raycastTarget = alpha > 0.01f;

            yield return null;
        }

        // Ensure final alpha
        color.a = endAlpha;
        fadeImage.color = color;
        fadeImage.raycastTarget = endAlpha > 0.01f;
    }
}