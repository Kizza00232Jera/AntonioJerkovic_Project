using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Enemy : MonoBehaviour
{
    public float speed = 3f;                // Enemy's movement speed
    private Transform player;               // Reference to the player's transform
    private float stoppingDistance = 1.5f;  // Distance at which the enemy stops chasing
    private CharacterController characterController; // Reference to CharacterController
    private Vector3 velocity;               // Enemy's current velocity (for gravity)
    private float gravity = -9.81f;         // Gravity force

    void Start()
    {
        // Find the player by tag (make sure your player has the tag "Player")
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Get the CharacterController component
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // If the player exists, chase the player
        if (player != null)
        {
            ChasePlayer();
        }

        // Apply gravity
        ApplyGravity();
    }

    void ChasePlayer()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Debug log to check distance to player
        Debug.Log("Distance to Player: " + distanceToPlayer);

        // Only move towards the player if the distance is greater than the stopping distance
        if (distanceToPlayer > stoppingDistance)
        {
            // Calculate the direction to the player, but ignore the Y-axis
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; // Prevent vertical movement

            // Move the enemy towards the player using CharacterController
            characterController.Move(direction * speed * Time.deltaTime);

            // Rotate the enemy to face the player
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }


    void ApplyGravity()
    {
        // If the enemy is grounded, reset downward velocity
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;  // Small downward force to stay grounded
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Apply vertical movement (gravity) to the enemy
        characterController.Move(velocity * Time.deltaTime);
    }
}
