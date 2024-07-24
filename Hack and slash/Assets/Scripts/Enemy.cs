using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    [SerializeField] private ParticleSystem particleSystem = default;
    [SerializeField] private ParticleSystem fireParticle = default;
    [SerializeField] private ParticleSystem playerHitParticle;
    
    [SerializeField] private DamageFlash damageFlash = default; // Reference to DamageFlash script
    private SpriteRenderer spriteRenderer;
    private Collider2D _collider2D;
    [SerializeField] private CameraShake cameraShake;


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
        cameraShake.ShakeCamera();
        playerHitParticle.Play();
        Debug.Log("Enemy took damage. Current health: " + health);

        // Trigger the damage flash effect
        if (damageFlash != null)
        {
            damageFlash.CallDamageFlash();
        }

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