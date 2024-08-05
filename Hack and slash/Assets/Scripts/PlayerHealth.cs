using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Image healthBar; 
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private ParticleSystem explosion;
    private SpriteRenderer sr;
    [SerializeField] private SpriteRenderer swordSr;
    [SerializeField] private TrailRenderer tr;
    public bool isPlayerDead = false;
    [SerializeField] private TMP_Text ScreenMessage;
    public GameManager gameManager;
    private AudioSource audioSource;
    
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        healthBar.fillAmount = currentHealth / 100f;

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / 100f;
        cameraShake.ShakeCamera();
        audioSource.Play();
        
        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
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
    

    IEnumerator Die()
    {
        isPlayerDead = true;
        explosion.Play();
        sr.enabled = false;
        swordSr.enabled = false;
        tr.enabled = false;
        gameObject.GetComponent<PlayerController>().enabled = false;
        
        
        gameObject.layer = LayerMask.NameToLayer("Default");

        yield return new WaitForSeconds(2);

        gameManager.audioSource.Play();
        ScreenMessage.text = "YOU";
        
        cameraShake.ShakeCamera();
        yield return new WaitForSeconds(0.6f);
        gameManager.audioSource.Play();

        ScreenMessage.text = "LOST";
        cameraShake.ShakeCamera();

        yield return new WaitForSeconds(1.0f);
        gameManager.audioSource.Play();

        ScreenMessage.text = "Click to Restart";
        cameraShake.ShakeCamera();

        gameManager.OnPlayerDeath();


    }
}