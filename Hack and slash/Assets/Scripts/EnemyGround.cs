using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGround : MonoBehaviour
{
    [SerializeField] private float hitRadius = 1.5f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float timeBetweenHits = 2f;
    [SerializeField] private float moveSpeed = 2f; 
    [SerializeField] private LayerMask playerLayer; 
    [SerializeField] private Transform attackCenter; 
    [SerializeField] private ParticleSystem deathEffect; 
    private float timeSinceLastHit = 0f;
    private Transform player;
    private bool isAttacking = false;
    private bool facingRight = false;
    private bool isDead = false; 
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Enemy enemyScript;

    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyScript = GetComponent<Enemy>();
    }

    void Update()
    {
        isDead = enemyScript.isDead;

        if (isDead)
        {
            
            attackCenter.position = new Vector3(attackCenter.position.x, 100f, attackCenter.position.z);
            return; 
        }
        
        
        timeSinceLastHit += Time.deltaTime;

        
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackCenter.position, hitRadius, playerLayer);
        bool playerInRange = hitColliders.Length > 0;


        if (playerInRange && timeSinceLastHit >= timeBetweenHits && !isAttacking)
        {
            Attack();
        }
        else if (!playerInRange && !isAttacking)
        {
            FollowPlayer();
        }

        animator.SetFloat("Speed", Mathf.Abs(player.position.x - transform.position.x));
    }

    void FollowPlayer()
    {
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position.x < player.position.x && !facingRight)
        {
            Flip();
        }
        else if (transform.position.x > player.position.x && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        spriteRenderer.flipX = !spriteRenderer.flipX;

        Vector3 localScale = attackCenter.localPosition;
        localScale.x *= -1;
        attackCenter.localPosition = localScale;
    }

    void Attack()
    {
        if (isDead) return; 

      
        isAttacking = true;

        animator.SetTrigger("Hit");

        StartCoroutine(DealDamageAfterDelay());
    }

    IEnumerator DealDamageAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackCenter.position, hitRadius, playerLayer);
        bool playerInRange = hitColliders.Length > 0;

        if (playerInRange)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }

        timeSinceLastHit = 0f;
        isAttacking = false;
    }
    

    void OnDrawGizmosSelected()
    {
        if (attackCenter != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackCenter.position, hitRadius);
        }
    }
}
