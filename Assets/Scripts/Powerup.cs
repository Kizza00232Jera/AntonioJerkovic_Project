using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public bool isPowerupActive = false;

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
    }
}
