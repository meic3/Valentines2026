using UnityEngine;
using UnityEngine.SceneManagement;

public class ConditionalDoor : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField]
    private string nextSceneName;
    [SerializeField]
    private string spawnID;

    [Header("Dialogue Before Items Collected")]
    [SerializeField]
    private DialogueData lockedDialogue; // Dialogue to show when door is locked

    private bool canEnter = false;
    private bool playerInRange = false;
    private PlayerManager playerManager;

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
        if (!playerInRange) return;

        // Check if all items are collected
        bool allItemsCollected = ItemManager.Instance != null && ItemManager.Instance.AllItemsCollected();

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
        {
            if (allItemsCollected)
            {
                // All items collected - act as a door
                playerManager.isNotify = false;
                GameManager.instance.nextSpawnID = spawnID;
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                // Items not collected - show dialogue
                if (lockedDialogue != null && DialogueManager.Instance != null)
                {
                    DialogueManager.Instance.StartDialogue(lockedDialogue);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (playerManager != null)
            {
                playerManager.isNotify = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (playerManager != null)
            {
                playerManager.isNotify = false;
            }
        }
    }
}