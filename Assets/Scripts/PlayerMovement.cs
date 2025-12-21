using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jump")]
    public float jumpForce = 14f;
    public float jumpHoldForce = 1.5f;
    public float maxJumpTime = 0.25f;

    [Header("Jump Limit")]
    public int maxJumpCount = 2;   // 🔥 DOUBLE JUMP
    private int currentJumpCount;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private float moveInput;

    // Variable jump
    private bool isJumping;
    private float jumpTimeCounter;

    // Sabit scale
    private const float SCALE_VALUE = 0.5f;

    // 🔒 FREEZE
    private bool isFrozen = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // 🔴 SPAWN POINT
        GameObject spawnPoint = GameObject.Find("SpawnPoint");
        if (spawnPoint != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            transform.position = spawnPoint.transform.position;
        }

        transform.localScale = new Vector3(SCALE_VALUE, SCALE_VALUE, SCALE_VALUE);
    }

    void Update()
    {
        if (isFrozen)
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("isRun", false);
            return;
        }

        moveInput = Input.GetAxisRaw("Horizontal");
        anim.SetBool("isRun", moveInput != 0);

        if (moveInput > 0)
            transform.localScale = new Vector3(SCALE_VALUE, SCALE_VALUE, SCALE_VALUE);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-SCALE_VALUE, SCALE_VALUE, SCALE_VALUE);

        // 🟢 DOUBLE JUMP KONTROLÜ
        if (Input.GetKeyDown(KeyCode.UpArrow) && currentJumpCount < maxJumpCount)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
            jumpTimeCounter = maxJumpTime;
            currentJumpCount++;
            anim.SetTrigger("jump");
        }

        if (Input.GetKey(KeyCode.UpArrow) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce + jumpHoldForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            isJumping = false;
        }
    }

    void FixedUpdate()
    {
        if (isFrozen)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        // 🟢 YERE DEĞİNCE ZIPLAMA RESET
        if (isGrounded)
        {
            currentJumpCount = 0;
        }
    }

    void LateUpdate()
    {
        float xSign = Mathf.Sign(transform.localScale.x);
        transform.localScale = new Vector3(
            xSign * SCALE_VALUE,
            SCALE_VALUE,
            SCALE_VALUE
        );
    }

    // 🔒 FREEZE
    public void Freeze(float duration)
    {
        StartCoroutine(FreezeRoutine(duration));
    }

    private IEnumerator FreezeRoutine(float duration)
    {
        isFrozen = true;
        yield return new WaitForSeconds(duration);
        isFrozen = false;
    }
}
