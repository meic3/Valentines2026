using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerSpawnHandler : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SpawnPoint[] spawns = FindObjectsOfType<SpawnPoint>();

        string targetID = GameManager.instance.nextSpawnID;
        if (string.IsNullOrEmpty(targetID))
        {
            targetID = "Initial";
        }

        foreach (SpawnPoint spawn in spawns)
        {
            if (spawn.spawnID == targetID)
            {
                transform.position = spawn.transform.position;

                // Set facing direction
                Animator anim = GetComponentInChildren<Animator>();
                anim.SetFloat("MoveX", spawn.facingDirection.x);
                anim.SetFloat("MoveY", spawn.facingDirection.y);

                break;
            }
        }

        // Reset input so player must press keys again to move
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.ResetInput();
            // Start coroutine to enable movement after delay
            StartCoroutine(EnableMovementAfterDelay(playerMovement));
        }
    }

    IEnumerator EnableMovementAfterDelay(PlayerMovement playerMovement)
    {
        // Wait 0.5 seconds
        yield return new WaitForSeconds(0.5f);

        // Only enable movement if no dialogue is happening
        if (DialogueManager.Instance == null || !DialogueManager.Instance.IsTalking())
        {
            playerMovement.canMove = true;
        }
    }
}