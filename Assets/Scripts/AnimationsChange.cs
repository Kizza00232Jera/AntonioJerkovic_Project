using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsController : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController; // Reference to the CharacterController
    private float jumpHeight = 2f; // Height of the jump
    private float gravity = -9.81f; // Gravity value
    private Vector3 velocity; // Velocity for jumping

    // Define booleans for movement states
    public bool isWalkingForward = false;
    public bool isIdle = true;
    public bool isJumping = false;

    public bool isRunning = false;
    public SpawnManager spawnManager;


    void Start()
    {
        // Reference to Animator and CharacterController components
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
       spawnManager = FindObjectOfType<SpawnManager>();

    }

    void Update()
    {
        // Get input from axes
        float verticalInput = Input.GetAxis("Vertical"); // Forward movement (W/S or Up/Down)

        // Update movement states based on input
        UpdateMovementStates(verticalInput);

        // Jumping logic
        HandleJump();

        // Apply gravity
        ApplyGravity();

        // Update animator booleans
        UpdateAnimationBooleans();
    }

    private void UpdateMovementStates(float verticalInput)
    {
        // Update walking and idle states based on input
        if (verticalInput > 0 && !spawnManager.isPowerupActive)
        {
            isWalkingForward = true;
            isIdle = false;
            isRunning = false;
        }
        else if (verticalInput > 0 && spawnManager.isPowerupActive)
        {
            isWalkingForward = false;
            isIdle = false;
            isRunning = true;
        }
        else {
            isWalkingForward = false;
            isIdle = true;
            isRunning = false;
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && !characterController.isGrounded) // Check if on the ground
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Calculate jump velocity
            isJumping = true; // Set jumping state
        }
    }

    private void ApplyGravity()
    {
        // Apply gravity and move the character
        velocity.y += gravity * Time.deltaTime; //apply gravity
        characterController.Move(velocity * Time.deltaTime); // Move the character
    }

    void UpdateAnimationBooleans()
    {
        // Set the Animator boolean parameters based on movement state
        animator.SetBool("isWalkingForward", isWalkingForward);
        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isRunning", isRunning);
    }

 
    private void LateUpdate()
    {
        // Reset jumping state if character is grounded
        if (characterController.isGrounded)
        {
            if (isJumping) // Only reset if the character was jumping
            {
                isJumping = false; // Reset jumping state when on the ground
                // After landing, check movement state again
                isIdle = !isWalkingForward; // Set to idle if not walking
            }
            velocity.y = 0; // Reset vertical velocity
        }
    }
}
