using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;                  // Enemy's movement speed
    private Transform player;                 // Reference to the player's transform
    private float runningDistance = 111.5f;    // Distance at which the enemy stops running away
    private float followingDistance = 2f;    // Distance at which the enemy stops running away
    public bool isRunningAway = false;        // State to check if the enemy is running away
    private CharacterController characterController; // Reference to CharacterController

    void Start()
    {
        // Find the player by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Get the CharacterController component
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (isRunningAway)
        {
            RunAwayFromPlayer();
        }
        else
        {
            ChasePlayer();
        }
    }

    public void RunAwayFromPlayer()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Only move away from the player if the distance is less than the stopping distance
        if (distanceToPlayer < runningDistance)
        {
            Vector3 direction = (transform.position - player.position).normalized; // Get the direction away from the player

            // Use CharacterController to move the enemy away
            characterController.Move(direction * speed * Time.deltaTime);

            Debug.Log("Enemy is running away."); // Debug statement
        }
        else
        {
            isRunningAway = false; // Stop running away if the player is outside the stopping distance
        }

        // Optional: Add randomness in the direction every few seconds
        if (Random.Range(0, 100) < 2) // Randomly change direction every few frames
        {
            float randomAngle = Random.Range(0f, 360f);
            Quaternion rotation = Quaternion.Euler(0f, randomAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2f);
        }
    }

    void ChasePlayer()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Only move towards the player if the distance is greater than the stopping distance
        if (distanceToPlayer > followingDistance)
        {
            // Move towards the player's position using CharacterController
            Vector3 direction = (player.position - transform.position).normalized;
            characterController.Move(direction * speed * Time.deltaTime);

            // Optional: Rotate the enemy to face the player
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    public void StartRunningAway()
    {
        isRunningAway = true; // Set the state to running away
    }
}
