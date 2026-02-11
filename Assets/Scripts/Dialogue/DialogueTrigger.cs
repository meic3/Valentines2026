using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueData dialogue;
    private bool playerInRange;
    private PlayerManager playerManager;
    public bool onlyOnce = false;

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
        if (playerInRange && !DialogueManager.Instance.IsTalking())
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
            {
                
                DialogueManager.Instance.StartDialogue(dialogue);
                if (onlyOnce)
                { Destroy(gameObject); }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            playerManager.isNotify = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            playerManager.isNotify = false;
        }
    }
}
