using UnityEngine;

public class KillPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        MarbleProgress progress = other.GetComponent<MarbleProgress>();

        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (progress == null || rb == null)
            return;

        Transform checkpoint = CheckpointManager.Instance.checkpoints[progress.currentCheckpoint];

        rb.position = checkpoint.position;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}