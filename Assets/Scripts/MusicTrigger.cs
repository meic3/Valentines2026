using UnityEngine;

// Add this to an object in your first scene to start background music
public class MusicTrigger : MonoBehaviour
{
    [SerializeField] private int musicTrackIndex = 0; // Which music track to play
    [SerializeField] private bool playOnStart = true;

    void Start()
    {
        if (playOnStart && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(musicTrackIndex);
        }
    }

    // Call this method to change music (e.g., from a trigger or event)
    public void ChangeMusic(int newTrackIndex)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(newTrackIndex);
        }
    }
}
