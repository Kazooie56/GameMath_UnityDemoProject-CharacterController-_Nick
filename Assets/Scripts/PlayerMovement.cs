// This first example shows how to move using the New Input System Package

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour

{
    // Controls the player's speed, jump height and gravity
    private float playerSpeed = 5.0f;
    private float jumpHeight = 1.5f;
    private float gravityValue = -9.81f;

    // uses the CharacterController Component in unity, stores our velocity, and checks if we're grounded
    // I believe playerVelocity and groundedPlayer might be things built into CharacterControllers
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [Header("Input Actions")]
    public InputActionReference moveAction; // expects Vector2, WASD
    public InputActionReference jumpAction; // expects Button, like Spacebar

    private void Awake()
    {
        // this lets us access the character controller immediately, even before start
        controller = gameObject.AddComponent<CharacterController>();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
    }

    void Update()
    {
        // if player is on the ground and still moving downwards...
        if (controller.isGrounded && playerVelocity.y < 0)                  // controller.isGrounded means the player is touching the ground.
        {
            // stop moving downwards. Need to do this because of our gravity.
            playerVelocity.y = 0f;
        }

        // Read input WASD
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        // Translate it to the third dimension
        Vector3 move = new Vector3(input.x, 0, input.y);

        // Makes it so diagonal isn't the combined speed of 1y and 1x floats
        move = Vector3.ClampMagnitude(move, 1f);

        if (move != Vector3.zero)
        {
            transform.forward = move;
        }

        // Jump
        if (jumpAction.action.triggered && groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Combine horizontal and vertical movement
        Vector3 finalMove = (move * playerSpeed) + (playerVelocity.y * Vector3.up);
        controller.Move(finalMove * Time.deltaTime);
    }
}
