using UnityEngine;

// Use this instead of DialogueTrigger for the FINAL dialogue that leads to choices
public class FinalDialogueTrigger : MonoBehaviour
{
    [Header("Final Dialogue")]
    public DialogueData dialogue;
    public DialogueChoice choiceSystem; // Reference to the DialogueChoice component
    
    private bool playerInRange;
    private PlayerManager playerManager;
    private bool hasTriggered = false; // Only trigger once

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerManager = player.GetComponent<PlayerManager>();
        }
    }

    void Update()
    {
        if (playerInRange && !DialogueManager.Instance.IsTalking() && !hasTriggered)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
            {
                hasTriggered = true;
                DialogueManager.Instance.StartDialogue(dialogue);
                
                // Tell the choice system to show choices after this dialogue ends
                if (choiceSystem != null)
                {
                    choiceSystem.TriggerChoicesAfterDialogue();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            playerInRange = true;

            if (playerManager != null)
                playerManager.isNotify = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (playerManager != null)
                playerManager.isNotify = false;
        }
    }
}
