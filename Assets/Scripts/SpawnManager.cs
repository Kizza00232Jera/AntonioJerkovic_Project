using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    public List<Enemy> enemies = new List<Enemy>();
    
    
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
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity); // Spawn enemy at the calculated position
        }
    }

    void SpawnPowerup() {
        Instantiate(powerupPrefab, new Vector3(74,0.3f,16), transform.rotation);
    }
}
