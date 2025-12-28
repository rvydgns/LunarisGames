using UnityEngine;

namespace ClearSky
{
    public class SimplePlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 8f;
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float deceleration = 15f;
        [SerializeField] private float velocityPower = 0.9f;

        [Header("Jump")]
        [SerializeField] private float jumpForce = 15f;
        [SerializeField] private float coyoteTime = 0.12f;
        [SerializeField] private float jumpBufferTime = 0.12f;

        [Header("Ground Check")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRadius = 0.1f;
        [SerializeField] private LayerMask groundLayer;

        [Header("Attack")]
        [SerializeField] private float attackCooldown = 0.25f;
        [SerializeField] private Transform attackPoint;
        [SerializeField] private GameObject attackVFXPrefab;

        private Rigidbody2D rb;
        private Animator anim;

        private float moveInput;
        private float coyoteCounter;
        private float jumpBufferCounter;
        private float lastAttackTime;

        private int facingDirection = 1;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            ReadInput();
            UpdateFacingDirection();
            UpdateTimers();
            HandleJumpInput();
            HandleAttackInput();
            HandleAnimationParameters();
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }

        // -------------------- INPUT --------------------
        private void ReadInput()
        {
            moveInput = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump") || Input.GetAxisRaw("Vertical") > 0f)
                jumpBufferCounter = jumpBufferTime;
        }

        // -------------------- MOVEMENT --------------------
        private void HandleMovement()
        {
            float targetSpeed = moveInput * moveSpeed;
            float speedDiff = targetSpeed - rb.velocity.x;

            float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? acceleration : deceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velocityPower) * Mathf.Sign(speedDiff);

            rb.AddForce(new Vector2(movement, 0f));
        }

        private void UpdateFacingDirection()
        {
            if (moveInput > 0.01f)
                facingDirection = 1;
            else if (moveInput < -0.01f)
                facingDirection = -1;

            transform.localScale = new Vector3(facingDirection, 1f, 1f);
        }

        // -------------------- JUMP --------------------
        private void UpdateTimers()
        {
            if (IsGrounded())
                coyoteCounter = coyoteTime;
            else
                coyoteCounter -= Time.deltaTime;

            if (jumpBufferCounter > 0)
                jumpBufferCounter -= Time.deltaTime;
        }

        private void HandleJumpInput()
        {
            if (jumpBufferCounter > 0 && coyoteCounter > 0)
            {
                Jump();
                jumpBufferCounter = 0f;
                coyoteCounter = 0f;
            }
        }

        private void Jump()
        {
            Vector2 vel = rb.velocity;
            vel.y = 0f;
            rb.velocity = vel;

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            anim.SetBool("isJump", true);
        }

        private bool IsGrounded()
        {
            return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        // -------------------- ATTACK --------------------
        private void HandleAttackInput()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                    Attack();
            }
        }

        private void Attack()
        {
            lastAttackTime = Time.time;
            anim.SetTrigger("attack");
            SpawnAttackVFX();
        }

        private void SpawnAttackVFX()
        {
            if (attackPoint == null || attackVFXPrefab == null) return;

            GameObject vfx = Instantiate(attackVFXPrefab, attackPoint.position, Quaternion.identity);

            Vector3 scale = vfx.transform.localScale;
            scale.x = Mathf.Abs(scale.x) * facingDirection;
            vfx.transform.localScale = scale;

            Destroy(vfx, 0.7f);
        }

        // -------------------- ANIMATIONS --------------------
        private void HandleAnimationParameters()
        {
            anim.SetBool("isRun", Mathf.Abs(moveInput) > 0.01f && IsGrounded());
            anim.SetBool("isJump", !IsGrounded());
        }
    }
}
