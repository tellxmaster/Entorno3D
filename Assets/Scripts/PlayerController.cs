using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 1.0f;
    public float sprintSpeed = 2.0f;
    public float rotationSpeed = 0.5f;
    public float jumpHeight = 2.0f;
    public float gravity = -9.8f;
    private Vector3 velocity;
    private bool isJumping;

    public float normalCameraSpeed = 1.0f; // Camera speed when walking
    public float sprintCameraSpeed = 0.5f; // Camera speed when sprinting

    public CameraController cameraController; // Make sure to assign this in the Inspector.

    public Animator animator; // Ensure this is set in the inspector.

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : speed;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);

        // Rotate player to face movement direction
        if (move != Vector3.zero)
        {
            // Smoothly rotate towards movement direction
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * rotationSpeed);
        }

        // Set movement animations
        if (move != Vector3.zero)
        {
            bool isRunning = moveSpeed == sprintSpeed;
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isWalking", !isRunning);

            // Change camera speed based on whether the player is running
            cameraController.SetCameraSpeed(isRunning ? sprintCameraSpeed : normalCameraSpeed);
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
        }

        if (move != Vector3.zero && Input.GetButton("Jump") && !isJumping)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJumping = true;
            animator.SetTrigger("Jump");
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Reset isJumping if on the ground
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
            isJumping = false;
        }
    }
}
