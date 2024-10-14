using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;                //make enemy slower than player probably
    private Transform player;                 // Transform stores a GameObject’s Position, Rotation, Scale
    private float runningDistance = 111.5f;    // Distance at which the enemy stops running away
    private float followingDistance = 2f;    // Distance at which the enemy stops running away
    public bool isRunningAway = false;        // State to check if the enemy is running away
    private CharacterController characterController; // Reference to CharacterController

    //Question!!!
    //Right now enemy doesnt fall if its out of map. add it later, if needed(cus ill restrict the map), or if its more properly to have it anyway
    private Vector3 velocity;
    //public float gravity = -9.81f;           // Gravity value


    private float directionChangeTimer; // Timer to keep track of direction changes
    public float directionChangeInterval = 1f; // Time interval for changing direction
    private float currentRotationAngle = 0f; // To keep track of the current rotation angle


    void Start()
    {
        // Getting players 'Transform' info
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // assign characterController in start so that it can be used later
        //GetComponent<CharacterController>() -> it takes CharacterController of the Enemy in this case, cus we declared Enemy in class
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        //Enemy logic. Run away (is isrunningaway is true) or chase the player
        if (isRunningAway)
        {
            RunAwayFromPlayer();
        }
        else
        {
            ChasePlayer();
        }
    }

    //   public void RunAwayFromPlayer()
    //{
    //    // Get the direction away from the player
    //    // -player.position will make end vector point away from the player,
    //    // therefore once it runs in that direction, it will be direction away from the player
    //    Vector3 direction = (transform.position - player.position).normalized;

    //    // Use CharacterController to move the enemy away
    //    //This will maybe move enemy faster depending on the players position!!! Maybe i need to delete *speed*, test later
    //    //get the Transofm of enemy, and move it based on direction (that changes based on player position)
    //    characterController.Move(direction * speed * Time.deltaTime);

    //    // Optional: Add randomness in the direction every few seconds
    //    if (Random.Range(0, 100) < 2) // Randomly change direction every few frames
    //    {
    //        float randomAngle = Random.Range(0f, 360f);
    //        Quaternion rotation = Quaternion.Euler(0f, randomAngle, 0f);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2f);
    //    }
    //}



    public void RunAwayFromPlayer()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        Vector3 direction;

        if (distanceToPlayer < 15f)
        {
            // Get the direction away from the player
            direction = (transform.position - player.position).normalized; // Move away from the player
        }
        else
        {
            // Increment the timer
            directionChangeTimer += Time.deltaTime;

            // Check if it's time to change direction
            if (directionChangeTimer >= directionChangeInterval)
            {
                // Reset the timer
                directionChangeTimer = 0f;

                // Change the rotation angle by 90 degrees
                currentRotationAngle += 90f;

                // Wrap around to keep it within 0-360 degrees
                if (currentRotationAngle >= 360f)
                {
                    currentRotationAngle -= 360f;
                }

                // Calculate new direction based on the current rotation angle
                Quaternion rotation = Quaternion.Euler(0f, currentRotationAngle, 0f);
                direction = rotation * Vector3.forward; // Calculate the new direction
            }
            else
            {
                // Maintain the current direction if not time to change
                direction = transform.forward; // Keep moving in the last direction
            }
        }

        // Move the enemy using CharacterController
        characterController.Move(direction.normalized * speed * Time.deltaTime);

        // Optional: Add randomness in the rotation every few frames
        if (Random.Range(0, 100) < 2) // Randomly change rotation every few frames
        {
            float randomAngle = Random.Range(0f, 360f);
            Quaternion rotation = Quaternion.Euler(0f, randomAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2f);
        }
    }





    void ChasePlayer()
    {
        // Move towards the player's position using CharacterController

        Vector3 direction = (player.position - transform.position).normalized;
        characterController.Move(direction * speed * Time.deltaTime);

        // Optional: Rotate the enemy to face the player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void StartRunningAway()
    {
        isRunningAway = true; // Set the state to running away
    }
}
