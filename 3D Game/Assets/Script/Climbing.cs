using UnityEngine;

public class Climbing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public CharacterController characterController;
    public Movement pm; // Assuming this is a custom movement script
    public LayerMask whatIsWall;

    [Header("Climbing")]
    public float climbSpeed;
    public float maxClimbTime;
    private float climbTimer;

    private bool climbing;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private bool wallFront;

    private void Update()
    {
        wallCheck();
        StateMachine();

        if (climbing) ClimbingMovement();
    }

    private void StateMachine()
    {
        // State 1 - Climbing
        if (wallFront && Input.GetKey(KeyCode.W) && wallLookAngle < maxWallLookAngle)
        {
            if (!climbing && climbTimer > 0) StartClimbing();

            // Timer
            if (climbTimer > 0) climbTimer -= Time.deltaTime;
            if (climbTimer < 0) StopClimbing();
        }

        // State 2 - Not Climbing
        else
        {
            if (climbing) StopClimbing();
        }
    }

    private void wallCheck()
    {
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

        if (pm.isGrounded)
        {
            climbTimer = maxClimbTime;
        }
    }

    private void StartClimbing()
    {
        climbing = true;

        // Optional: Add camera FOV change or any visual effect
    }

    private void ClimbingMovement()
    {
        // Move upwards at climbSpeed
        Vector3 climbDirection = Vector3.up * climbSpeed * Time.deltaTime;
        characterController.Move(climbDirection);

        // Optional: Play sound effects here
    }

    private void StopClimbing()
    {
        climbing = false;

        // Optional: Add particle effects or other visual indicators
    }
}
