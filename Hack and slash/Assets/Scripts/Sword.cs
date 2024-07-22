using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public float attackRange;
    public LayerMask whatIsEnemies;
    public int damage;

    // Adjust the attack offset to flip the attack position
    public Vector3 attackOffsetRight;
    public Vector3 attackOffsetLeft;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (timeBtwAttack <= 0)
        {
            if (Input.GetButtonDown("Attack"))
            {
                animator.SetTrigger("Attack1");
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    Enemy enemy = enemiesToDamage[i].GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damage);
                    }
                }
                timeBtwAttack = startTimeBtwAttack;
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }

        // Flip the sword based on the player's horizontal movement
        float moveInput = Input.GetAxis("Horizontal");
        if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
            attackPos.localPosition = attackOffsetLeft;
        }
        else if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
            attackPos.localPosition = attackOffsetRight;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}