using UnityEngine;

public class MarbleProgress : MonoBehaviour
{
    public int currentCheckpoint = 0;

    private void OnTriggerEnter(Collider other)
    {
        Checkpoint checkpoint = other.GetComponent<Checkpoint>();

        if (checkpoint == null)
            return;

        Debug.Log($"{gameObject.name} hit checkpoint {checkpoint.checkpointIndex}");

        if (checkpoint.checkpointIndex > currentCheckpoint)
        {
            currentCheckpoint = checkpoint.checkpointIndex;
            Debug.Log($"{gameObject.name} now has checkpoint {currentCheckpoint}");
        }
    }
}