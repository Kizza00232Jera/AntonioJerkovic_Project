using UnityEngine;

public class Coin : MonoBehaviour
{
    private SpawnManager spawnManager;

    void Start()
    {
        gameObject.GetComponent<Outline>().enabled = true;

        // Find the SpawnManager object in the scene and store a reference to it
        spawnManager = FindObjectOfType<SpawnManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collided with the coin
        if (other.CompareTag("Player"))
        {
            // Notify the SpawnManager that a coin has been collected
            spawnManager.CollectCoin();

            // Destroy the coin after it's collected
            Destroy(gameObject);
        }
    }
}
