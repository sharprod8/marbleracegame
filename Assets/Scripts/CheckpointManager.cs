using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    public Transform[] checkpoints;

    private void Awake()
    {
        Instance = this;
    }
}