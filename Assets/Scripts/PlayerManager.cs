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
        if (isNotify)
        {
            exclamation.SetActive(true);
        }
        else
        {
            exclamation.SetActive(false);
        }
    }
}
