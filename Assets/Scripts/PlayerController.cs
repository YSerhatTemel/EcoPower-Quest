using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Movement speed of the character")]
    public float moveSpeed = 5f;
    
    [Tooltip("Jump force applied to the character")]
    public float jumpForce = 12f;

    [Header("Ground Detection")]
    [Tooltip("Empty GameObject placed at the character's feet to detect ground")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Animation")]
    public Sprite idleSprite;
    public Sprite[] walkSprites;
    public float animationSpeed = 0.15f;
    private SpriteRenderer spriteRenderer;
    private float animationTimer;
    private int currentWalkFrame = 0;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float horizontalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Increase gravity scale for a better jump feel
        if (rb.gravityScale < 2f)
        {
            rb.gravityScale = 2.5f;
        }
    }

    void Update()
    {
        bool wasGrounded = isGrounded;

        // Ground detection using Physics2D.OverlapCircle
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
        else
        {
            isGrounded = true; 
            Debug.LogWarning("GroundCheck is not assigned!");
        }

        // Horizontal Movement Input (ONLY TAB AND BACKSPACE)
        horizontalInput = 0f;

        if (Input.GetKey(KeyCode.Tab))
        {
            horizontalInput = -1f; // Move Left
        }
        else if (Input.GetKey(KeyCode.Backspace))
        {
            horizontalInput = 1f; // Move Right
        }

        // Jump Input (ONLY ENTER/RETURN)
        if (Input.GetKeyDown(KeyCode.Return) && isGrounded)
        {
            Jump();
        }

        // --- Animation Logic ---
        if (spriteRenderer != null)
        {
            if (horizontalInput != 0 && isGrounded)
            {
                // Walking
                animationTimer += Time.deltaTime;
                if (animationTimer >= animationSpeed)
                {
                    animationTimer = 0f;
                    if (walkSprites != null && walkSprites.Length > 0)
                    {
                        currentWalkFrame = (currentWalkFrame + 1) % walkSprites.Length;
                        spriteRenderer.sprite = walkSprites[currentWalkFrame];
                    }
                }
            }
            else
            {
                // Idle or Jumping
                if (idleSprite != null) spriteRenderer.sprite = idleSprite;
                currentWalkFrame = 0;
                animationTimer = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        // Apply horizontal movement using Rigidbody2D velocity
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // Flip character sprite based on movement direction
        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void Jump()
    {
        // Reset vertical velocity before jumping for consistent jump height
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        // Play Sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayJump();
        }
    }

    // Draws the GroundCheck circle in the Unity Editor when selected
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
