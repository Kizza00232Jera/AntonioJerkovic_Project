using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject bossEnemyPrefab;
    public GameObject powerupPrefab;
    public GameObject coinPrefab;

    public GameObject titleScreen;
    public GameObject gameOverScreen;

    public GameObject playScreen;
    

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI enemyResultText;
    public TextMeshProUGUI remainingEnemyResultText;
    private int enemiesKilled = 0;
    private int totalEnemies = 0;
    public TextMeshProUGUI killCountText;
    public TextMeshProUGUI enemiesRemainingText;

    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public List<Enemy> enemies = new List<Enemy>();

    
    private float powerupDuration = 7f; // Duration of powerup
    public bool isPowerupActive = false;


    



    public Vector3[] spawnPositions;

    private int currentCoinCount = 0;
    private int coinsNeededForPowerup = 4;

    private List<Vector3[]> coinSpawnAreas = new List<Vector3[]>();


    void Start()
    {

    }


  public void SpawnEnemies(int numberOfEnemies)
{
    for (int i = 0; i < numberOfEnemies; i++) 
    {
        Vector3 randomPosition = GetRandomPositionForEnemy();
        GameObject enemyInstance = Instantiate(enemyPrefab, randomPosition, Quaternion.identity); // Spawn enemy at the random position
        enemies.Add(enemyInstance.GetComponent<Enemy>());
    }
}

    Vector3 GetRandomPositionForEnemy()
    {
        // Define the bounds for random position, adjust as needed
        float randomX = Random.Range(-50, 50); // Random X position within -50 to 50
        float randomY = 0;                     // Keep Y fixed (ground level)
        float randomZ = Random.Range(-50, 50); // Random Z position within -50 to 50

        Vector3 spawnManagerPosition = transform.position;

        // Return the new position relative to the SpawnManager
        return new Vector3(spawnManagerPosition.x + randomX, randomY, spawnManagerPosition.z + randomZ);
    }

    void CountEnemies()
    {
        // Find all enemies and count them
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] bossEnemies = GameObject.FindGameObjectsWithTag("BossEnemy");

        totalEnemies = enemies.Length + bossEnemies.Length;
        UpdateEnemiesRemaining();
    }

    void SpawnCoins()
    {
        // Spawn one coin in each area
        for (int i = 0; i < coinsNeededForPowerup; i++)
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
        currentCoinCount++;
        coinText.text = "Coins " + currentCoinCount + "/" + coinsNeededForPowerup;
        if (currentCoinCount >= coinsNeededForPowerup)
        {
            currentCoinCount = 0; // Reset the coin counter
            SpawnPowerup(); // Spawn the powerup
        }
    }

    public void OnPowerupCollected()
    {
        // Start the coroutine to respawn coins after a delay
        StartCoroutine(RespawnCoinsAfterDelay(5f));
    }

    private IEnumerator RespawnCoinsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnCoins(); // Spawn new coins
    }
    void SpawnPowerup()
    {
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

        GameObject[] bossEnemies = GameObject.FindGameObjectsWithTag("BossEnemy");
        foreach (GameObject bossEnemyObj in bossEnemies)
        {
            Enemy bossEnemy = bossEnemyObj.GetComponent<Enemy>();
            if (bossEnemy != null)
            {
                bossEnemy.isRunningAway = true; // Set BossEnemy to run away
            }
        }
    }

    private IEnumerator PowerupTimer()
    {
        float timeRemaining = powerupDuration;

        while (timeRemaining > 0)
        {
            timerText.text = "Time Remaining " + Mathf.Ceil(timeRemaining).ToString(); // Show whole seconds
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

        GameObject[] bossEnemies = GameObject.FindGameObjectsWithTag("BossEnemy");
        foreach (GameObject bossEnemyObj in bossEnemies)
        {
            Enemy bossEnemy = bossEnemyObj.GetComponent<Enemy>();
            if (bossEnemy != null)
            {
                bossEnemy.isRunningAway = false; // Set BossEnemy to run away
            }
        }
    }

    public void StartGame()
    {
        // Define the four areas where coins will spawn
        coinSpawnAreas.Add(new Vector3[] { new Vector3(0, 1, 21), new Vector3(70, 1, 21) });    // Area 1
        coinSpawnAreas.Add(new Vector3[] { new Vector3(80, 1, 21), new Vector3(155, 1, 21) });  // Area 2
        coinSpawnAreas.Add(new Vector3[] { new Vector3(75, 1, 27), new Vector3(71, 1, 90) });   // Area 3
        coinSpawnAreas.Add(new Vector3[] { new Vector3(75, 1, 15), new Vector3(75, 1, -60) });  // Area 4

        SpawnCoins();
        Instantiate(bossEnemyPrefab, transform.position + new Vector3(12, 0, 0), Quaternion.identity);

        playScreen.gameObject.SetActive(true);
        CountEnemies();

        coinText.text = "Coins " + currentCoinCount + "/" + coinsNeededForPowerup;
        killCountText.text = "Enemies Killed " + enemiesKilled;
        timerText.text = "Be careful";

        restartButton.onClick.AddListener(RestartGame);
        titleScreen.gameObject.SetActive(false);


        
    // Lock the cursor to the center of the screen and hide it
       Cursor.lockState = CursorLockMode.Locked;


    }

    public void GameOver()
    {
        gameOverScreen.gameObject.SetActive(true);
        playScreen.gameObject.SetActive(false); 
        enemyResultText.text = "Enemies killed " + enemiesKilled;
        Cursor.lockState = CursorLockMode.None;
    }

   public void  EnemyKilled() {
        enemiesKilled++;
        UpdateKillCountUI();
        UpdateEnemiesRemaining();
    }

    void UpdateEnemiesRemaining()
    {
        int currentRemainingEnemies = totalEnemies - enemiesKilled;
        enemiesRemainingText.text = "Enemies remaining: " + currentRemainingEnemies;
        remainingEnemyResultText.text = "Enemies remaining " + currentRemainingEnemies;

    }


       void UpdateKillCountUI()
    {
       killCountText.text = "Enemies Killed: " + enemiesKilled;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
