using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int maxJumps = 2; 
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
    private PlayerHealth playerHealth;
    private GameManager gameManager;

    public Image DashImage;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
        originalGravityScale = rb.gravityScale; 
        gameManager = FindObjectOfType<GameManager>(); 
    }

    private void Update()
    {
        if (playerHealth.isPlayerDead)
        {
            Die();
            return;
        }
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

      
        if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
        }

        
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

            
            yield return new WaitForSeconds(dashTime);

           
            gameObject.layer = LayerMask.NameToLayer("Player");
            rb.gravityScale = originalGravity;
            isDashing = false;

            
            DashImage.fillAmount = 0f;

            
            float elapsedTime = 0f;
            while (elapsedTime < dashCooldown)
            {
                elapsedTime += Time.deltaTime;
                DashImage.fillAmount = Mathf.Clamp01(elapsedTime / dashCooldown);
                yield return null;
            }

          
            DashImage.fillAmount = 1f;

            canDash = true;
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && (isGrounded || jumpCount < maxJumps))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); 
            animator.SetBool("isJumping", true); 
            jumpCount++;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false); 
            jumpCount = 0; 
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

    public void Die()
    {
        playerHealth.isPlayerDead = true;
        gameManager.OnPlayerDeath();
    }
}
