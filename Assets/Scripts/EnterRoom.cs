using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterRoom: MonoBehaviour
{
    [SerializeField]
    public string nextSceneName;
    private bool canEnter = false;
    [SerializeField]
    public bool requireInteraction = false;

    void Update()
    {
        if (requireInteraction && canEnter && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (requireInteraction && other.CompareTag("Player"))
        {
            canEnter = true;
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canEnter = false;
        }
    }

}
