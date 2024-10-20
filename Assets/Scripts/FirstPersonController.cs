using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float moveSpeed = 15f;
    private float lookSensitivity = 2f;   // mouse sensetivity
    private float gravity = -9.81f;       // gracity cus player doesn't have rigid body but charcontroller
    public float jumpHeight = 2f;


    private float maxLookX = 45f;
    private float minLookX = -45f;
    private float rotX;

    private Vector3 velocity;            // Player's current velocity
    public bool isGrounded;             // Check if the player is on the ground

    private CharacterController characterController;
    private Camera playerCamera;

    private SpawnManager spawnManager;
    private Enemy enemy;

    public AudioClip jumpSound; 
    public AudioSource audioSource;
    


    void Start()
    {

        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main;
        spawnManager = FindObjectOfType<SpawnManager>();
         audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        //handle moving (wasd) and camera(mouse)
        Move();
        CameraLook();
    }


    void Move()
    {
        // Ground check using Raycast
        //cus isgrounded was not working properly whenever i start to move. 


        isGrounded = Physics.Raycast(transform.position, Vector3.down, characterController.height / 2 + 0.1f);

        if (isGrounded && velocity.y < 0)
        {
            // Small downward force to keep grounded
            velocity.y = -2f;
        }

        // Get input for movement on the X and Z axes
        float moveForward = Input.GetAxis("Horizontal");
        float rotatePlayer = Input.GetAxis("Vertical");

        // Move the player relative to their local direction (transform)
        Vector3 moveDirection = transform.right * moveForward + transform.forward * rotatePlayer;
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            PlayJumpSound();
        }

        // Apply gravity to the player
        velocity.y += gravity * Time.deltaTime;

        // Move the player based on gravity
        characterController.Move(velocity * Time.deltaTime);
    }


    // Handle camera and player rotation (using mouse)
    void CameraLook()
    {
        // Mouse movement on x and y axis
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        //rotate player left right w mouse
        transform.Rotate(Vector3.up * mouseX);

        // rotate up down, but add limits so it can look only a little bit upwards and downwards
        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, minLookX, maxLookX);

        // Apply the rotation to the player's camera (only affects the camera's X rotation)
        playerCamera.transform.localRotation = Quaternion.Euler(rotX, 0f, 0f);
    }

void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Enemy"))
    {
        Debug.Log("Collided with enemy");

        // Get the enemy script
        Enemy enemy = other.GetComponent<Enemy>();

        if (!enemy.isRunningAway)
        {
            DestroyPlayer(); // Trigger enemy destruction and particle effect
            spawnManager.GameOver();
        }
        else
        {
            enemy.DestroyEnemy();
        }
    }

    if (other.CompareTag("BossEnemy"))
    {
        Debug.Log("Collided with Boss");

        // Get the BossEnemy component
        Enemy bossEnemy = other.GetComponent<Enemy>();

        // Ensure the boss enemy is not null
        if (bossEnemy != null)
        {
            // Check if all enemies are destroyed
            if (bossEnemy.allEnemiesDestroyed)
            {
                bossEnemy.DestroyEnemy(); // Destroy the BossEnemy
            }
            else
            {
                DestroyPlayer(); // Destroy player on collision
                spawnManager.GameOver();
            }
        }
    }
}

 void PlayJumpSound()
    {
        if (audioSource != null && jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound); // Plays the jump sound once
        }
    }

    private void DestroyPlayer()
    {
        Time.timeScale = 0;
        
    }




}
