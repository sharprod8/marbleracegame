using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("jump")]
    public float jumpForce = 20f;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;

        if (rb == null)
            return;

        Vector3 velocity = rb.linearVelocity;
        velocity.y = 0f;
        rb.linearVelocity = velocity;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    }
}