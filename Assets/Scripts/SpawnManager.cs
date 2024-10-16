using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;

       public TextMeshProUGUI timerText;
    public List<Enemy> enemies = new List<Enemy>();
     private float powerupDuration = 7f; // Duration of powerup
    public bool isPowerupActive = false; 
    
    public Vector3[] spawnPositions;

    private int coinsCollected = 0;
    private int coinsNeeded = 5;
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

        SpawnEnemies();
        SpawnPowerup();

        timerText.gameObject.SetActive(false);
    }


    void SpawnEnemies()
    {
        foreach (Vector3 position in spawnPositions)
        {
            Vector3 spawnPosition = transform.position + position; // Calculate spawn position relative to the SpawnManager
            GameObject enemyInstance = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity); // Spawn enemy at the calculated position
            enemies.Add(enemyInstance.GetComponent<Enemy>());
        }
    }

    void SpawnPowerup() {
        Instantiate(powerupPrefab, new Vector3(74, 0.3f, 16), transform.rotation);
    }

    public void CollectCoin() 
{
    coinsCollected++;
    if (coinsCollected >= coinsNeeded) 
    {
        SpawnPowerup(); // Spawn powerup after all coins are collected
    }
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
