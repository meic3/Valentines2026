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

    private HashSet<string> playedDialogues = new HashSet<string>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (!isTalking) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
        {
            NextLine();
        }
    }

    public void StartDialogue(DialogueData data)
    {
        if (data.playOnce && playedDialogues.Contains(data.dialogueID))
            return;

        isTalking = true;
        dialogueUI.SetActive(true);

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
        if (!isTalking) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
        {
            isTalking = false;
        }    
    }

    public bool IsTalking()
    {
        return isTalking;
    }
}
