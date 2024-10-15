using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Powerup : MonoBehaviour
{
    public bool isPowerupActive = false;
 
 private void Start()
    {
      
    }
    private void OnTriggerEnter(Collider other)
    {
        //if "player" picks up collides with powerup, it will trigger this script
        if (other.CompareTag("Player"))
        {
            //it will run activatepowerup and it will remove the powerup
            ActivatePowerup();
             // Remove the powerup from the scene
            Destroy(gameObject);
        }
          
    }

     void ActivatePowerup()
    {
        // Find all enemies in the scene
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        // Loop through each enemy and set them to run away
        foreach (Enemy enemy in enemies)
        {
            enemy.StartRunningAway();
        }
    }

   
}
