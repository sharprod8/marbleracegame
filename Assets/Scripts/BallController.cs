using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    public float speed = 45;
    public float maxSpeed = 120f;

    [Header("Speed-Based Steering")]
    public float speedThreshold = 30f;
    public float speedSteeringPenalty = 0.03f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

        if (inputDirection.magnitude > 0.1f)
        {
            Vector3 currentHorizontalVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            float currentSpeed = currentHorizontalVel.magnitude;

            float speedFactor = Mathf.Max(0f, currentSpeed - speedThreshold);
            float turnMultiplier = 1f / (1f + speedFactor * speedSteeringPenalty);

            rb.AddForce(inputDirection * speed * turnMultiplier, ForceMode.Acceleration);
        }

        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            Vector3 clampedVelocity = horizontalVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(clampedVelocity.x, rb.linearVelocity.y, clampedVelocity.z);
        }
    }
}