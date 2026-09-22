using UnityEngine;

public class BoostPad : MonoBehaviour
{
    [Header("boost")]
    public float boostForce = 20f;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;

        if (rb == null)
            return;

        rb.AddForce(transform.forward * boostForce, ForceMode.VelocityChange);
    }
}