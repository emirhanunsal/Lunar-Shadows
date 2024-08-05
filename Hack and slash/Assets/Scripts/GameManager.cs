using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Transform[] spawners; 
    public GameObject[] enemyPrefabs; 
    public float spawnInterval = 1f; 
    [SerializeField] private TMP_Text enemyCountText; 
    [SerializeField] private TMP_Text waveCountText; 
    [SerializeField] private TMP_Text waveMessageText; 
    [SerializeField] private CameraShake cameraShake;  

    public AudioSource audioSource;
    
    private int currentWave = 0;
    private int activeEnemies = 0;
    private bool isPlayerDead = false;  
    private bool isWaveImpossibleStarted = false;  

    private void Start()
    {

        audioSource = GetComponent<AudioSource>();
        
        spawners = new Transform[4];
        spawners[0] = GameObject.Find("GroundSpawner1").transform;
        spawners[1] = GameObject.Find("GroundSpawner2").transform;
        spawners[2] = GameObject.Find("AirSpawner1").transform;
        spawners[3] = GameObject.Find("AirSpawner2").transform;

        UpdateUI();
        StartCoroutine(SpawnWaves());
    }

    private void Update()
    {
        if (activeEnemies < 0)
        {
            activeEnemies = 0;
        }

        
        if (isPlayerDead)
        {
            StartCoroutine(HandlePlayerDeath());
        }
    }

    private IEnumerator HandlePlayerDeath()
    {
        while (isPlayerDead)
        {
            if (Input.GetMouseButtonDown(0))
            {
               
                RestartGame();
            }
            yield return null;
        }
    }

    private IEnumerator SpawnWaves()
    {
        while (true)
        {
            currentWave++;
            UpdateUI();
            yield return StartCoroutine(ShowWaveMessage(currentWave));  

            if (currentWave == 10)
            {
                
                yield return StartCoroutine(SpawnWave(GetWave(currentWave)));
                yield return new WaitUntil(() => activeEnemies == 0);  
                
                
                yield return StartCoroutine(EndOfWave10Messages());
                
                 
                yield return StartCoroutine(SpawnWave(GetWaveImpossible()));
                
                
                isWaveImpossibleStarted = true;
                break;
            }

            if (!isWaveImpossibleStarted)
            {
                List<SpawnInfo> wave = GetWave(currentWave);
                Debug.Log($"Starting Wave {currentWave}");
                yield return StartCoroutine(SpawnWave(wave));
                yield return new WaitUntil(() => activeEnemies == 0);
                Debug.Log($"Wave {currentWave} completed");

                 
                yield return new WaitForSeconds(0.5f);  
            }
        }
    }

    private IEnumerator EndOfWave10Messages()
    {
        audioSource.Play();
        waveMessageText.text = "Well Done";
        cameraShake.ShakeCamera();
        yield return new WaitForSeconds(1.0f);  
        audioSource.Play();
        waveMessageText.text = "You Beat The Game";
        cameraShake.ShakeCamera();
        yield return new WaitForSeconds(1.0f);  
        audioSource.Play();
        waveMessageText.text = "It was";
        cameraShake.ShakeCamera();
        yield return new WaitForSeconds(0.4f);  
        audioSource.Play();
        waveMessageText.text = "Only";
        cameraShake.ShakeCamera();
        yield return new WaitForSeconds(0.4f); 
        audioSource.Play();
        waveMessageText.text = "The Beginning";
        cameraShake.ShakeCamera();
        yield return new WaitForSeconds(2.0f);  
        audioSource.Play();
        waveMessageText.text = "Try this";
        cameraShake.ShakeCamera();
        yield return new WaitForSeconds(2.0f);
        audioSource.Play();
        waveMessageText.text = "Wave:";
        cameraShake.ShakeCamera();
        yield return new WaitForSeconds(0.5f);
        audioSource.Play();
        waveMessageText.text = "Wave: Impossible";
        cameraShake.ShakeCamera();  
        yield return new WaitForSeconds(1.0f);

        waveMessageText.text = "";
    }

    private IEnumerator ShowWaveMessage(int waveNumber)
    {
        
        yield return new WaitForSeconds(1.5f); 
        audioSource.Play();
        waveMessageText.text = "WAVE";
        cameraShake.ShakeCamera();
        yield return new WaitForSeconds(0.5f);  
        audioSource.Play();
        waveMessageText.text = $"WAVE: {waveNumber}";
        cameraShake.ShakeCamera();
        yield return new WaitForSeconds(2f);  
        waveMessageText.text = "";
        waveCountText.text = $"Wave: {waveNumber}";
    }

    private IEnumerator SpawnWave(List<SpawnInfo> wave)
    {
        
        foreach (var spawnInfo in wave)
        {
            for (int i = 0; i < spawnInfo.count; i++)
            {
                Debug.Log($"SpawnWave: Spawning enemyIndex {spawnInfo.enemyIndex} at spawnerIndex {spawnInfo.spawnerIndex}");
                Instantiate(enemyPrefabs[spawnInfo.enemyIndex], spawners[spawnInfo.spawnerIndex].position, Quaternion.identity);
                activeEnemies++;
                UpdateUI();
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    private List<SpawnInfo> GetWave(int waveNumber)
    {
        switch (waveNumber)
        {
            case 1:
                Debug.Log("Wave 1: 5 enemyIndex 0 enemies from spawners 0 and 1");
                return new List<SpawnInfo>
                {
                    new SpawnInfo { spawnerIndex = 0, enemyIndex = 0, count = 3 },
                    new SpawnInfo { spawnerIndex = 1, enemyIndex = 0, count = 2 }
                };
            case 2:
                Debug.Log("Wave 2: 5 enemyIndex 0 enemies, 2 enemyIndex 1 enemies from spawner 2");
                return new List<SpawnInfo>
                {
                    new SpawnInfo { spawnerIndex = 0, enemyIndex = 0, count = 2 },
                    new SpawnInfo { spawnerIndex = 1, enemyIndex = 0, count = 3 },
                    new SpawnInfo { spawnerIndex = 2, enemyIndex = 1, count = 2 }
                };
            case 3:
                Debug.Log("Wave 3: 6 enemyIndex 0 enemies, 3 enemyIndex 1 enemies from spawners 2 and 3");
                return new List<SpawnInfo>
                {
                    new SpawnInfo { spawnerIndex = 0, enemyIndex = 0, count = 3 },
                    new SpawnInfo { spawnerIndex = 1, enemyIndex = 0, count = 3 },
                    new SpawnInfo { spawnerIndex = 2, enemyIndex = 1, count = 3 }
                };
            case 4:
                Debug.Log("Wave 4: 4 enemyIndex 0 enemies, 3 enemyIndex 1 enemies, 2 enemyIndex 2 enemies");
                return new List<SpawnInfo>
                {
                    new SpawnInfo { spawnerIndex = 0, enemyIndex = 0, count = 2 },
                    new SpawnInfo { spawnerIndex = 1, enemyIndex = 0, count = 2 },
                    new SpawnInfo { spawnerIndex = 2, enemyIndex = 1, count = 3 },
                    new SpawnInfo { spawnerIndex = 3, enemyIndex = 2, count = 2 }
                };
            case 5:
                Debug.Log("Wave 5: 5 enemyIndex 0 enemies, 4 enemyIndex 1 enemies, 2 enemyIndex 2 enemies");
                return new List<SpawnInfo>
                {
                    new SpawnInfo { spawnerIndex = 0, enemyIndex = 0, count = 2 },
                    new SpawnInfo { spawnerIndex = 1, enemyIndex = 0, count = 3 },
                    new SpawnInfo { spawnerIndex = 2, enemyIndex = 1, count = 4 },
                    new SpawnInfo { spawnerIndex = 3, enemyIndex = 2, count = 2 }
                };
            case 6:
                Debug.Log("Wave 6: 4 enemyIndex 0 enemies, 5 enemyIndex 1 enemies, 3 enemyIndex 2 enemies");
                return new List<SpawnInfo>
                {
                    new SpawnInfo { spawnerIndex = 0, enemyIndex = 0, count = 2 },
                    new SpawnInfo { spawnerIndex = 1, enemyIndex = 0, count = 2 },
                    new SpawnInfo { spawnerIndex = 2, enemyIndex = 1, count = 5 },
                    new SpawnInfo { spawnerIndex = 3, enemyIndex = 2, count = 3 }
                };
            case 7:
                Debug.Log("Wave 7: 3 enemyIndex 0 enemies, 6 enemyIndex 1 enemies, 4 enemyIndex 2 enemies");
                return new List<SpawnInfo>
                {
                    new SpawnInfo { spawnerIndex = 0, enemyIndex = 0, count = 1 },
                    new SpawnInfo { spawnerIndex = 1, enemyIndex = 0, count = 2 },
                    new SpawnInfo { spawnerIndex = 2, enemyIndex = 1, count = 6 },
                    new SpawnInfo { spawnerIndex = 3, enemyIndex = 2, count = 4 }
                };
            case 8:
                Debug.Log("Wave 8: 3 enemyIndex 0 enemies, 7 enemyIndex 1 enemies, 4 enemyIndex 2 enemies");
                return new List<SpawnInfo>
                {
                    new SpawnInfo { spawnerIndex = 0, enemyIndex = 0, count = 1 },
                    new SpawnInfo { spawnerIndex = 1, enemyIndex = 0, count = 2 },
                    new SpawnInfo { spawnerIndex = 2, enemyIndex = 1, count = 7 },
                    new SpawnInfo { spawnerIndex = 3, enemyIndex = 2, count = 4 }
                };
            case 9:
                Debug.Log("Wave 9: 2 enemyIndex 0 enemies, 8 enemyIndex 1 enemies, 5 enemyIndex 2 enemies");
                return new List<SpawnInfo>
                {
                    new SpawnInfo { spawnerIndex = 0, enemyIndex = 0, count = 1 },
                    new SpawnInfo { spawnerIndex = 1, enemyIndex = 0, count = 1 },
                    new SpawnInfo { spawnerIndex = 2, enemyIndex = 1, count = 8 },
                    new SpawnInfo { spawnerIndex = 3, enemyIndex = 2, count = 5 }
                };
            case 10:
                Debug.Log("Wave 10: 2 enemyIndex 0 enemies, 10 enemyIndex 1 enemies, 6 enemyIndex 2 enemies");
                return new List<SpawnInfo>
                {
                    new SpawnInfo { spawnerIndex = 0, enemyIndex = 0, count = 1 },
                    new SpawnInfo { spawnerIndex = 1, enemyIndex = 0, count = 1 },
                    new SpawnInfo { spawnerIndex = 2, enemyIndex = 1, count = 10 },
                    new SpawnInfo { spawnerIndex = 3, enemyIndex = 2, count = 6 }
                };
            default:
                
                return new List<SpawnInfo>();
        }
    }

    private List<SpawnInfo> GetWaveImpossible()
    {
        Debug.Log("Wave Impossible: 40 enemyIndex 1 enemies from spawners 2 and 3");
        return new List<SpawnInfo>
        {
            new SpawnInfo { spawnerIndex = 2, enemyIndex = 2, count = 10 },
            new SpawnInfo { spawnerIndex = 3, enemyIndex = 2, count = 10 },
            new SpawnInfo { spawnerIndex = 2, enemyIndex = 2, count = 10 },
            new SpawnInfo { spawnerIndex = 3, enemyIndex = 2, count = 10 }
        };
    }

    private void UpdateUI()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = $"Enemies Left: {activeEnemies}";
        }

        if (waveCountText != null)
        {
            waveCountText.text = $"Wave: {currentWave}";
        }
    }

    public void OnPlayerDeath()
    {
        isPlayerDead = true;
    }
    
    public void EnemyDefeated()
    {
        activeEnemies--;
        UpdateUI();
    }

    private void RestartGame()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<SpriteRenderer>().enabled = true;
            player.GetComponent<PlayerController>().enabled = true;
            player.GetComponent<PlayerHealth>().isPlayerDead = false;
            player.layer = LayerMask.NameToLayer("Player"); 

            
            Transform swordAttack = player.transform.Find("Sword Attack");
            if (swordAttack != null)
            {
                SpriteRenderer swordRenderer = swordAttack.GetComponent<SpriteRenderer>();
                if (swordRenderer != null)
                {
                    swordRenderer.enabled = true;
                }
            }

            Transform dashTrail = player.transform.Find("DashTrailRenderer");
            if (dashTrail != null)
            {
                TrailRenderer trailRenderer = dashTrail.GetComponent<TrailRenderer>();
                if (trailRenderer != null)
                {
                    trailRenderer.enabled = true;
                }
            }

            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(100);  
            }

           
            player.transform.position = Vector3.zero;
        }

      
        currentWave = 0;
        activeEnemies = 0;
        isPlayerDead = false;
        StopAllCoroutines();
        StartCoroutine(SpawnWaves());

        UpdateUI();
    }


}


public class SpawnInfo
{
    public int spawnerIndex;
    public int enemyIndex;
    public int count;
}
