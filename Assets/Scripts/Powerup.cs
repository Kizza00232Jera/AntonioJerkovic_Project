using UnityEngine;

public class Powerup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //if "player" picks up collides with powerup, it will trigger this script
        if (other.CompareTag("Player"))
        {
            //it will run activatepowerup and it will remove the powerup
            ActivatePowerup();
            Destroy(gameObject);
        }
    }

    void ActivatePowerup()
    {
        //when picking up powerup, look for tag Enemy, and change its isRunningAway boolean to Yes.
        Enemy enemy = FindObjectOfType<Enemy>();
        if (enemy != null)
        {
            enemy.isRunningAway = true; 
        }

    }
}
