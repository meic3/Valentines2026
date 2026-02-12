using UnityEngine;

public class SceneDialogueTrigger : MonoBehaviour
{
    public DialogueData dialogue;
    private bool playerInRange;
    private PlayerManager playerManager;
    public bool onlyOnce = false;

    void Start()
    {
            DialogueManager.Instance.StartDialogue(dialogue);
            if (onlyOnce)
            { Destroy(gameObject); }
    }

}
