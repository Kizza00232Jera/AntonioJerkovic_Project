using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject bossEnemyPrefab;
    public GameObject powerupPrefab;
    public GameObject coinPrefab;

       public TextMeshProUGUI timerText;
       public TextMeshProUGUI coinText;
    public List<Enemy> enemies = new List<Enemy>();
     private float powerupDuration = 7f; // Duration of powerup
    public bool isPowerupActive = false; 
    
    public Vector3[] spawnPositions;

    private int coinsCollected = 0;
    private int coinsNeeded = 4;

     private List<Vector3[]> coinSpawnAreas = new List<Vector3[]>();
    void Start()
    {
        spawnPositions = new Vector3[]
        {
            new Vector3(0, 0, 0),      // Position for Enemy 1
            new Vector3(2, 0, 0),      // Position for Enemy 2
            new Vector3(4, 0, 0),      // Position for Enemy 3
            new Vector3(6, 0, 0),      // Position for Enemy 4
            new Vector3(8, 0, 0)       // Position for Enemy 5
        };

         // Define the four areas where coins will spawn
        coinSpawnAreas.Add(new Vector3[] { new Vector3(0, 1, 21), new Vector3(70, 1, 21) });    // Area 1
        coinSpawnAreas.Add(new Vector3[] { new Vector3(80, 1, 21), new Vector3(155, 1, 21) });  // Area 2
        coinSpawnAreas.Add(new Vector3[] { new Vector3(75, 1, 27), new Vector3(71, 1, 90) });   // Area 3
        coinSpawnAreas.Add(new Vector3[] { new Vector3(75, 1, 15), new Vector3(75, 1, -60) });  // Area 4

        SpawnCoins();
        SpawnEnemies();
        SpawnPowerup();

        timerText.gameObject.SetActive(false);
        coinText.text = "Coins " + coinsCollected + "/" + coinsNeeded;
    }


    void SpawnEnemies()
    {
        foreach (Vector3 position in spawnPositions)
        {
            Vector3 spawnPosition = transform.position + position; // Calculate spawn position relative to the SpawnManager
            GameObject enemyInstance = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity); // Spawn enemy at the calculated position
            enemies.Add(enemyInstance.GetComponent<Enemy>());
        }
        Instantiate(bossEnemyPrefab, new Vector3(74, 0.3f, 16), new Quaternion(0, 0, 0, 0));

    }

  void SpawnCoins()
{
    // Spawn one coin in each area
    for (int i = 0; i < coinsNeeded; i++)
    {
        Vector3 randomPosition = GetRandomPositionInArea(i);
        Instantiate(coinPrefab, randomPosition, Quaternion.identity);
    }
}

   

Vector3 GetRandomPositionInArea(int areaIndex)
    {
        Vector3[] areaBounds = coinSpawnAreas[areaIndex];
        Vector3 min = areaBounds[0]; // Minimum point
        Vector3 max = areaBounds[1]; // Maximum point

        // Randomize within the bounds of the area
        float randomX = Random.Range(min.x, max.x);
        float randomY = Random.Range(min.y, max.y);
        float randomZ = Random.Range(min.z, max.z);

        return new Vector3(randomX, randomY, randomZ);
    }

    public void CollectCoin() 
{
    coinsCollected++;
    coinText.text = "Coins " + coinsCollected + "/" + coinsNeeded;
    if (coinsCollected >= coinsNeeded) 
    {
        SpawnPowerup(); // Spawn powerup after all coins are collected
    }
}

 void SpawnPowerup() {
        Instantiate(powerupPrefab, new Vector3(74, 0.3f, 16), transform.rotation);
    }

     public void ActivatePowerup() 
    {
       if (!isPowerupActive)
        {
            isPowerupActive = true;

            // Show the timer text and start the countdown
            timerText.gameObject.SetActive(true); // Enable the timer text
            
            StartCoroutine(PowerupTimer());


      foreach (Enemy enemy in enemies) 
            {
                if (enemy != null) 
                {
                    enemy.isRunningAway = true; // Set enemies to run away
                }
            }

        }
    }

    private IEnumerator PowerupTimer() 
    {
        float timeRemaining = powerupDuration;
        
        while (timeRemaining > 0) 
        {
           timerText.text = Mathf.Ceil(timeRemaining).ToString(); // Show whole seconds
            timeRemaining -= Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Powerup duration is over
        isPowerupActive = false;

        timerText.gameObject.SetActive(false);

        // Reset all enemies' behavior
        foreach (Enemy enemy in enemies) 
        {
            if (enemy != null) 
            {
                enemy.isRunningAway = false;
            }
        }
    }
}
