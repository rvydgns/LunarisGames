using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Level5PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float acceleration = 60f;
    [SerializeField] private float deceleration = 80f;
    [SerializeField] private float maxFallSpeed = 25f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float coyoteTime = 0.10f;
    [SerializeField] private float jumpBuffer = 0.10f;
    [SerializeField] private float fallMultiplier = 2.2f;
    [SerializeField] private float jumpCutMultiplier = 0.55f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.18f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Refs")]
    [SerializeField] private Animator animator;

    [Header("Visual (Rig)")]
    [Tooltip("Sürükle-bırak: Wizard 1 altındaki 'Skeletal' objesi. Flip için bunu kullanacağız.")]
    [SerializeField] private Transform visualRoot;

    private Rigidbody2D rb;
    private float moveInput;

    private bool isGrounded;
    private float coyoteTimer;
    private float jumpBufferTimer;
    private bool jumpHeld;

    private float baseVisualScaleX = 1f;

    // Animator params (SENDEKİ İSİMLERLE AYNI)
    private static readonly int AnimIsRun = Animator.StringToHash("isRun");
    private static readonly int AnimIsJump = Animator.StringToHash("isJump");
    private static readonly int AnimIsLookUp = Animator.StringToHash("isLookUp");
    private static readonly int AnimAttack = Animator.StringToHash("attack");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();

        // Rig kökü: Skeletal
        if (visualRoot == null)
        {
            Transform t = transform.Find("Skeletal");
            visualRoot = (t != null) ? t : transform;
        }

        baseVisualScaleX = Mathf.Abs(visualRoot.localScale.x);
        if (baseVisualScaleX < 0.0001f) baseVisualScaleX = 1f;
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump")) jumpBufferTimer = jumpBuffer;
        else jumpBufferTimer -= Time.deltaTime;

        jumpHeld = Input.GetButton("Jump");

        isGrounded = CheckGrounded();
        if (isGrounded) coyoteTimer = coyoteTime;
        else coyoteTimer -= Time.deltaTime;

        if (jumpBufferTimer > 0f && coyoteTimer > 0f)
        {
            DoJump();
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }

        // Attack input (J)
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (animator != null)
                animator.SetTrigger(AnimAttack);
        }

        UpdateVisuals();
    }

    private void FixedUpdate()
    {
        ApplyHorizontalMovement();
        ApplyBetterJumpPhysics();
        ClampFallSpeed();
    }

    private void ApplyHorizontalMovement()
    {
        float targetSpeed = moveInput * moveSpeed;
        float speedDiff = targetSpeed - rb.velocity.x;

        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = accelRate * speedDiff;

        rb.AddForce(new Vector2(movement, 0f), ForceMode2D.Force);
    }

    private void DoJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void ApplyBetterJumpPhysics()
    {
        if (rb.velocity.y < 0f)
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;

        if (!jumpHeld && rb.velocity.y > 0f)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutMultiplier);
    }

    private void ClampFallSpeed()
    {
        if (rb.velocity.y < -maxFallSpeed)
            rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);
    }

    private bool CheckGrounded()
    {
        if (groundCheck == null) return false;
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void UpdateVisuals()
    {
        // Flip: SpriteRenderer değil, rig kökünü (Skeletal) çevir
        if (visualRoot != null)
        {
            if (moveInput > 0.01f)
                visualRoot.localScale = new Vector3(baseVisualScaleX, visualRoot.localScale.y, visualRoot.localScale.z);
            else if (moveInput < -0.01f)
                visualRoot.localScale = new Vector3(-baseVisualScaleX, visualRoot.localScale.y, visualRoot.localScale.z);
        }

        // Animator
        if (animator != null)
        {
            bool run = Mathf.Abs(moveInput) > 0.01f;
            bool jump = !isGrounded;

            animator.SetBool(AnimIsRun, run);
            animator.SetBool(AnimIsJump, jump);

            bool lookUp = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            animator.SetBool(AnimIsLookUp, lookUp);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
