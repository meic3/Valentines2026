using UnityEngine;

public class FinalDialogue : MonoBehaviour
{
    public DialogueData dialogue;
    private bool playerInRange;
    private PlayerManager playerManager;
    public bool onlyOnce = false;
    [SerializeField]
    GameObject beforeEnd;
    [SerializeField]
    GameObject afterEnd;

    void Awake()
    {
        beforeEnd.SetActive(true);
        afterEnd.SetActive(false);
    }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    bool allItemsCollected = ItemManager.Instance != null && ItemManager.Instance.AllItemsCollected();

        if (allItemsCollected)
        {
            Destroy(beforeEnd);
            afterEnd.SetActive(true);
            DialogueManager.Instance.StartDialogue(dialogue);
            if (onlyOnce)
            { Destroy(gameObject); }
        }
    }
    
}
