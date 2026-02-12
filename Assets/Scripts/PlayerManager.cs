using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    public GameObject exclamation;
    public bool isNotify = false;
    void Awake()
    {
        if (FindObjectsOfType<PlayerMovement>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // Hide exclamation if dialogue is active, otherwise show based on isNotify
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsTalking())
        {
            exclamation.SetActive(false);
        }
        else
        {
            exclamation.SetActive(isNotify);
        }
    }
}