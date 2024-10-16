using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Enemy : MonoBehaviour
{
    public float speed = 3f;                //make enemy slower than player probably
    private Transform player;                 // Transform stores a GameObjects Position, Rotation, Scale
    public bool isRunningAway = false;        // Enemy state, it can run towards you or from u
    private CharacterController characterController;

    private Animator animator;

    private Vector3 velocity;

    private float directionChangeTimer; // making enemy change direction every 2sec if its far from player
    private float currentRotationAngle; // To keep track of the current rotation angle

    public float rotationSpeed = 40f; //how fast enemy can rotate

public ParticleSystem explosionEffect;

    void Start()
    {

        // Getting players 'Transform' info
        player = GameObject.FindGameObjectWithTag("Player").transform;


        //outline
        var outline = gameObject.GetComponent<Outline>();
        if(outline != null) {
            outline.enabled = false;
        }
        


        animator = GetComponent<Animator>();

        animator.SetBool("isWalkingForward", true);

        // assign characterController in start so that it can be used later
        //GetComponent<CharacterController>() -> it takes CharacterController of the Enemy in this case, 
        //cus we declared Enemy in class and assigned script to Enemy prefab
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        //Enemy logic. Run away (is isrunningaway is true) or chase the player
        if (isRunningAway && gameObject.CompareTag("Enemy"))
        {
            RunAwayFromPlayer();
            gameObject.GetComponent<Outline>().enabled = true;

        }
        else if (gameObject.CompareTag("Enemy"))
        {
            ChasePlayer();
            gameObject.GetComponent<Outline>().enabled = false;
        }
        else if (gameObject.CompareTag("BossEnemy")) {
            ChasePlayer();
        }
    }


    public void RunAwayFromPlayer()
    {
        // Calculate the distance between the enemy and the player
        // This is added because enemy would just run in same direction, from the player.
        // I want to add some randomness, so when enemy is far enough, it will not just keep going straight
        // it will be done by calculating distance between enemy and player, and applying randomness if enemy is far from player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        Vector3 direction;

        if (distanceToPlayer < 15f)
        {
            // Make the direction be in opposite way of player
            // -player.position will make end vector point away from the player,
            // therefore once it runs in that direction, it will be direction away from the player
            direction = (transform.position - player.position).normalized;
        }
        else
        {
            // Increment the timer
            directionChangeTimer += Time.deltaTime;
            // timer will trigger his random movement
            if (directionChangeTimer >= 2f) // Change interval to 2 seconds
            {
                // Reset the timer
                directionChangeTimer = 0f;

                // adds rotation
                //and smoothens it
                float rotationAmount = Random.Range(30f, 90f);
               currentRotationAngle += rotationAmount;
               // currentRotationAngle = Mathf.Lerp(currentRotationAngle, currentRotationAngle + rotationAmount, Time.deltaTime);


                //keeps rotation in between 0-360
                //it could break the game cus some functions expect angle to be 0-360
                if (currentRotationAngle >= 360f)
                {
                    currentRotationAngle -= 360f;
                }
            }

            // Calculate new direction based on the current rotation angle
            Quaternion rotation = Quaternion.Euler(0f, currentRotationAngle, 0f);

            //move to that direction
            direction = rotation * Vector3.forward;
        }

        //// Rotate the enemy to face the direction it's moving towards
        //Quaternion lookRotation = Quaternion.LookRotation(direction); // look where youre going
        //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed); // rotates over time, making it more smooth

        //// Rotate the enemy to face the direction it's moving towards
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        // keep X and Z as they are at beginning
        lookRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        // Smoothly rotate the enemy towards the new look rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);


        // Move the enemy using CharacterController
        characterController.Move(direction.normalized * speed * Time.deltaTime);
    }






    void ChasePlayer()
    {
        // Move towards the player's position using CharacterController
        Vector3 direction = (player.position - transform.position).normalized;
        characterController.Move(direction * speed * Time.deltaTime);

        //Enemy looking the player when its chasing it
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void StartRunningAway()
    {
        isRunningAway = true; // Set the state to running away
         StartCoroutine(StopRunningAwayAfterTime(20f));
    }

     private IEnumerator StopRunningAwayAfterTime(float time)
    {
        yield return new WaitForSeconds(time); // Wait for the specified time
        isRunningAway = false; // Deactivate running away
    }   
public void DestroyEnemy()
    {
        // Activate the particle effect
        explosionEffect.transform.position = transform.position; // Set the position to the enemy's position
        explosionEffect.Play(); // Play the particle effect

        // Destroy the enemy object after a delay to allow the effect to play
        Destroy(gameObject); // Destroy the enemy
        Destroy(explosionEffect.gameObject, explosionEffect.main.duration); // Destroy the particle effect after its duration
    }
   
}
