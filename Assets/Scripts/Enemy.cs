using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;         // Enemy's movement speed
    private Transform player;        // Reference to the player's transform
    private float stoppingDistance = 1.5f;  // Distance at which the enemy stops chasing

    void Start()
    {
        // Find the player by tag (make sure your player has the tag "Player")
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // If the player exists, chase the player
        if (player != null)
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Only move towards the player if the distance is greater than the stopping distance
        if (distanceToPlayer > stoppingDistance)
        {
            // Move towards the player's position
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // Optional: Rotate the enemy to face the player
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}
