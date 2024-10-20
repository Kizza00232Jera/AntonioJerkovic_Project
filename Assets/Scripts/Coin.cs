using UnityEngine;

public class Coin : MonoBehaviour
{
    private SpawnManager spawnManager;
  public AudioClip coinSound; 
    public AudioSource audioSource;
    
    void Start()
    {
        // Find the SpawnManager object in the scene and store a reference to it
        spawnManager = FindObjectOfType<SpawnManager>();
         audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collided with the coin
        if (other.CompareTag("Player"))
        {
            // Notify the SpawnManager that a coin has been collected
            spawnManager.CollectCoin();
            PlayCoinSound();
            // Destroy the coin after it's collected
            Destroy(gameObject, 0.1f);
        }
    }  
    
     void PlayCoinSound()
    {
        if (audioSource != null && coinSound != null)
        {
            audioSource.PlayOneShot(coinSound); // Plays the jump sound once
        }
    }
}
