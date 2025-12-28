using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement23 : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Air Control")]
    [Range(0f, 1f)]
    public float airControlMultiplier = 0.35f;

    [Header("Jump Cut")]
    [Range(0f, 1f)]
    public float jumpCutMultiplier = 0.5f;

    [Header("Coyote Time")]
    public float coyoteTime = 0.1f;

    [Header("Jump Buffer")]
    public float jumpBufferTime = 0.1f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Cutscene Control")]
    public bool canMove = true;

    private Rigidbody2D rb;
    private Animator anim;

    private bool isGrounded;
    private float moveInput;

    private float coyoteTimer;
    private float jumpBufferTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!canMove)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            anim.SetBool("isRun", false);
            return;
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
        if (Input.GetKeyDown(KeyCode.J))
        {
            anim.SetTrigger("attack");
        }

        // RUN animasyonu
        anim.SetBool("isRun", moveInput != 0 && isGrounded);

        // Yön çevirme
        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // JUMP BUFFER
        if (Input.GetKeyDown(KeyCode.Space))
            jumpBufferTimer = jumpBufferTime;

        // COYOTE TIME
        if (isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;

        if (jumpBufferTimer > 0)
            jumpBufferTimer -= Time.deltaTime;

        // ZIPLAMA
        if (jumpBufferTimer > 0f && coyoteTimer > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }

        // JUMP CUT
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(
                rb.velocity.x,
                rb.velocity.y * jumpCutMultiplier
            );
        }

        anim.SetBool("isJump", !isGrounded);
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        float controlMultiplier = isGrounded ? 1f : airControlMultiplier;

        rb.velocity = new Vector2(
            moveInput * moveSpeed * controlMultiplier,
            rb.velocity.y
        );
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
