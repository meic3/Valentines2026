using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject textUI;
    [SerializeField]
    GameObject itemUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (FindObjectsOfType<UIManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        textUI.SetActive(false);
        itemUI.SetActive(false);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
