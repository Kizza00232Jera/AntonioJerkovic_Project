using UnityEngine;

public class Powerup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that collided is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Powerup collected by player."); // Debug statement

            // Call the method to activate the power-up
            ActivatePowerup();
            // Destroy the power-up object
            Destroy(gameObject);
        }
    }

    void ActivatePowerup()
    {
        Enemy enemy = FindObjectOfType<Enemy>();
        if (enemy != null)
        {
            enemy.isRunningAway = true;  // Ensure you have this line
        }

        // Optionally destroy the power-up
        Destroy(gameObject);
    }
}
