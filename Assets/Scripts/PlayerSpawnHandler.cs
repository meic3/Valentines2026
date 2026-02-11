using UnityEngine;
using UnityEngine.SceneManagement;

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

                // 向きもここで設定
                Animator anim = GetComponentInChildren<Animator>();
                anim.SetFloat("MoveX", spawn.facingDirection.x);
                anim.SetFloat("MoveY", spawn.facingDirection.y);

                break;
            }
        }
    }
}
