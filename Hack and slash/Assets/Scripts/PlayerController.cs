using System.Collections;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int maxJumps = 2; // Max number of jumps
    private int jumpCount;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] private TrailRenderer trailRenderer;
    private bool isGrounded;
    private float originalGravityScale;
    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;

    public Image DashImage;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        originalGravityScale = rb.gravityScale; // Başlangıçta yerçekimini sakla
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }
        
        Move();
        Jump();
        if (canDash)
        {
            StartCoroutine(Dash());
        }
        UpdateAnimator();
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveInput * speed, rb.velocity.y);
        rb.velocity = movement;

        // Yön değiştirme
        if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
        }

        // Animator'da speed değerini güncelle
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
    }

    private IEnumerator Dash()
    {
        if (Input.GetButtonDown("Dash") && canDash)
        {
            canDash = false;
            isDashing = true;
        
            gameObject.layer = LayerMask.NameToLayer("Dashing");
        
            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0;
            rb.velocity = new Vector2(transform.localScale.x * dashForce * Input.GetAxis("Horizontal"), 0f);

            // Dash duration
            yield return new WaitForSeconds(dashTime);
        
            // End of dash - reset state
            gameObject.layer = LayerMask.NameToLayer("Player");
            rb.gravityScale = originalGravity;
            isDashing = false;

            // Initialize the DashImage fill amount for cooldown
            DashImage.fillAmount = 0f;
        
            // Start cooldown and fill the image over the dashCooldown period
            float elapsedTime = 0f;
            while (elapsedTime < dashCooldown)
            {
                elapsedTime += Time.deltaTime;
                DashImage.fillAmount = Mathf.Clamp01(elapsedTime / dashCooldown);
                yield return null;
            }

            // Ensure fill amount is set to 1 at the end of cooldown
            DashImage.fillAmount = 1f;
        
            canDash = true;
        }
    }



    void Jump()
    {
        if (Input.GetButtonDown("Jump") && (isGrounded || jumpCount < maxJumps))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Set vertical velocity directly for consistent jump height
            animator.SetBool("isJumping", true); // isJumping booleanını ayarla
            jumpCount++;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false); // Karakter yere inince isJumping'i false yap
            jumpCount = 0; // Reset jump count when the player lands on the ground
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }
}
