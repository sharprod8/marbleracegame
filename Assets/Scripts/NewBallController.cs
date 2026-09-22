using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class NewBallController : MonoBehaviour
{
    [Header("Movement")]
    public float accel = 20f;
    public float maxSpeed = 15f;

    [Header("Air")]
    public float airControl = 0.05f;
    public float extraAirGravity = 15f;

    [Header("Ground Check")]
    public LayerMask groundLayer = ~0;
    public float groundCheckDistance = 0.6f;

    private Rigidbody rb;
    private Transform cameraTransform;
    private bool isGrounded;

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
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(moveHorizontal, 0f, moveVertical);

        if (input.sqrMagnitude > 0.01f)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;

            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = (camForward * input.z + camRight * input.x).normalized;

            float controlMultiplier;

            if (!isGrounded)
            {
                rb.AddForce(Vector3.down * 5f, ForceMode.Acceleration);
            }
            if (!isGrounded)
            {
                rb.AddForce(Vector3.down * 5f, ForceMode.Acceleration);
            }
            if (isGrounded)
            {
                controlMultiplier = 1f;
            }
            else
            {
                controlMultiplier = airControl;
            }

            rb.AddForce(moveDirection * accel, ForceMode.Acceleration);
        }

        //horizontal speed
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (horizontalVelocity.magnitude > maxSpeed)
        {
            Vector3 clampedVelocity = horizontalVelocity.normalized * maxSpeed;

            rb.linearVelocity = new Vector3(clampedVelocity.x, rb.linearVelocity.y, clampedVelocity.z);
        }

        //extra gravity
        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * extraAirGravity, ForceMode.Acceleration);
        }

        Debug.Log("rb.linearVelocity.magnitude: " + rb.linearVelocity.magnitude);
        Debug.Log("rb.angularVelocity.magnitude: " + rb.angularVelocity.magnitude);

    }

    private void OnDrawGizmos()
    {
        if (isGrounded)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}