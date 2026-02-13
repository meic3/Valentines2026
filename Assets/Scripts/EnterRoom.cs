using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterRoom : MonoBehaviour
{
    [SerializeField]
    public string nextSceneName;
    private bool canEnter = false;
    [SerializeField]
    public bool requireInteraction = false;
    [SerializeField]
    public string spawnID;
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
        if (requireInteraction && canEnter && Input.GetKeyDown(KeyCode.Space))
        {
            playerManager.isNotify = false;

            // Use SceneTransition for fade effect
            if (SceneTransition.Instance != null)
            {
                SceneTransition.Instance.LoadSceneWithFade(nextSceneName, spawnID);
            }
            else
            {
                // Fallback to direct scene load
                GameManager.instance.nextSpawnID = spawnID;
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (requireInteraction && other.CompareTag("Player"))
        {
            playerManager.isNotify = true;
            canEnter = true;
        }
        else if (other.CompareTag("Player"))
        {
            // Use SceneTransition for fade effect
            if (SceneTransition.Instance != null)
            {
                SceneTransition.Instance.LoadSceneWithFade(nextSceneName, spawnID);
            }
            else
            {
                // Fallback to direct scene load
                GameManager.instance.nextSpawnID = spawnID;
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerManager.isNotify = false;
            canEnter = false;

        }
    }

}