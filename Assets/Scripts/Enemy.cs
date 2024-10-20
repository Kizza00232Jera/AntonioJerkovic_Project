using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Enemy : MonoBehaviour
{
    public float moveSpeed;                //make enemy slower than player probably
    private Transform player;                 // Transform stores a GameObjects Position, Rotation, Scale
    public bool isRunningAway = false;        // Enemy state, it can run towards you or from u
    private CharacterController characterController;

    private Animator animator;


    private float directionChangeTimer; // making enemy change direction every 2sec if its far from player
    private float currentRotationAngle; // To keep track of the current rotation angle

    public float rotationSpeed = 40f; //how fast enemy can rotate

    //  public ParticleSystem explosion;
    public GameObject ghostPrefab;

    public bool allEnemiesDestroyed = false;

    public SpawnManager spawnManager;


    void Start()
    {

        // Getting players 'Transform' info
        player = GameObject.FindGameObjectWithTag("Player").transform;

        spawnManager = FindObjectOfType<SpawnManager>();


        animator = GetComponent<Animator>();

        animator.SetBool("isWalkingForward", true);

        // assign characterController in start so that it can be used later
        //GetComponent<CharacterController>() -> it takes CharacterController of the Enemy in this case, 
        //cus we declared Enemy in class and assigned script to Enemy prefab
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        areAllEnemiesDestroyed();

        // Check if the object is tagged as an enemy, boss, or ghost
        if (gameObject.CompareTag("Enemy"))
        {
            // If power-up is active, the enemy runs away, otherwise it chases the player
            if (isRunningAway)
            {
                RunAwayFromPlayer();
                gameObject.GetComponent<Outline>().enabled = true;
            }
            else
            {
                ChasePlayer();
                gameObject.GetComponent<Outline>().enabled = false;

            }
        }
        else if (gameObject.CompareTag("BossEnemy"))
        {
            // Boss enemy always chases the player at the start
            if (!allEnemiesDestroyed)
            {
                ChasePlayer();
            }
            // Boss runs away when all enemies are destroyed and the power-up is active
            else if (allEnemiesDestroyed && isRunningAway)
            {
                RunAwayFromPlayer();
                //gameObject.GetComponent<Outline>().enabled = true;
            }
            else if (allEnemiesDestroyed && !isRunningAway)
            {
                ChasePlayer();
                //gameObject.GetComponent<Outline>().enabled = true;

            }
        }
        else if (gameObject.CompareTag("Ghost"))
        {
            // Ghost always runs away from the player
            RunAwayFromPlayer();
        }

    }

    // Helper functions:
    public void areAllEnemiesDestroyed()
    {
        // Find all active GameObjects with the "Enemy" tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // If no enemies are found, return true
        if (enemies.Length == 0)
        {
            Debug.Log("All enemies destroyed");
            allEnemiesDestroyed = true;
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
        characterController.Move(direction.normalized * moveSpeed * Time.deltaTime);
    }

    void ChasePlayer()
    {
        // Move towards the player's position using CharacterController
        Vector3 direction = (player.position - transform.position).normalized;
        characterController.Move(direction * moveSpeed * Time.deltaTime);

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
        // Instantiate the ghost at the enemy's position and rotation
        if (ghostPrefab != null)
        {
            GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);


            // Play the ghost's sound if there is an AudioSource attached
            AudioSource ghostAudio = ghost.GetComponent<AudioSource>();
            if (ghostAudio != null)
            {
                ghostAudio.Play();
            }

            spawnManager.EnemyKilled();
        }


        // Destroy the enemy object itself
        Destroy(gameObject);
    }



}
