// This first example shows how to move using the New Input System Package

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; // needed for us to use our public InputActionReferences like moveAction and jumpAction.
using UnityEngine.Rendering;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour

{
    // Controls the player's speed, jump height, gravity, horizontal velocity, acceleration and deceleration.
    private float playerSpeed = 5.0f;
    private float jumpHeight = 1.5f;
    private float gravityValue = -9.81f;        // apparently people use -9.81 because it's earth's gravity. Cool.
    private Vector3 currentHorizontalVelocity;
    public float acceleration = 25f;
    public float deceleration = 12f;

    // uses the CharacterController Component in unity, stores our velocity, and checks if we're grounded
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer; // groundedPlayer is a CharacterController built in check for our jump to use.

    [Header("Input Actions")]
    public InputActionReference moveAction;     // expects Vector2, WASD
    public InputActionReference jumpAction;     // expects Button, like Spacebar
    public InputActionReference crouchAction;   // expects Button, like Ctrl, unused right now
    public InputActionReference runAction;      // expects Button, like Shift

    public Transform playerCamera;              // reference the playerCamera to know how to update where forwards is.
    

    private void Awake()
    {
        controller = GetComponent<CharacterController>(); // This grabs the CharacterController before anything else happens
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;                             // I think UnityDocumentation just puts this here so it's easier to understand than controller.isGrounded "It returns true if the controller collided with any object below it during the movement — typically used to determine if the character is standing on a surface (e.g., terrain, platform, floor)."

        Vector3 camForward = playerCamera.forward;          // this is what direction the camera is facing forwards
        camForward.y = 0f;                                  // y = 0f means vertical won't be affected.                         

        Vector3 camRight = playerCamera.right;              // this adds our direction we're looking from left to right
        camRight.y = 0f;

        // Read input 
        Vector2 input = moveAction.action.ReadValue<Vector2>();               // Look for what buttons we press
        Vector3 move = camForward * input.y + camRight * input.x;             // Translate it to the third dimension, specifically our x and y. NOTE HOW input.y IS FOR OUR Z, THAT'S BECAUSE OUR X AND Y IN A VECTOR 2 DOESNT MEAN THE SAME IN 3D SINCE Y IS NOW UP AND DOWN
        move = Vector3.ClampMagnitude(move, 1f);                              // Makes it so diagonal isn't the combined speed of 1y and 1x floats

        Vector3 desiredVelocity = move * playerSpeed; // Vector3 desiredVelocity is our move (the x and y input for our movement) * playerSpeed (just a flat float variable.)

        // Accelerate or decelerate
        if (move.magnitude > 0.1f) // magnitude is the length of a vector so it's guaranteed to be > 0.1f if you're moving on keyboard.)
        {
            // Accelerate toward desired velocity
            currentHorizontalVelocity = Vector3.MoveTowards(currentHorizontalVelocity, desiredVelocity, acceleration * Time.deltaTime);
            // Vector3.MoveTowards takes target, float and MaxDistanceDelta
            // The position to move from.
            // The position to move towards.
            // Distance to move currentHorizontalVelocity per call.
        }
        else
        {
            // Decelerate toward zero
            currentHorizontalVelocity = Vector3.MoveTowards(currentHorizontalVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        // Sprint Logic
        if (runAction.action.IsPressed())
        {
            playerSpeed = 10f;
            controller.height = 2.0f;     // hold crouch then sprint will make this player crouch height but run at run speed without this code.
        }

        // Crouch Logic
        else if (crouchAction.action.IsPressed() && !runAction.action.IsPressed())        // this works like old valve games where crouching actaully brings your character model upwards instead of moving you downwards
        {                                                                                 // so you can jump over higher obstacles by jumping. Not intentional and scuffed in implementation but cool.
            controller.height = 1.0f;
            playerSpeed = 2.5f;
        }
        else
        {
            controller.height = 2.0f;
            playerSpeed = 5f;
        }


        // Jump Logic

        // if player is on the ground and still moving downwards...
        //if (groundedPlayer && playerVelocity.y < 0)
        //{
        //    // stop moving downwards. Need to do this because of our gravity.
        //    playerVelocity.y = 0f;
        //}

        // Jump action
        if (jumpAction.action.triggered && groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue); // From CharacterController.Move on Unity Documentation, figure out Sqrt math
            // Mathf.Sqrt is finding the square root of f. this formula is v² = u² + 2as
            // v = final velocity at the top of the jump (this should be 0 given that if you're at the top of your jump, you should be going neither up nor down)
            // u = initial velocity (what we want to calculate)
            // a = acceleration (gravity)
            // s = displacement (jump height)
            // final velocity aka v is 0, 0 squared is 0. We don't have to worry about it anymore.
            // u squared is = 2*a*s, now we're back to our Mathf.Sqrt. 
            // the reason we use -2.0f is because this specific formula with the 2as. don't ask me why, it involves math I don't understand yet.
            // we have our acceleration and our displacement already as variables so if we plug it all in, we can jump and land.
            // the thing UnityDocumentation wrote for it's parameters is actually s*2*a instead of 2as but it works the same.
            // so from this one line of code. playerVelocity.y = our equation where we need to reach our jumpHeigt variable, then using gravity and our -2f which we use because of the base formula square rooting for velocity uses, we rapidly decrease in speed using that logic
            // it goes slightly higher than 1.5 though, i got as high as 1.56, idk how big a deal that is though or if I did something wrong.
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;          // this one just pulls us down faster and faster infinitely since our negative gravity value is being multiplied by time


        // Combine horizontal and vertical movement
        Vector3 finalMove = currentHorizontalVelocity + (playerVelocity.y * Vector3.up);    // our currentHorizontalVelocity is multiplied by acceleration, deceleration and time so it has those things now
        controller.Move(finalMove * Time.deltaTime);                                        // be careful when multiplying by Time.deltaTime multiple time, things will run either very quickly or very slowly.
    }
}
//The character controller must not be able to walk up steep slopes (> 50 degrees). 
//The player controller must not be able to jump up vertical walls repeatedly. 
//The test level should provide vertical walls, various slopes, and ledges to test the functionality of the character controller. 

//Submission: 

//Submit a build
//Submit github repo 
