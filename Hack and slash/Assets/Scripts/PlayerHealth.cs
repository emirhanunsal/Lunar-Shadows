using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Image healthBar; 
    [SerializeField] private CameraShake cameraShake;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.fillAmount = currentHealth / 100f;

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / 100f;
        cameraShake.ShakeCamera();
        Debug.Log("Player took damage. Current health: " + currentHealth);
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }


    public void Heal(int healthAmount)
    {
        currentHealth += healthAmount;
        healthBar.fillAmount = currentHealth / 100f;


        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    

    void Die()
    {
        Debug.Log("Player died.");
        // Handle player death (e.g., reload scene, show game over screen)
        // Destroy(gameObject); // Uncomment to destroy the player GameObject
    }
}