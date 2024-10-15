using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    public List<Enemy> enemies = new List<Enemy>();
     public float powerupDuration = 20f; // Duration of powerup
    public bool isPowerupActive = false; 
    
    public Vector3[] spawnPositions;
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
        Instantiate(powerupPrefab, new Vector3(74,0.3f,16), transform.rotation);
    }

     public void ActivatePowerup() 
    {
        if (!isPowerupActive) 
        {
            isPowerupActive = true;
            // Set all enemies to run away
            foreach (Enemy enemy in enemies) 
            {
                if (enemy != null) 
                {
                    enemy.isRunningAway = true;
                }
            }

            // Start the powerup timer
            StartCoroutine(PowerupTimer());
        }
    }

    private IEnumerator PowerupTimer() 
    {
        float timeRemaining = powerupDuration;
        
        while (timeRemaining > 0) 
        {
            // Here, you could update a UI Timer (if needed)
            timeRemaining -= Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Powerup duration is over
        isPowerupActive = false;

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
