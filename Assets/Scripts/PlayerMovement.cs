using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public bool canMove = true;

    Rigidbody2D rb;
    Animator anim;
    Vector2 move;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

    }

    void Update()
    {
        if (!canMove) return;

        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        bool isMoving = move != Vector2.zero;
        anim.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            anim.SetFloat("MoveZ", move.y);
            anim.SetFloat("MoveX", move.x);
        }
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

        // Disable movement (will be re-enabled after delay in PlayerSpawnHandler)
        canMove = false;
    }
}