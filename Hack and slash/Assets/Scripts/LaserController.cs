using System.Collections;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public Transform target; 
    public LineRenderer lineRenderer; 
    public GameObject startPoint; 
    public GameObject endPoint; 
    public float lockOnDuration = 2f; 
    public float laserDuration = 2f; 
    public float laserInterval = 10f; 
    public float damageDuration = 10f; 
    public int damage = 10; 
    public float laserLength = 50f;
    public float hitThreshold = 0.5f; 
    public float moveSpeed = 2f; 
    public ParticleSystem deathEffect; 

    private bool isLockedOn = false;
    private bool canDealDamage = false;
    private bool hasDealtDamage = false; 
    private bool isDead = false; 
    private Color originalColor;
    private Rigidbody2D rb;
    private Transform playerTransform;

    private void Start()
    {
       
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        target = playerTransform;

       
        lineRenderer.SetPosition(0, startPoint.transform.position);
        lineRenderer.SetPosition(1, endPoint.transform.position);
        originalColor = lineRenderer.startColor;

        StartCoroutine(LaserRoutine());
    }

    private void Update()
    {
        if (isDead) return; 

        if (!isLockedOn)
        {
            UpdateLaserPosition();
        }
        else
        {
            CheckDamage();
        }

        if (!canDealDamage && !isLockedOn)
        {
            FollowPlayer();
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private IEnumerator LaserRoutine()
    {
        while (true)
        {
            if (isDead) yield break;

          
            isLockedOn = false;
            lineRenderer.enabled = true;
            hasDealtDamage = false; 
            lineRenderer.startColor = originalColor;
            lineRenderer.endColor = originalColor;
            yield return new WaitForSeconds(lockOnDuration);

            
            isLockedOn = true;
            yield return new WaitForSeconds(laserDuration);

           
            canDealDamage = true;
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;
            yield return new WaitForSeconds(damageDuration);

           
            canDealDamage = false;
            StopLaser();
            yield return new WaitForSeconds(laserInterval - lockOnDuration - laserDuration - damageDuration);
        }
    }

    private void UpdateLaserPosition()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - startPoint.transform.position).normalized;
            Vector3 endPosition = target.position + direction * laserLength;
            endPoint.transform.position = endPosition;
            lineRenderer.SetPosition(0, startPoint.transform.position);
            lineRenderer.SetPosition(1, endPosition);
        }
    }

    private void StopLaser()
    {
        lineRenderer.enabled = false;
    }

    private void CheckDamage()
    {
        if (isDead || !canDealDamage || target == null || hasDealtDamage) return;

        if (target.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Vector3 laserStart = startPoint.transform.position;
            Vector3 laserEnd = endPoint.transform.position;
            Vector3 playerPos = target.position;

            
            if (Vector3.Distance(laserStart, playerPos) + Vector3.Distance(playerPos, laserEnd) <= Vector3.Distance(laserStart, laserEnd) + hitThreshold)
            {
                PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                    hasDealtDamage = true; 
                    StopLaser();
                }
            }
        }
    }

    private void FollowPlayer()
    {
        if (playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
        }
    }

   
    
}
