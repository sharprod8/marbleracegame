using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("Speed")]
    public float accelerationForce = 40f;
    public float maxFlatSpeed = 40f;
    public float maxTerminalSpeed = 80f;
    public float downhillForceMultiplier = 20f;

    [Header("Steer")]
    public float speedThreshold = 20f;
    public float speedSteeringPenalty = 0.03f;

    [Header("Air")]
    public float initialAirControl = 0.9f;
    public float airControlDecayRate = 0.2f;
    public float extraAirGravity = 5f;

    [Header("Ground Check")]
    public LayerMask groundLayer = ~0;
    public float groundCheckDistance = 0.6f;

    private Rigidbody rb;
    private Transform cameraTransform;
    private bool isGrounded;
    private Vector3 groundNormal = Vector3.up;
    private float timeInAir;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundLayer);

        if (isGrounded)
        {
            timeInAir = 0f;
            groundNormal = hit.normal;
        }
        else
        {
            timeInAir += Time.fixedDeltaTime;
            groundNormal = Vector3.up;

            rb.AddForce(Vector3.down * (extraAirGravity * (1f + timeInAir)), ForceMode.Acceleration);
        }

        float slopeAngle = Vector3.Angle(Vector3.up, groundNormal);
        if (isGrounded && slopeAngle > 5f)
        {
            Vector3 downhillDirection = Vector3.ProjectOnPlane(Vector3.down, groundNormal).normalized;
            rb.AddForce(downhillDirection * downhillForceMultiplier, ForceMode.Acceleration);
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 rawInput = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

        if (rawInput.magnitude > 0.1f)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = (camForward * rawInput.z + camRight * rawInput.x).normalized;

            Vector3 currentHorizontalVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            float currentSpeed = currentHorizontalVel.magnitude;

            float speedFactor = Mathf.Max(0f, currentSpeed - speedThreshold);
            float speedTurnMultiplier = 1f / (1f + speedFactor * speedSteeringPenalty);

            float airControlMultiplier;
            if (isGrounded)
            {
                airControlMultiplier = (float) 1f;
            }
            else
            {
                airControlMultiplier = (float) Mathf.Max(0f, initialAirControl - (timeInAir * airControlDecayRate));
            }

            rb.AddForce(moveDirection * accelerationForce * speedTurnMultiplier * airControlMultiplier, ForceMode.Acceleration);
        }

        float allowedMaxSpeed = Mathf.Lerp(maxFlatSpeed, maxTerminalSpeed, slopeAngle / 45f);
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (horizontalVelocity.magnitude > allowedMaxSpeed)
        {
            Vector3 clampedVelocity = horizontalVelocity.normalized * allowedMaxSpeed;
            rb.linearVelocity = new Vector3(clampedVelocity.x, rb.linearVelocity.y, clampedVelocity.z);
        }
    }
}