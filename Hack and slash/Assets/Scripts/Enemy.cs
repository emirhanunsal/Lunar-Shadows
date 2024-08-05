using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    [SerializeField] private ParticleSystem particleSystem = default;
    [SerializeField] private ParticleSystem fireParticle = default;
    [SerializeField] private ParticleSystem playerHitParticle;
    [SerializeField] private DamageFlash damageFlash = default; 
    private SpriteRenderer spriteRenderer;
    private Collider2D _collider2D;
    public CameraShake cameraShake;
    public bool isDead = false;
    [SerializeField] private GameObject laser;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<Collider2D>();

        if (cameraShake == null)
        {
            Debug.Log("nulllll");
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        CameraShake cameraShake = FindObjectOfType<CameraShake>();
        if (cameraShake != null && cameraShake.gameObject.activeInHierarchy)
        {
            cameraShake.ShakeCamera();
        }

        playerHitParticle.Play();

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
        isDead = true;

        
        if (fireParticle != null)
        {
            fireParticle.Stop();
        }
        if (laser != null)
        {
            Destroy(laser);
        }

        particleSystem.Play();
        _collider2D.enabled = false;
        
        spriteRenderer.enabled = false;

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            Debug.Log("gamemanagerfound");
            gameManager.EnemyDefeated();
        }
        
       
        yield return new WaitForSeconds(1.5f);

       
        

        
        Destroy(gameObject);
    }
}