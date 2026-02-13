using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialogueUI;
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    [Header("Typewriter Effect")]
    [SerializeField] private float typeSpeed = 0.05f; // Time between each character
    [SerializeField] private bool playBlipSound = true; // Play sound for each character

    private DialogueLine[] lines;
    private int index;
    private bool isTalking;
    private DialogueData currentDialogue; // Track current dialogue for item handling
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    private HashSet<string> playedDialogues = new HashSet<string>();

    private float dialogueCooldown = 0f;
    private float cooldownDuration = 0.5f; // Adjust this value as needed (in seconds)

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (dialogueCooldown > 0)
        {
            dialogueCooldown -= Time.deltaTime;
        }

        if (!isTalking) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping)
            {
                // Skip typewriter effect and show full text immediately
                SkipTypewriter();
            }
            else
            {
                // Move to next line
                NextLine();
            }
        }
    }

    public void StartDialogue(DialogueData data)
    {
        // Don't start dialogue if cooldown is active
        if (dialogueCooldown > 0)
            return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().canMove = false;
        }
        if (data.playOnce && playedDialogues.Contains(data.dialogueID))
            return;

        isTalking = true;
        dialogueUI.SetActive(true);

        currentDialogue = data; // Store current dialogue

        lines = data.lines;
        index = 0;

        ShowLine();

        if (data.playOnce)
            playedDialogues.Add(data.dialogueID);
    }

    void ShowLine()
    {
        nameText.text = lines[index].speakerName;

        // Stop any existing typing coroutine
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // Start typewriter effect
        typingCoroutine = StartCoroutine(TypewriterEffect(lines[index].text));
    }

    IEnumerator TypewriterEffect(string fullText)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in fullText)
        {
            dialogueText.text += c;

            // Play blip sound for each character (except spaces)
            if (playBlipSound && c != ' ' && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayDialogueBlip();
            }

            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
    }

    void SkipTypewriter()
    {
        // Stop the typing coroutine
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // Show full text immediately
        dialogueText.text = lines[index].text;
        isTyping = false;
    }

    void NextLine()
    {
        index++;
        AudioManager.Instance.PlayButtonClick();

        if (index >= lines.Length)
        {
            AudioManager.Instance.PlayButtonClick();
            RemoveDialogue();
            return;
        }

        ShowLine();
    }

    void RemoveDialogue()
    {
        dialogueUI.SetActive(false);
        isTalking = false;
        dialogueCooldown = cooldownDuration; // Start cooldown timer

        // Handle item-related actions
        if (currentDialogue != null && ItemManager.Instance != null)
        {
            // Unlock item UI if specified
            if (currentDialogue.unlockItemUI)
            {
                ItemManager.Instance.UnlockItemUI();
            }

            // Collect item if specified
            if (currentDialogue.collectItem && currentDialogue.itemIndex >= 0)
            {
                ItemManager.Instance.CollectItem(currentDialogue.itemIndex);
            }
        }

        currentDialogue = null; // Clear current dialogue reference

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().canMove = true;
        }
    }

    public bool IsTalking()
    {
        return isTalking;
    }
} 