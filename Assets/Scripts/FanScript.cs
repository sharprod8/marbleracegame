using System.Collections;
using UnityEngine;
public enum FanDirection { Upwards, Forwards }
public class FanScript : MonoBehaviour
{
    [Header("fan")]
    public float fanForce = 20f;
    public float fanSpeed = 1f;
    public float exitPower = 4f;

    private float timePassed = 0;
    [SerializeField] private FanDirection fanDirection;

    

    private void OnTriggerStay(Collider other)
    {
        
        Rigidbody rb = other.attachedRigidbody;

        if (rb == null)
            return;

        if (fanDirection == FanDirection.Upwards)
        {
            rb.AddForce(Vector3.up * fanForce, ForceMode.VelocityChange);
        }
        else if (fanDirection == FanDirection.Forwards)
        {
            rb.AddForce(-Vector3.forward * fanForce, ForceMode.VelocityChange);
        }
        

        //timePassed += Time.deltaTime;

        /*if (timePassed > fanSpeed)
        {
            Vector3 velocity = rb.linearVelocity;
            velocity.y = 0f;
            rb.linearVelocity = velocity;

            rb.AddForce(Vector3.up * fanForce, ForceMode.VelocityChange);

            timePassed = 0;
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        timePassed = 0;

        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null)
        {
            StartCoroutine(ExitFan(rb));
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