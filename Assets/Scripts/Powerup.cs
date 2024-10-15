using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Powerup : MonoBehaviour
{
    public bool isPowerupActive = false;
    public float powerupDuration = 20f;
    public TextMeshProUGUI timerText; 

    private void OnTriggerEnter(Collider other)
    {
        //if "player" picks up collides with powerup, it will trigger this script
        if (other.CompareTag("Player"))
        {
            //it will run activatepowerup and it will remove the powerup
            ActivatePowerup();
            isPowerupActive = true;
            Destroy(gameObject);
        }
    }

    void ActivatePowerup()
    {
        //when picking up powerup, look for all enemies and change their isRunningAway boolean to true.
        Enemy[] enemies = FindObjectsOfType<Enemy>(); // Get all enemies in the scene
        foreach (Enemy enemy in enemies)
        {
            enemy.isRunningAway = true;
        }

         
        // Start the powerup timer
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        isPowerupActive = true;
        
        float timeRemaining = powerupDuration;

        while (timeRemaining > 0)
        {
             // Update the timer text with TextMeshPro
              Debug.Log("Updating timer: " + timeRemaining); 
            timerText.text = "Powerup Active: " + Mathf.Ceil(timeRemaining) + "s";
            timeRemaining -= Time.deltaTime; // Decrease time
            yield return null; // Wait for the next frame
        }

        // Powerup duration has ended
        isPowerupActive = false;

        // Reset enemies' behavior
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.isRunningAway = false; // Change their isRunningAway boolean back to false
        }

         timerText.text = "Pu=0"; // Clear the timer text
    }

}
