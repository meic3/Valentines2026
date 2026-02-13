using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public bool canMove = true;

    [Header("Footstep Audio")]
    [SerializeField] private AudioSource footstepAudioSource; // Dedicated audio source for footsteps
    [SerializeField] private AudioClip footstepSound; // Single footstep sound
    [SerializeField] private float footstepInterval = 0.4f; // Time between footsteps

    Rigidbody2D rb;
    Animator anim;
    Vector2 move;

    private bool isPlayingFootsteps = false;
    private float footstepTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        // Set up footstep audio source if not assigned
        if (footstepAudioSource == null)
        {
            footstepAudioSource = gameObject.AddComponent<AudioSource>();
            footstepAudioSource.playOnAwake = false;
            footstepAudioSource.volume = 0.3f; // Adjust volume as needed
        }
    }

    void Update()
    {
        if (!canMove)
        {
            StopFootsteps();
            return;
        }

        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        bool isMoving = move != Vector2.zero;
        anim.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            anim.SetFloat("MoveZ", move.y);
            anim.SetFloat("MoveX", move.x);

            // Play footsteps
            PlayFootsteps();
        }
        else
        {
            // Stop footsteps when not moving
            StopFootsteps();
        }
    }

    void PlayFootsteps()
    {
        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0f)
        {
            // Play footstep sound
            if (footstepSound != null && footstepAudioSource != null)
            {
                footstepAudioSource.PlayOneShot(footstepSound);
            }
            else if (AudioManager.Instance != null)
            {
                // Fallback to AudioManager if no custom sound assigned
                AudioManager.Instance.PlayFootstep();
            }

            // Reset timer
            footstepTimer = footstepInterval;
            isPlayingFootsteps = true;
        }
    }

    void StopFootsteps()
    {
        isPlayingFootsteps = false;
        footstepTimer = 0f;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
    }

    // Call this to reset movement input (e.g., after scene change)
    public void ResetInput()
    {
        move = Vector2.zero;

        // Make sure rb is initialized
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        // Reset animator to stop walking animation
        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }

        if (anim != null)
        {
            anim.SetBool("IsMoving", false);
        }

        // Stop footsteps
        StopFootsteps();

        // Disable movement (will be re-enabled after delay in PlayerSpawnHandler)
        canMove = false;
    }
}