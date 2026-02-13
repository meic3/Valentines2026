using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music Clips")]
    [SerializeField] private AudioClip[] musicTracks;

    [Header("SFX Clips")]
    [SerializeField] private AudioClip dialogueBlip;
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip itemCollect;
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip footstep;

    [Header("Settings")]
    [SerializeField] private float musicVolume = 0.5f;
    [SerializeField] private float sfxVolume = 0.7f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Set volumes
            if (musicSource != null)
                musicSource.volume = musicVolume;
            if (sfxSource != null)
                sfxSource.volume = sfxVolume;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Play background music by index
    public void PlayMusic(int trackIndex)
    {
        if (musicSource == null || musicTracks == null || trackIndex < 0 || trackIndex >= musicTracks.Length)
            return;

        if (musicSource.clip == musicTracks[trackIndex] && musicSource.isPlaying)
            return; // Already playing this track

        musicSource.clip = musicTracks[trackIndex];
        musicSource.loop = true;
        musicSource.Play();
    }

    // Play background music by clip
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null)
            return;

        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    // Stop music
    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    // Pause music
    public void PauseMusic()
    {
        if (musicSource != null)
            musicSource.Pause();
    }

    // Resume music
    public void ResumeMusic()
    {
        if (musicSource != null)
            musicSource.UnPause();
    }

    // Play a sound effect
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
            sfxSource.PlayOneShot(clip);
    }

    // Quick access methods for common SFX
    public void PlayDialogueBlip()
    {
        if (dialogueBlip != null)
            PlaySFX(dialogueBlip);
    }

    public void PlayButtonClick()
    {
        if (buttonClick != null)
            PlaySFX(buttonClick);
    }

    public void PlayItemCollect()
    {
        if (itemCollect != null)
            PlaySFX(itemCollect);
    }

    public void PlayDoorOpen()
    {
        if (doorOpen != null)
            PlaySFX(doorOpen);
    }

    public void PlayFootstep()
    {
        if (footstep != null)
            PlaySFX(footstep);
    }

    // Set music volume (0 to 1)
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
            musicSource.volume = musicVolume;
    }

    // Set SFX volume (0 to 1)
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
            sfxSource.volume = sfxVolume;
    }
}