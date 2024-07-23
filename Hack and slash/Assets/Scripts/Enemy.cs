using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    [SerializeField] private ParticleSystem particleSystem = default;
    private SpriteRenderer spriteRenderer;
    private Collider2D _collider2D;
    [SerializeField] private ParticleSystem fireParticle;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<Collider2D>();
        
    }

    void Update()
    {
        // Implement enemy movement or behavior
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemey took damage. Current health: " + health);

        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        // Play particle 
        if (fireParticle != null)
        {
            fireParticle.Stop();
        }
        particleSystem.Play();
        _collider2D.enabled = false;
        // Disable SpriteRenderer
        spriteRenderer.enabled = false;

        // Wait for 1.5 seconds
        yield return new WaitForSeconds(1.5f);

        // Destroy the enemy GameObject
        Destroy(gameObject);
    }
}