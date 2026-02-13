using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueChoice : MonoBehaviour
{
    [Header("Choice Settings")]
    [SerializeField] private string choice1Text = "Yes";
    [SerializeField] private string choice2Text = "Yes";

    [Header("UI References")]
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private Button choice1Button;
    [SerializeField] private Button choice2Button;
    [SerializeField] private TMP_Text choice1Label;
    [SerializeField] private TMP_Text choice2Label;

    private bool shouldShowChoices = false;

    void Start()
    {
        // Hide choice panel at start
        if (choicePanel != null)
            choicePanel.SetActive(false);

        // Set up button listeners
        if (choice1Button != null)
            choice1Button.onClick.AddListener(() => OnChoiceSelected(1));

        if (choice2Button != null)
            choice2Button.onClick.AddListener(() => OnChoiceSelected(2));

        // Set button labels
        if (choice1Label != null)
            choice1Label.text = choice1Text;

        if (choice2Label != null)
            choice2Label.text = choice2Text;
    }

    void Update()
    {
        // Check if dialogue just ended and we should show choices
        if (shouldShowChoices && DialogueManager.Instance != null && !DialogueManager.Instance.IsTalking())
        {
            ShowChoices();
            shouldShowChoices = false;
        }
    }

    // Call this after starting the final dialogue
    public void TriggerChoicesAfterDialogue()
    {
        shouldShowChoices = true;
    }

    void ShowChoices()
    {
        if (choicePanel != null)
        {
            choicePanel.SetActive(true);

            // Disable player movement
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerMovement pm = player.GetComponent<PlayerMovement>();
                if (pm != null)
                    pm.canMove = false;
            }

            // Play sound if available
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayButtonClick();
        }
    }

    void OnChoiceSelected(int choiceNumber)
    {
        // Hide choice panel
        if (choicePanel != null)
            choicePanel.SetActive(false);

        // Play sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();

        // Both choices lead to the ending - start the full ending sequence
        if (EndingSequence.Instance != null)
        {
            EndingSequence.Instance.StartEnding();
        }
    }
}