using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFireBullet : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask groundLayer;
    
    [SerializeField] private ParticleSystem particleSystem;
    private Collider2D _collider2D;
    private SpriteRenderer spriteRenderer;
    

    private void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            StartCoroutine(DestroyBullet());
        }
        
        else if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            StartCoroutine(DestroyBullet());
        }
    }
    
    private IEnumerator DestroyBullet()
    {
        
        particleSystem.Play();
        _collider2D.enabled = false;
      
        spriteRenderer.enabled = false;

        
        yield return new WaitForSeconds(1.5f);

        
        Destroy(gameObject);
    }
}