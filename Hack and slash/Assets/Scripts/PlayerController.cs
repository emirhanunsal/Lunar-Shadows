using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float dashCooldown = 2f;
    private float lastDashTime;
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    
    private bool isGrounded;
    private float originalGravityScale;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        originalGravityScale = rb.gravityScale; // Başlangıçta yerçekimini sakla
    }

    private void Update()
    {
        Move();
        Jump();
        UpdateAnimator();
        if (Time.time >= lastDashTime + dashCooldown)
        {
            Dash();
        }
        
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

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            animator.SetBool("isJumping", true); // isJumping booleanını ayarla
        }
    }

    void Dash()
    {
        if (Input.GetButtonDown("Dash"))
        {
            float horizontalInput = Input.GetAxis("Horizontal");

            if (horizontalInput != 0)
            {
                Vector2 dashDirection = new Vector2(horizontalInput, 0).normalized;
                rb.AddForce(dashDirection * dashForce, ForceMode2D.Impulse);
                lastDashTime = Time.time; // Update the last dash time
                Debug.Log("Dash");
            }
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false); // Karakter yere inince isJumping'i false yap
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
