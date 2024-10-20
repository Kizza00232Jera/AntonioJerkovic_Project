using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Powerup : MonoBehaviour
{
    public SpawnManager spawnManager;
   
   
 
 private void Start()
    {
        gameObject.GetComponent<Outline>().enabled = true;
        spawnManager = FindObjectOfType<SpawnManager>();

    }
    private void OnTriggerEnter(Collider other)
    {
        //if "player" picks up collides with powerup, it will trigger this script
        if (other.CompareTag("Player"))
        {
            //it will run activatepowerup and it will remove the powerup
            spawnManager.ActivatePowerup();
            spawnManager.OnPowerupCollected();
             // Remove the powerup from the scene
            Destroy(gameObject);
        }
          
    }




   
}
