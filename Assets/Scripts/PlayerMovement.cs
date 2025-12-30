using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public bool canMove = true;

    [Header("Jump")]
    public float jumpForce = 12f;

    [Header("Coyote Time")]
    public float coyoteTime = 0.1f;

    [Header("Jump Buffer")]
    public float jumpBufferTime = 0.1f;

    [Header("Jump Cut")]
    [Range(0f, 1f)]
    public float jumpCutMultiplier = 0.5f;

    [Header("Air Control")]
    [Range(0f, 1f)]
    public float airControlMultiplier = 0.35f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Combat")]
    public float attackDuration = 0.4f;

    [Header("Speed Boost")]
    public bool canSpeedBoost = false; 
    public float speedMultiplier = 1.5f;
    public KeyCode boostKey = KeyCode.LeftShift;

    private float defaultMoveSpeed;

    private Rigidbody2D rb;
    private Animator anim;

    private float moveInput;
    private bool isGrounded;

    private float coyoteTimer;
    private float jumpBufferTimer;

    private bool isFrozen;
    private bool isAttacking;
    private bool isDead;

    private Vector3 originalScale;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        originalScale = transform.localScale;
        defaultMoveSpeed = moveSpeed;
    }

    void Update()
    {
        if (isDead) return;

        
        moveInput = Input.GetAxisRaw("Horizontal");

        bool jumpPressed = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        bool jumpReleased = Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow);
        bool attackPressed = Input.GetKeyDown(KeyCode.J);

        
        if (canSpeedBoost && Input.GetKey(boostKey))
            moveSpeed = defaultMoveSpeed * speedMultiplier;
        else
            moveSpeed = defaultMoveSpeed;

        
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        
        if (moveInput > 0.01f)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
        }
        else if (moveInput < -0.01f)
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
        }

        
        anim.SetBool("isRun", Mathf.Abs(moveInput) > 0.01f && isGrounded);
        anim.SetBool("isJump", !isGrounded);

        
        if (isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;

        
        if (jumpPressed)
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.deltaTime;

        
        if (jumpBufferTimer > 0 && coyoteTimer > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferTimer = 0;
            coyoteTimer = 0;
            anim.SetTrigger("jump");
        }

        
        if (jumpReleased && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(
                rb.velocity.x,
                rb.velocity.y * jumpCutMultiplier
            );
        }

        
        if (attackPressed && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    void FixedUpdate()
    {
        if (!canMove || isFrozen || isAttacking || isDead) return;

        float control = isGrounded ? 1f : airControlMultiplier;

        rb.velocity = new Vector2(
            moveInput * moveSpeed * control,
            rb.velocity.y
        );
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        anim.SetTrigger("attack");
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        canMove = false;

        rb.velocity = Vector2.zero;
        rb.simulated = false;

        anim.SetTrigger("die");
    }

    public void Freeze(float duration)
    {
        StartCoroutine(FreezeRoutine(duration));
    }

    IEnumerator FreezeRoutine(float duration)
    {
        isFrozen = true;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(duration);

        isFrozen = false;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
