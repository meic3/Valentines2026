using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EndingSequence : MonoBehaviour
{
    public static EndingSequence Instance;

    [Header("Ending Text Content")]
    [TextArea(3, 10)]
    [SerializeField] private string[] letterLines; // Array of text lines to display

    [Header("UI References")]
    [SerializeField] private GameObject endingCanvas;
    [SerializeField] private Image blackBackground;
    [SerializeField] private TMP_Text letterText;
    [SerializeField] private TMP_Text finalMessageText; // "Happy Valentines"

    [Header("Timing Settings")]
    [SerializeField] private float fadeToBlackDuration = 2f;
    [SerializeField] private float timeBetweenLines = 3f;
    [SerializeField] private float typeSpeed = 0.05f;
    [SerializeField] private float finalMessageDelay = 1f;

    [Header("Final Message Settings")]
    [SerializeField] private string finalMessage = "Happy Valentines ♥";
    [SerializeField] private float finalMessageFadeInDuration = 2f;

    private bool isPlayingEnding = false;
    private bool hasShownFinalMessage = false; // Prevent showing twice

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Hide everything at start
        if (endingCanvas != null)
            endingCanvas.SetActive(false);

        if (letterText != null)
            letterText.text = "";

        if (finalMessageText != null)
        {
            finalMessageText.text = "";
            finalMessageText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // No input handling needed - typewriter plays automatically
    }

    public void StartEnding()
    {
        if (!isPlayingEnding)
        {
            Debug.Log("Starting ending sequence...");
            StartCoroutine(PlayEndingSequence());
        }
        else
        {
            Debug.Log("Ending sequence already playing!");
        }
    }

    // Call this to skip letter lines and go straight to final message
    public void ShowFinalMessageOnly()
    {
        if (!isPlayingEnding)
        {
            StartCoroutine(ShowFinalMessageDirectly());
        }
    }

    IEnumerator PlayEndingSequence()
    {
        isPlayingEnding = true;

        // Hide item UI if it exists
        if (ItemManager.Instance != null)
        {
            ItemManager.Instance.HideItemUI();
        }

        // Activate ending canvas
        if (endingCanvas != null)
            endingCanvas.SetActive(true);

        // Fade to black
        yield return StartCoroutine(FadeToBlack());

        // Display each line of text
        if (letterLines != null && letterLines.Length > 0)
        {
            for (int i = 0; i < letterLines.Length; i++)
            {
                // Clear previous text
                if (letterText != null)
                    letterText.text = "";

                // Type out the line
                yield return StartCoroutine(TypewriterEffect(letterLines[i]));

                // Wait a moment before next line
                yield return new WaitForSeconds(timeBetweenLines);
            }

            // Clear letter text after all lines shown
            if (letterText != null)
                letterText.text = "";
        }

        // Wait a moment before final message
        yield return new WaitForSeconds(finalMessageDelay);

        // Show "Happy Valentines" message
        yield return StartCoroutine(ShowFinalMessage());

        isPlayingEnding = false;
    }

    IEnumerator ShowFinalMessageDirectly()
    {
        isPlayingEnding = true;

        // Hide item UI if it exists
        if (ItemManager.Instance != null)
        {
            ItemManager.Instance.HideItemUI();
        }

        // Activate ending canvas
        if (endingCanvas != null)
            endingCanvas.SetActive(true);

        // Fade to black
        yield return StartCoroutine(FadeToBlack());

        // Wait a moment
        yield return new WaitForSeconds(finalMessageDelay);

        // Show "Happy Valentines" message
        yield return StartCoroutine(ShowFinalMessage());

        isPlayingEnding = false;
    }

    IEnumerator FadeToBlack()
    {
        if (blackBackground == null)
            yield break;

        float elapsed = 0f;
        Color color = blackBackground.color;
        color.a = 0f;
        blackBackground.color = color;

        while (elapsed < fadeToBlackDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsed / fadeToBlackDuration);
            blackBackground.color = color;
            yield return null;
        }

        color.a = 1f;
        blackBackground.color = color;
    }

    IEnumerator TypewriterEffect(string text)
    {
        if (letterText == null)
            yield break;

        letterText.text = "";

        foreach (char c in text)
        {
            letterText.text += c;

            // Play sound for each character (except spaces)
            if (c != ' ' && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayDialogueBlip();
            }

            yield return new WaitForSeconds(typeSpeed);
        }
    }

    IEnumerator ShowFinalMessage()
    {
        if (finalMessageText == null)
            yield break;

        // Prevent showing twice
        if (hasShownFinalMessage)
        {
            Debug.Log("Final message already shown, skipping...");
            yield break;
        }

        hasShownFinalMessage = true;
        Debug.Log("Showing final message...");

        finalMessageText.gameObject.SetActive(true);
        finalMessageText.text = finalMessage;

        // Fade in the final message
        Color color = finalMessageText.color;
        color.a = 0f;
        finalMessageText.color = color;

        float elapsed = 0f;
        while (elapsed < finalMessageFadeInDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsed / finalMessageFadeInDuration);
            finalMessageText.color = color;
            yield return null;
        }

        color.a = 1f;
        finalMessageText.color = color;

        // Optional: Add a subtle scale animation
        //finalMessageText.transform.localScale = Vector3.zero;
        //float scaleTime = 0f;
        //while (scaleTime < 0.5f)
        //{
        //    scaleTime += Time.deltaTime;
        //    float scale = Mathf.Lerp(0f, 1f, scaleTime / 0.5f);
        //    finalMessageText.transform.localScale = Vector3.one * scale;
        //    yield return null;
        //}
        //finalMessageText.transform.localScale = Vector3.one;
    }
}