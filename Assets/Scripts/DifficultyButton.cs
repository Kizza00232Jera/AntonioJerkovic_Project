using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    private Button button;
    private SpawnManager spawnManager;

    public int spawnEnemy;
    public float enemySpeed; 
    public float selectedBossSpeed;



    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty);
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    void SetDifficulty()
    {
        Debug.Log(gameObject.name + " was clicked");
        
        // Set the enemy speed based on the button clicked
        //spawnManager.SetEnemySpeed(enemySpeed);

        // Spawn enemies and start the game
        spawnManager.SpawnEnemies(spawnEnemy, enemySpeed);
        spawnManager.SetBossEnemySpeed(selectedBossSpeed);
        spawnManager.StartGame();
    }
}
