using System.Collections;
using UnityEngine;
public enum FanDirection
{
    Forwards,
    Backwards,
    Left,
    Right,
    Upwards,
    Downwards
}

public class FanScript : MonoBehaviour
{
    [Header("fan")]
    public bool isActive = false;
    public float fanForce = 20f;
    public float fanSpeed = 1f;
    public float exitPower = 4f;

    private float timePassed = 0;
    [SerializeField] private FanDirection fanDirection;

    

    private void OnTriggerStay(Collider other)
    {
        if (!isActive)
            return;

        Rigidbody rb = other.attachedRigidbody;

        if (rb == null)
            return;

        switch (fanDirection)
        {
            case FanDirection.Forwards:
                rb.AddForce(transform.forward * fanForce, ForceMode.VelocityChange);
                break;

            case FanDirection.Backwards:
                rb.AddForce(-transform.forward * fanForce, ForceMode.VelocityChange);
                break;

            case FanDirection.Left:
                rb.AddForce(-transform.right * fanForce, ForceMode.VelocityChange);
                break;

            case FanDirection.Right:
                rb.AddForce(transform.right * fanForce, ForceMode.VelocityChange);
                break;

            case FanDirection.Upwards:
                rb.AddForce(transform.up * fanForce, ForceMode.VelocityChange);
                break;

            case FanDirection.Downwards:
                rb.AddForce(-transform.up * fanForce, ForceMode.VelocityChange);
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        timePassed = 0;

        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null)
        {
            //StartCoroutine(ExitFan(rb));
        }
    }

    private IEnumerator ExitFan(Rigidbody rb)
    {
        float originalDamping = rb.linearDamping;
        rb.linearDamping = exitPower;

        yield return new WaitForSeconds(0.4f);

        if (rb != null)
        {
            rb.linearDamping = originalDamping;
        }

    }
}