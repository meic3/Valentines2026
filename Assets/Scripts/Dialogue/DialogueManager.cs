using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialogueUI;
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    private DialogueLine[] lines;
    private int index;
    private bool isTalking;
    private DialogueData currentDialogue; // Track current dialogue for item handling

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
            NextLine();
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
        dialogueText.text = lines[index].text;
    }

    void NextLine()
    {
        index++;

        if (index >= lines.Length)
        {
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