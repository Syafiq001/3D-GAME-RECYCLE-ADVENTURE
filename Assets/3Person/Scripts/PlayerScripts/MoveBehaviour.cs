using UnityEngine;
using UnityEngine.Serialization;

// MoveBehaviour inherits from GenericBehaviour. This class corresponds to basic walk and run behaviour, it is the default behaviour.
public class MoveBehaviour : GenericBehaviour
{
    public float walkSpeed = 0.15f;                 // Default walk speed.
    public float runSpeed = 1.0f;                   // Default run speed.
    public float sprintSpeed = 2.0f;                // Default sprint speed.
    public float speedDampTime = 0.1f;              // Default damp time to change the animations based on current speed.
    public string jumpButton = "Jump";              // Default jump button.
    public float jumpHeight = 1.5f;                 // Default jump height.
    public float jumpInertialForce = 10f;           // Default horizontal inertial force when jumping.

    private float speed, speedSeeker;               // Moving speed.
    private int jumpBool;                           // Animator variable related to jumping.
    private int groundedBool;                       // Animator variable related to whether or not the player is on ground.
    private bool jump;                              // Boolean to determine whether or not the player started a jump.
    private bool isColliding;                       // Boolean to determine if the player has collided with an obstacle.
    public AudioSource jumpAudioSource;             // Reference to the AudioSource component for jump sound.

    // Start is always called after any Awake functions.
    void Start()
    {
        // Set up the references.
        jumpBool = Animator.StringToHash("Jump");
        groundedBool = Animator.StringToHash("Grounded");
        behaviourManager.GetAnim.SetBool(groundedBool, true); // Set the grounded animation parameter to true initially.

        // Subscribe and register this behaviour as the default behaviour.
        behaviourManager.SubscribeBehaviour(this); // Subscribe this behaviour to the behaviour manager.
        behaviourManager.RegisterDefaultBehaviour(this.behaviourCode); // Register this behaviour as the default one.
        speedSeeker = runSpeed; // Set the initial speed seeker value to the default run speed.
    }

    // Update is used to set features regardless the active behaviour.
    void Update()
    {
        // Get jump input.
        if (!jump && Input.GetButtonDown(jumpButton) && behaviourManager.IsCurrentBehaviour(this.behaviourCode) && !behaviourManager.IsOverriding())
        {
            jump = true; // Set the jump flag to true when the jump button is pressed.
            // Play the jump sound
            PlayJumpSound(); // Call the function to play the jump sound.
        }
    }

    // LocalFixedUpdate overrides the virtual function of the base class.
    public override void LocalFixedUpdate()
    {
        // Call the basic movement manager.
        MovementManagement(behaviourManager.GetH, behaviourManager.GetV); // Call the function to manage player movement.

        // Call the jump manager.
        JumpManagement(); // Call the function to manage jumping.
    }

    // Execute the idle and walk/run jump movements.
    void JumpManagement()
    {
        // Start a new jump.
        if (jump && !behaviourManager.GetAnim.GetBool(jumpBool) && behaviourManager.IsGrounded())
        {
            // Set jump related parameters.
            behaviourManager.LockTempBehaviour(this.behaviourCode); // Lock the current behaviour temporarily.
            behaviourManager.GetAnim.SetBool(jumpBool, true); // Set the jump animation parameter to true.
            // Is a locomotion jump?
            if (behaviourManager.GetAnim.GetFloat(speedFloat) > 0.1)
            {
                // Temporarily change player friction to pass through obstacles.
                GetComponent<CapsuleCollider>().material.dynamicFriction = 0f; // Set dynamic friction to zero.
                GetComponent<CapsuleCollider>().material.staticFriction = 0f; // Set static friction to zero.
                // Remove vertical velocity to avoid "super jumps" on slope ends.
                RemoveVerticalVelocity(); // Call the function to remove vertical velocity.
                // Set jump vertical impulse velocity.
                float velocity = 2f * Mathf.Abs(Physics.gravity.y) * jumpHeight; // Calculate jump velocity.
                velocity = Mathf.Sqrt(velocity); // Take square root of velocity.
                behaviourManager.GetRigidBody.AddForce(Vector3.up * velocity, ForceMode.VelocityChange); // Apply vertical force to jump.
                // Play the jump sound when a jump is initiated.
                PlayJumpSound(); // Call the function to play the jump sound.
            }
        }

        // Is already jumping?
        else if (behaviourManager.GetAnim.GetBool(jumpBool))
        {
            // Keep forward movement while in the air.
            if (!behaviourManager.IsGrounded() && !isColliding && behaviourManager.GetTempLockStatus())
            {
                behaviourManager.GetRigidBody.AddForce(transform.forward * (jumpInertialForce * Physics.gravity.magnitude * sprintSpeed), ForceMode.Acceleration); // Apply horizontal force while jumping.
            }
            // Has landed?
            if ((behaviourManager.GetRigidBody.velocity.y < 0) && behaviourManager.IsGrounded())
            {
                behaviourManager.GetAnim.SetBool(groundedBool, true); // Set the grounded animation parameter to true.
                // Change back player friction to default.
                GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f; // Set dynamic friction back to default.
                GetComponent<CapsuleCollider>().material.staticFriction = 0.6f; // Set static friction back to default.
                // Set jump related parameters.
                jump = false; // Reset the jump flag.
                behaviourManager.GetAnim.SetBool(jumpBool, false); // Set the jump animation parameter to false.
                behaviourManager.UnlockTempBehaviour(this.behaviourCode); // Unlock the current behaviour.
            }
        }
    }

    // Deal with the basic player movement
    void MovementManagement(float horizontal, float vertical)
    {
        // On ground, obey gravity.
        if (behaviourManager.IsGrounded())
            behaviourManager.GetRigidBody.useGravity = true; // Enable gravity when player is on the ground.

        // Avoid takeoff when reached a slope end.
        else if (!behaviourManager.GetAnim.GetBool(jumpBool) && behaviourManager.GetRigidBody.velocity.y > 0)
        {
            RemoveVerticalVelocity(); // Call the function to remove vertical velocity.
        }

        // Call function that deals with player orientation.
        Rotating(horizontal, vertical); // Call the function to rotate the player.

        // Set proper speed.
        Vector2 dir = new Vector2(horizontal, vertical); // Create a vector for movement direction.
        speed = Vector2.ClampMagnitude(dir, 1f).magnitude; // Calculate the magnitude of the movement vector.
        speedSeeker += Input.GetAxis("Mouse ScrollWheel"); // Adjust speed seeker based on mouse scroll wheel input.
        speedSeeker = Mathf.Clamp(speedSeeker, walkSpeed, runSpeed); // Clamp speed seeker value within the specified range.
        speed *= speedSeeker; // Calculate final speed.
        if (behaviourManager.IsSprinting())
        {
            speed = sprintSpeed; // Set speed to sprint speed if sprinting.
        }

        behaviourManager.GetAnim.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime); // Set the speed parameter in the animator.
    }

    // Remove vertical rigidbody velocity.
    private void RemoveVerticalVelocity()
    {
        Vector3 horizontalVelocity = behaviourManager.GetRigidBody.velocity; // Get the current rigidbody velocity.
        horizontalVelocity.y = 0; // Zero out the vertical component.
        behaviourManager.GetRigidBody.velocity = horizontalVelocity; // Set the modified velocity back to the rigidbody.
    }

    // Rotate the player to match correct orientation, according to camera and key pressed.
    Vector3 Rotating(float horizontal, float vertical)
    {
        // Get camera forward direction, without vertical component.
        Vector3 forward = behaviourManager.playerCamera.TransformDirection(Vector3.forward);

        // Player is moving on ground, Y component of camera facing is not relevant.
        forward.y = 0.0f;
        forward = forward.normalized;

        // Calculate target direction based on camera forward and direction key.
        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        Vector3 targetDirection = forward * vertical + right * horizontal;

        // Lerp current direction to calculated target direction.
        if ((behaviourManager.IsMoving() && targetDirection != Vector3.zero))
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection); // Calculate the target rotation.
            Quaternion newRotation = Quaternion.Slerp(behaviourManager.GetRigidBody.rotation, targetRotation, behaviourManager.turnSmoothing); // Smoothly interpolate between current rotation and target rotation.
            behaviourManager.GetRigidBody.MoveRotation(newRotation); // Apply the new rotation to the rigidbody.
            behaviourManager.SetLastDirection(targetDirection); // Set the last direction faced by the player.
        }
        // If idle, Ignore current camera facing and consider last moving direction.
        if (!(Mathf.Abs(horizontal) > 0.9 || Mathf.Abs(vertical) > 0.9))
        {
            behaviourManager.Repositioning(); // Reposition the player based on the last moving direction.
        }

        return targetDirection; // Return the calculated target direction.
    }

    // Collision detection.
    private void OnCollisionStay(Collision collision)
    {
        isColliding = true; // Set collision flag to true when colliding with an object.
        // Slide on vertical obstacles
        if (behaviourManager.IsCurrentBehaviour(this.GetBehaviourCode()) && collision.GetContact(0).normal.y <= 0.1f)
        {
            GetComponent<CapsuleCollider>().material.dynamicFriction = 0f; // Set dynamic friction to zero to allow sliding.
            GetComponent<CapsuleCollider>().material.staticFriction = 0f; // Set static friction to zero to allow sliding.
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        isColliding = false; // Set collision flag to false when no longer colliding.
        GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f; // Restore dynamic friction to default.
        GetComponent<CapsuleCollider>().material.staticFriction = 0.6f; // Restore static friction to default.
    }

    // Function to play the jump sound.
    public void PlayJumpSound()
    {
        if (jumpAudioSource != null) // Check if the jump audio source is assigned.
        {
            jumpAudioSource.Play(); // Play the jump sound.
        }
    }
}
