using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMahmut : MonoBehaviour
{
    [SerializeField] private float hitDistance = 1.5f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float timeBetweenHits = 2f;
    [SerializeField] private float moveSpeed = 2f; // Speed at which the enemy moves towards the player
    private float timeSinceLastHit = 0f;

    private Transform player;
    private bool playerInRange = false;
    private bool isAttacking = false;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Find the player by tag, assuming the player has the tag "Player"
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Update the time since the last hit
        timeSinceLastHit += Time.deltaTime;

        // Check if the player is within hit distance
        playerInRange = Vector2.Distance(transform.position, player.position) <= hitDistance;

        // If the player is in range and the enemy can attack (time since last hit is greater than time between hits)
        if (playerInRange && timeSinceLastHit >= timeBetweenHits && !isAttacking)
        {
            // Perform the attack
            Attack();
        }
        else if (!playerInRange && !isAttacking)
        {
            // Move towards the player on the x-axis only if not attacking and player is out of range
            FollowPlayer();
        }

        // Update the speed parameter for the animator
        animator.SetFloat("Speed", Mathf.Abs(player.position.x - transform.position.x));
    }

    void FollowPlayer()
    {
        // Move towards the player on the x-axis only
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Flip sprite based on the movement direction
        if (transform.position.x < player.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else if (transform.position.x > player.position.x)
        {
            spriteRenderer.flipX = false;
        }
    }

    void Attack()
    {
        // Start the attack process
        isAttacking = true;

        // Trigger the hit animation
        animator.SetTrigger("Hit");

        // Apply damage to the player after a short delay to sync with the animation
        StartCoroutine(DealDamageAfterDelay());
    }

    IEnumerator DealDamageAfterDelay()
    {
        // Wait for the animation to start (adjust the delay based on your animation length)
        yield return new WaitForSeconds(0.5f);

        // Apply damage to the player
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            Debug.Log("Enemy attacked the player!");
        }

        // Reset the time since the last hit and allow movement again
        timeSinceLastHit = 0f;
        isAttacking = false;
    }
}